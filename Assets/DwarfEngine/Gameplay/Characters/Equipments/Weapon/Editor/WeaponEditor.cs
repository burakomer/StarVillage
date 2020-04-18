using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace DwarfEngine
{
    [CustomEditor(typeof(Weapon), true)]
    public class WeaponEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Component", EditorStyles.miniButton))
            {
                var menu = new GenericMenu();

                Assembly asm = typeof(WeaponComponent).Assembly;
                var typeMap = asm.GetTypes().Where(t => (t.BaseType == typeof(WeaponComponent)) && (!t.IsAbstract));

                foreach (Type type in typeMap)
                {
                    GUIContent title = new GUIContent(ObjectNames.NicifyVariableName(type.ToString().Substring(type.ToString().IndexOf(".") + 1)));
                    bool exists = ((Weapon)target).GetComponent(type) != null;

                    if (!exists)
                        menu.AddItem(title, false, () => ((Weapon)target).gameObject.AddComponent(type));
                    else
                        menu.AddDisabledItem(title);
                }

                menu.ShowAsContext();
            }

            if (GUILayout.Button("Add Processor", EditorStyles.miniButton))
            {
                var menu = new GenericMenu();

                Assembly asm = typeof(WeaponComponent).Assembly;
                var typeMap = asm.GetTypes().Where(t => (t.BaseType == typeof(WeaponProcessor) || t.BaseType == typeof(WeaponProcessor)) && (!t.IsAbstract));

                foreach (Type type in typeMap)
                {
                    GUIContent title = new GUIContent(ObjectNames.NicifyVariableName(type.ToString().Substring(type.ToString().IndexOf(".") + 1)));
                    bool exists = ((Weapon)target).GetComponent<WeaponProcessor>() != null;

                    if (!exists)
                        menu.AddItem(title, false, () => ((Weapon)target).gameObject.AddComponent(type));
                    else
                        menu.AddDisabledItem(title);
                }

                menu.ShowAsContext();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
