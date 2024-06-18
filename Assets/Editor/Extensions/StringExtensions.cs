using System;
using System.Text.RegularExpressions;

namespace Armageddon.Editor.Extensions {

    public static class StringExtensions {
        
        private const string IndexKey = "index";
        private static readonly Regex ArrayRegex = new Regex($"\\[(?<{IndexKey}>\\d+)\\]", RegexOptions.Compiled);

        public static bool TryGetArrayIndex(this string _propertyStr, out int _index) {
            _index = -1;
            Match _arrayMatch = ArrayRegex.Match(_propertyStr);
            if (!_arrayMatch.Success) return false;

            _index = int.Parse(_arrayMatch.Groups[IndexKey].Value);
            return true;
        }
        
    }
}