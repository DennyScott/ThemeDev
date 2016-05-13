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
    using UnityEngine;

    [UTPropertyRenderer(typeof(UTRect), typeof(Rect))]
    public class UTRectPropertyRenderer : UTIPropertyRenderer
    {
        public void Render(UTFieldWrapper fieldWrapper)
        {
            EditorGUILayout.BeginHorizontal();
            if (fieldWrapper.Label != null)
            {
                EditorGUILayout.PrefixLabel(fieldWrapper.Label);
            }
            else
            {
                EditorGUILayout.PrefixLabel(" ");
            }

            Rect val = (Rect)fieldWrapper.Value;
            GUILayout.Label("X");
            val.x = EditorGUILayout.FloatField(val.x);
            GUILayout.Label("Y");
            val.y = EditorGUILayout.FloatField(val.y);
            GUILayout.Label("W");
            val.width = EditorGUILayout.FloatField(val.width);
            GUILayout.Label("H");
            val.height = EditorGUILayout.FloatField(val.height);
            fieldWrapper.Value = val;
            EditorGUILayout.EndHorizontal();
        }
    }
}
