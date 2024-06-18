using System;

namespace Armageddon.Exceptions {
    
    /// <summary>
    /// An exception that is thrown whenever a field or a property was not found inside of an object when using Reflection.
    /// </summary>
    [Serializable] public class PropertyOrFieldNotFoundException : Exception {
        
        public PropertyOrFieldNotFoundException() { }
        public PropertyOrFieldNotFoundException(string _message) : base(_message) { }
        public PropertyOrFieldNotFoundException(string _message, Exception _inner) : base(_message, _inner) { }
        protected PropertyOrFieldNotFoundException(System.Runtime.Serialization.SerializationInfo _info, System.Runtime.Serialization.StreamingContext _context) : base(_info, _context) { }
        
    }
    
}