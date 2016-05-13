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

    [CustomEditor(typeof(UTSaveSceneAction))]
    public class UTSaveSceneActionEditor : UTInspectorBase
    {
        public override UTVisibilityDecision IsVisible(System.Reflection.FieldInfo field)
        {
            if (field.Name == "filename")
            {
                var copy = ((UTSaveSceneAction)target).saveCopy;
                return (copy.UseExpression || copy.Value == true) ?
                    UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
            }
            return base.IsVisible(field);
        }
    }
}
