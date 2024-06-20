using System;
using Armageddon.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Armageddon.GameLogic.Character {

    /// <summary>
    /// The most sane and classic controller you'll find in this game!
    /// </summary>
    public class ClassicController : ACharacter {

        #region Attributes

        /// <summary>
        /// The actual <see cref="UnityEngine.InputSystem.InputAction"/> used by the <see cref="ACharacter"/>.
        /// </summary>
        private GenericActions actions = null;

        #endregion
        
        #region MonoBehaviour's Methods

        protected new void Awake() {
            base.Awake();
            
            actions = new GenericActions();
            Actions = actions.asset;
            
            actions.Enable(); //TODO: Remove this!
            
            //Bind inputs
            //Same thing as the GameModeSystem, no need to unbind them
            //This object will be destroyed as the same time as this object :)
            //actions.GenericCharacter.Rotate.performed += OnRotateActionPerformed;
            //actions.GenericCharacter.Charge.performed += OnChargeActionPerformed;
            actions.GenericCharacter.Shoot.started += OnShootActionPressed;
        }

        protected new void Update() {
            base.Update();
            
            if (actions.GenericCharacter.Rotate.IsPressed()) AddRotation(actions.GenericCharacter.Rotate.ReadValue<float>() * Time.deltaTime * 10f); //TODO: Use proper sensitivity settings
            if (actions.GenericCharacter.Charge.IsPressed()) CurrentCharge += actions.GenericCharacter.Charge.ReadValue<float>() * Time.deltaTime * 10f; //TODO: Use proper sensitivity settings
        }

        #endregion

        #region ClassicController's Internal Methods

        /// <summary>
        /// Called when the "<i>Shoot</i>" action on the <see cref="actions"/> Input Action has been pressed.
        /// </summary>
        private void OnShootActionPressed(InputAction.CallbackContext _context) {
            ApplyCharge();
        }

        #endregion
        
    }
    
}