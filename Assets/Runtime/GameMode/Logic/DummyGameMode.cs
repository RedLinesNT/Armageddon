using System;
using Armageddon.Serializer;

namespace Armageddon.GameMode.Logic {

    /// <summary>
    /// This <see cref="AGameModeLogic"/> serve absolutely no purpose...
    /// </summary>
    [ProvideSourceInfo, Serializable] public class DummyGameMode : AGameModeLogic {
        
        #region AGameModeLogic's External Virtual Methods

        public override void Initialize() {
            Logger.Trace("Dummy GameMode", "I serve no purpose, don't use this, you're waisting your time...");
        }

        public override void Shutdown() {
            Logger.Trace("Dummy GameMode", "You finally shut this useless thing down.");
        }

        #endregion
        
    }
    
}