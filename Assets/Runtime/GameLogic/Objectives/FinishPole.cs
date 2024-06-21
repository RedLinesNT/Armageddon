using System;
using Armageddon.GameLogic.Character;
using UnityEngine;

namespace Armageddon.GameLogic.Objectives {

    /// <summary>
    /// The main currently unique objective of this game, the <see cref="FinishPole"/>!
    /// </summary>
    /// <remarks>
    /// Must have a trigger collider!
    /// </remarks>
    [RequireComponent(typeof(Collider))] public class FinishPole : MonoBehaviour {

        #region Events

        /// <summary>
        /// Invoked when a <see cref="ACharacter"/> hit this <see cref="FinishPole"/>.
        /// </summary>
        private Action onCharacterHit = null;

        #endregion

        #region Properties

        /// <inheritdoc cref="onCharacterHit"/>
        public event Action OnCharacterHit { add { onCharacterHit += value; } remove { onCharacterHit -= value; } }

        #endregion
        
        #region MonoBehaviour's Methods

        private void OnTriggerEnter(Collider _other) {
            if (_other.GetComponentInParent<ACharacter>()) {
                onCharacterHit?.Invoke();
            }
        }

        #endregion

    }
    
}