using UnityEngine;
using UnityEditor;
using DwarfEngine;

namespace DwarfEngine
{
    [CustomEditor(typeof(SpriteAnimator))]
    public class SpriteAnimatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
