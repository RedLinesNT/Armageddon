using UnityEngine;

namespace Armageddon {
    
    /// <summary>
    /// This class allow to fetch <see cref="ScriptableObject"/> files placed in the "<i>Resources/</i>" folder at runtime.<br/>
    /// The files fetched should only be used to read content in order to avoid GameData modifications at runtime.
    /// </summary>
    public static class ResourceFetcher {
        
        /// <summary>
        /// Scan through every files in the Resources/ folder and returns the files from the type asked.
        /// </summary>
        /// <typeparam name="T">File type.</typeparam>
        public static T[] GetResourceFilesFromType<T>() where T : Object {
            return Resources.LoadAll<T>("");
        }

        /// <summary>
        /// Scan through every files in the Resources/ folder and returns the file at a specific index from the list of files found.
        /// </summary>
        /// <typeparam name="T">File type.</typeparam>
        public static T GetResourceFileFromTypeWithIndex<T>(int _index) where T : Object {
            T[] _tempResult = Resources.LoadAll<T>("");

            return _tempResult[_index] == null ? _tempResult[0] : _tempResult[_index];
        }

        /// <summary>
        /// Scan through every files in the Resources/ folder and returns the file from the type asked.
        /// </summary>
        /// <typeparam name="T">File type.</typeparam>
        public static T GetResourceFileFromType<T>() where T : Object {
            return Resources.LoadAll<T>("")[0];
        }
        
    }
    
}