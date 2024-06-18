using System;
using UnityEngine;
using Cinemachine;

namespace Armageddon.ICE {
    
    /// <summary>
    /// <see cref="AICEVirtualCamera"/> allows a <see cref="CinemachineVirtualCamera"/> to be registered within the system managing them, the <see cref="ICESystem"/>.
    /// </summary>
    /// <remarks>
    /// There are different methods for classes inheriting from a <see cref="AICEVirtualCamera"/>, which want to use <see cref="MonoBehaviour"/>'s methods such as "<i>Start</i>", "<i>Update</i>", ...<br/>
    /// See the virtual methods of this class for more details.
    /// </remarks>
    /// <inEditor>
    /// You can tune and setup basic settings of this instance via the Editor's GUI if not dynamically generated at runtime.
    /// </inEditor>
    public abstract class AICEVirtualCamera : MonoBehaviour {
        
        #region Properties

        /// <summary>
        /// The name of this <see cref="AICEVirtualCamera"/>.
        /// </summary>
        public string Name { get { return name; } private protected set { name = value; } }
        /// <summary>
        /// The Unique Identifier of this <see cref="AICEVirtualCamera"/>.
        /// </summary>
        [field: SerializeField, ReadOnly] public Guid Identifier { get; private set; } = default;
        /// <summary>
        /// The <see cref="CinemachineVirtualCamera"/> component.
        /// </summary>
        /// <remarks>
        /// This component is auto-generated if no reference is given.
        /// </remarks>
        [field: SerializeField, Tooltip("The Cinemachine Camera component.\nThis component is auto-generated if no reference is given.")] public CinemachineVirtualCamera VirtualCamera { get; private set; } = null;
        /// <summary>
        /// The target usage of this <see cref="AICEVirtualCamera"/>.
        /// </summary>
        [field: SerializeField, Tooltip("The target usage of this ICE Virtual Camera.")] public EICEVirtualCameraTargetUsage TargetUsage { get; private protected set; }
        
        #endregion
        
        #region MonoBehaviour's Methods

        private void Awake() {
            if(VirtualCamera == null) { //If no Virtual Camera component was assigned
                VirtualCamera = gameObject.AddComponent<CinemachineVirtualCamera>(); //Create the Virtual Camera component
            }
            
            VirtualCamera.Priority = 0; //Set the default priority value of the Virtual Camera
            
            ICESystem.RegisterVirtualCamera(this);
            
            OnAwakeCamera(); //Call this method on every sub-instances
        }
        private void OnDestroy() {
            ICESystem.UnregisterVirtualCamera(this);
            
            OnDestroyCamera(); //Call this method on every sub-instances
        }
        private void Start() => OnStartCamera();
        private void OnEnable() => OnEnableCamera();
        private void OnDisable() => OnDisableCamera();
        private void Update() => OnUpdateCamera();
        private void FixedUpdate() => OnFixedUpdateCamera();
        private void LateUpdate() => OnLateUpdateCamera();

        #endregion
        
        #region AICEVirtualCamera's Internal Virtual Methods

        /// <summary>
        /// <see cref="OnAwakeCamera"/> is called when an enabled script instance is being loaded.
        /// </summary>
        private protected virtual void OnAwakeCamera(){}
        /// <summary>
        /// <see cref="OnStartCamera"/> is called on the frame when a script is enabled just before any of the Update methods are called for the first frame.
        /// </summary>
        private protected virtual void OnStartCamera(){}
        /// <summary>
        /// <see cref="OnEnableCamera"/> is called when the object becomes enabled and active.
        /// </summary>
        private protected virtual void OnEnableCamera(){}
        /// <summary>
        /// <see cref="OnDisableCamera"/> is called when the behaviour becomes disabled.
        /// </summary>
        private protected virtual void OnDisableCamera(){}
        /// <summary>
        /// Destroying the attached Behaviour will result in the game or Scene receiving OnDestroy.
        /// </summary>
        private protected virtual void OnDestroyCamera() {}
        /// <summary>
        /// <see cref="OnUpdateCamera"/> is called every frames.
        /// </summary>
        private protected virtual void OnUpdateCamera(){}
        /// <summary>
        /// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
        /// </summary>
        private protected virtual void OnFixedUpdateCamera(){}
        /// <summary>
        /// <see cref="OnLateUpdateCamera"/> is called every frame, if the Behaviour is enabled.
        /// </summary>
        private protected virtual void OnLateUpdateCamera(){}

        #endregion

        #region AICEVirtualCamera's External Virtual Methods

        /// <summary>
        /// <see cref="OnVirtualCameraShowed"/> is called when this <see cref="AICEVirtualCamera"/> is the main camera.
        /// </summary>
        /// <remarks>
        /// Must be called by the <see cref="ICESystem"/>!
        /// </remarks>
        public virtual void OnVirtualCameraShowed(){}
        /// <summary>
        /// <see cref="OnVirtualCameraHidden"/> is called when this <see cref="AICEVirtualCamera"/> is no longer the main camera.
        /// </summary>
        /// <remarks>
        /// Must be called by the <see cref="ICESystem"/>!
        /// </remarks>
        public virtual void OnVirtualCameraHidden(){}

        #endregion
        
    }
    
}