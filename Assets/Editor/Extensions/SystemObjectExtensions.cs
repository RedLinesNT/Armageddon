using System;
using System.Reflection;

namespace Armageddon.Editor.Extensions {

    public static class SystemObjectExtensions {
        
        private const BindingFlags FieldFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        private const BindingFlags PropertyFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        public static FieldInfo GetFieldInfo(this object _target, string _propertyStr) {
            return _target.GetType().GetField(_propertyStr, FieldFlags);
        }
        
        public static object GetFieldValue(this object _target, string _fieldName) {
            if (_target == null) throw new ArgumentNullException(nameof(_target));
            if (string.IsNullOrWhiteSpace(_fieldName)) throw new ArgumentException("field cannot be null or whitespace", nameof(_fieldName));

            FieldInfo _field = _target.GetFieldInfo(_fieldName);
            if (_field != null) return _field.GetValue(_target);

            PropertyInfo _property = _target.GetPropertyInfo(_fieldName);
            if (_property != null) return _property.GetValue(_target);

            return null;
        }

        public static PropertyInfo GetPropertyInfo(this object _target, string _propertyStr) {
            return _target.GetType().GetProperty(_propertyStr, PropertyFlags);
        }
        
    }

}