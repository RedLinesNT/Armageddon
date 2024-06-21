using System;
using Armageddon.GameLogic.Character;
using UnityEngine;

namespace Armageddon.GameLogic {

    /// <summary>
    /// A simple trigger to reset the position of a <see cref="ACharacter"/> when entering it.
    /// </summary>
    [RequireComponent(typeof(Collider))] public class DeathTrigger : MonoBehaviour {

        #region MonoBehaviour's Methods

        private void OnTriggerEnter(Collider _other) {
            if (_other.TryGetComponent<ACharacter>(out ACharacter _character)) {
                _character.ResetToSafePosition();
            }
        }

        #endregion
        
    }
    
}