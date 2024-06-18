using Armageddon.Serializer;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Armageddon.Editor.Extensions;
using static UnityEditor.EditorGUIUtility;

namespace Armageddon.Editor.Serializer {

    [CustomPropertyDrawer(typeof(ShowSerializeReferenceAttribute))] public class ShowSerializeReferenceDrawer : PropertyDrawer {
        
        private static (Rect label, Rect property) LabelPositions(Rect position) {
            Rect _label = new Rect(position.x, position.y, labelWidth, singleLineHeight);
            Rect _property = new Rect(position.x + labelWidth + 2f, position.y, position.width - labelWidth - 2f, singleLineHeight);

            return (_label, _property);
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
            GUIEvent _currentEvent = GUIEvent.FromCurrentUnityEvent;
            _currentEvent = OnGUI(_position, _property, _label, _currentEvent, fieldInfo);
            
            if (_currentEvent.Type == EventType.Used) {
                Event.current.Use();
            }
        }

        public static GUIEvent OnGUI(Rect _position, SerializedProperty _property, GUIContent _label, GUIEvent _currentEvent, FieldInfo _fieldInfo) {
            (Rect _labelPosition, Rect _propertyPosition) = LabelPositions(_position);

            Lazy<IEnumerable<Type>> _selectableTypes = new Lazy<IEnumerable<Type>>(() => GetSelectableTypes(_property, _fieldInfo));
            new TypeField(_propertyPosition, _property.GetManagedReferenceValueType(), _selectableTypes, _property.SetManagedReferenceValueToNew, _currentEvent).DrawGui();

            EditorGUI.PropertyField(_position, _property, _label, true);
            return _currentEvent;
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) {
            return EditorGUI.GetPropertyHeight(_property, _label);
        }

        public static IEnumerable<Type> GetSelectableTypes(SerializedProperty _property, FieldInfo _fieldInfo) {
            IEnumerable<Type> _selectableTypes = _property.GetSelectableManagedReferenceValueTypes();
            TypeFilterAttribute _typeFilterAttribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(_fieldInfo, typeof(TypeFilterAttribute));
            if (_typeFilterAttribute != null) {
                object _parentObject = _property.GetValue(p => EnumerableExtensions.SkipLast(p, 1));
                Func<IEnumerable<Type>, IEnumerable<Type>> _filter = _typeFilterAttribute?.GetFilter(_parentObject);
                _selectableTypes = _filter(_selectableTypes);
            }
            return _selectableTypes ?? Enumerable.Empty<Type>();
        }
        
    }

}