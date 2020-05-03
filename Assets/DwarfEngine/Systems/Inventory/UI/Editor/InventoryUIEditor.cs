using UnityEngine;
using UnityEditor;

namespace DwarfEngine
{
    [CustomEditor(typeof(InventoryUI))]
    public class InventoryUIEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            InventoryUI targetObj = (InventoryUI)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Initialize Inventory"))
            {
                targetObj.SetSlots();
                AssetDatabase.Refresh();
            }
        }
    }
}
