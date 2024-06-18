using UnityEngine;

namespace Armageddon.GameMode {

    /// <summary>
    /// Contain the prerequisite for a Game Mode running.
    /// </summary>
    public abstract class AGameModeLogic {
        
        #region AServerGameModeLogic's External Virtual Methods

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

        #endregion
        
    }
    
}