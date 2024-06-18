using System.IO;
using Armageddon.GameMode;
using Armageddon.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace Armageddon.Editor {

    /// <summary>
    /// Create <see cref="ScriptableObject"/>s instances in the editor.
    /// </summary>
    public static class SOObjectCreator {
        
        #region SceneSystem's ScriptableObjects

        [MenuItem("Armageddon/Scenes/Scene Data file...")] public static void CreateSceneDataFile() {
            SceneInfoData _asset = ScriptableObject.CreateInstance<SceneInfoData>();
            
            if (!Directory.Exists("Assets/Resources/Scenes/")) { //Check if the directory exists
                Directory.CreateDirectory("Assets/Resources/Scenes/");
            }
    
            string _assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Scenes/SceneData.asset");
            AssetDatabase.CreateAsset(_asset, _assetPath);
            AssetDatabase.SaveAssets();
    
            EditorUtility.FocusProjectWindow();
    
            Selection.activeObject = _asset;
        }

        #endregion
        
        #region GameModeSystem's ScriptableObjects

        [MenuItem("Armageddon/Game Logic/Game Mode Configuration file...")] public static void CreateGameConfigFile() {
            GameModeConfiguration _asset = ScriptableObject.CreateInstance<GameModeConfiguration>();
            
            if (!Directory.Exists("Assets/Resources/Game Modes/")) { //Check if the directory exists
                Directory.CreateDirectory("Assets/Resources/Game Modes/");
            }
    
            string _assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Game Modes/GameConfig.asset");
            AssetDatabase.CreateAsset(_asset, _assetPath);
            AssetDatabase.SaveAssets();
    
            EditorUtility.FocusProjectWindow();
    
            Selection.activeObject = _asset;
        }

        #endregion
        
    }

}