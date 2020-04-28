using UnityEditor;
using UnityEngine;

namespace DwarfEngine
{
    [CustomPropertyDrawer(typeof(Stat))]
    public class StatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var valueProp = property.FindPropertyRelative("baseValue");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label); ;
            valueProp.floatValue = EditorGUI.DelayedFloatField(position, valueProp.floatValue);

            EditorGUI.EndProperty();
        }
    }
}
