//
// Copyright (c) 2013-2016 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

namespace AncientLightStudios.uTomate
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Net.FtpClient;
    using API;
    using UnityEngine;

    /// <summary>
    ///     uTomate's FTP client (basically a wrapper around System.Net.FtpClient).
    /// </summary>
    internal class UTFtpClient : IDisposable
    {
        private readonly string host;
        private readonly string password;
        private readonly string username;
        private readonly FtpClient ftpClient;


        public UTFtpClient(string host, string username, string password)
        {
            ftpClient = new FtpClient
            {
                Host = host,
                Credentials = new NetworkCredential(username, password)
            };
        }


        public void Dispose()
        {
            ftpClient.Dispose();
        }

        /// <summary>
        ///     Recursively creates folders on the remote end.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="then">action to call after the path has been created</param>
        public UTDeferredExecution MkDirs(string remotePath)
        {
            var normalizedPath = Normalize(remotePath);

            return ExecuteInBackground((sender, args) => { ftpClient.CreateDirectory(normalizedPath, true); });
        }

        /// <summary>
        ///     Uploads a file.
        /// </summary>
        /// <param name="localFile">The file to upload</param>
        /// <param name="remoteFile">Path of the remote file.</param>
        /// <returns></returns>
        public UTDeferredExecution Upload(string localFile, string remoteFile)
        {
            return ExecuteInBackground((sender, args) =>
            {
                var worker = (BackgroundWorker) sender;

                using (var output = ftpClient.OpenWrite(Normalize(remoteFile)))
                {
                    var size = new FileInfo(localFile).Length;
                    using (var input = File.OpenRead(localFile))
                    {
                        var buffer = new byte[32768];
                        int read;
                        var total = 0;

                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {

                            if (worker.CancellationPending)
                            {
                                return;
                            }

                            output.Write(buffer, 0, read);

                            total += read;
                            var message = string.Format("{0}/{1}", UTils.BytesToHumanReadable(total), UTils.BytesToHumanReadable(size));
                            var progress = Mathf.CeilToInt(100f*total/size);
                            worker.ReportProgress(progress, message);
                        }
                    }
                }
            });
        }

        private static string Normalize(string input)
        {
            var normalized = UTFileUtils.NormalizeSlashes(input);
            if (normalized.EndsWith("/"))
            {
                normalized = normalized.Substring(0, normalized.Length - 1);
            }
            return normalized;
        }

        /// <summary>
        ///     This is an attempt of marrying Unitys nonexisting threading model with asynchronous execution to keep the
        ///     UI responsive.
        /// </summary>
        /// <param name="doWork">the function that actually does work</param>
        /// <returns>a deferred execution object which can be used to see the progress and cancel the execution</returns>
        private static UTDeferredExecution ExecuteInBackground(DoWorkEventHandler doWork)
        {
            var currentWorker = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            var result = new UTDeferredExecution(currentWorker);

            currentWorker.RunWorkerCompleted += (sender, args) =>
            {
                currentWorker.Dispose();
                if (args.Error != null)
                {
                    result.FinishedWithError(args.Error.Message);
                }
                else
                {
                    result.FinishedSuccessfully();
                }
            };

            currentWorker.ProgressChanged += (sender, args) => result.UpdateProgress((string) args.UserState, args.ProgressPercentage/100f);

            currentWorker.DoWork += doWork;
            currentWorker.RunWorkerAsync();

            return result;
        }

        /// <summary>
        /// This objects reports status from the threaded execution into Unity's non-threaded world. It can also be used to abort
        /// a threaded execution.
        /// </summary>
        public class UTDeferredExecution
        {
            private readonly BackgroundWorker backgroundWorker;
            private string errorMessage;
            private bool finished;
            private float progress;
            private string status;
            private bool successful;

            public UTDeferredExecution(BackgroundWorker backgroundWorker)
            {
                this.backgroundWorker = backgroundWorker;
            }

            public bool Finished
            {
                get
                {
                    lock (this)
                    {
                        return finished;
                    }
                }
                private set
                {
                    lock (this)
                    {
                        finished = value;
                    }
                }
            }

            public bool Successful
            {
                get
                {
                    lock (this)
                    {
                        return successful;
                    }
                }
                private set
                {
                    lock (this)
                    {
                        successful = value;
                    }
                }
            }

            public string ErrorMessage
            {
                get
                {
                    lock (this)
                    {
                        return errorMessage;
                    }
                }
                private set
                {
                    lock (this)
                    {
                        errorMessage = value;
                    }
                }
            }

            public string Status
            {
                get
                {
                    lock (this)
                    {
                        return status;
                    }
                }
                private set
                {
                    lock (this)
                    {
                        status = value;
                    }
                }
            }

            public float Progress
            {
                get
                {
                    lock (this)
                    {
                        return progress;
                    }
                }
                private set
                {
                    lock (this)
                    {
                        progress = value;
                    }
                }
            }

            public void UpdateProgress(string status, float progress)
            {
                lock (this)
                {
                    Status = status;
                    Progress = progress;
                }
            }

            public void Cancel()
            {
                backgroundWorker.CancelAsync();
            }

            public void FinishedSuccessfully()
            {
                lock (this)
                {
                    Successful = true;
                    Finished = true;
                    ErrorMessage = null;
                }
            }

            public void FinishedWithError(string error)
            {
                lock (this)
                {
                    ErrorMessage = error;
                    Successful = false;
                    Finished = true;
                }
            }
        }
    }
}
