using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Armageddon.Serializer {
    
    [Conditional("UNITY_EDITOR")] public class TypeFilterAttribute : PropertyAttribute {

        #region Properties

        public Type DerivedFrom { get; }
        public string FilterName { get; }
        public Func<IEnumerable<Type>, IEnumerable<Type>> DerivedFromFilter => sequence => sequence;
        public Func<IEnumerable<Type>, IEnumerable<Type>> Filter { get; protected set; }
        

        #endregion

        #region TypeFilterAttribute's Constructor Methods

        public TypeFilterAttribute(Type _derivedFrom) {
            DerivedFrom = _derivedFrom;
            Filter = DerivedFromFilter;
        }

        public TypeFilterAttribute(string _filterName) {
            FilterName = _filterName;
        }

        #endregion

        #region TypeFilterAttribute's External Methods

        public Func<IEnumerable<Type>, IEnumerable<Type>> GetFilter(object _parent) {
            return Filter ?? BindFilterDelegate(_parent);
        }

        public Func<IEnumerable<Type>, IEnumerable<Type>> BindFilterDelegate(object _parent) {
            Filter = (Func<IEnumerable<Type>, IEnumerable<Type>>)Delegate.CreateDelegate(typeof(Func<IEnumerable<Type>, IEnumerable<Type>>), _parent, FilterName);
            return Filter;
        }

        #endregion
        
    }
    
}