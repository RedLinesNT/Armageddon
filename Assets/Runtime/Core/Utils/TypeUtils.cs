using System;

namespace Armageddon.Utils {
    
    public static class TypeUtils {
        
        /// <summary>
        /// Is the type of obj numeric?
        /// </summary>
        public static bool IsNumericType(this object _obj)  {
            switch (Type.GetTypeCode(_obj.GetType())) {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Is the type numeric?
        /// </summary>
        public static bool IsNumericType(this Type _type) {
            switch (Type.GetTypeCode(_type)) {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                default:
                    return false;
            }
        }
        
    }
    
}