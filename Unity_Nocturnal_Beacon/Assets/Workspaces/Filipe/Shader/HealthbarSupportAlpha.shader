Shader "Shader Graphs/HealthbarSupportAlpha"
{
    Properties
    {
        _pctHealthNShield("_pctHealthNShield", Range(0, 1)) = 1
        [NoScaleOffset]_MainTex("_MainTex", 2D) = "white" {}
        _healthSpeed("_healthSpeed", Float) = -0.9
        [HDR]_healthGlowColor("_healthGlowColor", Color) = (1, 0.4715431, 0, 0)
        _healthColor("_healthColor", Color) = (0.8113208, 0.198764, 0, 0)
        _healthFlowSpeed("_healthFlowSpeed", Float) = 4
        _healthScale("_healthScale", Float) = 5
        _shieldScale("_shieldScale", Vector) = (5, 5, 0, 0)
        _pctShield("_pctShield", Float) = 0.62
        _shieldPulseSpeed("_shieldPulseSpeed", Float) = 1
        _shieldBackground("_shieldBackground", Color) = (0, 0.3511805, 0.8490566, 0)
        _shieldGlow("_shieldGlow", Color) = (0.4283019, 0.9551609, 1, 0)
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
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteLitSubTarget"
        }
        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_SCREENPOSITION
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITELIT
        #define ALPHA_CLIP_THRESHOLD 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
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
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
             float4 screenPosition;
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
             float3 WorldSpacePosition;
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
             float4 color : INTERP1;
             float4 screenPosition : INTERP2;
             float3 positionWS : INTERP3;
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
            output.color.xyzw = input.color;
            output.screenPosition.xyzw = input.screenPosition;
            output.positionWS.xyz = input.positionWS;
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
            output.color = input.color.xyzw;
            output.screenPosition = input.screenPosition.xyzw;
            output.positionWS = input.positionWS.xyz;
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
        float _pctHealthNShield;
        float4 _healthColor;
        float4 _healthGlowColor;
        float _pctShield;
        float _healthFlowSpeed;
        float _healthSpeed;
        float4 _shieldBackground;
        float4 _shieldGlow;
        float _shieldPulseSpeed;
        float2 _shieldScale;
        float _healthScale;
        float4 _MainTex_TexelSize;
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
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
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
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Sine_float2(float2 In, out float2 Out)
        {
            Out = sin(In);
        }
        
        void Unity_Absolute_float2(float2 In, out float2 Out)
        {
            Out = abs(In);
        }
        
        void Unity_Power_float2(float2 A, float2 B, out float2 Out)
        {
            Out = pow(A, B);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_Modulo_float(float A, float B, out float Out)
        {
            Out = fmod(A, B);
        }
        
        void Unity_Modulo_float2(float2 A, float2 B, out float2 Out)
        {
            Out = fmod(A, B);
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Maximum_float(float A, float B, out float Out)
        {
            Out = max(A, B);
        }
        
        void Unity_Absolute_float(float In, out float Out)
        {
            Out = abs(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        struct Bindings_HexLattice_ae6f2edd46e88d5459d149f7a35446e1_float
        {
        half4 uv0;
        };
        
        void SG_HexLattice_ae6f2edd46e88d5459d149f7a35446e1_float(float2 _tiling, float _scale, float _edge, Bindings_HexLattice_ae6f2edd46e88d5459d149f7a35446e1_float IN, out float Out_1)
        {
        float _Property_ad523d03d9c557848bd05c6d29e4f76f_Out_0_Float = _edge;
        float2 _Property_3c37fdea2f2393849f42e2ab1c17d623_Out_0_Vector2 = _tiling;
        float2 _TilingAndOffset_9f3f116662dbdf8d9ff54290ac261dca_Out_3_Vector2;
        Unity_TilingAndOffset_float(IN.uv0.xy, _Property_3c37fdea2f2393849f42e2ab1c17d623_Out_0_Vector2, float2 (0, 0), _TilingAndOffset_9f3f116662dbdf8d9ff54290ac261dca_Out_3_Vector2);
        float _Split_ec4d176bb7c0888aaa50d9d2141f8ab2_R_1_Float = _TilingAndOffset_9f3f116662dbdf8d9ff54290ac261dca_Out_3_Vector2[0];
        float _Split_ec4d176bb7c0888aaa50d9d2141f8ab2_G_2_Float = _TilingAndOffset_9f3f116662dbdf8d9ff54290ac261dca_Out_3_Vector2[1];
        float _Split_ec4d176bb7c0888aaa50d9d2141f8ab2_B_3_Float = 0;
        float _Split_ec4d176bb7c0888aaa50d9d2141f8ab2_A_4_Float = 0;
        float _Multiply_e1c38402c4e27a829eb8a879a85946f6_Out_2_Float;
        Unity_Multiply_float_float(1.5, _Split_ec4d176bb7c0888aaa50d9d2141f8ab2_R_1_Float, _Multiply_e1c38402c4e27a829eb8a879a85946f6_Out_2_Float);
        float _Floor_fda6e33420cb098bbc4e4348442b564d_Out_1_Float;
        Unity_Floor_float(_Multiply_e1c38402c4e27a829eb8a879a85946f6_Out_2_Float, _Floor_fda6e33420cb098bbc4e4348442b564d_Out_1_Float);
        float _Modulo_5dedbffee9d5af839b1ae7631799e4b3_Out_2_Float;
        Unity_Modulo_float(_Floor_fda6e33420cb098bbc4e4348442b564d_Out_1_Float, float(2), _Modulo_5dedbffee9d5af839b1ae7631799e4b3_Out_2_Float);
        float _Multiply_81429557179b168ebee15641f7a6f012_Out_2_Float;
        Unity_Multiply_float_float(0.5, _Modulo_5dedbffee9d5af839b1ae7631799e4b3_Out_2_Float, _Multiply_81429557179b168ebee15641f7a6f012_Out_2_Float);
        float _Add_011d279ae54bd5898c3ffb1a7d4c108b_Out_2_Float;
        Unity_Add_float(_Split_ec4d176bb7c0888aaa50d9d2141f8ab2_G_2_Float, _Multiply_81429557179b168ebee15641f7a6f012_Out_2_Float, _Add_011d279ae54bd5898c3ffb1a7d4c108b_Out_2_Float);
        float2 _Vector2_c37863a01c2a6e83b8b8f0564aced3b9_Out_0_Vector2 = float2(_Multiply_e1c38402c4e27a829eb8a879a85946f6_Out_2_Float, _Add_011d279ae54bd5898c3ffb1a7d4c108b_Out_2_Float);
        float2 _Modulo_4a298f0a5349bb81aa2648907b930be4_Out_2_Vector2;
        Unity_Modulo_float2(_Vector2_c37863a01c2a6e83b8b8f0564aced3b9_Out_0_Vector2, float2(1, 1), _Modulo_4a298f0a5349bb81aa2648907b930be4_Out_2_Vector2);
        float2 _Subtract_620c7f298c4db9859ff68f1b681d3d33_Out_2_Vector2;
        Unity_Subtract_float2(_Modulo_4a298f0a5349bb81aa2648907b930be4_Out_2_Vector2, float2(0.5, 0.5), _Subtract_620c7f298c4db9859ff68f1b681d3d33_Out_2_Vector2);
        float2 _Absolute_82fa645a6d76768e81b16c1264b2ebab_Out_1_Vector2;
        Unity_Absolute_float2(_Subtract_620c7f298c4db9859ff68f1b681d3d33_Out_2_Vector2, _Absolute_82fa645a6d76768e81b16c1264b2ebab_Out_1_Vector2);
        float _Split_a67e597f5f79af8da6821647925258c4_R_1_Float = _Absolute_82fa645a6d76768e81b16c1264b2ebab_Out_1_Vector2[0];
        float _Split_a67e597f5f79af8da6821647925258c4_G_2_Float = _Absolute_82fa645a6d76768e81b16c1264b2ebab_Out_1_Vector2[1];
        float _Split_a67e597f5f79af8da6821647925258c4_B_3_Float = 0;
        float _Split_a67e597f5f79af8da6821647925258c4_A_4_Float = 0;
        float _Multiply_d6b75e862a379b868c8a7574e3ead437_Out_2_Float;
        Unity_Multiply_float_float(1.5, _Split_a67e597f5f79af8da6821647925258c4_R_1_Float, _Multiply_d6b75e862a379b868c8a7574e3ead437_Out_2_Float);
        float _Add_8eb27aba22a3128da5476346686c30e0_Out_2_Float;
        Unity_Add_float(_Multiply_d6b75e862a379b868c8a7574e3ead437_Out_2_Float, _Split_a67e597f5f79af8da6821647925258c4_G_2_Float, _Add_8eb27aba22a3128da5476346686c30e0_Out_2_Float);
        float _Multiply_2e68df5428e25e8498465a3dbb50a936_Out_2_Float;
        Unity_Multiply_float_float(_Split_a67e597f5f79af8da6821647925258c4_G_2_Float, 2, _Multiply_2e68df5428e25e8498465a3dbb50a936_Out_2_Float);
        float _Maximum_f88e9595003eea8fa653d97f727e2a91_Out_2_Float;
        Unity_Maximum_float(_Add_8eb27aba22a3128da5476346686c30e0_Out_2_Float, _Multiply_2e68df5428e25e8498465a3dbb50a936_Out_2_Float, _Maximum_f88e9595003eea8fa653d97f727e2a91_Out_2_Float);
        float _Property_7ccb379b6695758eaded473f165e48cf_Out_0_Float = _scale;
        float _Subtract_428b6ff76803e18d825a2d88fb2a686f_Out_2_Float;
        Unity_Subtract_float(_Maximum_f88e9595003eea8fa653d97f727e2a91_Out_2_Float, _Property_7ccb379b6695758eaded473f165e48cf_Out_0_Float, _Subtract_428b6ff76803e18d825a2d88fb2a686f_Out_2_Float);
        float _Absolute_321aa5642a4a1a8fb3b1a54ceca808f6_Out_1_Float;
        Unity_Absolute_float(_Subtract_428b6ff76803e18d825a2d88fb2a686f_Out_2_Float, _Absolute_321aa5642a4a1a8fb3b1a54ceca808f6_Out_1_Float);
        float _Multiply_835acd47380f758b982a50a8064ea46d_Out_2_Float;
        Unity_Multiply_float_float(_Absolute_321aa5642a4a1a8fb3b1a54ceca808f6_Out_1_Float, 2, _Multiply_835acd47380f758b982a50a8064ea46d_Out_2_Float);
        float _Smoothstep_0cd10edc36bef589a04ab9b5be10c276_Out_3_Float;
        Unity_Smoothstep_float(float(0), _Property_ad523d03d9c557848bd05c6d29e4f76f_Out_0_Float, _Multiply_835acd47380f758b982a50a8064ea46d_Out_2_Float, _Smoothstep_0cd10edc36bef589a04ab9b5be10c276_Out_3_Float);
        Out_1 = _Smoothstep_0cd10edc36bef589a04ab9b5be10c276_Out_3_Float;
        }
        
        void Unity_Subtract_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A - B;
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
            float4 SpriteMask;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_5e9e920d7df44e2791fa86af63a2163a_Out_0_Vector4 = IN.uv0;
            float _Property_90c3e7e7da8345ca9370a2729cec86f6_Out_0_Float = _pctHealthNShield;
            float _OneMinus_18338d1890034d87ab25b983a9e42eac_Out_1_Float;
            Unity_OneMinus_float(_Property_90c3e7e7da8345ca9370a2729cec86f6_Out_0_Float, _OneMinus_18338d1890034d87ab25b983a9e42eac_Out_1_Float);
            float2 _Vector2_8997fe3b71f5446595c03c0041f774e4_Out_0_Vector2 = float2(_OneMinus_18338d1890034d87ab25b983a9e42eac_Out_1_Float, float(0));
            float2 _TilingAndOffset_cef8126c26244970979d40337e3557a0_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_5e9e920d7df44e2791fa86af63a2163a_Out_0_Vector4.xy), float2 (1, 1), _Vector2_8997fe3b71f5446595c03c0041f774e4_Out_0_Vector2, _TilingAndOffset_cef8126c26244970979d40337e3557a0_Out_3_Vector2);
            float _Split_843f3c3cd5c64176a17b28bf57ac69bc_R_1_Float = _TilingAndOffset_cef8126c26244970979d40337e3557a0_Out_3_Vector2[0];
            float _Split_843f3c3cd5c64176a17b28bf57ac69bc_G_2_Float = _TilingAndOffset_cef8126c26244970979d40337e3557a0_Out_3_Vector2[1];
            float _Split_843f3c3cd5c64176a17b28bf57ac69bc_B_3_Float = 0;
            float _Split_843f3c3cd5c64176a17b28bf57ac69bc_A_4_Float = 0;
            float _OneMinus_b903dda6874e4014a62e501afdcd5820_Out_1_Float;
            Unity_OneMinus_float(_Split_843f3c3cd5c64176a17b28bf57ac69bc_R_1_Float, _OneMinus_b903dda6874e4014a62e501afdcd5820_Out_1_Float);
            float _Step_fb2da87ef293439195d409aef2817a4f_Out_2_Float;
            Unity_Step_float(float(0), _OneMinus_b903dda6874e4014a62e501afdcd5820_Out_1_Float, _Step_fb2da87ef293439195d409aef2817a4f_Out_2_Float);
            float _Subtract_3a8f08deb6d84483b93c0808bbf240a2_Out_2_Float;
            Unity_Subtract_float(_Step_fb2da87ef293439195d409aef2817a4f_Out_2_Float, _OneMinus_b903dda6874e4014a62e501afdcd5820_Out_1_Float, _Subtract_3a8f08deb6d84483b93c0808bbf240a2_Out_2_Float);
            float _Power_2356cc1a794a44c684dc1435a10d20c8_Out_2_Float;
            Unity_Power_float(_Subtract_3a8f08deb6d84483b93c0808bbf240a2_Out_2_Float, float(12), _Power_2356cc1a794a44c684dc1435a10d20c8_Out_2_Float);
            float4 _Property_887d9400d78e491dace1eccb353b0d74_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_healthGlowColor) : _healthGlowColor;
            float4 _Multiply_238d25725b2a433f8e03c730211bc8e0_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Power_2356cc1a794a44c684dc1435a10d20c8_Out_2_Float.xxxx), _Property_887d9400d78e491dace1eccb353b0d74_Out_0_Vector4, _Multiply_238d25725b2a433f8e03c730211bc8e0_Out_2_Vector4);
            float4 _Property_c82a6eb5254c40e896086f989277594e_Out_0_Vector4 = _healthColor;
            float _Property_cbc7067ab3da467a999337217de6ae8d_Out_0_Float = _healthSpeed;
            float _Multiply_4fc609fbf8db4034a6aa00c0fb908cb7_Out_2_Float;
            Unity_Multiply_float_float(_Property_cbc7067ab3da467a999337217de6ae8d_Out_0_Float, IN.TimeParameters.x, _Multiply_4fc609fbf8db4034a6aa00c0fb908cb7_Out_2_Float);
            float2 _Vector2_8959f2eec73d4a6a843c2bfa670f324b_Out_0_Vector2 = float2(_Multiply_4fc609fbf8db4034a6aa00c0fb908cb7_Out_2_Float, float(0));
            float2 _TilingAndOffset_20d1823215864f9b9fdd824ae7a1b284_Out_3_Vector2;
            Unity_TilingAndOffset_float((IN.WorldSpacePosition.xy), float2 (1, 1), _Vector2_8959f2eec73d4a6a843c2bfa670f324b_Out_0_Vector2, _TilingAndOffset_20d1823215864f9b9fdd824ae7a1b284_Out_3_Vector2);
            float _Property_128216041eb3499692c1e0c611f9f871_Out_0_Float = _healthFlowSpeed;
            float _Multiply_a42ec91ddcaf4a04a8b79bf9e1991973_Out_2_Float;
            Unity_Multiply_float_float(_Property_128216041eb3499692c1e0c611f9f871_Out_0_Float, IN.TimeParameters.x, _Multiply_a42ec91ddcaf4a04a8b79bf9e1991973_Out_2_Float);
            float _Property_f09663a61be54e809e5c21eecfef73d4_Out_0_Float = _healthScale;
            float _Voronoi_ee8739cdf7b0484aa46c6d2bd3338b0d_Out_3_Float;
            float _Voronoi_ee8739cdf7b0484aa46c6d2bd3338b0d_Cells_4_Float;
            Unity_Voronoi_Deterministic_float(_TilingAndOffset_20d1823215864f9b9fdd824ae7a1b284_Out_3_Vector2, _Multiply_a42ec91ddcaf4a04a8b79bf9e1991973_Out_2_Float, _Property_f09663a61be54e809e5c21eecfef73d4_Out_0_Float, _Voronoi_ee8739cdf7b0484aa46c6d2bd3338b0d_Out_3_Float, _Voronoi_ee8739cdf7b0484aa46c6d2bd3338b0d_Cells_4_Float);
            float _Power_7f32a5adb6d2425c83febdf3a34a9284_Out_2_Float;
            Unity_Power_float(_Voronoi_ee8739cdf7b0484aa46c6d2bd3338b0d_Out_3_Float, float(3), _Power_7f32a5adb6d2425c83febdf3a34a9284_Out_2_Float);
            float4 _Multiply_2952654e4da743a5978793fc965ae6e4_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_887d9400d78e491dace1eccb353b0d74_Out_0_Vector4, (_Power_7f32a5adb6d2425c83febdf3a34a9284_Out_2_Float.xxxx), _Multiply_2952654e4da743a5978793fc965ae6e4_Out_2_Vector4);
            float4 _Add_dfa0488798c544168f75c3275d175253_Out_2_Vector4;
            Unity_Add_float4(_Property_c82a6eb5254c40e896086f989277594e_Out_0_Vector4, _Multiply_2952654e4da743a5978793fc965ae6e4_Out_2_Vector4, _Add_dfa0488798c544168f75c3275d175253_Out_2_Vector4);
            float4 _Add_226cf198266b4ab9a0f1695728cc10b3_Out_2_Vector4;
            Unity_Add_float4(_Multiply_238d25725b2a433f8e03c730211bc8e0_Out_2_Vector4, _Add_dfa0488798c544168f75c3275d175253_Out_2_Vector4, _Add_226cf198266b4ab9a0f1695728cc10b3_Out_2_Vector4);
            float _Property_90277e3221a343068232c71e396c368b_Out_0_Float = _pctHealthNShield;
            float _Property_443f83475f614ea69fb98a6fa6aece42_Out_0_Float = _pctShield;
            float _OneMinus_edbdc33f14d04b0ba627d641abad276c_Out_1_Float;
            Unity_OneMinus_float(_Property_443f83475f614ea69fb98a6fa6aece42_Out_0_Float, _OneMinus_edbdc33f14d04b0ba627d641abad276c_Out_1_Float);
            float _Multiply_085eab2d6060428aa68fe92f29ebb5be_Out_2_Float;
            Unity_Multiply_float_float(_OneMinus_edbdc33f14d04b0ba627d641abad276c_Out_1_Float, _Property_90277e3221a343068232c71e396c368b_Out_0_Float, _Multiply_085eab2d6060428aa68fe92f29ebb5be_Out_2_Float);
            float _Subtract_2b37be35fa0044fbbd2d42625ab6203d_Out_2_Float;
            Unity_Subtract_float(_Property_90277e3221a343068232c71e396c368b_Out_0_Float, _Multiply_085eab2d6060428aa68fe92f29ebb5be_Out_2_Float, _Subtract_2b37be35fa0044fbbd2d42625ab6203d_Out_2_Float);
            float _Step_7d29f5f2fe224f78a7cb132fc06c74c3_Out_2_Float;
            Unity_Step_float(_Subtract_2b37be35fa0044fbbd2d42625ab6203d_Out_2_Float, _OneMinus_b903dda6874e4014a62e501afdcd5820_Out_1_Float, _Step_7d29f5f2fe224f78a7cb132fc06c74c3_Out_2_Float);
            float _Subtract_b9d89cfa6de44222897c65ad85a6a269_Out_2_Float;
            Unity_Subtract_float(_Step_fb2da87ef293439195d409aef2817a4f_Out_2_Float, _Step_7d29f5f2fe224f78a7cb132fc06c74c3_Out_2_Float, _Subtract_b9d89cfa6de44222897c65ad85a6a269_Out_2_Float);
            float4 _Property_8b0febbb75ae4f9f88e7064a841e8db5_Out_0_Vector4 = _shieldBackground;
            float4 _Property_18cefaad1fbc48eab16d42ac282688c4_Out_0_Vector4 = _shieldGlow;
            float2 _Property_222e0234e8094dc48b88a4ff77a0fdcb_Out_0_Vector2 = _shieldScale;
            float _Property_eb4d7b4e5f82492b9edea510551e7b67_Out_0_Float = _shieldPulseSpeed;
            float _Multiply_8a1159f6df7d4f0e8fb0dc7a79953c15_Out_2_Float;
            Unity_Multiply_float_float(_Property_eb4d7b4e5f82492b9edea510551e7b67_Out_0_Float, IN.TimeParameters.x, _Multiply_8a1159f6df7d4f0e8fb0dc7a79953c15_Out_2_Float);
            float2 _Vector2_df0b1dd232074ad3bae676ae5b70ec10_Out_0_Vector2 = float2(_Multiply_8a1159f6df7d4f0e8fb0dc7a79953c15_Out_2_Float, float(1));
            float2 _TilingAndOffset_9462c258fe4a4f48a0822bbb7f608289_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_df0b1dd232074ad3bae676ae5b70ec10_Out_0_Vector2, _TilingAndOffset_9462c258fe4a4f48a0822bbb7f608289_Out_3_Vector2);
            float2 _Sine_2b90521e2b8f445da989cb4629b32f49_Out_1_Vector2;
            Unity_Sine_float2(_TilingAndOffset_9462c258fe4a4f48a0822bbb7f608289_Out_3_Vector2, _Sine_2b90521e2b8f445da989cb4629b32f49_Out_1_Vector2);
            float2 _Absolute_370d3dfc14244bdda7e4959212b0dbd1_Out_1_Vector2;
            Unity_Absolute_float2(_Sine_2b90521e2b8f445da989cb4629b32f49_Out_1_Vector2, _Absolute_370d3dfc14244bdda7e4959212b0dbd1_Out_1_Vector2);
            float2 _Power_b287402da2494c0fa43c4532b32f3174_Out_2_Vector2;
            Unity_Power_float2(_Absolute_370d3dfc14244bdda7e4959212b0dbd1_Out_1_Vector2, float2(3, 3), _Power_b287402da2494c0fa43c4532b32f3174_Out_2_Vector2);
            float2 _Multiply_aca31043e80a4823868115d4a380ce64_Out_2_Vector2;
            Unity_Multiply_float2_float2(float2(0.6, 0.6), _Power_b287402da2494c0fa43c4532b32f3174_Out_2_Vector2, _Multiply_aca31043e80a4823868115d4a380ce64_Out_2_Vector2);
            float2 _Add_5a554c3e63744998b4e58e90abaf7686_Out_2_Vector2;
            Unity_Add_float2(_Multiply_aca31043e80a4823868115d4a380ce64_Out_2_Vector2, float2(0.7, 0.7), _Add_5a554c3e63744998b4e58e90abaf7686_Out_2_Vector2);
            float _Sine_de0901cdff76484389d88e124143526f_Out_1_Float;
            Unity_Sine_float(_Multiply_8a1159f6df7d4f0e8fb0dc7a79953c15_Out_2_Float, _Sine_de0901cdff76484389d88e124143526f_Out_1_Float);
            float _Add_e937c7d1e10341a8ac2cafec96b9345a_Out_2_Float;
            Unity_Add_float(float(3), _Sine_de0901cdff76484389d88e124143526f_Out_1_Float, _Add_e937c7d1e10341a8ac2cafec96b9345a_Out_2_Float);
            float _Clamp_13b57746615048f2be2c9ca59a2cb31c_Out_3_Float;
            Unity_Clamp_float(_Add_e937c7d1e10341a8ac2cafec96b9345a_Out_2_Float, float(0.2), float(4), _Clamp_13b57746615048f2be2c9ca59a2cb31c_Out_3_Float);
            Bindings_HexLattice_ae6f2edd46e88d5459d149f7a35446e1_float _HexLattice_1735f150200043449da80171712f29df;
            _HexLattice_1735f150200043449da80171712f29df.uv0 = IN.uv0;
            float _HexLattice_1735f150200043449da80171712f29df_Out_1_Float;
            SG_HexLattice_ae6f2edd46e88d5459d149f7a35446e1_float(_Property_222e0234e8094dc48b88a4ff77a0fdcb_Out_0_Vector2, (_Add_5a554c3e63744998b4e58e90abaf7686_Out_2_Vector2).x, _Clamp_13b57746615048f2be2c9ca59a2cb31c_Out_3_Float, _HexLattice_1735f150200043449da80171712f29df, _HexLattice_1735f150200043449da80171712f29df_Out_1_Float);
            float _Step_eb3847f46ec744e6ae33192bbd588de3_Out_2_Float;
            Unity_Step_float(float(0.02), _HexLattice_1735f150200043449da80171712f29df_Out_1_Float, _Step_eb3847f46ec744e6ae33192bbd588de3_Out_2_Float);
            float4 _Multiply_394cd6b0828b40078e19741ae04a423a_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_18cefaad1fbc48eab16d42ac282688c4_Out_0_Vector4, (_Step_eb3847f46ec744e6ae33192bbd588de3_Out_2_Float.xxxx), _Multiply_394cd6b0828b40078e19741ae04a423a_Out_2_Vector4);
            float _Power_89624df6b4bc4514861da8a417d15d3d_Out_2_Float;
            Unity_Power_float(_HexLattice_1735f150200043449da80171712f29df_Out_1_Float, float(1), _Power_89624df6b4bc4514861da8a417d15d3d_Out_2_Float);
            float4 _Subtract_0d5c75e1ac6646ec9c68c492f220f09b_Out_2_Vector4;
            Unity_Subtract_float4(_Multiply_394cd6b0828b40078e19741ae04a423a_Out_2_Vector4, (_Power_89624df6b4bc4514861da8a417d15d3d_Out_2_Float.xxxx), _Subtract_0d5c75e1ac6646ec9c68c492f220f09b_Out_2_Vector4);
            float4 _Add_f2c6f7d660ae4a65baa954fe31dfcc70_Out_2_Vector4;
            Unity_Add_float4(_Property_8b0febbb75ae4f9f88e7064a841e8db5_Out_0_Vector4, _Subtract_0d5c75e1ac6646ec9c68c492f220f09b_Out_2_Vector4, _Add_f2c6f7d660ae4a65baa954fe31dfcc70_Out_2_Vector4);
            float4 _Multiply_a1363798860f4cdc904c0f7f47165b46_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Subtract_b9d89cfa6de44222897c65ad85a6a269_Out_2_Float.xxxx), _Add_f2c6f7d660ae4a65baa954fe31dfcc70_Out_2_Vector4, _Multiply_a1363798860f4cdc904c0f7f47165b46_Out_2_Vector4);
            float4 _Add_6e97cfbecf26413ba70e2771c54aed1c_Out_2_Vector4;
            Unity_Add_float4(_Add_226cf198266b4ab9a0f1695728cc10b3_Out_2_Vector4, _Multiply_a1363798860f4cdc904c0f7f47165b46_Out_2_Vector4, _Add_6e97cfbecf26413ba70e2771c54aed1c_Out_2_Vector4);
            surface.BaseColor = (_Add_6e97cfbecf26413ba70e2771c54aed1c_Out_2_Vector4.xyz);
            surface.Alpha = _Step_fb2da87ef293439195d409aef2817a4f_Out_2_Float;
            surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
            surface.AlphaClipThreshold = float(0.5);
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
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
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
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteLitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}