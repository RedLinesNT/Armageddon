using Armageddon.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace Armageddon.Editor {
    
    /// <summary>
    /// Create <see cref="ScriptableObject"/>s instances in the editor.
    /// </summary>
    public static class ScenesUtils {
        
        #region SceneSystem's ScriptableObjects

        [MenuItem("Armageddon/Scenes/Refresh Checksums")] public static void RefreshChecksums() {
            SceneInfoData[] _infos = ResourceFetcher.GetResourceFilesFromType<SceneInfoData>();

            for (int i=0; i<_infos.Length; i++) {
                _infos[i].Scene.RefreshChecksum();
            }
            
            Logger.Trace("Build Pipeline", $"Refreshed the checksum of {_infos.Length} scenes!");
        }

        #endregion
        
    }
    
}