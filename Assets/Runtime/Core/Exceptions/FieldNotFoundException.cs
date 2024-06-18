using System;

namespace Armageddon.Exceptions {
    
    [Serializable] public class FieldNotFoundException : Exception {
        
        public FieldNotFoundException() { }
        public FieldNotFoundException(string _message) : base(_message) { }
        public FieldNotFoundException(string _message, Exception _inner) : base(_message, _inner) { }
        protected FieldNotFoundException(System.Runtime.Serialization.SerializationInfo _info, System.Runtime.Serialization.StreamingContext _context) : base(_info, _context) { }
        
    }
    
}