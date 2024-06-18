using UnityEngine;

namespace Armageddon.Editor.Serializer {
    
    public struct GUIEvent {

        #region Properties

        public EventType Type { get; set; }
        public Vector2 MousePosition { get; }
        public int ClickCount { get; }
        public int Button { get; }

        public static GUIEvent FromCurrentUnityEvent {
            get {
                Event _event = Event.current;
                return new GUIEvent(_event.type, _event.mousePosition, _event.clickCount, _event.button);
            }
        }
        
        #endregion

        #region GUIEvent's Constructor Method

        public GUIEvent(EventType _type, Vector2 _mousePosition, int _clickCount, int _button) {
            Type = _type;
            MousePosition = _mousePosition;
            ClickCount = _clickCount;
            Button = _button;
        }

        #endregion
        
    }
    
}