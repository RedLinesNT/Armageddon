using UnityEngine;
using UnityEditor;

namespace Armageddon.Editor.Drawers {
    
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))] public class ReadOnlyPropertyDrawer : PropertyDrawer {
        
        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) {
            return EditorGUI.GetPropertyHeight(_property, _label, true);
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
            GUI.enabled = false;
            EditorGUI.PropertyField(_position, _property, _label, true);
            GUI.enabled = true;
        }
        
    }
    
}