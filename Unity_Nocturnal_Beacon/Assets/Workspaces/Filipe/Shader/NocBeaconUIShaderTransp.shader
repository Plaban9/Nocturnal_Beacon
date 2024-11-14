Shader "Shader Graphs/NocBeacUIShaderTransp"
{
    Properties
    {
        [HDR]_backgroundColor("_backgroundColor", Color) = (0, 0.2754717, 0.0274127, 0.6431373)
        _edgeBlurriness("_edgeBlurriness", Float) = 1
        _flameSpeed("_flameSpeed", Float) = -1
        _flameAngle("_flameAngle", Float) = 120
        _flameVolatility("_flameVolatility", Float) = 12
        _flameSize("_flameSize", Float) = 6
        _flameWavering("_flameWavering", Float) = 1
        _flameWeaveringIntesity("_flameWeaveringIntesity", Float) = 0.5
        _flameLength("_flameLength", Float) = 3
        [HDR]_flameColor("_flameColor", Color) = (0.2050343, 1, 0, 0)
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
        ZTest LEqual
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
        #define _ALPHATEST_ON 1
        
        
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
             float2 NDCPosition;
             float2 PixelPosition;
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
        float4 _backgroundColor;
        float _edgeBlurriness;
        float _flameSpeed;
        float _flameAngle;
        float _flameVolatility;
        float _flameSize;
        float _flameWavering;
        float _flameWeaveringIntesity;
        float _flameLength;
        float4 _flameColor;
        CBUFFER_END
        
        
        // Object and Global properties
        
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
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
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
        
        void Unity_Preview_float2(float2 In, out float2 Out)
        {
            Out = In;
        }
        
        void Unity_Preview_float(float In, out float Out)
        {
            Out = In;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Blend_Screen_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
        {
            Out = 1.0 - (1.0 - Blend) * (1.0 - Base);
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_InverseLerp_float(float A, float B, float T, out float Out)
        {
            Out = (T - A)/(B - A);
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
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_0323ff82da804cd8a53ea16889b5ea1c_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_backgroundColor) : _backgroundColor;
            float4 _ScreenPosition_95d1962780734c70820870c90aa7feba_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            float _Property_833c834a15074e13b9ddf41c5e8c6e03_Out_0_Float = _flameAngle;
            float2 _Rotate_10e7a82ae0be44d39810e1ee3aae67c5_Out_3_Vector2;
            Unity_Rotate_Degrees_float((_ScreenPosition_95d1962780734c70820870c90aa7feba_Out_0_Vector4.xy), float2 (0.5, 0.5), _Property_833c834a15074e13b9ddf41c5e8c6e03_Out_0_Float, _Rotate_10e7a82ae0be44d39810e1ee3aae67c5_Out_3_Vector2);
            float _Property_26cf07e2d7a14f5181ce20fa7e6dfa6f_Out_0_Float = _flameSpeed;
            float _Multiply_35fe4ff0521e46d99d25ed60f11c0158_Out_2_Float;
            Unity_Multiply_float_float(_Property_26cf07e2d7a14f5181ce20fa7e6dfa6f_Out_0_Float, IN.TimeParameters.x, _Multiply_35fe4ff0521e46d99d25ed60f11c0158_Out_2_Float);
            float2 _Vector2_ec1e2275351d4c2fb82c39bc7a106b32_Out_0_Vector2 = float2(float(0), _Multiply_35fe4ff0521e46d99d25ed60f11c0158_Out_2_Float);
            float2 _TilingAndOffset_1abb3118f4b34dda95c6722d53aebbae_Out_3_Vector2;
            Unity_TilingAndOffset_float(_Rotate_10e7a82ae0be44d39810e1ee3aae67c5_Out_3_Vector2, float2 (1, 1), _Vector2_ec1e2275351d4c2fb82c39bc7a106b32_Out_0_Vector2, _TilingAndOffset_1abb3118f4b34dda95c6722d53aebbae_Out_3_Vector2);
            float _Property_81b42353dcb343dba3e76063ae3d4a89_Out_0_Float = _flameWeaveringIntesity;
            float _Property_88a7c2f6fb014dbf8520e80a079303f2_Out_0_Float = _flameWavering;
            float _Multiply_5b6c1f01b8c240089111ab9052ea5ca4_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_88a7c2f6fb014dbf8520e80a079303f2_Out_0_Float, _Multiply_5b6c1f01b8c240089111ab9052ea5ca4_Out_2_Float);
            float _Sine_e8aa960bd8d443878d015a979779241a_Out_1_Float;
            Unity_Sine_float(_Multiply_5b6c1f01b8c240089111ab9052ea5ca4_Out_2_Float, _Sine_e8aa960bd8d443878d015a979779241a_Out_1_Float);
            float _Multiply_0b76cea9f43b433783cb872db55b0200_Out_2_Float;
            Unity_Multiply_float_float(_Property_81b42353dcb343dba3e76063ae3d4a89_Out_0_Float, _Sine_e8aa960bd8d443878d015a979779241a_Out_1_Float, _Multiply_0b76cea9f43b433783cb872db55b0200_Out_2_Float);
            float _Split_4fd9c48d519b4fc09aaacc67187b8da7_R_1_Float = _Rotate_10e7a82ae0be44d39810e1ee3aae67c5_Out_3_Vector2[0];
            float _Split_4fd9c48d519b4fc09aaacc67187b8da7_G_2_Float = _Rotate_10e7a82ae0be44d39810e1ee3aae67c5_Out_3_Vector2[1];
            float _Split_4fd9c48d519b4fc09aaacc67187b8da7_B_3_Float = 0;
            float _Split_4fd9c48d519b4fc09aaacc67187b8da7_A_4_Float = 0;
            float _OneMinus_b227e96cf2a3449094d9bb1e4f553cfc_Out_1_Float;
            Unity_OneMinus_float(_Split_4fd9c48d519b4fc09aaacc67187b8da7_R_1_Float, _OneMinus_b227e96cf2a3449094d9bb1e4f553cfc_Out_1_Float);
            float _Multiply_c982c4c323ad47eaa55a4340353092bd_Out_2_Float;
            Unity_Multiply_float_float(_Split_4fd9c48d519b4fc09aaacc67187b8da7_G_2_Float, _OneMinus_b227e96cf2a3449094d9bb1e4f553cfc_Out_1_Float, _Multiply_c982c4c323ad47eaa55a4340353092bd_Out_2_Float);
            float _Power_070fe4361dee47e5bf88ebb19616454c_Out_2_Float;
            Unity_Power_float(_Multiply_c982c4c323ad47eaa55a4340353092bd_Out_2_Float, float(1), _Power_070fe4361dee47e5bf88ebb19616454c_Out_2_Float);
            float _Clamp_1c86b515e5ff4df4bd8aa0b8777c4b83_Out_3_Float;
            Unity_Clamp_float(_Power_070fe4361dee47e5bf88ebb19616454c_Out_2_Float, float(0), float(1), _Clamp_1c86b515e5ff4df4bd8aa0b8777c4b83_Out_3_Float);
            float _Multiply_f420d0f9d35d4499b2e56fa47b9ac8b3_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_0b76cea9f43b433783cb872db55b0200_Out_2_Float, _Clamp_1c86b515e5ff4df4bd8aa0b8777c4b83_Out_3_Float, _Multiply_f420d0f9d35d4499b2e56fa47b9ac8b3_Out_2_Float);
            float2 _TilingAndOffset_295f509074a44ba1ab70ff1dc342e3d9_Out_3_Vector2;
            Unity_TilingAndOffset_float(_TilingAndOffset_1abb3118f4b34dda95c6722d53aebbae_Out_3_Vector2, float2 (1, 1), (_Multiply_f420d0f9d35d4499b2e56fa47b9ac8b3_Out_2_Float.xx), _TilingAndOffset_295f509074a44ba1ab70ff1dc342e3d9_Out_3_Vector2);
            float _Property_eb4529e8dfa64554ab4b72ad3bd968f6_Out_0_Float = _flameVolatility;
            float _Multiply_7b8a81de9ec2419c80dfce12b478727c_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_eb4529e8dfa64554ab4b72ad3bd968f6_Out_0_Float, _Multiply_7b8a81de9ec2419c80dfce12b478727c_Out_2_Float);
            float _Property_8dd67a4c63ad4e1c93450fc4e771df70_Out_0_Float = _flameSize;
            float _Voronoi_06e801772cf64d56a1cc99a2e0a1ce11_Out_3_Float;
            float _Voronoi_06e801772cf64d56a1cc99a2e0a1ce11_Cells_4_Float;
            Unity_Voronoi_Deterministic_float(_TilingAndOffset_295f509074a44ba1ab70ff1dc342e3d9_Out_3_Vector2, _Multiply_7b8a81de9ec2419c80dfce12b478727c_Out_2_Float, _Property_8dd67a4c63ad4e1c93450fc4e771df70_Out_0_Float, _Voronoi_06e801772cf64d56a1cc99a2e0a1ce11_Out_3_Float, _Voronoi_06e801772cf64d56a1cc99a2e0a1ce11_Cells_4_Float);
            float _Property_7180d447fb844534a9988c7d893966ef_Out_0_Float = _flameAngle;
            float2 _Rotate_89a46ab6d06d47b0a61872efe75e073d_Out_3_Vector2;
            Unity_Rotate_Degrees_float((_ScreenPosition_95d1962780734c70820870c90aa7feba_Out_0_Vector4.xy), float2 (0.5, 0.5), _Property_7180d447fb844534a9988c7d893966ef_Out_0_Float, _Rotate_89a46ab6d06d47b0a61872efe75e073d_Out_3_Vector2);
            float2 _Preview_4faff0caa6f24662b39483f6d84e38c1_Out_1_Vector2;
            Unity_Preview_float2(_Rotate_89a46ab6d06d47b0a61872efe75e073d_Out_3_Vector2, _Preview_4faff0caa6f24662b39483f6d84e38c1_Out_1_Vector2);
            float _Split_e26e93db7aa84673951594bc7a677f67_R_1_Float = _Preview_4faff0caa6f24662b39483f6d84e38c1_Out_1_Vector2[0];
            float _Split_e26e93db7aa84673951594bc7a677f67_G_2_Float = _Preview_4faff0caa6f24662b39483f6d84e38c1_Out_1_Vector2[1];
            float _Split_e26e93db7aa84673951594bc7a677f67_B_3_Float = 0;
            float _Split_e26e93db7aa84673951594bc7a677f67_A_4_Float = 0;
            float _Preview_3c9dc66b5223482d93650beaadceead2_Out_1_Float;
            Unity_Preview_float(_Split_e26e93db7aa84673951594bc7a677f67_G_2_Float, _Preview_3c9dc66b5223482d93650beaadceead2_Out_1_Float);
            float _Property_5dc60513a44f47efa76450cbf2cf3f80_Out_0_Float = _flameLength;
            float _Power_b9bee5226f654a2b9278f1832a980d25_Out_2_Float;
            Unity_Power_float(_Preview_3c9dc66b5223482d93650beaadceead2_Out_1_Float, _Property_5dc60513a44f47efa76450cbf2cf3f80_Out_0_Float, _Power_b9bee5226f654a2b9278f1832a980d25_Out_2_Float);
            float _Clamp_bfdd9d6a729a41689e3767245549c8d2_Out_3_Float;
            Unity_Clamp_float(_Power_b9bee5226f654a2b9278f1832a980d25_Out_2_Float, float(0), float(1), _Clamp_bfdd9d6a729a41689e3767245549c8d2_Out_3_Float);
            float _Multiply_f923ec86e62a4cfeae8e3d5f33a2a993_Out_2_Float;
            Unity_Multiply_float_float(_Clamp_bfdd9d6a729a41689e3767245549c8d2_Out_3_Float, 125, _Multiply_f923ec86e62a4cfeae8e3d5f33a2a993_Out_2_Float);
            float _Power_d1094673fea84d9f91b1c6be6617115d_Out_2_Float;
            Unity_Power_float(_Voronoi_06e801772cf64d56a1cc99a2e0a1ce11_Out_3_Float, _Multiply_f923ec86e62a4cfeae8e3d5f33a2a993_Out_2_Float, _Power_d1094673fea84d9f91b1c6be6617115d_Out_2_Float);
            float4 _Property_fe9983c9f4884c60862a7217735d996a_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_flameColor) : _flameColor;
            float4 _Multiply_f6171ad1d15a45ac9d433947f1aa78dc_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Power_d1094673fea84d9f91b1c6be6617115d_Out_2_Float.xxxx), _Property_fe9983c9f4884c60862a7217735d996a_Out_0_Vector4, _Multiply_f6171ad1d15a45ac9d433947f1aa78dc_Out_2_Vector4);
            float4 _Blend_258a9fb561d947c8a07d2c379369a76d_Out_2_Vector4;
            Unity_Blend_Screen_float4(_Property_0323ff82da804cd8a53ea16889b5ea1c_Out_0_Vector4, _Multiply_f6171ad1d15a45ac9d433947f1aa78dc_Out_2_Vector4, _Blend_258a9fb561d947c8a07d2c379369a76d_Out_2_Vector4, float(1));
            float _Split_ca3ad5b65f2b4b0fbbae51b56f8733d1_R_1_Float = _Property_0323ff82da804cd8a53ea16889b5ea1c_Out_0_Vector4[0];
            float _Split_ca3ad5b65f2b4b0fbbae51b56f8733d1_G_2_Float = _Property_0323ff82da804cd8a53ea16889b5ea1c_Out_0_Vector4[1];
            float _Split_ca3ad5b65f2b4b0fbbae51b56f8733d1_B_3_Float = _Property_0323ff82da804cd8a53ea16889b5ea1c_Out_0_Vector4[2];
            float _Split_ca3ad5b65f2b4b0fbbae51b56f8733d1_A_4_Float = _Property_0323ff82da804cd8a53ea16889b5ea1c_Out_0_Vector4[3];
            float4 _UV_a8d7a72c1baa4b3ea2a2fe44b2080644_Out_0_Vector4 = IN.uv0;
            float _Split_73038925ce014004a922cae6cad5ea70_R_1_Float = _UV_a8d7a72c1baa4b3ea2a2fe44b2080644_Out_0_Vector4[0];
            float _Split_73038925ce014004a922cae6cad5ea70_G_2_Float = _UV_a8d7a72c1baa4b3ea2a2fe44b2080644_Out_0_Vector4[1];
            float _Split_73038925ce014004a922cae6cad5ea70_B_3_Float = _UV_a8d7a72c1baa4b3ea2a2fe44b2080644_Out_0_Vector4[2];
            float _Split_73038925ce014004a922cae6cad5ea70_A_4_Float = _UV_a8d7a72c1baa4b3ea2a2fe44b2080644_Out_0_Vector4[3];
            float _Preview_201b92ad56e04ae9a9de6b0780a5577b_Out_1_Float;
            Unity_Preview_float(_Split_73038925ce014004a922cae6cad5ea70_R_1_Float, _Preview_201b92ad56e04ae9a9de6b0780a5577b_Out_1_Float);
            float _Property_0bd535c995be47e5a712e382f0b50663_Out_0_Float = _edgeBlurriness;
            float _Power_dfabab1b4bbd451e9ff4fec7392b8e30_Out_2_Float;
            Unity_Power_float(_Preview_201b92ad56e04ae9a9de6b0780a5577b_Out_1_Float, _Property_0bd535c995be47e5a712e382f0b50663_Out_0_Float, _Power_dfabab1b4bbd451e9ff4fec7392b8e30_Out_2_Float);
            float _Preview_ceafd3cb328f4b2bab62bc2bf04a9061_Out_1_Float;
            Unity_Preview_float(_Split_73038925ce014004a922cae6cad5ea70_G_2_Float, _Preview_ceafd3cb328f4b2bab62bc2bf04a9061_Out_1_Float);
            float _Power_a28792bded5a488cb558ac5e7d7731cc_Out_2_Float;
            Unity_Power_float(_Preview_ceafd3cb328f4b2bab62bc2bf04a9061_Out_1_Float, _Property_0bd535c995be47e5a712e382f0b50663_Out_0_Float, _Power_a28792bded5a488cb558ac5e7d7731cc_Out_2_Float);
            float _Multiply_f9e0ed94b9e84c3baaa422a7742b6bf8_Out_2_Float;
            Unity_Multiply_float_float(_Power_dfabab1b4bbd451e9ff4fec7392b8e30_Out_2_Float, _Power_a28792bded5a488cb558ac5e7d7731cc_Out_2_Float, _Multiply_f9e0ed94b9e84c3baaa422a7742b6bf8_Out_2_Float);
            float _OneMinus_7eca546df04646999d9c444f9a11eead_Out_1_Float;
            Unity_OneMinus_float(_Split_73038925ce014004a922cae6cad5ea70_R_1_Float, _OneMinus_7eca546df04646999d9c444f9a11eead_Out_1_Float);
            float _Preview_f86c86da94ea423994984f80b635275c_Out_1_Float;
            Unity_Preview_float(_OneMinus_7eca546df04646999d9c444f9a11eead_Out_1_Float, _Preview_f86c86da94ea423994984f80b635275c_Out_1_Float);
            float _Power_55aab2b3eb0d477a9223dbac86b7e048_Out_2_Float;
            Unity_Power_float(_Preview_f86c86da94ea423994984f80b635275c_Out_1_Float, _Property_0bd535c995be47e5a712e382f0b50663_Out_0_Float, _Power_55aab2b3eb0d477a9223dbac86b7e048_Out_2_Float);
            float _OneMinus_dacc9a8e1574432cb78fa10b885a2c8e_Out_1_Float;
            Unity_OneMinus_float(_Split_73038925ce014004a922cae6cad5ea70_G_2_Float, _OneMinus_dacc9a8e1574432cb78fa10b885a2c8e_Out_1_Float);
            float _Preview_201d566f0641440198b7a797c7b09c0b_Out_1_Float;
            Unity_Preview_float(_OneMinus_dacc9a8e1574432cb78fa10b885a2c8e_Out_1_Float, _Preview_201d566f0641440198b7a797c7b09c0b_Out_1_Float);
            float _Power_52b29d501df6450492fdd04774f81b83_Out_2_Float;
            Unity_Power_float(_Preview_201d566f0641440198b7a797c7b09c0b_Out_1_Float, _Property_0bd535c995be47e5a712e382f0b50663_Out_0_Float, _Power_52b29d501df6450492fdd04774f81b83_Out_2_Float);
            float _Multiply_55e091480b7e41c5811738a052984002_Out_2_Float;
            Unity_Multiply_float_float(_Power_55aab2b3eb0d477a9223dbac86b7e048_Out_2_Float, _Power_52b29d501df6450492fdd04774f81b83_Out_2_Float, _Multiply_55e091480b7e41c5811738a052984002_Out_2_Float);
            float _Multiply_eedd1654e4454913a01cb646be3ef86c_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_f9e0ed94b9e84c3baaa422a7742b6bf8_Out_2_Float, _Multiply_55e091480b7e41c5811738a052984002_Out_2_Float, _Multiply_eedd1654e4454913a01cb646be3ef86c_Out_2_Float);
            float _Multiply_44db123f58d04981bc0297ddb13de8a3_Out_2_Float;
            Unity_Multiply_float_float(_Split_ca3ad5b65f2b4b0fbbae51b56f8733d1_A_4_Float, _Multiply_eedd1654e4454913a01cb646be3ef86c_Out_2_Float, _Multiply_44db123f58d04981bc0297ddb13de8a3_Out_2_Float);
            float _InverseLerp_4bc5cb598dee493da446cb1954a6f363_Out_3_Float;
            Unity_InverseLerp_float(float(0), float(0.1), _Multiply_44db123f58d04981bc0297ddb13de8a3_Out_2_Float, _InverseLerp_4bc5cb598dee493da446cb1954a6f363_Out_3_Float);
            float4 _Add_c422091a7f8c42b0be0ae075d63c9b61_Out_2_Vector4;
            Unity_Add_float4(_Multiply_f6171ad1d15a45ac9d433947f1aa78dc_Out_2_Vector4, (_InverseLerp_4bc5cb598dee493da446cb1954a6f363_Out_3_Float.xxxx), _Add_c422091a7f8c42b0be0ae075d63c9b61_Out_2_Vector4);
            surface.BaseColor = (_Blend_258a9fb561d947c8a07d2c379369a76d_Out_2_Vector4.xyz);
            surface.Alpha = (_Add_c422091a7f8c42b0be0ae075d63c9b61_Out_2_Vector4).x;
            surface.AlphaClipThreshold = float(0.1);
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
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
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