using System;
using Armageddon.Serializer;

namespace Armageddon.GameMode.Logic {
    
    /// <summary>
    /// The most sane (and the only) Game Mode of this game!
    /// </summary>
    [ProvideSourceInfo, Serializable] public class ClassicGameMode : AGameModeLogic {
        
        #region AGameModeLogic's External Virtual Methods

        public override void Initialize() {
            SpawnCharacter();
            Character.Actions.Enable();
        }

        public override void Shutdown() {
            DestroyCharacter();
        }

        protected override void OnPlayerHitPole() {
            Logger.Trace("Classic GameMode", $"YAY YOU FINISHED !!!!!!!!!!! (you did it in {Character.ShotsCount} shots!!!!!!!!)");
            DestroyCharacter();
        }

        #endregion
        
    }
    
}