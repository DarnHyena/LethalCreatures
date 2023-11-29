using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CreatureModels
{
    public static class MaterialAssembler
    {
        public static Texture TexSpec;
        public static Texture TexEmit;
        public static Texture TexNorm;

        public static void UpdateMaterial(ref Material mat,ref Shader shader, ref Texture baseTexture)
        {
            mat.shader = shader;

            mat.EnableKeyword("_EMISSION");
            mat.EnableKeyword("_SPECGLOSSMAP");
            mat.EnableKeyword("_NORMALMAP");

            mat.SetTexture("_BaseColorMap", baseTexture);
            mat.SetTexture("_SpecularColorMap", TexSpec);
            mat.SetFloat("_Smoothness", .30f);
            mat.SetTexture("_EmissiveColorMap", TexEmit);
            mat.SetTexture("_BumpMap", TexNorm);
            mat.SetColor("_EmissiveColor", Color.white);
        }
    }
}
