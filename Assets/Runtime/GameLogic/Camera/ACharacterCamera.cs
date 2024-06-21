using System;
using Armageddon.GameLogic.Character;
using Armageddon.ICE;
using UnityEngine;

namespace Armageddon.GameLogic.Camera {

    /// <summary>
    /// The primitive settings for a <see cref="ACharacterCamera"/>.
    /// </summary>
    [Serializable] public struct CharacterCameraSettings {
        
        /// <summary>
        /// The pivot point of this <see cref="ACharacterCamera"/>.
        /// </summary>
        [field: SerializeField, Header("References"), Tooltip("The pivot point of this Character Camera.")] public ACharacter Pivot { get; set; }
        
        /// <summary>
        /// The offset added to the pivot position of the <see cref="ACharacterCamera"/>.
        /// </summary>
        [field: SerializeField, Header("Offsets"), Tooltip("The offset added to the pivot position of this Character Camera.")] public Vector3 PivotOffset { get; private set; }
        
        /// <summary>
        /// The follow "<i>lag/delay</i>" to get into the <see cref="Pivot"/>'s position.
        /// </summary>
        [field: SerializeField, Header("Settings"), Tooltip("The follow 'lag/delay' to get into the pivot's position.")] public float FollowSpeed { get; private set; }
        
    }
    
    /// <summary>
    /// The simple base for every camera following the player's ball.
    /// </summary>
    public abstract class ACharacterCamera : AICEVirtualCamera {

        #region Attributes

        /// <summary>
        /// The <see cref="ACharacterCamera"/>'s velocity.
        /// </summary>
        /// <remarks>
        /// Only used to smooth things out!
        /// </remarks>
        private Vector3 velocity = Vector3.zero;

        #endregion
        
        #region Properties

        /// <inheritdoc cref="CharacterCameraSettings"/>
        [field: SerializeField, Tooltip("The primitive settings for this Character Camera.")] public CharacterCameraSettings Settings { get; set; }

        #endregion

        #region MonoBehaviour's Methods

        private protected override void OnUpdateCamera() {
            transform.position = Vector3.SmoothDamp(transform.position, Settings.Pivot.Rigidbody.position + Settings.PivotOffset, ref velocity, Settings.FollowSpeed);
        }

        #endregion
        
    }
    
}