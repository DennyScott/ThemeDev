//
// Copyright (c) 2013-2016 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

namespace AncientLightStudios.uTomate.API
{
using System;
    using System.Reflection;

    /// <summary>
    /// Annotation for controlling extended behaviour of the inspector for action properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class UTInspectorHint : System.Attribute
    {
        /// <summary>
        /// Required property indicator. If set to true, the property will be marked with an asterisk in the inspector.
        /// </summary>
        public bool required = false;

        /// <summary>
        /// Indicator for array properties. When set to true, the annotated array needs at least one item.
        /// </summary>
        public bool arrayNotEmpty = false;

        /// <summary>
        /// Indicator for enum properties. If true, multiple enum values can be selected and the combined value
        /// will be returned. Otherwise only a single enum element can be selected.
        /// </summary>
        public bool multiSelect = false;

        /// <summary>
        /// Allows limiting the allowed values. Must be strings, but will be coerced to the target type.
        /// </summary>
        public string[] allowedValues = null;

        /// <summary>
        /// Modifies the way in which the property is rendered in the inspector.
        /// </summary>
        /// <see cref="UTInspectorHint.DisplayAs"/>
        public DisplayAs displayAs = DisplayAs.Default;

        /// <summary>
        /// The min value of the slider. Only relevant, if property is rendered as a slider, otherwise ignored.
        /// </summary>
        public float minValue = 0;

        /// <summary>
        /// The max value of the slider. Only relevant, if property is rendered as a slider, otherwise ignored.
        /// </summary>
        public float maxValue = 0;

        /// <summary>
        /// The caption of file selection dialogs. Only relevant, if property is rendered as any kind of file selector. Otherwise ignored.
        /// </summary>
        public string caption = "";

        /// <summary>
        /// The captions of vector, rect and enum elements as well as fixed-length arrays. If null, the default captions will be used.
        /// </summary>
        public string[] captions = null;

        /// <summary>
        /// The extension which should be filtered for in file selection dialogs. Only relevant, if property is rendered as any kind of file selector. Otherwise ignored.
        /// </summary>
        public string extension = "";

        /// <summary>
        /// The group in which the property should be placed. If not set, the property will be placed below all groups.
        /// </summary>
        public string group = "";

        /// <summary>
        /// The order in which the property should be placed. If not set, the property will be ordered by it's name.
        /// </summary>
        public int order = int.MaxValue;

        /// <summary>
        /// The number of lines of the text area. Only relevant, if property is rendered as text area, otherwise ignored.
        /// </summary>
        public int lines = 5;

        /// <summary>
        /// The indent level.
        /// </summary>
        public int indentLevel = 0;

        /// <summary>
        /// The base value type of a property. Only relevant for properties of type <see cref="UTType"/> or <see cref="Type"/>
        /// </summary>
        public Type baseType = typeof(object);

        /// <summary>
        /// Whether the property may contain expressions or not. Necessary to supress warnings etc.
        /// </summary>
        public bool containsExpression = false;

        /// <summary>
        /// Allows to specify a fixed length for array members. Ignored on other members.
        /// </summary>
        public int fixedLength = -1;

        /// <summary>
        /// Allows to specify a sub section header that is rendered directly before the field.
        /// </summary>
        public string subSectionHeader = "";

        private static UTInspectorHint Default = new UTInspectorHint();

        /// <summary>
        /// Returns the caption that is specified at the given index. If no caption is specified at that index
        /// returns the default caption.
        /// </summary>
        /// <param name="index">the index within the captions list</param>
        /// <param name="defaultValue">the default value to be used</param>
        /// <returns>the matching caption or the default value if there is no matching caption.</returns>
        public string GetCaptionAtIndex(int index, string defaultValue)
        {
            if (captions != null && captions.Length >= index)
            {
                return captions[index];
            }
            return defaultValue;
        }

        /// <summary>
        /// Convenience getter checking whether or not a fixed array length is set.
        /// </summary>
        public bool HasFixedLength
        {
            get { return fixedLength >= 0;  }
        }

        /// <summary>
        /// Enum for controlling various ways of displaying an action property in the inspector.
        /// </summary>
        public enum DisplayAs
        {
            /// <summary>
            /// The property will be displayed according to it's type. E.g. a bool will be displayed as checkbox, string as
            /// a text field, etc.
            /// </summary>
            Default,

            /// <summary>
            /// The property will be displayed as a slider. Only applicable for float, int, UTFloat and UTInt properties.
            /// </summary>
            Slider,

            /// <summary>
            /// The property will be displayed as a folder selector. Only applicable for string properties.
            /// </summary>
            FolderSelect,

            /// <summary>
            /// The property will be displayed as a file open selector. Only applicable for string properties.
            /// </summary>
            OpenFileSelect,

            /// <summary>
            /// The property will be displayed as a save file selector. Only applicable for string properties.
            /// </summary>

            SaveFileSelect,

            /// <summary>
            /// The property will be displayed as a text area. Only applicable for string properties.
            /// </summary>
            TextArea,

            /// <summary>
            /// The property will be displayed as a password field. Only applicable for string properties.
            /// </summary>
            Password
        }

        /// <summary>
        /// Gets the inspector hint annotation attached to a field. If no inspector hint is attached, a default hint is returned.
        /// </summary>
        /// <returns>
        /// The attached inspector hint.
        /// </returns>
        /// <param name='info'>
        /// The field information object.
        /// </param>
        public static UTInspectorHint GetFor(FieldInfo info)
        {
            var hints = info.GetCustomAttributes(typeof(UTInspectorHint), true);
            if (hints.Length > 0)
            {
                var hint = (UTInspectorHint)hints[0];
                return hint;
            }
            return Default;
        }
    }
}
