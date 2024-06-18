using UnityEngine.LowLevel;

namespace Armageddon.Engine {

    /// <summary>
    /// The timing of each internal sub-system managed by the engine (<see cref="PlayerLoopSystem.subSystemList"/>).
    /// </summary>
    public enum EPlayerLoopTiming : int {
        Initialization = 0,
        EarlyUpdate = 1,
        FixedUpdate = 2,
        PreUpdate = 3,
        Update = 4,
        PreLateUpdate = 5,
        PostLateUpdate = 6,
    }

    /// <summary>
    /// <see cref="PlayerLoop"/> contain utils methods to manipulate the <see cref="UnityEngine.LowLevel.PlayerLoop"/> in runtime.
    /// </summary>
    public static class PlayerLoop {
        
        /// <summary>
        /// Insert a <see cref="UnityEngine.LowLevel.PlayerLoopSystem.UpdateFunction"/> into the <see cref="UnityEngine.LowLevel.PlayerLoop"/>.
        /// </summary>
        /// <param name="_method">The method to insert into the Loop.</param>
        /// <param name="_timing">The timing to insert the method at.</param>
        public static void InsertAt(PlayerLoopSystem.UpdateFunction _method, EPlayerLoopTiming _timing) {
            PlayerLoopSystem _currentLoopSystem = UnityEngine.LowLevel.PlayerLoop.GetCurrentPlayerLoop();
            _currentLoopSystem.subSystemList[(int)_timing].updateDelegate += _method; //Set the method to be called at the same time as the timing asked
            UnityEngine.LowLevel.PlayerLoop.SetPlayerLoop(_currentLoopSystem); //Set our modified PlayerLoop back into the engine
        }
        
    }

}