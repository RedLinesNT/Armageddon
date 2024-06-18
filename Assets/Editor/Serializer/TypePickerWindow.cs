using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Armageddon.Editor.Serializer {

    public class TypePickerWindow : EditorWindow {

        #region Constants

        public const string ContentContainer = "content-container";
        public const string TypeIcon = "type-icon";
        public const string TypeNameLabel = "type-name";
        public const string TypeNamespaceLabel = "type-namespace";
        public const string TypeListName = "type-list";
        public const string SearchFieldName = "search-field";
        public const string WindowAssetPath = "type_picker_window";
        public const string ItemAssetPath = "type_picker_item";

        public static readonly Regex TypeLabelRegex = new Regex(@"^(?<name>\w+\s?)(\<i\>(?<namespace>[\w\.\(\)]*)\</i\>)?", RegexOptions.Compiled);
        
        #endregion

        #region Attributes

        private VisualTreeAsset itemVisualTreeAsset;
        private Type preselected;
        private IEnumerable<Type> types;
        private List<Type> searchedTypes;
        private Action<Type> onSelected;
        private string searchValue;
        private ListView listView;

        private ToolbarSearchField searchField;

        private ToolbarSearchField SearchField { get { return searchField ?? (searchField = rootVisualElement.Q<ToolbarSearchField>(name: SearchFieldName)); } }

        #endregion

        #region TypePickerWindow's External Methods

        public static TypePickerWindow ShowWindow(Type _preselected, IEnumerable<Type> _types, Action<Type> _onSelected, string _title = null) {
            TypePickerWindow _window = GetWindow<TypePickerWindow>(utility: true, title: _title ?? "Select Type");
            _window.InitData(_preselected, _types, _onSelected);
            _window.ShowAuxWindow();
            return _window;
        }

        public void InitData(Type _preselected, IEnumerable<Type> _types, Action<Type> _onSelected) {
            this.preselected = _preselected;
            this.types = _types;
            this.onSelected = _onSelected;
            UpdateTypeSearch(null);
            SearchField.RegisterValueChangedCallback(UpdateTypeSearch);
        }

        void OnEnable() {
            InitWindow();
            InitListView();
#if UNITY_2020_1_OR_NEWER
            listView.itemsChosen += OnItemsChosen;
#else
            listView.onItemChosen += OnItemChosen;
#endif
        }

        void OnDisable() {
#if UNITY_2020_1_OR_NEWER
            listView.itemsChosen -= OnItemsChosen;
#else
            listView.onItemChosen -= OnItemChosen;
#endif
        }

        private void InitWindow() {
            VisualTreeAsset _windowVisualTreeAsset = Resources.Load<VisualTreeAsset>(WindowAssetPath);
            _windowVisualTreeAsset.CloneTree(rootVisualElement);
            itemVisualTreeAsset = Resources.Load<VisualTreeAsset>(ItemAssetPath);
        }

        public VisualElement MakeItem() { 
            return itemVisualTreeAsset.CloneTree().Q<VisualElement>(name: ContentContainer); 
        }

        public void BindItem(VisualElement _element, int _index) {
            Type _type = searchedTypes.ElementAt(_index);
            string _text = TypeField.GetLabelString(_type);
            GroupCollection _groups = TypeLabelRegex.Match(_text).Groups;

            Group _typeNameGroup = _groups["name"];
            Label _typeNameLabel = _element.Q<Label>(name: TypeNameLabel);
            _typeNameLabel.text = _typeNameGroup.Success ? _typeNameGroup.Value : string.Empty;

            Group _typeNamespaceGroup = _groups["namespace"];
            Label _typeNamespaceLabel = _element.Q<Label>(name: TypeNamespaceLabel);
            _typeNamespaceLabel.text = _typeNamespaceGroup.Success ? _typeNamespaceGroup.Value : string.Empty;

            VisualElement typeIcon = _element.Q<VisualElement>(name: TypeIcon);
            typeIcon.style.backgroundImage = (Texture2D)TypeField.GetLabelImage();
        }

        public void OnItemChosen(object _item) {
            onSelected?.Invoke((Type)_item);
            Close();
        }

        public void UpdateTypeSearch(ChangeEvent<string> _stringChangeEvent) {
            searchValue = _stringChangeEvent?.newValue;
            searchedTypes = types.Where(IsInSearch).Prepend(null).ToList();
            listView.itemsSource = searchedTypes;
            listView.selectedIndex = searchedTypes.IndexOf(preselected);
#if UNITY_2021_2_OR_NEWER
            listView.Rebuild();
#else
            listView.Refresh();
#endif
        }

        public bool IsInSearch(Type _type) {
            return string.IsNullOrEmpty(searchValue) || _type.AssemblyQualifiedName.Contains(searchValue);
        }

        #endregion

        #region TypePickerWindow's Internal Methods

        private ListView InitListView() {
            listView = rootVisualElement.Q<ListView>(name: TypeListName);
            listView.selectionType = SelectionType.Single;
            listView.itemsSource = searchedTypes;
            listView.makeItem = MakeItem;
            listView.bindItem = BindItem;
            return listView;
        }
        
#if UNITY_2020_1_OR_NEWER
        private void OnItemsChosen(IEnumerable<object> _items) {
            OnItemChosen(_items.FirstOrDefault());
        }
#endif
        
        #endregion
        
    }

}