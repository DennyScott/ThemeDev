//
// Copyright (c) 2013-2016 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

namespace AncientLightStudios.uTomate
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using API;
    using ThirdParty.MiniJson;
    using UnityEditor;
    using UnityEngine;

    [UTActionInfo(actionCategory = "Import & Export", sinceUTomateVersion="1.6.0")]
    [UTDoc(title = "Upload to HockeyApp", description = "Uploads a build to HockeyApp.")]
    [UTInspectorGroups(groups = new[] {"General", "Files", "Release Settings"})]
    [UTDefaultAction]
    public class UTUploadToHockeyAppAction : UTAction
    {
        private const string HockeyAppUploadUrl = "https://rink.hockeyapp.net/api/2/apps/upload";

        [UTDoc(title = "API Token", description = "The API token you want to use for uploading to HockeyApp.")]
        [UTInspectorHint(displayAs = UTInspectorHint.DisplayAs.Password, required = true, order = 1, group = "General")]
        public UTString apiToken;

        [UTDoc(
            description =
                "Optional property name. If set, the action will put the parsed JSON as a dictionary into the context " +
                "under the given property name. This is useful for further processing, e.g. inviting testers.")]
        [UTInspectorHint(order = 2, group = "General")]
        public UTString resultProperty;

        [UTDoc(description = "The file that should be uploaded. Must be an IPA, APK or ZIP file.")]
        [UTInspectorHint(displayAs = UTInspectorHint.DisplayAs.OpenFileSelect, caption = "Please select file to upload", required = true, order = 1, group = "Files")]
        public UTString fileToUpload;

        [UTDoc(
            description =
                "The file that contains your symbol mappings, if you are using an obfuscator. Note that the extension " +
                "has to be .dsym.zip (case-insensitive) for iOS and OS X and the file name has to be mapping.txt for Android "
            )]
        [UTInspectorHint(displayAs = UTInspectorHint.DisplayAs.OpenFileSelect, caption = "Please select the symbol file to upload", order = 2, group = "Files")]
        public UTString symbolMappingFile;

        [UTDoc(description = "Release notes. Can be in markdown or textile format.")]
        [UTInspectorHint(displayAs = UTInspectorHint.DisplayAs.TextArea, group = "Release Settings", order = 1)]
        public UTString releaseNotes;

        [UTDoc(description = "The format of your release notes.")]
        [UTInspectorHint(group = "Release Settings", order = 2)]
        public UTHockeyAppReleaseNotesFormat releaseNotesFormat;

        [UTDoc(description = "Which group of testers should be notified about the new release?")]
        [UTInspectorHint(group = "Release Settings", order = 3)]
        public UTHockeyAppTesterNotification notifyTesters;

        [UTDoc(description = "Are testers allowed to download this release?")]
        [UTInspectorHint(group = "Release Settings", order = 4)]
        public UTHockeyAppDownloadPermission downloadAllowed;

        [UTDoc(description = "Restrict download to this list of tags. Please add one tag per line.")]
        [UTInspectorHint(group = "Release Settings", order = 5)]
        public UTString[] restrictToTags;

        [UTDoc(description = "Restrict download to this list of team IDs. Please add one team ID per line.")]
        [UTInspectorHint(group = "Release Settings", order = 6)]
        public UTString[] restrictToTeams;

        [UTDoc(description = "Restrict download to this list of user IDs. Please add one user ID per line.")]
        [UTInspectorHint(group = "Release Settings", order = 7)]
        public UTString[] restrictToUsers;

        [UTDoc(description = "Should this version be marked as mandatory?")]
        [UTInspectorHint(group = "Release Settings", order = 8)]
        public UTBool mandatory;

        [UTDoc(description = "The release type of your build.")]
        [UTInspectorHint(group = "Release Settings", order = 9)]
        public UTHockeyAppReleaseType releaseType;

        [UTDoc(description = "Should the private download page be enabled?")]
        [UTInspectorHint(group = "Release Settings", order = 10)]
        public UTBool enablePrivateDownloadPage;

        [UTDoc(title = "Commit SHA", description = "The git commit SHA for this build.")]
        [UTInspectorHint(group = "Release Settings", order = 11)]
        public UTString commitSha;

        [UTDoc(title = "Build job URL", description = "The URL of the build job on your build server.")]
        [UTInspectorHint(group = "Release Settings", order = 12)]
        public UTString buildJobUrl;

        [UTDoc(title = "Repository URL", description = "The URL of your VCS repository.")]
        [UTInspectorHint(group = "Release Settings", order = 13)]
        public UTString repositoryUrl;

        public override IEnumerator Execute(UTContext context)
        {
            var theApiToken = apiToken.EvaluateIn(context);
            if (string.IsNullOrEmpty(theApiToken))
            {
                throw new UTFailBuildException("Please specify an API token for uploading to HockeyApp.", this);
            }

            var theFileToUpload = fileToUpload.EvaluateIn(context);
            if (!File.Exists(theFileToUpload))
            {
                throw new UTFailBuildException("The file '" + theFileToUpload + "' does not exist. Please check if you have set up the correct file to upload.", this);
            }

            var theSymbolMappingFile = symbolMappingFile.EvaluateIn(context);
            if (!string.IsNullOrEmpty(theSymbolMappingFile) && !File.Exists(theSymbolMappingFile))
            {
                throw new UTFailBuildException("The file '" + theSymbolMappingFile + "' does not exist. Please check if you have set up the correct symbol mapping file.", this);
            }

            var theResultProperty = resultProperty.EvaluateIn(context);
            if (!string.IsNullOrEmpty(theResultProperty) && !UTContext.IsValidPropertyName(theResultProperty))
            {
                throw new UTFailBuildException("The name '" + theResultProperty + "' is no valid property name.", this);
            }


            var wwwForm = new WWWForm();

            var fileContents = File.ReadAllBytes(theFileToUpload);
            yield return null; // give the editor a break

            wwwForm.AddBinaryData("ipa", fileContents, Path.GetFileName(theFileToUpload));
            yield return null; // give the editor another break


            if (!string.IsNullOrEmpty(theSymbolMappingFile))
            {
                var symbolFileContents = File.ReadAllBytes(theSymbolMappingFile);
                yield return null;
                wwwForm.AddBinaryData("dsym", symbolFileContents);
                yield return null;
            }

            var theReleaseNotes = releaseNotes.EvaluateIn(context);
            if (!string.IsNullOrEmpty(theReleaseNotes))
            {
                wwwForm.AddField("notes", theReleaseNotes);

                var theReleaseNotesFormat = releaseNotesFormat.EvaluateIn(context);
                wwwForm.AddField("notes_type", theReleaseNotesFormat == ReleaseNotesFormat.Markdown ? "1" : "0");
            }

            var theTesterNotification = notifyTesters.EvaluateIn(context);
            if (theTesterNotification != TesterNotification.Default)
            {
                var testerNotificationValue =
                    theTesterNotification == TesterNotification.NoNotification
                        ? "0"
                        : theTesterNotification == TesterNotification.NotifyAllTesters ? "2" : "1";
                wwwForm.AddField("notify", testerNotificationValue);
            }

            var downloadPermission = downloadAllowed.EvaluateIn(context);
            if (downloadPermission != DownloadPermission.Default)
            {
                wwwForm.AddField("status", downloadPermission == DownloadPermission.AllowDownload ? "2" : "1");
            }

            // ReSharper disable once CoVariantArrayConversion
            var theRestrictToTags = EvaluateAll(restrictToTags, context);
            if (theRestrictToTags.Length > 0)
            {
                wwwForm.AddField("tags", string.Join(",", theRestrictToTags));
            }

            // ReSharper disable once CoVariantArrayConversion
            var theRestrictToTeams = EvaluateAll(restrictToTeams, context);
            if (theRestrictToTeams.Length > 0)
            {
                wwwForm.AddField("teams", string.Join(",", theRestrictToTeams));
            }

            // ReSharper disable once CoVariantArrayConversion
            var theRestrictToUsers = EvaluateAll(restrictToUsers, context);
            if (theRestrictToUsers.Length > 0)
            {
                wwwForm.AddField("users", string.Join(",", theRestrictToUsers));
            }

            var isMandatory = mandatory.EvaluateIn(context);
            wwwForm.AddField("mandatory", isMandatory ? "1" : "0");

            var theReleaseType = releaseType.EvaluateIn(context);
            wwwForm.AddField("release_type", theReleaseType == ReleaseType.Alpha ? "2" : 
                theReleaseType == ReleaseType.ForEnterprise ? "3" : 
                theReleaseType == ReleaseType.ForStore ? "1" : "0");

            var isEnablePrivateDownloadPage = enablePrivateDownloadPage.EvaluateIn(context);
            wwwForm.AddField("private", isEnablePrivateDownloadPage ? "true" : "false");

            var theCommitSha = commitSha.EvaluateIn(context);
            if (!string.IsNullOrEmpty(theCommitSha))
            {
                wwwForm.AddField("commit_sha", theCommitSha);
            }

            var theBuildJobUrl = buildJobUrl.EvaluateIn(context);
            if (!string.IsNullOrEmpty(theBuildJobUrl))
            {
                wwwForm.AddField("build_server_url", theBuildJobUrl);
            }

            var theRepositoryUrl = repositoryUrl.EvaluateIn(context);
            if (!string.IsNullOrEmpty(theRepositoryUrl))
            {
                wwwForm.AddField("repository_url", theRepositoryUrl);
            }


            if (UTPreferences.DebugMode)
            {
                Debug.Log("Uploading build: " + theFileToUpload + " (" + fileContents.Length + " bytes) to HockeyApp.");
            }
            var headers = wwwForm.headers;
            headers.Add("X-HockeyAppToken", theApiToken);

            // workaround around HockeyApp middleware issue see
            // http://support.hockeyapp.net/discussions/problems/37308-unable-to-upload-an-apk-file-using-the-api
            headers["Content-Type"] = headers["Content-Type"].Replace("\"", "");

            using (var www = new WWW(HockeyAppUploadUrl, wwwForm.data, headers))
            {
                try
                {
                    do
                    {
                        UTils.ShowAsyncProgressBar(string.Format("Uploading to HockeyApp: {0:P2}", www.uploadProgress), www.uploadProgress);
                        // upload in background
                        yield return "";
                    } while (!www.isDone && !context.CancelRequested);

                    if (context.CancelRequested)
                    {
                        yield break;
                    }
                }
                finally
                {
                    UTils.ClearAsyncProgressBar();
                }


                if (UTPreferences.DebugMode)
                {
                    Debug.Log("Server Response: " + www.text);
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new UTFailBuildException("Upload failed. Reason: '" + www.error + "'.", this);
                }

                var result = Json.Deserialize(www.text) as Dictionary<string, object>;
                if (result == null)
                {
                    throw new UTFailBuildException("Unable to parse response from HockeyApp.", this);
                }

                if (result.ContainsKey("errors"))
                {
                    throw new UTFailBuildException("HockeyApp reported an error." + (UTPreferences.DebugMode ? "" : " Please enable debug mode for details."), this);
                }

                if (result.ContainsKey("status") && (string) result["status"] == "error")
                {
                    throw new UTFailBuildException("HockeyApp reported an error. Message: " + result["message"], this);
                }

                Debug.Log("Successfully uploaded '" + theFileToUpload + "' to HockeyApp.", this);

                if (!string.IsNullOrEmpty(theResultProperty))
                {
                    if (UTPreferences.DebugMode)
                    {
                        Debug.Log("Setting result property '" + theResultProperty + "'.", this);
                    }
                    context[theResultProperty] = result;
                }
            }
        }


        [MenuItem("Assets/Create/uTomate/Import + Export/Upload to HockeyApp", false, 271)]
        public static void AddAction()
        {
            Create<UTUploadToHockeyAppAction>();
        }

        public enum ReleaseNotesFormat
        {
            Markdown,
            Textile
        }

        public enum TesterNotification
        {
            Default,
            NoNotification,
            NotifyTestersWithAccess,
            NotifyAllTesters
        }

        public enum DownloadPermission
        {
            Default,
            AllowDownload,
            DisallowDownload
        }

        public enum ReleaseType
        {
            Beta,
            Alpha,
            ForStore,
            ForEnterprise
        }
    }
}
