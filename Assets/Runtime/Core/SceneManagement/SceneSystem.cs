using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Armageddon.SceneManagement {

    public static class SceneSystem {
        
        #region Attributes

        /// <summary>
        /// <see cref="List{T}"/> of every <see cref="SceneInfoData"/> files found upon initialization inside the application.
        /// </summary>
        private static List<SceneInfoData> sceneInfos = new List<SceneInfoData>();
        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of the currently loaded <see cref="Scene"/> with their linked <see cref="SceneInfoData"/> file.
        /// </summary>
        private static Dictionary<SceneInfoData, Scene> loadedScenes = new Dictionary<SceneInfoData, Scene>();

        #endregion
        
        #region Events

        /// <summary>
        /// Invoked when a <see cref="SceneInfoData"/> is about to be loaded.
        /// </summary>
        /// <param name="01">The <see cref="SceneInfoData"/> file that'll be loaded.</param>
        private static Action<SceneInfoData> onSceneBeginLoading = null;
        /// <summary>
        /// Invoked when a <see cref="SceneInfoData"/> has been loaded.
        /// </summary>
        /// <param name="01">The <see cref="SceneInfoData"/> file that got loaded.</param>
        private static Action<SceneInfoData> onSceneLoaded = null;
        /// <summary>
        /// Invoked before <see cref="onSceneLoaded"/>.
        /// </summary>
        /// <remarks>
        /// Can be used to prepare specific sub-systems.
        /// </remarks>
        /// <param name="01">The <see cref="SceneInfoData"/> file that got loaded.</param>
        private static Action<SceneInfoData> onSceneLoadedEarly = null;
        /// <summary>
        /// Invoked when a <see cref="SceneInfoData"/> is about to be unloaded.
        /// </summary>
        /// <param name="01">The <see cref="SceneInfoData"/> file that'll be unloaded.</param>
        private static Action<SceneInfoData> onSceneBeginUnloading = null;
        /// <summary>
        /// Invoked when a <see cref="SceneInfoData"/> has been unloaded.
        /// </summary>
        /// <param name="01">The <see cref="SceneInfoData"/> file that got unloaded.</param>
        private static Action<SceneInfoData> onSceneUnloaded = null;
        /// <summary>
        /// Invoked when the active scene has been changed.
        /// </summary>
        /// <param name="01">The <see cref="SceneInfoData"/> file of the new scene currently active.</param>
        private static Action<SceneInfoData> onActiveSceneChanged = null;

        #endregion
        
        #region Properties

        /// <inheritdoc cref="loadedScenes"/>
        public static IReadOnlyDictionary<SceneInfoData, Scene> LoadedScenes { get { return loadedScenes; } }
        /// <summary>
        /// <see cref="KeyValuePair{TKey,TValue}"/> of the current scene active.
        /// </summary>
        public static KeyValuePair<SceneInfoData, Scene> ActiveScene { get; private set; } = new KeyValuePair<SceneInfoData, Scene>();
        
        /// <inheritdoc cref="onSceneBeginLoading"/>
        public static event Action<SceneInfoData> OnSceneBeginLoading { add { onSceneBeginLoading += value; } remove { onSceneBeginLoading -= value; } }
        /// <inheritdoc cref="onSceneLoaded"/>
        public static event Action<SceneInfoData> OnSceneLoaded { add { onSceneLoaded += value; } remove { onSceneLoaded -= value; } }
        /// <inheritdoc cref="onSceneLoadedEarly"/>
        public static event Action<SceneInfoData> OnSceneLoadedEarly { add { onSceneLoadedEarly += value; } remove { onSceneLoadedEarly -= value; } }
        /// <inheritdoc cref="onSceneBeginUnloading"/>
        public static event Action<SceneInfoData> OnSceneBeginUnloading { add { onSceneBeginUnloading += value; } remove { onSceneBeginUnloading -= value; } }
        /// <inheritdoc cref="onSceneUnloaded"/>
        public static event Action<SceneInfoData> OnSceneUnloaded { add { onSceneUnloaded += value; } remove { onSceneUnloaded -= value; } }
        /// <inheritdoc cref="onActiveSceneChanged"/>
        public static event Action<SceneInfoData> OnActiveSceneChanged { add { onActiveSceneChanged += value; } remove { onActiveSceneChanged -= value; } }

        #endregion
        
        #region SceneSystem's Initialization Methods
        
        /// <summary>
        /// Initialize the <see cref="SceneSystem"/> (<i>First Initialization Pass</i>).
        /// </summary>
        /// <remarks>
        /// <see cref="RuntimeInitializeLoadType.AfterAssembliesLoaded"/>
        /// </remarks>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)] private static void FirstInitializationPass() {
            sceneInfos = ResourceFetcher.GetResourceFilesFromType<SceneInfoData>().ToList();
            Logger.Trace("Scene System", $"Found and cached '{sceneInfos.Count}' Internal Level Data.");
        }
        
        /// <summary>
        /// The second initialization pass of the <see cref="SceneSystem"/>
        /// </summary>
        /// <remarks>
        /// Register the default <see cref="Scene"/> loaded by the engine into the <see cref="SceneSystem"/>.<br/>
        /// <see cref="RuntimeInitializeLoadType.BeforeSceneLoad"/>
        /// </remarks>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void SecondInitializationPass() {
            Scene _defaultScene = SceneManager.GetActiveScene();
            SceneInfoData _defaultSceneInfo = FindInfoFromScenePath(_defaultScene.path);

            if(_defaultSceneInfo == null) { //If the default scene isn't registered in any SceneInfo file
                loadedScenes.Add(FindInfoFromIdentifier("Empty Scene"), _defaultScene); //Set a dummy SceneInfo file
                return;
            }
            
            //Set the default values of the SceneSystem
            loadedScenes.Add(_defaultSceneInfo, _defaultScene);
            SetActiveScene(_defaultScene, false, false);
        }
        
        #endregion
        
        #region SceneSystem's External Scene Loading Methods

        /// <summary>
        /// Load a <see cref="Scene"/> referenced in a <see cref="SceneInfoData"/> file asynchronously in the background.
        /// </summary>
        /// <remarks>
        /// The new <see cref="Scene"/> loaded will automatically be the new active scene by default.
        /// </remarks>
        /// <param name="_data">The <see cref="SceneInfoData"/> containing the <see cref="Scene"/> to load.</param>
        /// <param name="_loadMode">The <see cref="LoadSceneMode"/>.</param>
        public static async Task LoadSceneAsync(SceneInfoData _data, LoadSceneMode _loadMode = LoadSceneMode.Single) {
            onSceneBeginLoading?.Invoke(_data);
            await Task.Run(async () => { await Task.Delay(1500).ConfigureAwait(false); }); //Wait a little bit here

            Stopwatch _stopwatch = new Stopwatch(); //Create a stopwatch to trace the loading time
            _stopwatch.Start();

            await SceneManager.LoadSceneAsync(_data.Scene, LoadSceneMode.Additive); //Load the scene referenced directly (This scene is already baked here)

            if(_loadMode == LoadSceneMode.Single) { //If the LoadMode is set to single
                Dictionary<SceneInfoData, Scene> _scenesCopy = new Dictionary<SceneInfoData, Scene>(loadedScenes);
                foreach (KeyValuePair<SceneInfoData, Scene> _loadedScene in _scenesCopy) {
                    await UnloadSceneAsync(_loadedScene.Value, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                }
            }
            
            //Clear everything
            loadedScenes.Clear();
            ActiveScene = new KeyValuePair<SceneInfoData, Scene>();

            for (int i=0; i<SceneManager.sceneCount; i++) {
                loadedScenes.Add(FindInfoFromScenePath(SceneManager.GetSceneAt(i).path), SceneManager.GetSceneAt(i));
            }

            Scene _newSceneAdded = FindLoadedSceneFromData(_data);
            SetActiveScene(_newSceneAdded);
            
            _stopwatch.Stop();
            Logger.Trace("Scene System", $"Loaded scene '{_data.Identifier}' ({_stopwatch.Elapsed.Milliseconds} milliseconds elapsed).");
            onSceneLoadedEarly?.Invoke(_data); //Invoke this event
            
            await Task.Run(async () => { await Task.Delay(2500).ConfigureAwait(false); }); //Wait a little bit here
            onSceneLoaded?.Invoke(_data);
        }
        
        /// <summary>
        /// Unload a running <see cref="Scene"/> asynchronously in the background.
        /// </summary>
        /// <param name="_scene">The <see cref="Scene"/> to unload.</param>
        /// <param name="_unloadMode">The <see cref="UnloadSceneOptions"/> to unload the <see cref="Scene"/>.</param>
        public static async Task UnloadSceneAsync(Scene _scene, UnloadSceneOptions _unloadMode = UnloadSceneOptions.None) {
            SceneInfoData _sceneInfo = FindInfoFromScenePath(_scene.path);
            if(_sceneInfo == null) { Logger.TraceError("Scene System", $"Unable to unload '{_scene.path}'! This scene isn't registered in the Scene System."); return; }

            onSceneBeginUnloading?.Invoke(_sceneInfo);
            await Task.Run(async () => { await Task.Delay(500).ConfigureAwait(false); }); //Wait a little bit here

            loadedScenes.Remove(_sceneInfo); //Remove this element from the loaded scenes
            
            await SceneManager.UnloadSceneAsync(_scene, _unloadMode); //Unload the scene

            onSceneUnloaded?.Invoke(_sceneInfo);
        }

        #endregion
        
        #region SceneSystem's External Methods

        /// <summary>
        /// Define the active <see cref="Scene"/> running.
        /// </summary>
        /// <param name="_scene">The <see cref="Scene"/> to define as active.</param>
        /// <param name="_invokeEvents">Should this action invoke related events of the <see cref="SceneSystem"/> ?</param>
        /// <param name="_declareOnInternalSceneManager">Should the internal <see cref="SceneManager"/> be informed about this change ?</param>
        public static void SetActiveScene(Scene _scene, bool _invokeEvents = true, bool _declareOnInternalSceneManager = true) {
            SceneInfoData _sceneInfo = FindInfoFromScenePath(_scene.path);
            ActiveScene = new KeyValuePair<SceneInfoData, Scene>(_sceneInfo, _scene);

            if(_declareOnInternalSceneManager) SceneManager.SetActiveScene(_scene);
            if(_invokeEvents) {
                Logger.Trace("Scene System", $"Scene '{_scene.name}' has been set as active.");
                onActiveSceneChanged?.Invoke(_sceneInfo);
            }
        }

        /// <summary>
        /// Find and return a <see cref="SceneInfoData"/> file.
        /// </summary>
        /// <param name="_identifier">The identifier of the <see cref="SceneInfoData"/> file to find.</param>
        public static SceneInfoData FindInfoFromIdentifier(string _identifier) {
            for (int i=0; i<sceneInfos.Count; i++) {
                if(sceneInfos[i].Identifier == _identifier) return sceneInfos[i];
            }
            
            return null;
        }

        /// <summary>
        /// Find and return a <see cref="SceneInfoData"/> file.
        /// </summary>
        /// <param name="_scenePath">The <see cref="Scene"/>'s path of the <see cref="SceneReference"/> referenced in a <see cref="SceneInfoData"/> file.</param>
        public static SceneInfoData FindInfoFromScenePath(string _scenePath) {
            for (int i=0; i<sceneInfos.Count; i++) {
                if(sceneInfos[i].Scene == _scenePath) return sceneInfos[i];
            }

            return null;
        }
        
        public static Scene FindLoadedSceneFromData(SceneInfoData _sceneInfo) {
            foreach (KeyValuePair<SceneInfoData, Scene> _loadedScene in loadedScenes) {
                if(_loadedScene.Value.path == _sceneInfo.Scene) {
                    return _loadedScene.Value;
                }
            }

            return default;
        }
        
        /// <summary>
        /// Is the <see cref="Scene"/> referenced in a <see cref="SceneInfoData"/> file valid.
        /// </summary>
        public static bool IsDataValid(SceneInfoData _sceneData) {
            return SceneUtility.GetBuildIndexByScenePath(_sceneData.Scene) != -1;
        }

        #endregion
        
    }
    
}