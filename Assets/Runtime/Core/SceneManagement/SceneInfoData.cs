using System;
using Armageddon.GameMode;
using UnityEngine;

namespace Armageddon.SceneManagement {
    
    /// <summary>
    /// <see cref="SceneInfoData"/> file are <see cref="ScriptableObject"/>s created by the editor when about to create LevelBundles.<br/>
    /// These files contain the primitive data of a playable level inside a LevelBundle.
    /// </summary>
    [Serializable] public class SceneInfoData : ScriptableObject {
        
        #region Properties

        /// <summary>
        /// The path of the <see cref="Scene"/> this <see cref="SceneInfoData"/> file is referring to.
        /// </summary>
        [field: SerializeField, Tooltip("The Scene reference file of this SceneInfoData.")] public SceneReference Scene { get; private set; } = null;
        /// <summary>
        /// The string identifier of this <see cref="SceneInfoData"/> file.
        /// </summary>
        [field: SerializeField, Space(15), Tooltip("The string identifier of this SceneInfoData")] public string Identifier { get; private set; } = string.Empty;

        /// <summary>
        /// The target <see cref="GameModeConfiguration"/> to execute when loading this <see cref="SceneInfoData"/>.
        /// </summary>
        [field: SerializeField, Tooltip("The target Game Mode to execute for this scene.")] public GameModeConfiguration GameMode { get; private set; } = null;

        #endregion

    }
    
}