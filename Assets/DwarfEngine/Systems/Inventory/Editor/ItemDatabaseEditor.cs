using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DwarfEngine
{
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ItemDatabase catalogObject = (ItemDatabase)target;
            if (GUILayout.Button("Set IDs"))
            {
                catalogObject.SetItemIds();
                foreach (var entry in catalogObject.catalog)
                {
                    EditorUtility.SetDirty(entry.item);
                }
                EditorUtility.SetDirty(catalogObject);
            }
        }
    }
}