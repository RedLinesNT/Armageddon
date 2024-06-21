using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Armageddon.GameLogic.Character {

    /// <summary>
    /// The primitive settings of a <see cref="ACharacter"/> entity.
    /// </summary>
    [Serializable] public struct CharacterBaseSettings {

        /// <summary>
        /// The maximum force the player can apply to the character.
        /// </summary>
        [field: SerializeField, Tooltip("The maximum force the player can apply to the character.")] public float MaxForce { get; set; }
        /// <summary>
        /// Should the <see cref="ACharacter"/> by stopped in order to shoot it ?
        /// </summary>
        [field: SerializeField, Tooltip("Should the character be stopped in order to shoot it ?")] public bool WaitForStop { get; private set; }
        
    }
    
    /// <summary>
    /// A simple base for every playable characters (golf balls of course) in the game. 
    /// </summary>
    public abstract class ACharacter : MonoBehaviour {

        #region Attributes

        /// <summary>
        /// Should this <see cref="ACharacter"/> be frozen ?
        /// </summary>
        [SerializeField, ReadOnly, Tooltip("Should this character be frozen ?")] private bool freeze = false;
        /// <summary>
        /// The current charge that's going to be applied to the <see cref="ACharacter"/>'s <see cref="Rigidbody"/>.
        /// </summary>
        [SerializeField, ReadOnly, Tooltip("The current charge that's going to be applied to this character's Rigidbody.")] private float currentCharge = 0f;
        /// <summary>
        /// Is the player currently able to apply the ongoing charge to this character ?
        /// </summary>
        [SerializeField, ReadOnly, Tooltip("Is the player currently able to apply the ongoing charge to this character ?")] private bool canShoot = true;
        
        #endregion
        
        #region Properties

        /// <summary>
        /// The <see cref="UnityEngine.Rigidbody"/> component of this <see cref="ACharacter"/>.
        /// </summary>
        /// <remarks>
        /// This component is auto-assigned if no reference is given here before play-time.
        /// </remarks>
        [field: SerializeField, Header("References"), Tooltip("The Rigidbody component of this character.\nThis component is auto-assigned if no reference is given here.")] public Rigidbody Rigidbody { get; private protected set; } = null;
        /// <summary>
        /// The <see cref="Armageddon.GameLogic.Character.ChargeLine"/> component used by this <see cref="ACharacter"/>. (if used at all) 
        /// </summary>
        /// <remarks>
        /// This reference is required becuase some other components might need it.
        /// </remarks>
        [field: SerializeField, Tooltip("The ChargeLine component used by this character, if used.")] public ChargeLine ChargeLine { get; private set; } = null;

        [field: SerializeField, Header("Settings"), Tooltip("The primivite settings of this Character.")] public CharacterBaseSettings Settings { get; private set; } = default;
        
        /// <inheritdoc cref="freeze"/>
        public bool Freeze {
            get { return freeze; }
            set {
                freeze = value;
                Rigidbody.isKinematic = value; //Freeze the Rigidbody
                currentCharge = 0;
            }
        }
        /// <inheritdoc cref="currentCharge"/>
        public float CurrentCharge { 
            get { return currentCharge; }
            private protected set { 
                if (!freeze) currentCharge = Mathf.Clamp(value, 0f, Settings.MaxForce);
            }
        }
        /// <summary>
        /// The current target rotation angle.
        /// </summary>
        public float CurrentAngle { get; private set; } = 0f;
        /// <summary>
        /// The number of shots the player did since the creation of this <see cref="ACharacter"/>'s creation.
        /// </summary>
        public int ShotsCount { get; private set; } = 0;
        /// <summary>
        /// The position of the last safe position.
        /// </summary>
        public Vector3 LastSafePosition { get; private set; } = Vector3.zero;

        /// <summary>
        /// The <see cref="InputAction"/> used by this <see cref="ACharacter"/>.
        /// </summary>
        public InputActionAsset Actions { get; private protected set; } = null;
        
        #endregion

        #region MonoBehaviour's Methods

        protected void Awake() {
            //Check references
            if (Rigidbody == null) {
                Rigidbody = GetComponent<Rigidbody>();

                if (Rigidbody == null) { //If there's still no Rigidbody
                    Logger.TraceError($"Character {name}", $"Unable to find a Rigidbody component! Destroying component...");
                    Destroy(this);
                }
            }

            LastSafePosition = transform.position;
        }

        protected void Start() {
            if (Actions == null) { Logger.TraceWarning($"Character {name}", $"Unable to find any Actions for this character! Is this behaviour intended ?"); }
        }

        protected void Update() {
            if (!canShoot && Settings.WaitForStop && Rigidbody.velocity.magnitude < 0.25f) {
                canShoot = true;
                LastSafePosition = transform.position;
                
                //Fix the ball still rotating after stopping
                Freeze = true;
                Freeze = false;
            }
        }

        #endregion

        #region ACharacter's Internal Methods

        /// <summary>
        /// Apply the <see cref="currentCharge"/> to the <see cref="Rigidbody"/> of this <see cref="ACharacter"/>.
        /// </summary>
        protected void ApplyCharge() {
            if (freeze || !canShoot) return;
            
            Rigidbody.AddRelativeForce(Vector3.forward * currentCharge, ForceMode.Impulse);
            currentCharge = 0f;
            ShotsCount++;
            
            if (Settings.WaitForStop) canShoot = false;
        }

        /// <summary>
        /// Add a specified amount a rotation to this <see cref="ACharacter"/>.
        /// </summary>
        /// <param name="_amount">The amount to add (value in DEG)!</param>
        protected void AddRotation(float _amount) {
            CurrentAngle += _amount;
            Rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * CurrentAngle));
        }

        #endregion

        #region Attributes

        /// <summary>
        /// Reset the <see cref="ACharacter"/>'s position to the <see cref="LastSafePosition"/> registered.
        /// </summary>
        public void ResetToSafePosition() {
            Freeze = true;
            transform.position = LastSafePosition;
            Freeze = false;
            
            Logger.Trace($"Character {name}", $"Position reset to the last safe one registered.");
        }

        #endregion

        #region ACharacter's Internal Editor Methods

#if UNITY_EDITOR
        private void OnGUI() {
            Debug.DrawRay(transform.position, transform.forward * CurrentCharge, Color.cyan);
        }
#endif

        #endregion
        
    }
    
}