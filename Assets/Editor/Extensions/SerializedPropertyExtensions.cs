using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using static UnityEditor.TypeCache;

namespace Armageddon.Editor.Extensions {

    public static class SerializedPropertyExtensions {
        
        public static Type GetManagedReferenceFieldType(this SerializedProperty _property) {
            return TypeExtensions.GetTypeFromManagedReferenceFullTypename(_property.managedReferenceFieldTypename, out var _type) ? _type : null;
        }
        
        public static Type GetManagedReferenceValueType(this SerializedProperty _property) {
            return TypeExtensions.GetTypeFromManagedReferenceFullTypename(_property.managedReferenceFullTypename, out var _type) ? _type : null;
        }
        
        public static Type[] GetSelectableManagedReferenceValueTypes(this SerializedProperty _property) {
            Type _baseType = _property.GetManagedReferenceFieldType();
            if (_baseType == null) throw new ArgumentException(nameof(_property));

            return GetTypesDerivedFrom(_baseType).Prepend(_baseType).Where(TypeExtensions.IsSerializeReferenceable).ToArray();
        }
        
        public static object GetValue(this SerializedProperty _property, Func<IEnumerable<string>, IEnumerable<string>> _pathModifier = null) {
            IEnumerable<string> _path = _property.propertyPath.Replace("Array.data[", "[").Split('.');
            if (_pathModifier != null) _path = _pathModifier(_path);

            object _target = (object)_property.serializedObject.targetObject;
            return _target.GetValueRecur(_path);
        }

        private static object GetValueRecur(this object _target, IEnumerable<string> _propertyPath) {
            if (_target == null) throw new ArgumentNullException(nameof(_target));

            string _propertyStr = _propertyPath.FirstOrDefault();
            if (_propertyStr == null) return _target;

            _target = _propertyStr.TryGetArrayIndex(out int _index) ? (_target as IEnumerable).ElementAtOrDefault(_index) : _target.GetFieldValue(_propertyStr);

            return _target.GetValueRecur(_propertyPath.Skip(1));
        }
        
        public static void SetManagedReferenceValueToNew(this SerializedProperty _property, Type _type) {
            _property.managedReferenceValue = _type != null ? Activator.CreateInstance(_type) : null;
            SerializedObject _so = _property.serializedObject;
            _so.ApplyModifiedProperties();
        }
        
    }

}