using UnityEngine;
using UnityEditor;

namespace Armageddon.Editor {
    
    /// <summary>
    /// Editor tool that should correct some "<i>issues</i>" with Directional Light-maps
    /// </summary>
    public class DirectionalLightmapperCorrector : AssetPostprocessor {

        private void OnPostprocessTexture(Texture2D _tex) {
            if(!(assetPath.Contains("Lightmap-") && assetPath.Contains("_comp_dir"))) return; //Don't execute anything if the Texture isn't a Lightmap one

            Color[] _colors = _tex.GetPixels(); //Retrieve the Pixels' color of this Texture

            for (int i=0; i<_colors.Length; i++) {
                _colors[i].a = Mathf.Max(_colors[i].a, 0.1f);
            }
            
            //We're done, apply everything
            _tex.SetPixels(_colors);
            _tex.Apply(true);
        }

    }
    
}