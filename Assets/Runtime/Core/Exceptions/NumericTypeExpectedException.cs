using System;

namespace Armageddon.Exceptions {
    
    /// <summary>
    /// An exception that is thrown whenever a numeric type is expected as an input somewhere but the input wasn't numeric.
    /// </summary>
    [Serializable] public class NumericTypeExpectedException : Exception {
        
        public NumericTypeExpectedException() { }
        public NumericTypeExpectedException(string _message) : base(_message) { }
        public NumericTypeExpectedException(string _message, Exception _inner) : base(_message, _inner) { }
        
        protected NumericTypeExpectedException(System.Runtime.Serialization.SerializationInfo _info, System.Runtime.Serialization.StreamingContext _context) : base(_info, _context) { }
        
    }
    
}