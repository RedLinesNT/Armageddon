using System;
using System.Linq;

namespace Armageddon.Editor.Extensions {

    public static class TypeExtensions {
        
        public static T GetCustomAttribute<T>(this Type _type) where T : Attribute {
            if (_type == null) throw new ArgumentNullException(nameof(_type));

            return _type.GetCustomAttributes(typeof(T), true).Select(_attr => (T)_attr).FirstOrDefault();
        }
        
        public static bool GetTypeFromManagedReferenceFullTypename(string _managedReferenceFullTypename, out Type _type) {
            _type = null;

            string[] _parts = _managedReferenceFullTypename.Split(' ');
            
            if (_parts.Length == 2) {
                string _assemblyPart = _parts[0];
                string _nsClassnamePart = _parts[1];
                _type = Type.GetType($"{_nsClassnamePart}, {_assemblyPart}");
            }

            return _type != null;
        }
        
        public static bool IsSerializeReferenceable(this Type _type) {
            return !_type.IsGenericType && !_type.IsAbstract && !_type.IsInterface && Attribute.IsDefined(_type, typeof(SerializableAttribute)) && !typeof(UnityEngine.Object).IsAssignableFrom(_type);
        }
        
    }

}