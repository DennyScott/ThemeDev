//
// Copyright (c) 2013-2016 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

namespace AncientLightStudios.uTomate
{
    using API;
    using System;
    using System.IO;
    using UnityEditor;

    [UTDoc(title = "Touch files", description = "Changes the modification date of the given files to the current date.")]
    public class UTTouchFileAction : UTAction
    {
        [UTDoc(title = "Base Folder", description = "Base folder where the files should be collected from. If empty, files will be collected from the project's assets folder.")]
        [UTInspectorHint(order = 0, required = false, displayAs = UTInspectorHint.DisplayAs.FolderSelect, caption = "Select base folder for touching files")]
        public UTString baseDirectory;

        [UTDoc(description = "Files to include.")]
        [UTInspectorHint(order = 1)]
        public UTString[] includes;

        [UTDoc(description = "Files to exclude.")]
        [UTInspectorHint(order = 2)]
        public UTString[] excludes;

        public override System.Collections.IEnumerator Execute(UTContext context)
        {
            var theBaseDirectory = baseDirectory.EvaluateIn(context);
            if (string.IsNullOrEmpty(theBaseDirectory))
            {
                theBaseDirectory = UTFileUtils.ProjectAssets;
            }

            if (!Directory.Exists(theBaseDirectory))
            {
                throw new UTFailBuildException("The base directory " + theBaseDirectory + " does not exist.", this);
            }

            theBaseDirectory = UTFileUtils.NormalizeSlashes(theBaseDirectory);
            var theIncludes = EvaluateAll(includes, context);
            var theExcludes = EvaluateAll(excludes, context);

            var theFiles = UTFileUtils.CalculateFileset(theBaseDirectory, theIncludes, theExcludes, UTFileUtils.FileSelectionMode.Files);

            var now = DateTime.Now;
            foreach (var file in theFiles)
            {
                FileInfo src = new FileInfo(file);
                src.LastWriteTime = now;
                yield return "";
            }
        }

        [MenuItem("Assets/Create/uTomate/Files + Folders/Touch Files", false, 235)]
        public static void AddAction()
        {
            Create<UTTouchFileAction>();
        }
    }
}
