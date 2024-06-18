using System;
using UnityEditor;
using UnityEngine;
using static Armageddon.Serializer.SerializableType;
using static Armageddon.Editor.Serializer.SerializableTypeMeta;
using System.Runtime.InteropServices;
using UnityEditor.Compilation;
using System.Collections.Generic;
using System.Linq;
using Armageddon.Serializer;
using static Armageddon.Editor.Extensions.SerializedPropertyExtensions;

namespace Armageddon.Editor.Serializer {
    
    [CustomPropertyDrawer(typeof(SerializableType))] public class SerializableTypeDrawer : PropertyDrawer {
        
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
            SerializedProperty _typeIdProperty = _property.FindPropertyRelative(TypeIdProperty);
            Type _type = Validate(_typeIdProperty.stringValue, _label);

            _position = EditorGUI.PrefixLabel(_position, _label);
            TypeFilterAttribute typeFilterAttribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));
            Lazy<IEnumerable<Type>> selectableTypes = new Lazy<IEnumerable<Type>>(() => GetFilteredTypes(_property, typeFilterAttribute).ToArray());
            new TypeField(_position, _type, selectableTypes, SetTypeValue(_typeIdProperty)).DrawGui();
        }

        public static Type Validate(string _typeId, GUIContent _label) {
            bool _typeReferenceExists = TryGetType(_typeId, out Type _type);
            if (!_typeReferenceExists) {
                _label.image = EditorGUIUtility.IconContent("console.erroricon").image;
                _label.tooltip = $"Type reference could not be found for the typeId, \"{_typeId}\". This can happen when renaming a type without a GuidAttribute defined.";
            } else if (IsMissingGuidAttribute(_type)) {
                _label.image = EditorGUIUtility.IconContent("console.warnicon").image;
                _label.tooltip = "The current type doesn't have a GuidAttribute defined. Renaming this type will cause it to lose its reference!";
            }
            return _type;
        }

        public static bool IsMissingGuidAttribute(Type _type) {
            return _type != null && !Attribute.IsDefined(_type, typeof(GuidAttribute));
        }

        public static IEnumerable<Type> GetFilteredTypes(SerializedProperty _property, PropertyAttribute _propertyAttribute) {
            (Type _baseType, Func<IEnumerable<Type>, IEnumerable<Type>> _filter) = GetTypeFilter(_property, _propertyAttribute);
            return _filter.Invoke(GetDerivedTypes(_baseType)) ?? new Type[0];
        }

        public static (Type baseType, Func<IEnumerable<Type>, IEnumerable<Type>> filter) GetTypeFilter(SerializedProperty _property, PropertyAttribute _propertyAttribute) {
            if (_propertyAttribute is TypeFilterAttribute _t) {
                var _parentObject = _property.GetValue(p => p.SkipLast(1));
                return (_t.DerivedFrom, _t.GetFilter(_parentObject));
            }
            return (null, _sequence => _sequence);
        }

        public static IEnumerable<Type> GetDerivedTypes(Type _baseType) {
            IEnumerable<Type> _derivedTypes = _baseType != null ? TypeCache.GetTypesDerivedFrom(_baseType) : CompilationPipeline.GetAssemblies().Select(_assembly => _assembly.name).Select(System.Reflection.Assembly.Load).SelectMany(_a => _a.GetExportedTypes());
            return _derivedTypes.Where(_type => !_type.IsGenericType);
        }

        public static Action<Type> SetTypeValue(SerializedProperty _property) {
            return _type => {
                string _value = ToSerializedType(_type);
                if (_property.stringValue != _value) {
                    _property.stringValue = _value;
                    _property.serializedObject.ApplyModifiedProperties();
                }
            };
        }
    }

    public class SerializableTypeMeta : SerializableType {
        public static string TypeIdProperty = nameof(typeID);
    }
    
}