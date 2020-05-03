using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DwarfEngine
{
    [CustomEditor(typeof(Inventory), true)]
    public class InventoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Inventory inventoryObject = (Inventory)target;
            if (GUILayout.Button("Save Inventory"))
            {
                inventoryObject.OnSaveInitiated();
            }
        }
    }
}