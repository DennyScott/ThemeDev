//
// Copyright (c) 2013-2016 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

namespace AncientLightStudios.uTomate
{
    using UnityEngine;

    /// <summary>
    /// Exception being thrown to signal that the build should be aborted.
    /// </summary>
    public class UTFailBuildException : UnityException
    {
        private UnityEngine.Object context;

        public UTFailBuildException(string message, UnityEngine.Object context)
            : base(message)
        {
            this.context = context;
        }

        public void LogToConsole()
        {
            Debug.LogError(Message, context);
        }
    }
}
