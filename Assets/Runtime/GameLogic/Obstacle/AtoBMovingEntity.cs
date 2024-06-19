using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Armageddon.GameLogic.Obstacle {

    /// <summary>
    /// Settings for <see cref="AtoBMovingEntity"/> entities.
    /// </summary>
    [System.Serializable] public struct AtoBMovingEntitySettings {
        
        /// <summary>
        /// The "A" point of the <see cref="AtoBMovingEntity"/>.
        /// </summary>
        [field: SerializeField, Header("References"), Tooltip("The A point of the moving entity.")] public Transform APoint { get; private set; }
        /// <summary>
        /// The "B" point of the <see cref="AtoBMovingEntity"/>.
        /// </summary>
        [field: SerializeField, Tooltip("The B point of the moving obstacle.")] public Transform BPoint { get; private set; }
        /// <summary>
        /// The actual <see cref="GameObject"/> to move.
        /// </summary>
        [field: SerializeField, Tooltip("The actual object to move.")] public GameObject TargetObject { get; private set; }
        
        /// <summary>
        /// The movement speed of the moving entity.
        /// </summary>
        [field: SerializeField, Header("Settings"), Tooltip("The time for the obstacle to moving from one point to another.")] public float TravelTime { get; private set; }
        /// <summary>
        /// The stop time when the obstacle reached a target point.
        /// </summary>
        [field: SerializeField, Tooltip("The stop time when the obstacle reached a target point.")] public float StopTime { get; private set; }
        /// <summary>
        /// SHould the <see cref="AtoBMovingEntity"/> starts its journey at the A point ?
        /// </summary>
        [field: SerializeField, Tooltip("Should this entity starts its journey at the A point ?")] public bool StartFromAPoint { get; private set; }

    }
    
    /// <summary>
    /// A simple obstale moving from one point to another.
    /// Mainly used to spice-up your levels :)
    /// </summary>
    public class AtoBMovingEntity : MonoBehaviour {

        #region Properties

        /// <inheritdoc cref="AtoBMovingEntitySettings"/>
        [field: SerializeField, Tooltip("The settings of this 'A to B Moving Entity'.")] public AtoBMovingEntitySettings Settings { get; private set; } = default(AtoBMovingEntitySettings);

        /// <summary>
        /// Is this <see cref="AtoBMovingEntity"/> currently moving ?
        /// </summary>
        [field: SerializeField, ReadOnly, Tooltip("Is this moving entity currently moving ?")] public bool IsMoving { get; private set; } = false;
        
        #endregion

        #region MonoBehaviour's Methods

        private void Start() {
            Settings.TargetObject.transform.position = Settings.StartFromAPoint ? Settings.APoint.position : Settings.BPoint.position; //Set the default position
            
            StartCoroutine(LoopLogic()); //Start the whole loop
        }

        #endregion
        
        #region AtoBMovingEntity's Internal Methods

        /// <summary>
        /// Simply start the whole movement loop for this <see cref="AtoBMovingEntity"/>.
        /// </summary>
        /// <remarks>
        /// Note to myself, why not optimizing this trash thing ?
        /// </remarks>
        private IEnumerator LoopLogic() {
            if (Settings.StartFromAPoint) {
                yield return MoveObject(Settings.APoint); //Go to the A point
                yield return MoveObject(Settings.BPoint); //Then to the B one
            } else {
                yield return MoveObject(Settings.BPoint); //Go to the B point
                yield return MoveObject(Settings.APoint); //Then to the A one
            }
            
            StartCoroutine(LoopLogic()); //Do it again
        }
        
        /// <summary>
        /// Move this <see cref="AtoBMovingEntity"/> to another point using the <see cref="Settings"/> provided.
        /// </summary>
        /// <param name="_targetPosition">The target <see cref="Transform"/>.</param>
        private IEnumerator MoveObject(Transform _targetPosition) {
            float _elapsedTime = 0.0f; //The time elapsed
            
            Vector3 _startPosition = Settings.TargetObject.transform.position; //The start position
            Vector3 _endPosition = _targetPosition.position; //The target position

            IsMoving = true;
            
            while (_elapsedTime < Settings.TravelTime) {
                Settings.TargetObject.transform.position = Vector3.Lerp(_startPosition, _endPosition, (_elapsedTime / Settings.TravelTime));

                _elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            //Correctly set target values
            Settings.TargetObject.transform.position = _endPosition;
            IsMoving = false;

            yield return new WaitForSeconds(Settings.StopTime); //Wait
            
            yield return null;
        }

        #endregion
        
        #region AtoBMovingEntity's Editor Internal Methods

        [Conditional("UNITY_EDITOR")] private void OnDrawGizmos() {
            if (Settings.Equals(default(AtoBMovingEntitySettings))) return; //The entity currently doesn't have any valid settings
            if (Settings.APoint is null || Settings.BPoint is null) return; //Some target points are missing
            
            Gizmos.color = IsMoving ? Color.green : Color.red; { //The "moving" line
                Gizmos.DrawLine(Settings.APoint.position, Settings.BPoint.position);
            }

            Gizmos.color = Color.gray; {
                Gizmos.DrawSphere(Settings.APoint.position, 0.1f);
                Gizmos.DrawSphere(Settings.BPoint.position, 0.1f);
            }
        }

        #endregion

    }
    
}