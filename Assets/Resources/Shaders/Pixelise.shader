Shader "Hidden/Shader/Pixelise"

{

    HLSLINCLUDE

    #pragma target 4.5

    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes

    {

        uint vertexID : SV_VertexID;

        UNITY_VERTEX_INPUT_INSTANCE_ID

    };

    struct Varyings

    {

        float4 positionCS : SV_POSITION;

        float2 texcoord   : TEXCOORD0;

        UNITY_VERTEX_OUTPUT_STEREO

    };


    Varyings Vert(Attributes input)

    {

        Varyings output;

        UNITY_SETUP_INSTANCE_ID(input);

        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);

        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);

        return output;

    }

    // pseudorandom number generator
    float rand(float2 co) {
        return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
    }

    float2 randfloat2(float xmax, float ymax, float2 co, float2 co2)
    {
        return float2(rand(co) * xmax, rand(co2) * ymax);
    }


    // cursed shit
    float when_eq(float x, float y)
    {
        return 1.0 - abs(sign(x - y));
    }
    
    float4 when_eq(float4 x, float4 y) {
        return 1.0 - abs(sign(x - y));
    }

    float4 when_neq(float4 x, float4 y) {
        return abs(sign(x - y));
    }

    float4 when_gt(float4 x, float4 y) {
        return max(sign(x - y), 0.0);
    }

    float4 when_lt(float4 x, float4 y) {
        return max(sign(y - x), 0.0);
    }

    float4 when_ge(float4 x, float4 y) {
        return 1.0 - when_lt(x, y);
    }

    float4 when_le(float4 x, float4 y) {
        return 1.0 - when_gt(x, y);
    }

    float4 and(float4 a, float4 b) {
        return a * b;
    }

    float4 or(float4 a, float4 b) {
        return min(a + b, 1.0);
    }

    float4 xor(float4 a, float4 b) {
        return (a + b) % 2.0;
    }

    float4 not(float4 a) {
        return 1.0 - a;
    }


    // List of properties to control your post process effect

    float _Intensity;

    TEXTURE2D_X(_InputTexture);

    float4 CustomPostProcess(Varyings input) : SV_Target

    {

        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        /*uint2 positionSS = input.texcoord * _ScreenSize.xy;

        float3 outColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

        return float4(lerp(outColor, Luminance(outColor).xxx, _Intensity), 1);*/
        //float2 brickCounts = { 1920.0/10.0, 1080.0/10.0 };
        //float2 brickSize = 1.0 / brickCounts;
        //float2 brickSize = _ScreenSize.xy * 0.01;
        float2 brickSize = float2(7, 7);

        // Offset every other row of bricks
        float2 offsetuv = input.texcoord * _ScreenSize.xy;
        /*bool oddRow = floor(offsetuv.y / brickSize.y) % 2.0 >= 1.0;
        if (oddRow)
        {
            offsetuv.x += brickSize.x / 2.0;
        }*/

        float2 brickNum = floor(offsetuv / brickSize);
        float2 centerOfBrick = brickNum * brickSize + brickSize / 2;
        // take just center of brick color
        //float3 color = LOAD_TEXTURE2D_X(_InputTexture, centerOfBrick).xyz;
        
        float2 constant = brickNum * brickSize;
        float3 color = float3(0,0,0);
        // we keep our trolling to a minimum
        for (int i = 0; i < brickSize.x; i++)
        {
            for (int z = 0; z < brickSize.y; z++)
            {
                color += LOAD_TEXTURE2D_X(_InputTexture, constant + float2(i, z)).xyz;
            }
        }
        color /= brickSize.x * brickSize.y;
        //============
        

        float3 copy = color;
        // oversaturate stuff
        //color.x = saturate(color.x + color.x * 0.5 * when_eq(max(max(copy.x, copy.y), copy.z), copy.x));
        //color.y = saturate(color.y + color.y * 0.5 * when_eq(max(max(copy.x, copy.y), copy.z), copy.y));
        //color.z = saturate(color.z + color.z * 0.5 * when_eq(max(max(copy.x, copy.y), copy.z), copy.z));
        // for debugging purposes; we'll make it orange by adjusting the noise averages
        color += float3(255, 165, 0)*0.00001;
        color += float3(rand(_Time.xx* constant) * 0.016, rand(_Time.xx* constant+float2(1,1)) * 0.012, rand(_Time.xx* constant+float2(2,2)) * 0.004)*0.5;

        return float4(color, 1.0);

    }

    ENDHLSL

    SubShader

    {

        Pass

        {

            Name "Pixelise"

            ZWrite Off

            ZTest Always

            Blend Off

            Cull Off

            HLSLPROGRAM

                #pragma fragment CustomPostProcess

                #pragma vertex Vert

            ENDHLSL

        }

    }

    Fallback Off

}