Shader "Custom/ShellShader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Pass
            {
                Tags { "LightMode" = "UniversalForward" }

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionHCS : SV_POSITION;
                    float3 normalWS : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                CBUFFER_START(UnityPerMaterial)
                    float4 _MainTex_ST;
                    float4 _Color;
                CBUFFER_END

                Varyings vert(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                    OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                    OUT.uv = IN.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                    return OUT;
                }

                half4 LightingToon(half ndotl)
                {
                    ndotl = ceil(ndotl * 4.0) / 4.0;
                    return half4(ndotl, ndotl, ndotl, 1.0);
                }

                half4 frag(Varyings IN) : SV_Target
                {
                    Light mainLight = GetMainLight();
                    half3 lightDir = normalize(mainLight.direction);
                    half ndotl = saturate(dot(normalize(IN.normalWS), lightDir));

                    half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                    baseColor *= _Color; 

                    half4 lighting = LightingToon(ndotl);

                    return baseColor * lighting;
                }
                ENDHLSL
            }
        }
            FallBack "Diffuse"
}