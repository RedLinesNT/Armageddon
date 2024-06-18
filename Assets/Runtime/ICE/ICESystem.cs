using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

#pragma warning disable CS0162 //Unreachable code detected

namespace Armageddon.ICE {
    
    /// <summary>
    /// Manage and control the cameras throughout the game.
    /// </summary>
    public static class ICESystem {
        
        #region Attributes

        /// <summary>
        /// The <see cref="List{T}"/> of every <see cref="AICEVirtualCamera"/>s registered.
        /// </summary>
        private static List<AICEVirtualCamera> virtualCameras = new List<AICEVirtualCamera>();

        #endregion
        
        #region Events

        /// <summary>
        /// Invoked when a <see cref="AICEVirtualCamera"/> has been registered.
        /// </summary>
        /// <param name="01">The <see cref="AICEVirtualCamera"/> that have been registered.</param>
        /// <param name="02">The <see cref="EICEVirtualCameraTargetUsage"/> of this <see cref="AICEVirtualCamera"/>.</param>
        private static Action<AICEVirtualCamera, EICEVirtualCameraTargetUsage> onVirtualCameraRegistered = null;
        /// <summary>
        /// Invoked when a <see cref="AICEVirtualCamera"/> has been unregistered.
        /// </summary>
        /// <param name="01">The <see cref="AICEVirtualCamera"/> that have been unregistered.</param>
        /// <param name="02">The <see cref="EICEVirtualCameraTargetUsage"/> of this <see cref="AICEVirtualCamera"/>.</param>
        private static Action<AICEVirtualCamera, EICEVirtualCameraTargetUsage> onVirtualCameraUnregistered = null;
        
        #endregion
        
        #region Properties

        /// <inheritdoc cref="virtualCameras"/>
        public static ReadOnlyCollection<AICEVirtualCamera> VirtualCameras { get { return virtualCameras.AsReadOnly(); } }
        /// <summary>
        /// The number of <see cref="AICEVirtualCamera"/>s currently registered.
        /// </summary>
        public static int VirtualCamerasRegisteredCount { get { return virtualCameras.Count; } }
        
        /// <summary>
        /// The active <see cref="AICEVirtualCamera"/> currently used.
        /// </summary>
        public static AICEVirtualCamera ActiveVirtualCamera { get; private set; }
        /// <inheritdoc cref="ICEBrainCamera.Instance"/>
        public static ICEBrainCamera BrainCamera { get { return ICEBrainCamera.Instance; } }
        
        /// <inheritdoc cref="onVirtualCameraRegistered"/>
        public static event Action<AICEVirtualCamera, EICEVirtualCameraTargetUsage> OnVirtualCameraRegistered { add { onVirtualCameraRegistered += value; } remove { onVirtualCameraRegistered -= value; } } 
        /// <inheritdoc cref="onVirtualCameraUnregistered"/>
        public static event Action<AICEVirtualCamera, EICEVirtualCameraTargetUsage> OnVirtualCameraUnregistered { add { onVirtualCameraUnregistered += value; } remove { onVirtualCameraUnregistered -= value; } } 
        
        #endregion
        
        #region ICESystem's Initialization Method

        /// <summary>
        /// Initialize the <see cref="ICESystem"/> and create the <see cref="ICEBrainCamera"/> instance.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Initialize() {
            ICEBrainCamera.Instance.name = $"ICE Brain Camera"; //Set the default name. Doing this will wake up and create a new ICE Brain Camera instance
        }

        #endregion
        
        #region ICESystem's External Registering Methods

        /// <summary>
        /// Register a <see cref="AICEVirtualCamera"/> in the <see cref="ICESystem"/>.
        /// </summary>
        /// <param name="_virtualCamera">The <see cref="AICEVirtualCamera"/> to register.</param>
        public static void RegisterVirtualCamera(AICEVirtualCamera _virtualCamera) {
            bool _isCameraRegistered = virtualCameras.Contains(_virtualCamera); //Is this ICE Virtual Camera already registered
            
            if(_isCameraRegistered) { //If this ICE Virtual Camera is already registered
                Logger.TraceWarning("ICE", $"Unable to register the Virtual Camera '{_virtualCamera.name}'. This Virtual Camera is already registered!");
                return; //Don't execute anything
            }
            
            if(_virtualCamera.VirtualCamera == null) { //If there's no Virtual Camera component reference
                Logger.TraceError("ICE", $"Unable to register the Virtual Camera '{_virtualCamera.name}'. No Virtual Camera component could be found! (This ICE Virtual Camera will be destroyed)");
                Object.Destroy(_virtualCamera); //Destroy this invalid Virtual Camera
                return;
            }
            
            Logger.Trace("ICE", $"Virtual Camera '{_virtualCamera.Name}' has been registered! (Target Usage: {_virtualCamera.TargetUsage})");
            
            virtualCameras.Add(_virtualCamera); //Add it into the list
            onVirtualCameraRegistered?.Invoke(_virtualCamera, _virtualCamera.TargetUsage); //Invoke this event
        }
        
        /// <summary>
        /// Unregister a <see cref="AICEVirtualCamera"/> from the <see cref="ICESystem"/>.
        /// </summary>
        /// <param name="_virtualCamera">The <see cref="AICEVirtualCamera"/> to unregister.</param>
        public static void UnregisterVirtualCamera(AICEVirtualCamera _virtualCamera) {
            bool _isCameraRegistered = virtualCameras.Contains(_virtualCamera); //Is this ICE Virtual Camera already registered
            
            if(!_isCameraRegistered) { //If this ICE Virtual Camera isn't already registered
                Logger.TraceWarning("ICE", $"Unable to unregister the Virtual Camera '{_virtualCamera.name}'. This Virtual Camera isn't registered!");
                return; //Don't execute anything
            }
            
            Logger.Trace("ICE", $"Virtual Camera '{_virtualCamera.Name}' has been unregistered!");
            
            onVirtualCameraUnregistered?.Invoke(_virtualCamera, _virtualCamera.TargetUsage); //Invoke this event
            virtualCameras.Remove(_virtualCamera); //Remove it from the list
        }

        #endregion
        
        #region ICESystem's External Methods

        /// <summary>
        /// Set the target <see cref="AICEVirtualCamera"/> that should be used.
        /// </summary>
        public static void SetMainVirtualCamera(AICEVirtualCamera _virtualCamera) {
            if(_virtualCamera == null) return; //Don't execute anything if the value given is null
                
            for(int i=0; i<virtualCameras.Count; i++) {
                if(virtualCameras[i] == _virtualCamera) {
                    virtualCameras[i].VirtualCamera.Priority = 10;
                    virtualCameras[i].OnVirtualCameraShowed();
                } else if(virtualCameras[i] == ActiveVirtualCamera) {
                    ActiveVirtualCamera.VirtualCamera.Priority = 0;
                    ActiveVirtualCamera.OnVirtualCameraHidden();
                } else {
                    virtualCameras[i].VirtualCamera.Priority = 0;
                }
            }
            
            ActiveVirtualCamera = _virtualCamera;
            BrainCamera.name = $"Ice Brain Camera (Target {ActiveVirtualCamera.Name})";
            
            Logger.Trace("ICE", $"The active Virtual Camera has been modified! (Virtual Camera '{ActiveVirtualCamera.Name}' Target usage: {ActiveVirtualCamera.TargetUsage})");
        }
        
        #endregion

        #region ICESystem's Getters

        /// <summary>
        /// Find the first or default <see cref="AICEVirtualCamera"/> from its <see cref="EICEVirtualCameraTargetUsage"/>.
        /// </summary>
        public static AICEVirtualCamera FindFirstOrDefaultVirtualCamera(EICEVirtualCameraTargetUsage _targetUsage) {
            return virtualCameras.FirstOrDefault(_cam => _cam.TargetUsage == _targetUsage);
        }
        
        /// <summary>
        /// Find the first or default <see cref="AICEVirtualCamera"/> from its <see cref="Guid"/>.
        /// </summary>
        public static AICEVirtualCamera FindFirstOrDefaultVirtualCamera(Guid _cameraID) {
            return virtualCameras.FirstOrDefault(_cam => _cam.Identifier == _cameraID);
        }

        /// <summary>
        /// Find and return a <see cref="List{T}"/> of every <see cref="AICEVirtualCamera"/> registered using a specific <see cref="EICEVirtualCameraTargetUsage"/>.
        /// </summary>
        public static List<AICEVirtualCamera> FindVirtualCamerasFromTargetUsage(EICEVirtualCameraTargetUsage _targetUsage) {
            return virtualCameras.Where(_camera => _camera.TargetUsage == _targetUsage).ToList();
        }
        
        #endregion
        
    }
    
}