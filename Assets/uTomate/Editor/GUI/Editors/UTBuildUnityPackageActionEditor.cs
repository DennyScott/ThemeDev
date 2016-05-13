//
// Copyright (c) 2013-2016 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

namespace AncientLightStudios.uTomate
{
    using API;
    using UnityEditor;

    [CustomEditor(typeof(UTBuildUnityPackageAction))]
    public class UTBuildUnityPackageActionEditor : UTInspectorBase
    {
        public override UTVisibilityDecision IsVisible(System.Reflection.FieldInfo fieldInfo)
        {
            switch (fieldInfo.Name)
            {
                case "settingsIncludes":
                case "settingsExcludes":
                    var projectSettingsMode = ((UTBuildUnityPackageAction)target).includeProjectSettings;
                    if (projectSettingsMode.UseExpression || projectSettingsMode.Value == UTBuildUnityPackageAction.IncludeProjectSettingsMode.Some)
                    {
                        return UTVisibilityDecision.Visible;
                    }
                    else
                    {
                        return UTVisibilityDecision.Invisible;
                    }
            }
            return base.IsVisible(fieldInfo);
        }
    }
}
