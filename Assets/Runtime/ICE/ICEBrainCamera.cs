using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Armageddon.ICE {
    
    /// <summary>
    /// <see cref="ICEBrainCamera"/> is a single-instance main camera of the entire game used by the <see cref="ICESystem"/>.
    /// </summary>
    public class ICEBrainCamera : MonoBehaviour {

        #region Attributes

        /// <summary>
        /// The instance of this <see cref="ICEBrainCamera"/>.
        /// </summary>
        private static ICEBrainCamera instance = null;

        #endregion
        
        #region Properties
        
        /// <inheritdoc cref="instance"/>
        public static ICEBrainCamera Instance {
            get {
                if (instance != null) return instance; //If there's an instance running
                
                //Else, if there's no instance running
                new GameObject("ICE Brain Camera", typeof(ICEBrainCamera)); //Create a new Object with our component
                return instance; //Return the new instance
            }
        }
        
        /// <summary>
        /// The <see cref="UnityEngine.Camera"/> component used by this <see cref="ICEBrainCamera"/>.
        /// </summary>
        [field: SerializeField, Header("ICE Brain Camera - References"), Tooltip("The Unity Engine camera component.")] public Camera Camera { get; private set; }
        /// <summary>
        /// The <see cref="CinemachineBrain"/> component used by this <see cref="ICEBrainCamera"/>.
        /// </summary>
        [field: SerializeField, Tooltip("The Cinemachine Brain component.")] public CinemachineBrain Brain { get; private set; }
        
        /// <summary>
        /// The <see cref="UnityEngine.AudioListener"/> component used by this <see cref="ICEBrainCamera"/>.
        /// </summary>
        public AudioListener AudioListener { get; private set; } = null;
        /// <summary>
        /// The <see cref="UniversalAdditionalCameraData"/> component used by this <see cref="ICEBrainCamera"/>.
        /// </summary>
        public UniversalAdditionalCameraData URPCameraData { get; private set; } = null;

        #endregion

        #region MonoBehaviour's Methods

        private void Awake() {
            if(instance != null) { //If there's already an instance running
                Logger.TraceWarning("ICE Brain Camera", $"Unable to create a new {nameof(ICEBrainCamera)} component. There's already an instance running!");
                DestroyImmediate(this.gameObject); //Destroy this object
                return; 
            }
            
            instance = this; //Set the new instance
            
            DontDestroyOnLoad(this);

            if (Camera == null) {
                gameObject.AddComponent<Camera>(); //If there's no Camera component assigned
                AudioListener = gameObject.AddComponent<AudioListener>(); //Also add this
            }
            if (Brain == null) Brain = gameObject.AddComponent<CinemachineBrain>(); //If there's no CinemachineBrain component assigned
            URPCameraData = GetComponent<UniversalAdditionalCameraData>();
            if (URPCameraData == null) {
                URPCameraData = gameObject.AddComponent<UniversalAdditionalCameraData>();
            }
            
            Brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f); //Set the default blend
            
            Logger.Trace("ICE Brain Camera", $"New instance created.");
        }

        private void Start() {
            URPCameraData.renderPostProcessing = true; //Allow Post-Processing
        }

        #endregion
        
    }
    
}