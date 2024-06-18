using System;
using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEditor;
using System.Linq;
#endif

namespace Armageddon.Serializer {
    
    public class SerializableType : ISerializationCallbackReceiver {

        #region Attributes

        [SerializeField] public string typeID = string.Empty;
        
        #endregion

        #region Properties

        public string TypeNotFoundError { get { return $"Could not find type for typeId[{typeID}] when trying to deserialize this SerializableType."; } }
        public Type Type { get; set; }

#if UNITY_EDITOR
        public static bool IsBuilding { get; protected set; }
#endif
        
        #endregion

        #region SerializableType's Constructor Methods

        public SerializableType() {}
        
        public SerializableType(Type _type) {
            Type = _type;
        }

        #endregion
        
        #region ISerializationCallbackReceiver's Internal Methods

        public void OnBeforeSerialize() {
            string _value = ToSerializedType(Type);
            if (!string.IsNullOrEmpty(_value)) typeID = _value;
        }
        
        public void OnAfterDeserialize() {
            if (!TryGetType(typeID, out Type _type)) Logger.TraceError(TypeNotFoundError);
            Type = _type;
        }

        #endregion

        #region SerializableType's External Methods

        public static bool TryGetType(string _typeString, out Type _type) {
#if UNITY_EDITOR
            if (Guid.TryParse(_typeString, out Guid _guid)) _type = TypeCache.GetTypesWithAttribute(typeof(GuidAttribute)).FirstOrDefault(_t => _t.GUID == _guid);
            else _type = Type.GetType(_typeString);
#else
            _type = Type.GetType(_typeString);
#endif
            return _type != null || string.IsNullOrEmpty(_typeString);
        }

        public static string ToSerializedType(Type _type) {
            if (_type == null) return string.Empty;
#if UNITY_EDITOR
            if (Attribute.IsDefined(_type, typeof(GuidAttribute)) && !IsBuilding) return _type.GUID.ToString();
            else return _type.AssemblyQualifiedName;
#else
            return _type.AssemblyQualifiedName;
#endif
        }

        #endregion
        
    }
    
}