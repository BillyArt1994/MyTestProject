Shader "Genshin/Character/Body"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightMap ("LightMap",2D) = "white" {}
        _Color ("Color",Color) = (1.0,1.0,1.0,1.0)

        _OutlineColor ("OutLine Color" ,Color) = (1.0,1.0,1.0,1.0)
        _OutlineColor2 ("OutLine Color" ,Color) = (1.0,1.0,1.0,1.0)
        _OutlineColor3 ("OutLine Color" ,Color) = (1.0,1.0,1.0,1.0)
        _OutlineColor4 ("OutLine Color" ,Color) = (1.0,1.0,1.0,1.0)
        _OutlineColor5 ("OutLine Color" ,Color) = (1.0,1.0,1.0,1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _LightMap;

            v2f vert (a2v v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half outlineMask = tex2D(_LightMap, i.uv).w;
                float4 flag ;
                flag.x = outlineMask >= 0.8;
                flag.y = outlineMask >= 0.4;
                flag.z = outlineMask >= 0.2;
                flag.w = outlineMask >= 0.6;

                float4 flag1 ;
                flag1.x = outlineMask < 0.6;
                flag1.y = outlineMask < 0.4;
                flag1.z = outlineMask < 0.8;
                flag1.w = outlineMask < 0.8;

                return outlineMask;
            }
            ENDCG
        }

    }
}
