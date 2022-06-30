// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/Fresnel + Alpha"
{
    Properties
    {
        [Header(OutLine)]
        _OutLineScale ("边缘线范围", Range(0, 1)) = 0.5
        _OutLineColor ("边缘线颜色", Color) = (0.5, 0.5 ,0.5, 1)

        [Header(InnerColor)]
        _MainTex ("颜色纹理", 2D) = "White" {}
        _MainTexAlpha ("纹理消失程度", Range(0, 1)) = 0.5
        _Color ("颜色",Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            Tags { "Queue"="Transparent" "IgnoreProjector" = "True" "RendType" = "Transparent"}

            Blend srcAlpha OneMinusSrcAlpha

            // ZWrite Off

            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 nDirWS : TEXCOORD1;
                float3 posWS : TEXCOORD2;
            };

            sampler2D _MainTex;
            float _OutLineScale;
            float4 _OutLineColor;

            v2f vert (a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.posWS = mul(unity_ObjectToWorld, v.vertex);
                o.nDirWS = mul(v.normal, unity_WorldToObject);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 nDirWS = normalize(i.nDirWS);
                float3 vDirWS = normalize(UnityWorldSpaceViewDir(i.posWS));

                float fresenl = saturate(dot(nDirWS, vDirWS));
                float Step_fresnel = step(1 - fresenl, _OutLineScale);
                float Step_fresnel2 = step(1 - fresenl, _OutLineScale);

                // fixed4 col = tex2D(_MainTex, i.uv);

                float3 finCol = (1 - Step_fresnel) * _OutLineColor;
                float finCol_Alpha = (1 - Step_fresnel);

                return float4(finCol, finCol_Alpha);
            }
            ENDCG
        }

        //Pass
        //{
        //    Tags { "Queue"="Transparent" "IgnoreProjector" = "True" "RendType" = "Transparent"}

        //    Blend srcAlpha OneMinusSrcAlpha

        //    ZWrite Off

        //    Cull Back

        //    CGPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag

        //    #include "UnityCG.cginc"
        //    #include "AutoLight.cginc"
        //    #include "Lighting.cginc"

        //    struct a2v
        //    {
        //        float4 vertex : POSITION;
        //        float2 uv : TEXCOORD0;
        //        float3 normal : NORMAL;
        //    };

        //    struct v2f
        //    {
        //        float4 pos : SV_POSITION;
        //        float2 uv : TEXCOORD0;
        //        float3 nDirWS : TEXCOORD1;
        //        float3 posWS : TEXCOORD2;
        //    };

        //    sampler2D _MainTex;
        //    float _OutLineScale;
        //    float _MainTexAlpha;
        //    fixed4 _Color;

        //    v2f vert (a2v v)
        //    {
        //        v2f o;
        //        o.pos = UnityObjectToClipPos(v.vertex);
        //        o.uv = v.uv;
        //        o.posWS = mul(unity_ObjectToWorld, v.vertex);
        //        o.nDirWS = mul(v.normal, unity_WorldToObject);
        //        return o;
        //    }

        //    fixed4 frag (v2f i) : SV_Target
        //    {
        //        float3 nDirWS = normalize(i.nDirWS);
        //        float3 vDirWS = normalize(UnityWorldSpaceViewDir(i.posWS));

        //        float fresenl = saturate(dot(nDirWS, vDirWS));
        //        float StepMainTexAlpha = step(1 - fresenl, _OutLineScale);          //  控制纹理在裁边菲捏尔的遮罩范围内

        //        fixed4 col = tex2D(_MainTex, i.uv);
        //        col *= _Color;
        //        float3 lDirWS = _WorldSpaceLightPos0.xyz;

        //        float3 finCol = col.rgb * StepMainTexAlpha * (dot(lDirWS, i.nDirWS) * 0.5 + 0.5);
        //        float finCol_Alpha = col.a * StepMainTexAlpha * _MainTexAlpha;      //  控制纹理在裁边菲捏尔的遮罩范围内进行透明度控制

        //        return float4(finCol, finCol_Alpha);
        //    }
        //    ENDCG
        //}
    }
}
