using System;
using Armageddon.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Armageddon.GameLogic.Character {

    /// <summary>
    /// Contain the settings of the <see cref="ChargeLine"/> component.
    /// </summary>
    [System.Serializable] public struct ChargeLineSettings {
        
        /// <summary>
        /// The <see cref="ACharacter"/> this <see cref="ChargeLine"/> will be looking at.
        /// </summary>
        [field: SerializeField, Tooltip("The ACharacter this component will be looking at.")] public ACharacter Character { get; private set; }
        /// <summary>
        /// The smoothing applied when changing it according to the ongoing charge.
        /// </summary>
        [field: SerializeField, Tooltip("The smoothing applied when changing it according to the ongoing charge.")] public float LengthSmoothing { get; private set; }
        
    }
    
    /// <summary>
    /// A component designed to simply display the ongoing throwing charge of a <see cref="ACharacter"/> with a <see cref="LineRenderer"/>.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))] public class ChargeLine : MonoBehaviour {
        
        #region Attributes

        private float lineLengthVelocity = 0f;
        
        #endregion

        #region Properties

        /// <inheritdoc cref="ChargeLineSettings"/>
        [field: SerializeField, Tooltip("The settings of this Charge Line component.")] public ChargeLineSettings Settings { get; private set; } = default;
        /// <summary>
        /// The <see cref="LineRenderer"/> component used.
        /// </summary>
        /// <remarks>
        /// This component is dynamically set at runtime.
        /// </remarks>
        [field: SerializeField, ReadOnly, Tooltip("The LineRenderer component used (Dynamically set at runtime).")] public LineRenderer Line { get; private set; } = null;

        #endregion

        #region MonoBehaviour's Methods

        private void Awake() {
            Line = GetComponent<LineRenderer>(); //No need to check, the "RequireComponent" is here for us
            Line.SetPosition(1, Vector3.zero); //Reset the Line
        }

        private void Update() {
            //Trash code...
            //TODO: Remaster this?

            float _newZPos = Mathf.SmoothDamp(Line.GetPosition(1).z, Mathf.Clamp(Settings.Character.CurrentCharge, 0f, Settings.Character.CurrentCharge / 20f), ref lineLengthVelocity, Settings.LengthSmoothing);
            Line.SetPosition(1, new Vector3(0, 0, _newZPos));
        }

        #endregion

    }
    
}