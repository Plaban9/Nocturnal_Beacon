Shader "Shader Graphs/ScreenSwitch"
{
    Properties
    {
        _Transition("_Transition", Range(0, 1)) = 0.85
        [NoScaleOffset]_MainTex("_MainTex", 2D) = "white" {}
        _EdgeThickness("_EdgeThickness", Float) = 0.1
        [HDR]_LighthouseColor("_LighthouseColor", Color) = (0.02601683, 1, 0, 1)
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest Less
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Transition;
        float4 _MainTex_TexelSize;
        float _EdgeThickness;
        float4 _LighthouseColor;
        CBUFFER_END
        
        
        // Object and Global properties
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Ceiling_float2(float2 In, out float2 Out)
        {
            Out = ceil(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Sign_float(float In, out float Out)
        {
            Out = sign(In);
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_OneMinus_float2(float2 In, out float2 Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Fraction_float(float In, out float Out)
        {
            Out = frac(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        struct Bindings_Truchet_443a06ee36571b848aa0cc4eb4f681b4_float
        {
        half4 uv0;
        };
        
        void SG_Truchet_443a06ee36571b848aa0cc4eb4f681b4_float(float2 _tiling, float _seed, float _repetition, Bindings_Truchet_443a06ee36571b848aa0cc4eb4f681b4_float IN, out float Out_1)
        {
        float _Property_d870e879a0d1db8a841c61b19745d769_Out_0_Float = _repetition;
        float2 _Property_d83fb46e71e99d83a94d63a6600d54bc_Out_0_Vector2 = _tiling;
        float2 _TilingAndOffset_33ebf501a454a486871fd6d7d91b5888_Out_3_Vector2;
        Unity_TilingAndOffset_float(IN.uv0.xy, _Property_d83fb46e71e99d83a94d63a6600d54bc_Out_0_Vector2, float2 (0, 0), _TilingAndOffset_33ebf501a454a486871fd6d7d91b5888_Out_3_Vector2);
        float2 _Ceiling_74477c623383c78caf562638c8643af9_Out_1_Vector2;
        Unity_Ceiling_float2(_TilingAndOffset_33ebf501a454a486871fd6d7d91b5888_Out_3_Vector2, _Ceiling_74477c623383c78caf562638c8643af9_Out_1_Vector2);
        float _Length_956ccab6f0af248e97ec5c9860138951_Out_1_Float;
        Unity_Length_float2(_Ceiling_74477c623383c78caf562638c8643af9_Out_1_Vector2, _Length_956ccab6f0af248e97ec5c9860138951_Out_1_Float);
        float _Property_432583befd4fbf8c89982283c76d43d5_Out_0_Float = _seed;
        float _Multiply_1794450e91046d8b8b8a4fbc17e0bf58_Out_2_Float;
        Unity_Multiply_float_float(_Length_956ccab6f0af248e97ec5c9860138951_Out_1_Float, _Property_432583befd4fbf8c89982283c76d43d5_Out_0_Float, _Multiply_1794450e91046d8b8b8a4fbc17e0bf58_Out_2_Float);
        float _Cosine_818804a0b30d9080a6c3eb1a4ad5f67f_Out_1_Float;
        Unity_Cosine_float(_Multiply_1794450e91046d8b8b8a4fbc17e0bf58_Out_2_Float, _Cosine_818804a0b30d9080a6c3eb1a4ad5f67f_Out_1_Float);
        float _Sign_731f02f9348a1084b28e61d319a8ddc6_Out_1_Float;
        Unity_Sign_float(_Cosine_818804a0b30d9080a6c3eb1a4ad5f67f_Out_1_Float, _Sign_731f02f9348a1084b28e61d319a8ddc6_Out_1_Float);
        float _Split_accd05460635048d923d30f91d7775ec_R_1_Float = _TilingAndOffset_33ebf501a454a486871fd6d7d91b5888_Out_3_Vector2[0];
        float _Split_accd05460635048d923d30f91d7775ec_G_2_Float = _TilingAndOffset_33ebf501a454a486871fd6d7d91b5888_Out_3_Vector2[1];
        float _Split_accd05460635048d923d30f91d7775ec_B_3_Float = 0;
        float _Split_accd05460635048d923d30f91d7775ec_A_4_Float = 0;
        float _Multiply_86f3fb89ccf8698295790be1e1fe8b81_Out_2_Float;
        Unity_Multiply_float_float(_Sign_731f02f9348a1084b28e61d319a8ddc6_Out_1_Float, _Split_accd05460635048d923d30f91d7775ec_R_1_Float, _Multiply_86f3fb89ccf8698295790be1e1fe8b81_Out_2_Float);
        float2 _Vector2_0bb7b8b50fa35a89ad0868729a478d15_Out_0_Vector2 = float2(_Multiply_86f3fb89ccf8698295790be1e1fe8b81_Out_2_Float, _Split_accd05460635048d923d30f91d7775ec_G_2_Float);
        float2 _Fraction_bb86df7d67ecdf8dbbec1e5f6d2d1f0a_Out_1_Vector2;
        Unity_Fraction_float2(_Vector2_0bb7b8b50fa35a89ad0868729a478d15_Out_0_Vector2, _Fraction_bb86df7d67ecdf8dbbec1e5f6d2d1f0a_Out_1_Vector2);
        float _Length_46c2da05de63c38daa63859a93c5faec_Out_1_Float;
        Unity_Length_float2(_Fraction_bb86df7d67ecdf8dbbec1e5f6d2d1f0a_Out_1_Vector2, _Length_46c2da05de63c38daa63859a93c5faec_Out_1_Float);
        float2 _OneMinus_f479dfa52aa48280a0b9e9a66d87deef_Out_1_Vector2;
        Unity_OneMinus_float2(_Fraction_bb86df7d67ecdf8dbbec1e5f6d2d1f0a_Out_1_Vector2, _OneMinus_f479dfa52aa48280a0b9e9a66d87deef_Out_1_Vector2);
        float _Length_99b197592589a98d99d00d1f540669d8_Out_1_Float;
        Unity_Length_float2(_OneMinus_f479dfa52aa48280a0b9e9a66d87deef_Out_1_Vector2, _Length_99b197592589a98d99d00d1f540669d8_Out_1_Float);
        float _Minimum_593fbcfcd11cc08dad37d8d9151101f3_Out_2_Float;
        Unity_Minimum_float(_Length_46c2da05de63c38daa63859a93c5faec_Out_1_Float, _Length_99b197592589a98d99d00d1f540669d8_Out_1_Float, _Minimum_593fbcfcd11cc08dad37d8d9151101f3_Out_2_Float);
        float _Multiply_b40b1ee72c7eb28eb0eaee91164c032b_Out_2_Float;
        Unity_Multiply_float_float(_Property_d870e879a0d1db8a841c61b19745d769_Out_0_Float, _Minimum_593fbcfcd11cc08dad37d8d9151101f3_Out_2_Float, _Multiply_b40b1ee72c7eb28eb0eaee91164c032b_Out_2_Float);
        float _Fraction_ccc5db17d0a5ad80947f669867e247ac_Out_1_Float;
        Unity_Fraction_float(_Multiply_b40b1ee72c7eb28eb0eaee91164c032b_Out_2_Float, _Fraction_ccc5db17d0a5ad80947f669867e247ac_Out_1_Float);
        float _Smoothstep_85806da33565bd85b179df783dabec24_Out_3_Float;
        Unity_Smoothstep_float(float(0.8), float(0.6), _Fraction_ccc5db17d0a5ad80947f669867e247ac_Out_1_Float, _Smoothstep_85806da33565bd85b179df783dabec24_Out_3_Float);
        float _Smoothstep_85c9e4ba7f27ff83b26532c3ca1cea09_Out_3_Float;
        Unity_Smoothstep_float(float(0.4), float(0.2), _Fraction_ccc5db17d0a5ad80947f669867e247ac_Out_1_Float, _Smoothstep_85c9e4ba7f27ff83b26532c3ca1cea09_Out_3_Float);
        float _Subtract_b29f4fe31f6bfd8d9cceb61faf5dab52_Out_2_Float;
        Unity_Subtract_float(_Smoothstep_85806da33565bd85b179df783dabec24_Out_3_Float, _Smoothstep_85c9e4ba7f27ff83b26532c3ca1cea09_Out_3_Float, _Subtract_b29f4fe31f6bfd8d9cceb61faf5dab52_Out_2_Float);
        Out_1 = _Subtract_b29f4fe31f6bfd8d9cceb61faf5dab52_Out_2_Float;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        float2 Unity_Voronoi_RandomVector_Deterministic_float (float2 UV, float offset)
        {
            Hash_Tchou_2_2_float(UV, UV);
            return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
        }
        
        void Unity_Voronoi_Deterministic_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
            float2 g = floor(UV * CellDensity);
            float2 f = frac(UV * CellDensity);
            float t = 8.0;
            float3 res = float3(8.0, 0.0, 0.0);
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    float2 lattice = float2(x, y);
                    float2 offset = Unity_Voronoi_RandomVector_Deterministic_float(lattice + g, AngleOffset);
                    float d = distance(lattice + offset, f);
                    if (d < res.x)
                    {
                        res = float3(d, offset.x, offset.y);
                        Out = res.x;
                        Cells = res.y;
                    }
                }
            }
        }
        
        void Unity_Clamp_float4(float4 In, float4 Min, float4 Max, out float4 Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 Color_0a7332f2ff1d4aabb0391e5f02ee5916 = IsGammaSpace() ? float4(0, 0, 0, 1) : float4(SRGBToLinear(float3(0, 0, 0)), 1);
            float4 _UV_7bf3dfe5789147529b9fa6e26b9d2f64_Out_0_Vector4 = IN.uv0;
            float _Property_8840328ddece4b1e83ec4b508bdadc43_Out_0_Float = _Transition;
            float _Lerp_30b36bb9ce2b4445b72ed5c582f28440_Out_3_Float;
            Unity_Lerp_float(float(-1.5), float(1), _Property_8840328ddece4b1e83ec4b508bdadc43_Out_0_Float, _Lerp_30b36bb9ce2b4445b72ed5c582f28440_Out_3_Float);
            float2 _Vector2_d7977d07e5a84b7aa13f04afdd27f1d6_Out_0_Vector2 = float2(_Lerp_30b36bb9ce2b4445b72ed5c582f28440_Out_3_Float, float(-0.5));
            float2 _TilingAndOffset_7688e8c424be4ac093274a0cfbf78780_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7bf3dfe5789147529b9fa6e26b9d2f64_Out_0_Vector4.xy), float2 (1, 1), _Vector2_d7977d07e5a84b7aa13f04afdd27f1d6_Out_0_Vector2, _TilingAndOffset_7688e8c424be4ac093274a0cfbf78780_Out_3_Vector2);
            float _Distance_955bc07e63fc45a3b6c5f24dcc3d8a93_Out_2_Float;
            Unity_Distance_float2(_TilingAndOffset_7688e8c424be4ac093274a0cfbf78780_Out_3_Vector2, float2(0, 0), _Distance_955bc07e63fc45a3b6c5f24dcc3d8a93_Out_2_Float);
            float _OneMinus_1c97de8181294648b050e094886b62c5_Out_1_Float;
            Unity_OneMinus_float(_Distance_955bc07e63fc45a3b6c5f24dcc3d8a93_Out_2_Float, _OneMinus_1c97de8181294648b050e094886b62c5_Out_1_Float);
            float4 _Multiply_fa87983bac0f49128e7bf30fc7d5a3f8_Out_2_Vector4;
            Unity_Multiply_float4_float4(Color_0a7332f2ff1d4aabb0391e5f02ee5916, (_OneMinus_1c97de8181294648b050e094886b62c5_Out_1_Float.xxxx), _Multiply_fa87983bac0f49128e7bf30fc7d5a3f8_Out_2_Vector4);
            float _Lerp_82563b1e15ae44efa28d2ba1e5585c38_Out_3_Float;
            Unity_Lerp_float(float(0), float(2.5), _Property_8840328ddece4b1e83ec4b508bdadc43_Out_0_Float, _Lerp_82563b1e15ae44efa28d2ba1e5585c38_Out_3_Float);
            float _Sine_829b5f5172854f30a18da9da049a3af6_Out_1_Float;
            Unity_Sine_float(_Lerp_82563b1e15ae44efa28d2ba1e5585c38_Out_3_Float, _Sine_829b5f5172854f30a18da9da049a3af6_Out_1_Float);
            float _Multiply_3a10734ead4d47598305a2d108a77db6_Out_2_Float;
            Unity_Multiply_float_float(_Sine_829b5f5172854f30a18da9da049a3af6_Out_1_Float, 21, _Multiply_3a10734ead4d47598305a2d108a77db6_Out_2_Float);
            float _Clamp_73961d45316048c9a7b5e6d20287eb7b_Out_3_Float;
            Unity_Clamp_float(_Multiply_3a10734ead4d47598305a2d108a77db6_Out_2_Float, float(0), float(99), _Clamp_73961d45316048c9a7b5e6d20287eb7b_Out_3_Float);
            float _Property_8cadc83bb258453bb9407f64f5cf5321_Out_0_Float = _EdgeThickness;
            float _Add_5b121a1acb7d4980b592ac227e854c78_Out_2_Float;
            Unity_Add_float(float(1), _Property_8cadc83bb258453bb9407f64f5cf5321_Out_0_Float, _Add_5b121a1acb7d4980b592ac227e854c78_Out_2_Float);
            float _Multiply_6b13d582e8d140d9b8144d10e1c9d5d3_Out_2_Float;
            Unity_Multiply_float_float(_Clamp_73961d45316048c9a7b5e6d20287eb7b_Out_3_Float, _Add_5b121a1acb7d4980b592ac227e854c78_Out_2_Float, _Multiply_6b13d582e8d140d9b8144d10e1c9d5d3_Out_2_Float);
            float _Power_5ab6badf13cb41098fa48956d63299e4_Out_2_Float;
            Unity_Power_float(_Distance_955bc07e63fc45a3b6c5f24dcc3d8a93_Out_2_Float, _Multiply_6b13d582e8d140d9b8144d10e1c9d5d3_Out_2_Float, _Power_5ab6badf13cb41098fa48956d63299e4_Out_2_Float);
            float _Subtract_e5c05cf305464343bec92622b29f87dd_Out_2_Float;
            Unity_Subtract_float(float(1), _Property_8cadc83bb258453bb9407f64f5cf5321_Out_0_Float, _Subtract_e5c05cf305464343bec92622b29f87dd_Out_2_Float);
            float _Multiply_e1deed620d514d0bb7bd3475c6a67dfb_Out_2_Float;
            Unity_Multiply_float_float(_Subtract_e5c05cf305464343bec92622b29f87dd_Out_2_Float, _Clamp_73961d45316048c9a7b5e6d20287eb7b_Out_3_Float, _Multiply_e1deed620d514d0bb7bd3475c6a67dfb_Out_2_Float);
            float _Power_df2c8c3ccebb4a35a727f9a3bce7bb14_Out_2_Float;
            Unity_Power_float(_Distance_955bc07e63fc45a3b6c5f24dcc3d8a93_Out_2_Float, _Multiply_e1deed620d514d0bb7bd3475c6a67dfb_Out_2_Float, _Power_df2c8c3ccebb4a35a727f9a3bce7bb14_Out_2_Float);
            float _OneMinus_f6772e5b03b44ba2b843e0285f9e025b_Out_1_Float;
            Unity_OneMinus_float(_Power_df2c8c3ccebb4a35a727f9a3bce7bb14_Out_2_Float, _OneMinus_f6772e5b03b44ba2b843e0285f9e025b_Out_1_Float);
            float _Multiply_517eab10a86b4c4388018536122cea63_Out_2_Float;
            Unity_Multiply_float_float(_Power_5ab6badf13cb41098fa48956d63299e4_Out_2_Float, _OneMinus_f6772e5b03b44ba2b843e0285f9e025b_Out_1_Float, _Multiply_517eab10a86b4c4388018536122cea63_Out_2_Float);
            float4 _Property_5738e24ea92b4d24ba0c4ebee72b03c2_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_LighthouseColor) : _LighthouseColor;
            float4 _Multiply_75af5d3171304004947b24146128d080_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Multiply_517eab10a86b4c4388018536122cea63_Out_2_Float.xxxx), _Property_5738e24ea92b4d24ba0c4ebee72b03c2_Out_0_Vector4, _Multiply_75af5d3171304004947b24146128d080_Out_2_Vector4);
            Bindings_Truchet_443a06ee36571b848aa0cc4eb4f681b4_float _Truchet_6c5675309bdf4fb09b67a6951d7af940;
            _Truchet_6c5675309bdf4fb09b67a6951d7af940.uv0 = IN.uv0;
            float _Truchet_6c5675309bdf4fb09b67a6951d7af940_Out_1_Float;
            SG_Truchet_443a06ee36571b848aa0cc4eb4f681b4_float(float2 (8, 8), IN.TimeParameters.x, float(9), _Truchet_6c5675309bdf4fb09b67a6951d7af940, _Truchet_6c5675309bdf4fb09b67a6951d7af940_Out_1_Float);
            float2 _Vector2_b1af453305494c58af0d2a2ab4b900ea_Out_0_Vector2 = float2(IN.TimeParameters.x, float(0));
            float2 _Multiply_630b2e58f5ad4f038d7f6682f3c56281_Out_2_Vector2;
            Unity_Multiply_float2_float2(float2(1, 1), _Vector2_b1af453305494c58af0d2a2ab4b900ea_Out_0_Vector2, _Multiply_630b2e58f5ad4f038d7f6682f3c56281_Out_2_Vector2);
            float2 _TilingAndOffset_6255b958628a4762af1536efb7d9051d_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_630b2e58f5ad4f038d7f6682f3c56281_Out_2_Vector2, _TilingAndOffset_6255b958628a4762af1536efb7d9051d_Out_3_Vector2);
            float _Voronoi_f1f455afa29d4d64b0cb3478f0fc15b1_Out_3_Float;
            float _Voronoi_f1f455afa29d4d64b0cb3478f0fc15b1_Cells_4_Float;
            Unity_Voronoi_Deterministic_float(_TilingAndOffset_6255b958628a4762af1536efb7d9051d_Out_3_Vector2, IN.TimeParameters.x, float(5), _Voronoi_f1f455afa29d4d64b0cb3478f0fc15b1_Out_3_Float, _Voronoi_f1f455afa29d4d64b0cb3478f0fc15b1_Cells_4_Float);
            float _OneMinus_e6489aabd3804f048b7c83713c7ea1b4_Out_1_Float;
            Unity_OneMinus_float(_Voronoi_f1f455afa29d4d64b0cb3478f0fc15b1_Out_3_Float, _OneMinus_e6489aabd3804f048b7c83713c7ea1b4_Out_1_Float);
            float _Power_1d45f51450f44799868f3bff7ec43be8_Out_2_Float;
            Unity_Power_float(_OneMinus_e6489aabd3804f048b7c83713c7ea1b4_Out_1_Float, float(5), _Power_1d45f51450f44799868f3bff7ec43be8_Out_2_Float);
            float _Clamp_2fe8644518534cb484cf99c2d0ef474d_Out_3_Float;
            Unity_Clamp_float(_Power_1d45f51450f44799868f3bff7ec43be8_Out_2_Float, float(0), float(1), _Clamp_2fe8644518534cb484cf99c2d0ef474d_Out_3_Float);
            float _Add_bcea2103a9384ff6837e256184bc437a_Out_2_Float;
            Unity_Add_float(float(0.1), _Clamp_2fe8644518534cb484cf99c2d0ef474d_Out_3_Float, _Add_bcea2103a9384ff6837e256184bc437a_Out_2_Float);
            float _Multiply_f7b7dac4ebad4bc6b8c9459d6ecefcef_Out_2_Float;
            Unity_Multiply_float_float(_Truchet_6c5675309bdf4fb09b67a6951d7af940_Out_1_Float, _Add_bcea2103a9384ff6837e256184bc437a_Out_2_Float, _Multiply_f7b7dac4ebad4bc6b8c9459d6ecefcef_Out_2_Float);
            float4 _Multiply_e62a3a0078b74bac85fe882dac2c29e3_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Multiply_75af5d3171304004947b24146128d080_Out_2_Vector4, (_Multiply_f7b7dac4ebad4bc6b8c9459d6ecefcef_Out_2_Float.xxxx), _Multiply_e62a3a0078b74bac85fe882dac2c29e3_Out_2_Vector4);
            float4 _Clamp_abdf122306ca4aadb076748f4b5a6e73_Out_3_Vector4;
            Unity_Clamp_float4(_Multiply_e62a3a0078b74bac85fe882dac2c29e3_Out_2_Vector4, float4(0, 0, 0, 0), float4(1, 1, 1, 1), _Clamp_abdf122306ca4aadb076748f4b5a6e73_Out_3_Vector4);
            float4 _Multiply_cf51ae7865bd4ba795f1d5eb011b11e3_Out_2_Vector4;
            Unity_Multiply_float4_float4((_OneMinus_1c97de8181294648b050e094886b62c5_Out_1_Float.xxxx), _Clamp_abdf122306ca4aadb076748f4b5a6e73_Out_3_Vector4, _Multiply_cf51ae7865bd4ba795f1d5eb011b11e3_Out_2_Vector4);
            float4 _Add_55c2d7ac45d14ef0a7333638bcaa90d0_Out_2_Vector4;
            Unity_Add_float4(_Multiply_fa87983bac0f49128e7bf30fc7d5a3f8_Out_2_Vector4, _Multiply_cf51ae7865bd4ba795f1d5eb011b11e3_Out_2_Vector4, _Add_55c2d7ac45d14ef0a7333638bcaa90d0_Out_2_Vector4);
            float _OneMinus_7ff3f5ca2ace4ee38c05bcfb0acea23d_Out_1_Float;
            Unity_OneMinus_float(_Power_5ab6badf13cb41098fa48956d63299e4_Out_2_Float, _OneMinus_7ff3f5ca2ace4ee38c05bcfb0acea23d_Out_1_Float);
            surface.BaseColor = (_Add_55c2d7ac45d14ef0a7333638bcaa90d0_Out_2_Vector4.xyz);
            surface.Alpha = _OneMinus_7ff3f5ca2ace4ee38c05bcfb0acea23d_Out_1_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}