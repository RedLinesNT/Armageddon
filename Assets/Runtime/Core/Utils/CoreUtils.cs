using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public static T PickRandom<T>(this IEnumerable<T> _source) {
            return _source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> _source, int _count) {
            return _source.Shuffle().Take(_count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> _source) {
            return _source.OrderBy(_x => GenerateUUID());
        }
        
    }
    
}