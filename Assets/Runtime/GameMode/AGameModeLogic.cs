using System.Collections.Generic;
using System.Linq;
using Armageddon.GameLogic.Camera;
using Armageddon.GameLogic.Character;
using Armageddon.GameLogic.Objectives;
using Armageddon.Utils;
using UnityEngine;

namespace Armageddon.GameMode {

    /// <summary>
    /// Contain the prerequisite for a Game Mode running.
    /// </summary>
    public abstract class AGameModeLogic {

        #region Attributes

        /// <summary>
        /// A copy of the <see cref="GameModeConfiguration"/>.
        /// </summary>
        private GameModeConfiguration configuration = null;
        /// <summary>
        /// <see cref="List{T}"/> of every spawn points found on the current level.
        /// </summary>
        private List<GameObject> spawnPoints = new List<GameObject>();
        /// <summary>
        /// <see cref="List{T}"/> of every <see cref="FinishPole"/>s found on the current level.
        /// </summary>
        private List<FinishPole> finishPoles = new List<FinishPole>();

        #endregion

        #region Properties

        /// <summary>
        /// The instance of the current <see cref="ACharacter"/> instantiated.
        /// </summary>
        /// <remarks>
        /// Might be null!
        /// </remarks>
        public ACharacter Character { get; private set; } = null;
        /// <summary>
        /// The instance of the current <see cref="ACharacterCamera"/> instantiated for the <see cref="Character"/>.
        /// </summary>
        /// <remarks>
        /// Might be null!
        /// </remarks>
        public ACharacterCamera CharacterCamera { get; private set; } = null;

        #endregion

        #region AGameModeLogic's External Method

        public void PreInitialize() {
            configuration = GameModeSystem.Configuration;
            spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnPoint").ToList()); //Retreive every spawn points
            finishPoles = new List<FinishPole>(Object.FindObjectsByType<FinishPole>(FindObjectsSortMode.None)); //Retreive every finish poles
            
            //Check required references
            if(spawnPoints.Count <= 0) Logger.TraceWarning($"{this.GetType().Name}", $"Unable to find any spawn positions on this level!");
            if(finishPoles.Count <= 0) Logger.TraceWarning($"{this.GetType().Name}", $"Unable to find any finish pole on this level!");

            for (int i=0; i<finishPoles.Count; i++) { //Loop throughout every poles
                finishPoles[i].OnCharacterHit += OnPlayerHitPole;
            }
        }

        #endregion
        
        #region AGameModeLogic's External Virtual Methods

        /// <summary>
        /// Initialize and setup this <see cref="AGameModeLogic"/> instance.
        /// </summary>
        public virtual void Initialize() {}
        /// <summary>
        /// Update is called every frame to update this <see cref="AGameModeLogic"/> instance.
        /// </summary>
        public virtual void Update() {}
        /// <summary>
        /// Update is called every physics' tick to update this <see cref="AGameModeLogic"/> instance.
        /// </summary>
        public virtual void FixedUpdate() {}
        /// <summary>
        /// Shutdown and clean up the content created by this <see cref="AGameModeLogic"/> instance.
        /// </summary>
        public virtual void Shutdown() {}
        
        /// <summary>
        /// Called when the instantiated <see cref="Character"/> has hit a <see cref="FinishPole"/> on the current level.
        /// </summary>
        protected virtual void OnPlayerHitPole(){}

        #endregion

        #region AGameModeLogic's Internal Methods

        /// <summary>
        /// Create an instance of the <see cref="ACharacter"/> specified in the <see cref="configuration"/>. 
        /// </summary>
        /// <remarks>
        /// This will also create the <see cref="ACharacterCamera"/> for the <see cref="ACharacter"/>.
        /// </remarks>
        /// <param name="_useRandomSpawnPoint">Should a random spawn point be used ?</param>
        /// <param name="_wantedPosition">The specific spawn point to use (Won't be taken in account if <see cref="_useRandomSpawnPoint"/> is false).</param>
        protected void SpawnCharacter(bool _useRandomSpawnPoint = false, Vector3 _wantedPosition = new Vector3()) {
            Vector3 _targetSpawnPosition = _useRandomSpawnPoint ? _wantedPosition : spawnPoints.PickRandom().transform.position; //Define the target spawn point

            Character = Object.Instantiate(configuration.Character, _targetSpawnPosition, Quaternion.identity); //Spawn the character
            CharacterCamera = Object.Instantiate(configuration.CharacterCamera, _targetSpawnPosition, Quaternion.identity); //Spawn the camera
            
            //Setup the Camera
            //TODO: Note to myself, I shouldn't had used structs...
            CharacterCameraSettings _newSettings = CharacterCamera.Settings;
            _newSettings.Pivot = Character;
            CharacterCamera.Settings = _newSettings;
            
            Logger.Trace($"{this.GetType().Name}", $"The character has been spawned (Pos: {_targetSpawnPosition})!");
        }

        /// <summary>
        /// Destroy the <see cref="Character"/> and its <see cref="CharacterCamera"/>.
        /// </summary>
        /// <remarks>
        /// Nothing will happen if there's no <see cref="Character"/> or <see cref="CharacterCamera"/> created.
        /// </remarks>
        protected void DestroyCharacter(float _delay = 0f) {
            if (CharacterCamera != null) Object.Destroy(CharacterCamera.gameObject, _delay); 
            if (Character != null) Object.Destroy(Character.gameObject, _delay);
            
            Logger.Trace($"{this.GetType().Name}", $"The character has been destroyed!");
        }

        #endregion
        
    }
    
}