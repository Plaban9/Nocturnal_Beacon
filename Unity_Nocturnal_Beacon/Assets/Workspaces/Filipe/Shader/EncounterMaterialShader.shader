Shader "Shader Graphs/EncounterImageShaderTransp"
{
    Properties
    {
        [NoScaleOffset]_MainTex("_MainTex", 2D) = "white" {}
        _speed("speed", Float) = 1
        _clipThreshold("clipThreshold", Float) = 0
        _power("power", Float) = 2

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
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteUnlitSubTarget"
        }
        Pass
        {
            Name "Sprite Unlit"
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
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEUNLIT
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
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
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
        float4 _MainTex_TexelSize;
        float _speed;
        float _clipThreshold;
        float _power;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
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
        
        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }
        
        void Unity_Preview_float(float In, out float Out)
        {
            Out = In;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_InverseLerp_float(float A, float B, float T, out float Out)
        {
            Out = (T - A)/(B - A);
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
            float x; Hash_Tchou_2_1_float(p, x);
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
            float2 p = UV * Scale.xy;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
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
            UnityTexture2D _Property_392e9605139a4cb19d88af317b24bb0e_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_392e9605139a4cb19d88af317b24bb0e_Out_0_Texture2D.tex, _Property_392e9605139a4cb19d88af317b24bb0e_Out_0_Texture2D.samplerstate, _Property_392e9605139a4cb19d88af317b24bb0e_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_R_4_Float = _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_RGBA_0_Vector4.r;
            float _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_G_5_Float = _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_RGBA_0_Vector4.g;
            float _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_B_6_Float = _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_RGBA_0_Vector4.b;
            float _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_A_7_Float = _SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_RGBA_0_Vector4.a;
            float4 _UV_624d97f759484ee985ab3caab1e68e61_Out_0_Vector4 = IN.uv0;
            float _Split_4b69e0d72c4e44218018d84a7b1b861f_R_1_Float = _UV_624d97f759484ee985ab3caab1e68e61_Out_0_Vector4[0];
            float _Split_4b69e0d72c4e44218018d84a7b1b861f_G_2_Float = _UV_624d97f759484ee985ab3caab1e68e61_Out_0_Vector4[1];
            float _Split_4b69e0d72c4e44218018d84a7b1b861f_B_3_Float = _UV_624d97f759484ee985ab3caab1e68e61_Out_0_Vector4[2];
            float _Split_4b69e0d72c4e44218018d84a7b1b861f_A_4_Float = _UV_624d97f759484ee985ab3caab1e68e61_Out_0_Vector4[3];
            float _OneMinus_c4a152d3148646ab8b469bec20e69c80_Out_1_Float;
            Unity_OneMinus_float(_Split_4b69e0d72c4e44218018d84a7b1b861f_R_1_Float, _OneMinus_c4a152d3148646ab8b469bec20e69c80_Out_1_Float);
            float _Power_d9215d3da42d40fb830b6abe72bec54f_Out_2_Float;
            Unity_Power_float(_OneMinus_c4a152d3148646ab8b469bec20e69c80_Out_1_Float, _power, _Power_d9215d3da42d40fb830b6abe72bec54f_Out_2_Float);
            float _Preview_b9a43793ae764e9c98d179fc3fafb7bd_Out_1_Float;
            Unity_Preview_float(_Split_4b69e0d72c4e44218018d84a7b1b861f_R_1_Float, _Preview_b9a43793ae764e9c98d179fc3fafb7bd_Out_1_Float);
            float _Power_d6de447a03ad46d9b87cb067dfee817a_Out_2_Float;
            Unity_Power_float(_Preview_b9a43793ae764e9c98d179fc3fafb7bd_Out_1_Float, _power, _Power_d6de447a03ad46d9b87cb067dfee817a_Out_2_Float);
            float _Multiply_d5c728a78b8e42beb41305695a3a6640_Out_2_Float;
            Unity_Multiply_float_float(_Power_d9215d3da42d40fb830b6abe72bec54f_Out_2_Float, _Power_d6de447a03ad46d9b87cb067dfee817a_Out_2_Float, _Multiply_d5c728a78b8e42beb41305695a3a6640_Out_2_Float);
            float _Preview_5cf057e9c9db4062a56e163000a623b7_Out_1_Float;
            Unity_Preview_float(_Split_4b69e0d72c4e44218018d84a7b1b861f_G_2_Float, _Preview_5cf057e9c9db4062a56e163000a623b7_Out_1_Float);
            float _Power_1cf942df380747c1ab1ab0feeb1c355b_Out_2_Float;
            Unity_Power_float(_Preview_5cf057e9c9db4062a56e163000a623b7_Out_1_Float,_power, _Power_1cf942df380747c1ab1ab0feeb1c355b_Out_2_Float);
            float _OneMinus_423d98ba13324be3aabe72065e6bc189_Out_1_Float;
            Unity_OneMinus_float(_Split_4b69e0d72c4e44218018d84a7b1b861f_G_2_Float, _OneMinus_423d98ba13324be3aabe72065e6bc189_Out_1_Float);
            float _Power_87871724407c4ef8b77d09667a35f7dd_Out_2_Float;
            Unity_Power_float(_OneMinus_423d98ba13324be3aabe72065e6bc189_Out_1_Float,_power, _Power_87871724407c4ef8b77d09667a35f7dd_Out_2_Float);
            float _Multiply_0a5d997558a14d818b7e1fd93924d94a_Out_2_Float;
            Unity_Multiply_float_float(_Power_1cf942df380747c1ab1ab0feeb1c355b_Out_2_Float, _Power_87871724407c4ef8b77d09667a35f7dd_Out_2_Float, _Multiply_0a5d997558a14d818b7e1fd93924d94a_Out_2_Float);
            float _Multiply_3032812b5ce642928381138b3d3762e1_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_d5c728a78b8e42beb41305695a3a6640_Out_2_Float, _Multiply_0a5d997558a14d818b7e1fd93924d94a_Out_2_Float, _Multiply_3032812b5ce642928381138b3d3762e1_Out_2_Float);
            float _InverseLerp_af22274ba76f47c2aaf102b44239184a_Out_3_Float;
            Unity_InverseLerp_float(float(0), float(0.001), _Multiply_3032812b5ce642928381138b3d3762e1_Out_2_Float, _InverseLerp_af22274ba76f47c2aaf102b44239184a_Out_3_Float);
            float _Clamp_7f8d512ffeca4cc59b5d26a35e934a3b_Out_3_Float;
            Unity_Clamp_float(_InverseLerp_af22274ba76f47c2aaf102b44239184a_Out_3_Float, float(0), float(1), _Clamp_7f8d512ffeca4cc59b5d26a35e934a3b_Out_3_Float);
            float _Property_a7fdd2424d4a417eaffba1d833c82098_Out_0_Float = _speed;
            float _Multiply_04164692662447938744f8746885f781_Out_2_Float;
            Unity_Multiply_float_float(_Property_a7fdd2424d4a417eaffba1d833c82098_Out_0_Float, IN.TimeParameters.x, _Multiply_04164692662447938744f8746885f781_Out_2_Float);
            float2 _Vector2_3420fdc66c74489e991cf3ac5a6235a6_Out_0_Vector2 = float2(float(0), _Multiply_04164692662447938744f8746885f781_Out_2_Float);
            float2 _TilingAndOffset_87ae7a9211b54eabb7365a140a55577e_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_3420fdc66c74489e991cf3ac5a6235a6_Out_0_Vector2, _TilingAndOffset_87ae7a9211b54eabb7365a140a55577e_Out_3_Vector2);
            float _GradientNoise_05fd21d946084f939aad420a4309a28f_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_TilingAndOffset_87ae7a9211b54eabb7365a140a55577e_Out_3_Vector2, float(3), _GradientNoise_05fd21d946084f939aad420a4309a28f_Out_2_Float);
            float _Multiply_2645c7ce74a94eaead4d5beb6c71f4be_Out_2_Float;
            Unity_Multiply_float_float(_Clamp_7f8d512ffeca4cc59b5d26a35e934a3b_Out_3_Float, _GradientNoise_05fd21d946084f939aad420a4309a28f_Out_2_Float, _Multiply_2645c7ce74a94eaead4d5beb6c71f4be_Out_2_Float);
            float _Add_0926a5fe944043c2be1c453f5e40d397_Out_2_Float;
            Unity_Add_float(_Clamp_7f8d512ffeca4cc59b5d26a35e934a3b_Out_3_Float, _Multiply_2645c7ce74a94eaead4d5beb6c71f4be_Out_2_Float, _Add_0926a5fe944043c2be1c453f5e40d397_Out_2_Float);
            float _Clamp_718f54c472e443feb80a9b4f2d73e0a9_Out_3_Float;
            Unity_Clamp_float(_Add_0926a5fe944043c2be1c453f5e40d397_Out_2_Float, float(0), float(1), _Clamp_718f54c472e443feb80a9b4f2d73e0a9_Out_3_Float);
            float _Property_8392cd4c8795420983bf32235bbafd75_Out_0_Float = _clipThreshold;
            surface.BaseColor = (_SampleTexture2D_b261cef3bd1a4cb788c1311b5fab0b64_RGBA_0_Vector4.xyz);
            surface.Alpha = _Clamp_718f54c472e443feb80a9b4f2d73e0a9_Out_3_Float;
            surface.AlphaClipThreshold = _Property_8392cd4c8795420983bf32235bbafd75_Out_0_Float;
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
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
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