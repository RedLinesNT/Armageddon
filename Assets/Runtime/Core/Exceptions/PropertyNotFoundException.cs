using System;

namespace Armageddon.Exceptions {
    
    /// <summary>
    /// An exception that is thrown whenever a property was not found inside of an object when using Reflection.
    /// </summary>
    [Serializable] public class PropertyNotFoundException : Exception {
        
        public PropertyNotFoundException() { }
        public PropertyNotFoundException(string _message) : base(_message) { }
        public PropertyNotFoundException(string _message, Exception _inner) : base(_message, _inner) { }
        protected PropertyNotFoundException(System.Runtime.Serialization.SerializationInfo _info, System.Runtime.Serialization.StreamingContext _context) : base(_info, _context) { }
        
    }
    
}