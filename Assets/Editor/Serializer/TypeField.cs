using System;
using System.Collections.Generic;
using Armageddon.Editor.Extensions;
using Armageddon.Serializer;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUI;
using static UnityEditor.AssetDatabase;
using static UnityEditor.EditorGUIUtility;

namespace Armageddon.Editor.Serializer {

    public struct TypeField {

        #region TypeField's Styling Class

        public static class Styles {
            public static readonly GUIStyle pickerButton = "ObjectFieldButton";
            public static readonly GUIStyle objectField = new GUIStyle("ObjectField") { richText = true };
        }

        #endregion

        #region Properties

        public Rect Position { get; }
        public Type SelectedType { get; }
        public Lazy<IEnumerable<Type>> SelectableTypes { get; }
        public Action<Type> OnSelectType { get; }
        public GUIEvent CurrentGuiEvent { get; }

        public GUIContent LabelGuiContent { get; }
        public Rect PickerButtonArea { get; }
        public ProvideSourceInfoAttribute SourceInfo { get; }

        #endregion

        #region TypeField's Constructor Methods

        public TypeField(Rect _position, Type _selectedType, Lazy<IEnumerable<Type>> _selectableTypes, Action<Type> _onSelectType, GUIEvent? _currentGuiEvent = null) {
            Position = _position;
            SelectedType = _selectedType;
            SelectableTypes = _selectableTypes;
            OnSelectType = _onSelectType;
            CurrentGuiEvent = _currentGuiEvent ?? GUIEvent.FromCurrentUnityEvent;

            LabelGuiContent = GetLabelGuiContent(_selectedType);
            PickerButtonArea = GetPickerButtonArea(_position);
            SourceInfo = GetSourceInfo(_selectedType);
        }

        #endregion

        #region TypeField's External Methods

        public static GUIContent GetLabelGuiContent(Type _type) {
            string _label = GetLabelString(_type);
            Texture _image = GetLabelImage();
            return new GUIContent(_label, _image);
        }

        public static Rect GetPickerButtonArea(Rect _position) {
            _position.height = singleLineHeight - 2f;
            _position.y += 1f;
            _position.xMax -= 1f;
            _position.xMin = _position.xMax - singleLineHeight;
            return _position;
        }

        public static ProvideSourceInfoAttribute GetSourceInfo(Type _type) {
            return _type?.GetCustomAttribute<ProvideSourceInfoAttribute>();
        }

        public static string GetLabelString(Type _type) {
            string label = _type != null ? string.IsNullOrEmpty(_type.Namespace) ? _type.Name : $"{_type.Name} <i>({_type.Namespace})</i>" : "None <i>(Type)</i>";
            return label;
        }

        public static Texture GetLabelImage() {
            return IconContent("FilterByType").image;
        }

        public void DrawGui() {
            DrawLabel();
            DrawPickerButton();
            HandleCurrentEvent();
        }

        public void DrawLabel() {
            Button(Position, LabelGuiContent, Styles.objectField);
        }

        public void DrawPickerButton() {
            Button(PickerButtonArea, GUIContent.none, Styles.pickerButton);
        }

        public void HandleCurrentEvent() {
            if (Position.Contains(CurrentGuiEvent.MousePosition) && CurrentGuiEvent.Type == EventType.MouseDown) HandleMouseDown();
        }

        public void HandleMouseDown() {
            if (PickerButtonArea.Contains(CurrentGuiEvent.MousePosition)) HandlePickerButtonClicked();
            else HandleTypeLabelClicked();
        }

        public void HandlePickerButtonClicked() {
            TypePickerWindow.ShowWindow(SelectedType, SelectableTypes.Value, OnSelectType);
        }

        public MonoScript HandleTypeLabelClicked() {
            if (SourceInfo == null) {
                if (SelectedType != null) Logger.TraceWarning($"Cannot find source script of \"{SelectedType.FullName}\" because it doesn't have a \"{nameof(ProvideSourceInfoAttribute)}\".");
                return null;
            }
            
            MonoScript _monoScript = LoadAssetAtPath<MonoScript>(SourceInfo.AssetPath);
            
            if (CurrentGuiEvent.ClickCount > 1) OpenAsset(_monoScript, SourceInfo.LineNumber);
            else PingObject(_monoScript);
            
            return _monoScript;
        }
        
        #endregion
        
    }

}