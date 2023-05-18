Shader "Poly/PbrBlendDoubleSided" {
  Properties {
    _BaseColorFactor ("Base Color Factor", Color) = (1,1,1,1)
    _BaseColorTex ("Base Color Texture", 2D) = "white" {}
    _MetallicFactor ("Metallic Factor", Range(0,1)) = 1.0
    _RoughnessFactor ("Roughness Factor", Range(0,1)) = 1.0
  }
  SubShader {
    Cull Off
    ZWrite Off
    Tags { "Queue"="Transparent" "RenderType"="Transparent" }

    CGPROGRAM
    // Physically based Standard lighting model
    // No shadow-receiving passes
    #pragma surface surf Standard noshadow alpha:blend

    // Use shader model 3.0 target, because we use VFACE and to get nicer looking lighting
    #pragma target 3.0

    sampler2D _BaseColorTex;

    struct Input {
      float2 uv_BaseColorTex;
      // Filled in with (rgb=1, a=1) if the mesh doesn't have color
      float4 color : COLOR;
      fixed vface : VFACE;
    };

    fixed4 _BaseColorFactor;
    half _MetallicFactor;
    half _RoughnessFactor;

    void surf (Input IN, inout SurfaceOutputStandard o) {
      // Albedo comes from a texture tinted by color
      float4 c = tex2D(_BaseColorTex, IN.uv_BaseColorTex) * _BaseColorFactor * IN.color;
      o.Normal = float3(0, 0, IN.vface);
      o.Albedo = c.rgb;
      o.Alpha = c.a;
      // Metallic and smoothness come from parameters.
      o.Metallic = _MetallicFactor;
      // Smoothness is the opposite of roughness.
      o.Smoothness = 1.0 - _RoughnessFactor;
    }
    ENDCG
  }
}