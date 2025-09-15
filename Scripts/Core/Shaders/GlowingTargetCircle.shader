Shader "Custom/GlowingTargetCircle"
{
    Properties
    {
        _GlowColor ("Glow Color", Color) = (1,0,0,1)
        _Pulse ("Pulse", Range(0,2)) = 1
        _InnerRadius ("Inner Radius", Range(0,1)) = 0.7
        _OuterRadius ("Outer Radius", Range(0,1)) = 0.9
        _EdgeSoftness ("Edge Softness", Range(0,0.5)) = 0.1
    }
    
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            float4 _GlowColor;
            float _Pulse;
            float _InnerRadius;
            float _OuterRadius;
            float _EdgeSoftness;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center) * 2.0;
                
                // Create a ring shape
                float innerEdge = smoothstep(_InnerRadius - _EdgeSoftness, _InnerRadius + _EdgeSoftness, dist);
                float outerEdge = 1.0 - smoothstep(_OuterRadius - _EdgeSoftness, _OuterRadius + _EdgeSoftness, dist);
                float ring = innerEdge * outerEdge;
                
                // Apply pulse and glow
                float intensity = ring * _Pulse;
                
                fixed4 col = _GlowColor * intensity;
                col.a = intensity * _GlowColor.a;
                
                return col;
            }
            ENDCG
        }
    }
}