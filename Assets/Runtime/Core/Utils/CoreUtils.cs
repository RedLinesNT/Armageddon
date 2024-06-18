using System;
using UnityEngine;

namespace Armageddon.Utils {
    
    /// <summary>
    /// Utilitairy methods for the Unity infrastructure or even Excalibur.
    /// </summary>
    public static class CoreUtils {
        
        /// <summary>
        /// Returns a generated Universally Unique Identifier (<see cref="Guid"/>) as a string
        /// </summary>
        public static string GenerateUUID() {
            return Guid.NewGuid().ToString();
        }
        
        /// <summary>
        /// Activates-Deactivates a array of <see cref="GameObject"/>s, depending on the value true-false.
        /// </summary>
        public static void SetActive(this GameObject[] _objects, bool _value) {
            for (int i=0; i<_objects.Length; i++) {
                _objects[i].SetActive(_value);
            }
        }
        
    }
    
}