using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Armageddon.Exceptions;

namespace Armageddon.Utils {
    
    public static class ReflectionUtils {
        
        #region Attributes

        private static Assembly[] allAssemblies = null;

        #endregion

        #region Properties

        public static Assembly[] AllAssemblies { get { return allAssemblies ??= AppDomain.CurrentDomain.GetAssemblies(); } }

        #endregion

        #region ReflectionUtils' External Methods

        public static IEnumerable<Type> GetAllTypesWithAttributeAsEnumerable(this Assembly _assembly, Type _attribute) {
            foreach (Type _type in _assembly.GetTypes()) {
                if (_type.GetCustomAttributes(_attribute.GetType(), true).Length > 0) {
                    yield return _type;
                }
            }
        }

        public static Type[] GetAllTypesWithAttribute(this Assembly _assembly, Type _attribute) {
            return _assembly.GetAllTypesWithAttributeAsEnumerable(_attribute).ToArray();
        }

        /// <summary>
        /// Finds the most nested object inside of an object.
        /// </summary>
        public static T GetNestedObject<T>(this object _obj, string _path) {
            foreach (string _part in _path.Split('.')) {
                _obj = _obj.GetFieldOrProperty<T>(_part);
            }
            return (T)_obj;
        }

        /// <summary>
        /// Gets a property or a field of an object by a name.
        /// </summary>
        /// <typeparam name="T">Type of the field/property.</typeparam>
        /// <param name="_obj">Object the field/property should be found in.</param>
        /// <param name="_name">Name of the field/property.</param>
        /// <param name="_bindingFlags">Filters for the field/property it can find. (optional)</param>
        /// <returns>The field/property.</returns>
        public static T GetFieldOrProperty<T>(this object _obj, string _name, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            try { //Try getting the field. If the property wasn't found...
                return GetField<T>(_obj, _name, _bindingFlags);
            } catch (FieldNotFoundException) { //...try getting the property. If that wasn't found as well, throw an exception
                try {
                    return GetProperty<T>(_obj, _name, _bindingFlags);
                } catch (PropertyNotFoundException) {
                    throw new PropertyOrFieldNotFoundException($"Couldn't find a filed nor a property with the name of '{_name}' inside of the object '{_obj.GetType().Name}'");
                }
            }
        }

        /// <summary>
        /// Gets a field inside of an object by a name.
        /// </summary>
        /// <typeparam name="T">Type of the field.</typeparam>
        /// <param name="_obj">Object the field should be found in.</param>
        /// <param name="_name">Name of the field.</param>
        /// <param name="_bindingFlags">Filters for the fields it can find. (optional)</param>
        /// <returns>The field.</returns>
        public static T GetField<T>(this object _obj, string _name, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            FieldInfo _field = _obj.GetType().GetField(_name, _bindingFlags); //Try getting the field and returning it.
            if (_field != null) return (T)_field.GetValue(_obj);

            //If a field couldn't be found. Throw an exception about it.
            throw new PropertyOrFieldNotFoundException($"Couldn't find a filed nor a property with the name of '{_name}' inside of the object '{_obj.GetType().Name}'");
        }

        /// <summary>
        /// Gets a property inside of an object by a name.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="_obj">Object the property should be found in.</param>
        /// <param name="_name">Name of the property.</param>
        /// <param name="_bindingFlags">Filters for the properties it can find. (optional)</param>
        /// <returns>The property.</returns>
        public static T GetProperty<T>(this object _obj, string _name, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            PropertyInfo _property = _obj.GetType().GetProperty(_name, _bindingFlags); //Try getting the field and returning it.
            if (_property != null) return (T)_property.GetValue(_obj, null);

            //If a field couldn't be found. Throw an exception about it.
            throw new PropertyOrFieldNotFoundException($"Couldn't find a field with the name of '{_name}' inside of the object '{_obj.GetType().Name}'");
        }

        /// <summary>
        /// Sets a field or a property inside of an object by name.
        /// </summary>
        /// <typeparam name="T">Type of the field/property.</typeparam>
        /// <param name="_obj">Object containing the field/property.</param>
        /// <param name="_name">Name of the field/property.</param>
        /// <param name="_value">New value of the field/property.</param>
        /// <param name="_bindingFlags">Filters for the field/property it can find. (optional)</param>
        public static void SetFieldOrProperty<T>(this object _obj, string _name, T _value, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            try { //Try getting the field. If the property wasn't found...
                SetField(_obj, _name, _value, _bindingFlags);
                return;
            } catch (FieldNotFoundException) {
                try {
                    SetProperty(_obj, _name, _value, _bindingFlags);
                    return;
                } catch (PropertyNotFoundException) {
                    throw new PropertyOrFieldNotFoundException($"Couldn't find a property with the name of '{_name}' inside of the object '{_obj.GetType().Name}'");
                }
            }

        }

        /// <summary>
        /// Sets a field inside of an object by name.
        /// </summary>
        /// <typeparam name="T">Type of the field.</typeparam>
        /// <param name="_obj">Object containing the field.</param>
        /// <param name="_name">Name of the field.</param>
        /// <param name="_value">New value of the field.</param>
        /// <param name="_bindingFlags">Filters for the fields it can find. (optional)</param>>
        public static void SetField<T>(this object _obj, string _name, T _value, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            //Try getting the field and returning it.
            FieldInfo _field = _obj.GetType().GetField(_name, _bindingFlags);
            if (_field != null) {
                _field.SetValue(_obj, _value);
                return;
            }

            //If a field couldn't be found. Throw an exception about it.
            throw new FieldNotFoundException($"Couldn't find a field with the name of '{_name}' inside of the object '{_obj.GetType().Name}'");
        }

        /// <summary>
        /// Sets a property inside of an object by name.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="_obj">Object containing the property.</param>
        /// <param name="_name">Name of the property.</param>
        /// <param name="_value">New value of the property.</param>
        /// <param name="_bindingFlags">Filters for the properties it can find. (optional)</param>
        public static void SetProperty<T>(this object _obj, string _name, T _value, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            //Try getting the field and returning it.
            PropertyInfo _property = _obj.GetType().GetProperty(_name, _bindingFlags);
            if (_property != null) {
                _property.SetValue(_obj, _value, null);
                return;
            }

            // If a field couldn't be found. Throw an exception about it.
            throw new PropertyNotFoundException($"Couldn't find a property with the name of '{_name}' inside of the object '{_obj.GetType().Name}'");
        }

        /// <summary>
        /// Gets all the properties and fields in obj of type T.
        /// </summary>
        /// <typeparam name="T">The type of the fields/properties.</typeparam>
        /// <param name="_obj">Object to find the fields/properties in.</param>
        /// <param name="_bindingFlags">Filters for the types of fields/properties that can be found.</param>
        /// <returns>The fields/properties found.</returns>
        public static IEnumerable<T> GetAllFieldsOrProperties<T>(this object _obj, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            //Get the fields and the properties in the object.
            T[] _fields = _obj.GetAllFields<T>(_bindingFlags).ToArray();
            T[] _properties = _obj.GetAllProperties<T>(_bindingFlags).ToArray();

            //Only return the fields if fields were found.
            if (_fields.Length != 0) {
                //Loop through the fields and return each one.
                for (int i = 0; i < _fields.Length; i++) {
                    yield return _fields[i];
                }
            }

            //Only return the properties if properties were found.
            if (_properties.Length != 0) {
                //Loop through the properties and return each one if they have the right type.
                for (int i = 0; i < _properties.Length; i++) {
                    yield return _properties[i];
                }
            }
        }

        /// <summary>
        /// Gets all the properties and fields in obj.
        /// </summary>
        /// <param name="_obj">Object to find the fields/properties in.</param>
        /// <param name="_bindingFlags">Filters for the types of fields/properties that can be found.</param>
        /// <returns>The fields/properties found.</returns>
        public static IEnumerable GetAllFieldsOrProperties(this object _obj, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            //Get the fields and the properties in the object.
            object[] _fields = _obj.GetAllFields(_bindingFlags).Cast<object>().ToArray();
            object[] _properties = _obj.GetAllProperties(_bindingFlags).Cast<object>().ToArray();

            //Only return the fields if fields were found.
            if (_fields.Length != 0) {
                //Loop through the fields and return each one.
                for (int i = 0; i < _fields.Length; i++) {
                    yield return _fields[i];
                }
            }

            //Only return the properties if properties were found.
            if (_properties.Length != 0) {
                //Loop through the properties and return each one if they have the right type.
                for (int i = 0; i < _properties.Length; i++) {
                    yield return _properties[i];
                }
            }
        }

        /// <summary>
        /// Gets all the fields in obj of type T.
        /// </summary>
        /// <typeparam name="T">Type of the fields allowed.</typeparam>
        /// <param name="_obj">Object to find the fields in.</param>
        /// <param name="_bindingFlags">Filters of the fields allowed.</param>
        /// <returns>The fields found.</returns>
        public static IEnumerable<T> GetAllFields<T>(this object _obj, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            //Get all the properties.
            FieldInfo[] _fields = _obj.GetType().GetFields(_bindingFlags);

            //If there are no properties, break.
            if (_fields.Length == 0) yield break;

            //If there are properties in the array, return each element.
            for (int i = 0; i < _fields.Length; i++) {
                object _currentValue = _fields[i].GetValue(_obj);

                if (_currentValue.GetType() == typeof(T)) yield return (T)_currentValue;
            }
        }

        /// <summary>
        /// Gets all the fields in obj.
        /// </summary>
        /// <param name="_obj">Object to find the fields in.</param>
        /// <param name="_bindingFlags">Filters of the fields allowed.</param>
        /// <returns>The fields found.</returns>
        public static IEnumerable GetAllFields(this object _obj, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            //Get all the properties.
            FieldInfo[] _fields = _obj.GetType().GetFields(_bindingFlags);

            //If there are no properties, break.
            if (_fields.Length == 0) yield break;

            //If there are properties in the array, return each element.
            for (int i = 0; i < _fields.Length; i++) {
                yield return _fields[i].GetValue(_obj);
            }
        }

        /// <summary>
        /// Gets all the properties in obj of type T.
        /// </summary>
        /// <typeparam name="T">Type of the properties allowed.</typeparam>
        /// <param name="_obj">Object to find the properties in.</param>
        /// <param name="_bindingFlags">Filters of the properties allowed.</param>
        /// <returns>The properties found.</returns>
        public static IEnumerable<T> GetAllProperties<T>(this object _obj, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            //Get all the properties.
            PropertyInfo[] _properties = _obj.GetType().GetProperties(_bindingFlags);

            //If there are no properties, break.
            if (_properties.Length == 0) yield break;

            //If there are properties in the array, return each element.
            for (int i = 0; i < _properties.Length; i++) {
                object _currentValue = _properties[i].GetValue(_obj, null);

                if (_currentValue.GetType() == typeof(T)) yield return (T)_currentValue;
            }
        }

        /// <summary>
        /// Gets all the properties in obj.
        /// </summary>
        /// <param name="_obj">Object to find the properties in.</param>
        /// <param name="_bindingFlags">Filters of the properties allowed.</param>
        /// <returns>The properties found.</returns>
        public static IEnumerable GetAllProperties(this object _obj, BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) {
            //Get all the properties.
            PropertyInfo[] _properties = _obj.GetType().GetProperties(_bindingFlags);

            //If there are no properties, break.
            if (_properties.Length == 0) yield break;

            //If there are properties in the array, return each element.
            for (int i = 0; i < _properties.Length; i++) {
                yield return _properties[i].GetValue(_obj, null);
            }
        }

        #endregion
        
    }

    
}