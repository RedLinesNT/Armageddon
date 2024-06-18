using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Armageddon.Serializer {
    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false), Conditional("UNITY_EDITOR")] public class ProvideSourceInfoAttribute : Attribute {

        #region Attributes

        private static readonly int lengthOfPathToProject = Application.dataPath.Length - "Assets".Length;
        private readonly string absoluteFilePath = string.Empty;
        private string assetPath = string.Empty;

        #endregion

        #region Properties

        public string Member { get; } = string.Empty;
        public int LineNumber { get; } = -1;
        public string AssetPath {
            get {
                if (assetPath == null) assetPath = absoluteFilePath.Replace('\\', '/').Substring(lengthOfPathToProject);
                return assetPath;
            }
        }

        #endregion

        #region ProvideSourceInfoAttribute's Constructor Method

        public ProvideSourceInfoAttribute([CallerMemberName] string _member = "", [CallerFilePath] string _filePath = "", [CallerLineNumber] int _lineNumber = 0) {
            Member = _member;
            absoluteFilePath = _filePath;
            LineNumber = _lineNumber;
        }

        #endregion

    }

    
}