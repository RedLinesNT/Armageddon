using System;
using Armageddon.Engine;
using Armageddon.SceneManagement;
using UnityEngine;

namespace Armageddon.GameMode {

    public static class GameModeSystem {
        
        #region Attributes

        /// <summary>
        /// The <see cref="AGameModeLogic"/> instance currently running.
        /// </summary>
        private static AGameModeLogic modeInstance = null;
        /// <summary>
        /// The <see cref="SceneInfoData"/> who requested to start the <see cref="GameModeSystem"/>.
        /// </summary>
        private static SceneInfoData targetScene = null;

        #endregion
        
        #region Events

        /// <summary>
        /// Invoked when the <see cref="GameModeConfiguration"/> has been initialized.
        /// </summary>
        private static Action onInitialized = null;
        /// <summary>
        /// Invoked when the <see cref="GameModeSystem"/> is shutting down.
        /// </summary>
        private static Action onShutdown = null;

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="GameModeConfiguration"/> currently running.
        /// </summary>
        public static GameModeConfiguration Configuration { get; private set; } = null;
        
        /// <inheritdoc cref="onInitialized"/>
        public static event Action OnInitialized { add { onInitialized += value; } remove { onInitialized -= value; } }
        /// <inheritdoc cref="onShutdown"/>
        public static event Action OnShutdown { add { onShutdown += value; } remove { onShutdown -= value; } }

        #endregion

        #region GameModeSystem's Initialization Method

        /// <summary>
        /// Initialize the <see cref="GameModeSystem"/>.
        /// </summary>
        /// <remarks>
        /// <inheritdoc cref="RuntimeInitializeLoadType.AfterAssembliesLoaded"/>
        /// </remarks>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)] private static void Initialize() {
            //There's no specific need to bind directly a method, because this system
            //won't ever be "disposed", so doing like that won't cause any issues
            SceneSystem.OnSceneBeginLoading += (_newSceneInfo) => {
                if (_newSceneInfo.GameMode == null) {
                    Logger.Trace("Game Mode System", $"New scene incoming '{_newSceneInfo.Identifier}', no Game Mode requested.");
                    return;
                } 
                
                if (modeInstance != null || targetScene != null) { //If this system is currently running for another scene
                    Logger.TraceWarning("Game Mode System", $"Unable to setup a new context for the incoming scene '{_newSceneInfo.Identifier}' (Another scene has already setup a context up! {_newSceneInfo.Identifier} - {modeInstance?.GetType().Name})!");
                    return;
                }

                targetScene = _newSceneInfo; //Set this before loading the context, to insure will set the context for the correct scene, not something in the middle
                SceneSystem.OnSceneLoadedEarly += SetContext; //This method will unbind itself later...
            };
            
            //Ditto
            SceneSystem.OnSceneLoaded += (_sceneInfo) => {
                if (modeInstance != null && targetScene == _sceneInfo) { //If this system is currently running for this scene
                    Shutdown();
                }
            };

            Application.quitting += Shutdown;
            
            PlayerLoop.InsertAt(InternalUpdate, EPlayerLoopTiming.Update);
            PlayerLoop.InsertAt(InternalFixedUpdate, EPlayerLoopTiming.FixedUpdate);
        }

        #endregion

        #region GameModeSystem's Internal Methods

        /// <summary>
        /// Set the <see cref="GameModeSystem"/>'s context from a freshly loaded scene.
        /// </summary>
        /// <param name="_targetScene"></param>
        private static void SetContext(SceneInfoData _targetScene) {
            if (_targetScene != targetScene) {
                return; //We got the wrong scene...
            }

            SceneSystem.OnSceneLoadedEarly -= SetContext; //Unbind this method previously bind

#if !UNITY_EDITOR
            if (!targetScene.GameMode.AllowOnBuild) {
                Logger.TraceError("Game Mode System", $"The configuration '{targetScene.GameMode.InternalName}' for the level '{targetScene.Identifier}' isn't allowed to execute outside the dev environment!");
                targetScene = null;
                return;
            }
#endif

            modeInstance = (AGameModeLogic)Activator.CreateInstance(targetScene.GameMode.OperatingLogic.GetType());
            modeInstance.Initialize();
            
            Logger.Trace("Game Mode System", $"The context for the level '{targetScene.Identifier}' has been loaded! ({Configuration.InternalName} - {modeInstance.GetType().Name})");
            
            onInitialized?.Invoke(); //Invoke this event
        }
        
        /// <summary>
        /// Try to shut down the current game mode context active.
        /// </summary>
        private static void Shutdown() {
            modeInstance?.Shutdown(); //If a GameModeLogic instance is running, shut it down
            
            modeInstance = null;
            targetScene = null;
            
            onShutdown?.Invoke(); //Invoke this event
        }

        private static void InternalUpdate() {
            modeInstance?.Update();
        }

        private static void InternalFixedUpdate() {
            modeInstance?.FixedUpdate();
        }

        #endregion
        
    }
    
}