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

    public abstract class UTSetPlayerSettingsBaseActionEditor : UTInspectorBase
    {
        public override UTVisibilityDecision IsVisible(System.Reflection.FieldInfo fieldInfo)
        {
            var self = (UTSetPlayerSettingsActionBase)target;


            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (fieldInfo.Name)
            {
#if !UNITY_5_0 // VR: 5.1
                case "splashScreenStyle":
                    return VisibleIf(self.showUnitySplashScreen.HasValueOrExpression(true));
#endif
                case "allowPortrait":
                case "allowPortraitUpsideDown":
                case "allowLandscapeRight":
                case "allowLandscapeLeft":
                    return VisibleIf(self.HasAutorotation && self.defaultOrientation.HasValueOrExpression(UIOrientation.AutoRotation));
                case "defaultOrientation":
                    return VisibleIf(self.HasAutorotation);
                    
            }

            return base.IsVisible(fieldInfo);
        }
    }
}
