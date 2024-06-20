using UnityEngine;

namespace Armageddon.GameLogic.Camera {
    
    /// <summary>
    /// The camera for the most sane and classic controller you'll find in this game! (<see cref="ClassicCharacterCamera"/>)
    /// </summary>
    public class ClassicCharacterCamera : ACharacterCamera {

        private protected override void OnUpdateCamera() {
            base.OnUpdateCamera();
            
            transform.LookAt(Settings.Pivot.ChargeLine.Line.transform.TransformPoint(Settings.Pivot.ChargeLine.Line.GetPosition(1)));
        }
        
    }
    
}