using System.Diagnostics;
using UnityEngine;
#if UNITY_EDITOR
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

#pragma warning disable CS0162 //Unreachable code detected

namespace Armageddon.SceneManagement {

    /// <summary>
    /// A wrapper that provides the means to safely serialize <see cref="SceneAsset"/> References.
    /// </summary>
    [System.Serializable] public class SceneReference : ISerializationCallbackReceiver {

        #region Attributes

#if UNITY_EDITOR //What is used inside the Editor to select a Scene Asset
        [SerializeField, Tooltip("The Scene asset file.")] private SceneAsset sceneAsset = null;
#endif

        [SerializeField, HideInInspector] private string scenePath; //This should only be set during serialization/deserialization.
        [SerializeField, HideInInspector] private string checksum; //This should only be set during serialization/deserialization.

        #endregion

        #region Properties

#if UNITY_EDITOR
        /// <summary>
        /// (Editor only) Returns if the <see cref="sceneAsset"/> object given is a valid one.
        /// </summary>
        private bool IsSceneAssetValid { get { return !sceneAsset ? false : sceneAsset; } }
#endif

        /// <summary>
        /// The path of this Scene.
        /// </summary>
        public string ScenePath {
            get {
                return scenePath; //At runtime, we rely on the stored path value, which we assume has been serialized correctly on Build time.
            }
            set {
                scenePath = value;

#if UNITY_EDITOR
                sceneAsset = GetSceneAssetFromPath();
#endif
            }
        }

        /// <summary>
        /// The MD5 Checksum of the scene asset given.
        /// </summary>
        /// <remarks>
        /// The checksum of the scene given is only updated when being built.
        /// </remarks>
        public string Checksum {
            get { return checksum; }
#if UNITY_EDITOR
            set { checksum = value; }
#endif
        }

        #endregion

        #region SceneReference's Implicit Operator Method

        public static implicit operator string(SceneReference _sceneReference) {
            return _sceneReference.ScenePath;
        }

        #endregion

        #region SceneReference's Serialize/Deserialize Methods

        /// <summary>
        /// Called to prepare this SceneReference's data for serialization.<br/>
        /// Removed from builds by Unity.
        /// </summary>
        public void OnBeforeSerialize() {
#if UNITY_EDITOR
            HandleBeforeSerialize();
#endif
        }

        /// <summary>
        /// Called to prepare this SceneReference's data for deserialization.<br/>
        /// Removed from builds by Unity.
        /// </summary>
        public void OnAfterDeserialize() {
#if UNITY_EDITOR
            //Impossible to touch the AssetDatabase at this time, deferring this operation.
            EditorApplication.update += HandleAfterDeserialize;
#endif
        }

#if UNITY_EDITOR

        public void HandleBeforeSerialize() {
            if (!IsSceneAssetValid && !string.IsNullOrEmpty(scenePath)) { //If the SceneAsset isn't valid but we have the SceneAsset's path, try to get the SceneAsset from its path
                sceneAsset = GetSceneAssetFromPath();

                if (sceneAsset == null) scenePath = string.Empty; //If no SceneAsset was found, remove the path

                EditorSceneManager.MarkAllScenesDirty();
            } else { //The SceneAsset is valid but there's no SceneAsset path set, overwrite it based on the SceneAsset's one.
                scenePath = GetScenePathFromAsset();
            }

            RefreshChecksum();
        }

        public void HandleAfterDeserialize() {
            EditorApplication.update -= HandleAfterDeserialize; //Remove this method from being called

            //If the SceneAsset is valid, don't do anything.
            //The SceneAsset's path will always be set based on its path.
            if (IsSceneAssetValid) return;
            if (string.IsNullOrEmpty(scenePath)) return;

            sceneAsset = GetSceneAssetFromPath();

            //If no SceneAsset was found and its path is invalid, make sure we don't keep theses values with the invalid path.
            if (!sceneAsset) scenePath = string.Empty;

            //Mark all scenes dirty if not currently playing
            if (!Application.isPlaying) EditorSceneManager.MarkAllScenesDirty();
        }

#endif

        #endregion

        #region SceneReference's External Methods

        [Conditional("UNITY_EDITOR")] public void RefreshChecksum(bool _traceinfo = true) {
            if (!IsSceneAssetValid || string.IsNullOrEmpty(scenePath)) {
                Logger.TraceWarning("Scene Reference", $"Unable to refresh the checksum (a reference is missing)!");
                return;
            }

            using (MD5 _md5 = MD5.Create()) {
                using (FileStream _stream = File.OpenRead(scenePath)) {
                    byte[] _hash = _md5.ComputeHash(_stream);
                    checksum = BitConverter.ToString(_hash).Replace("-", "").ToLowerInvariant();
                }
            }

            if (_traceinfo) Logger.Trace("Scene Reference", $"Refreshed checksum for '{sceneAsset.name}' ({checksum})");
        }

        #endregion

        #region SceneReference's Editor Methods

#if UNITY_EDITOR

        /// <summary>
        /// Returns the SceneAsset from its path on this instance.
        /// </summary>
        private SceneAsset GetSceneAssetFromPath() {
            return string.IsNullOrEmpty(scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        }

        /// <summary>
        /// Returns the SceneAsset's path of this instance.
        /// </summary>
        private string GetScenePathFromAsset() {
            return sceneAsset == null ? string.Empty : AssetDatabase.GetAssetPath(sceneAsset);
        }

#endif

        #endregion

    }

}