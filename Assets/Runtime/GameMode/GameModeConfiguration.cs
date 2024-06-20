using Armageddon.GameLogic.Camera;
using Armageddon.GameLogic.Character;
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

        /// <summary>
        /// The <see cref="ACharacter"/> that'll be used inside this <see cref="GameModeConfiguration"/>'s execution.
        /// </summary>
        [field: SerializeField, Header("Controllers"), Tooltip("The character that'll be used inside this game mode.")] public ACharacter Character { get; private set; } = null;
        /// <summary>
        /// The <see cref="ACharacterCamera"/> that'll be used with the specified <see cref="Character"/> inside this <see cref="GameModeConfiguration"/>'s execution.
        /// </summary>
        [field: SerializeField, Tooltip("The camera controller that'll be used inside this game mode.")] public ACharacterCamera CharacterCamera { get; private set; } = null;

        #endregion

    }
    
}