using System;
using Armageddon.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Armageddon.Editor.Serializer {

    public static class SerializeReferenceContextMenu {
        
        public struct Context {

            #region Properties

            public SerializedProperty Property { get; }
            public Type FieldType { get; }

            #endregion

            #region Context's Constructor Method

            public Context(SerializedProperty _property) {
                Property = _property;
                FieldType = _property.GetManagedReferenceFieldType();
            }

            #endregion

            #region Context's External Methods

            public void AddToMenu(GenericMenu _menu) {
                _menu.AddItem(new GUIContent(CopyContextName), false, Copy);
                bool _canPasteCopiedValue = copiedValue != null && FieldType.IsInstanceOfType(copiedValue);
                
                if (_canPasteCopiedValue) _menu.AddItem(new GUIContent(PasteContextName), false, Paste);
                else _menu.AddDisabledItem(new GUIContent(PasteContextName));
            }

            public void Copy() {
                copiedValue = Property.GetValue();
            }

            public void Paste() {
                Property.managedReferenceValue = copiedValue;
                Property.serializedObject.ApplyModifiedProperties();
            }

            #endregion
            
        }

        #region Constants

        public const string CopyContextName = "Copy Managed Reference";
        public const string PasteContextName = "Paste Managed Reference";
        public static object copiedValue;

        #endregion

        #region SerializeReferenceContextMenu's External Methods

        [InitializeOnLoadMethod] public static void RegisterContextMenuCallback() {
            EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
        }

        #endregion

        #region SerializeReferenceContextMenu's Internal Methods

        private static void OnPropertyContextMenu(GenericMenu _menu, SerializedProperty _property) {
            if (_property.propertyType != SerializedPropertyType.ManagedReference) return;

            Context _context = new Context(_property);
            _context.AddToMenu(_menu);
        }

        #endregion
        
    }

}