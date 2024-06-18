using Armageddon.Serializer;
using UnityEngine;

namespace Armageddon.GameMode {

    /// <summary>
    /// Contain the configuration for a GameMode.
    /// </summary>
    public class GameModeConfiguration : ScriptableObject {

        #region Properties

        /// <summary>
        /// The internal name of this <see cref="GameModeConfiguration"/>.
        /// </summary>
        [field: SerializeField, Header("Game Mode Configuration"), Tooltip("The internal name of this configuration.")] public string InternalName { get; private set; } = string.Empty;
        /// <summary>
        /// Is this <see cref="GameModeConfiguration"/> allowed to execute outside the editor ?
        /// </summary>
        [field: SerializeField, Tooltip("Is this configuration allowed to execute outside the editor ?")] public bool AllowOnBuild { get; private set; } = true;

        /// <summary>
        /// The <see cref="AGameModeLogic"/> that'll be running for this <see cref="GameModeConfiguration"/>.
        /// </summary>
        [field: SerializeField, Header("Operating Logic"), Tooltip("The GameMode Logic instance that'll be running."), SerializeReference, ShowSerializeReference] public AGameModeLogic OperatingLogic { get; private set; }
        
        #endregion

    }
    
}