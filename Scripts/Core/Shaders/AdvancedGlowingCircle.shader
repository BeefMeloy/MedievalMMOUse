Shader "Custom/AdvancedGlowingCircle"
{
    Properties
    {
        [Header(Circle Shape)]
        _InnerRadius ("Inner Radius", Range(0, 1)) = 0.6
        _OuterRadius ("Outer Radius", Range(0, 1)) = 0.8
        _EdgeSoftness ("Edge Softness", Range(0.01, 0.5)) = 0.15
        _Thickness ("Ring Thickness", Range(0.01, 0.5)) = 0.2
        
        [Header(Colors and Glow)]
        _GlowColor ("Glow Color", Color) = (1, 0, 0, 1)
        _InnerGlowColor ("Inner Glow Color", Color) = (1, 0.5, 0.5, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 2
        _OuterGlow ("Outer Glow", Range(0, 2)) = 1
        
        [Header(Animation)]
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2
        _PulseIntensity ("Pulse Intensity", Range(0, 1)) = 0.3
        _RotationSpeed ("Rotation Speed", Range(-10, 10)) = 1
        
        [Header(Advanced)]
        _Fresnel ("Fresnel Effect", Range(0, 5)) = 1
        _NoiseScale ("Noise Scale", Range(0, 20)) = 5
        _NoiseSpeed ("Noise Speed", Range(0, 5)) = 1
        _CircularFalloff ("Circular Falloff", Range(0.5, 1.0)) = 0.9
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
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
                float3 worldPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            
            // Properties
            float _InnerRadius;
            float _OuterRadius;
            float _EdgeSoftness;
            float _Thickness;
            float4 _GlowColor;
            float4 _InnerGlowColor;
            float _GlowIntensity;
            float _OuterGlow;
            float _PulseSpeed;
            float _PulseIntensity;
            float _RotationSpeed;
            float _Fresnel;
            float _NoiseScale;
            float _NoiseSpeed;
            float _CircularFalloff;
            
            // Simple noise function
            float noise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            float smoothNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);
                
                float a = noise(i);
                float b = noise(i + float2(1.0, 0.0));
                float c = noise(i + float2(0.0, 1.0));
                float d = noise(i + float2(1.0, 1.0));
                
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float2 uv = i.uv;
                
                // Apply rotation
                float angle = _Time.y * _RotationSpeed;
                float2 rotUV = uv - center;
                float cosA = cos(angle);
                float sinA = sin(angle);
                rotUV = float2(
                    rotUV.x * cosA - rotUV.y * sinA,
                    rotUV.x * sinA + rotUV.y * cosA
                ) + center;
                
                // Calculate distance from center (this is the key fix)
                float dist = distance(rotUV, center) * 2.0;
                
                // CRITICAL FIX: Add circular boundary mask to eliminate square edges
                float2 uvFromCenter = uv - center;
                float maxDist = length(uvFromCenter) * 2.0;
                float circularBoundary = 1.0 - smoothstep(_CircularFalloff, 1.0, maxDist);
                
                // Create pulse effect
                float pulse = sin(_Time.y * _PulseSpeed) * _PulseIntensity + (1.0 - _PulseIntensity);
                
                // Apply pulse to radius
                float innerRad = _InnerRadius * pulse;
                float outerRad = _OuterRadius * pulse;
                
                // Ensure edge softness scales properly with pulse
                float dynamicSoftness = _EdgeSoftness * pulse;
                
                // Create ring shape with soft edges
                float innerEdge = smoothstep(innerRad - dynamicSoftness, innerRad + dynamicSoftness, dist);
                float outerEdge = 1.0 - smoothstep(outerRad - dynamicSoftness, outerRad + dynamicSoftness, dist);
                float ring = innerEdge * outerEdge;
                
                // Apply the circular boundary mask - THIS PREVENTS SQUARE OUTLINE
                ring *= circularBoundary;
                
                // Add noise for organic feel (reduced to prevent square artifacts)
                float2 noiseUV = rotUV * _NoiseScale + _Time.y * _NoiseSpeed;
                float noiseValue = smoothNoise(noiseUV) * 0.1 + 0.9; // Reduced noise impact
                ring *= noiseValue;
                
                // Create gradient from inner to outer
                float gradient = 1.0 - smoothstep(innerRad, outerRad, dist);
                
                // Mix colors based on gradient
                float4 finalColor = lerp(_GlowColor, _InnerGlowColor, gradient);
                
                // Apply glow intensity
                float intensity = ring * _GlowIntensity * pulse;
                
                // Add outer glow with circular boundary
                float outerGlowMask = 1.0 - smoothstep(outerRad, outerRad + _OuterGlow, dist);
                outerGlowMask *= (1.0 - ring); // Only outside the main ring
                outerGlowMask *= circularBoundary; // Apply circular boundary to outer glow too
                intensity += outerGlowMask * _GlowIntensity * 0.3;
                
                // Additional edge fade for ultra-smooth circular edge
                float edgeFade = 1.0 - smoothstep(0.85, 1.0, maxDist);
                intensity *= edgeFade;
                
                finalColor.rgb *= intensity;
                finalColor.a = intensity * _GlowColor.a;
                
                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor;
            }
            ENDCG
        }
    }
    
    FallBack "Sprites/Default"
}