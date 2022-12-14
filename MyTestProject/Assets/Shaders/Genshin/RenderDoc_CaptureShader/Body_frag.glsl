#version 330
vec4 ImmCB_0_0_0[4];
uniform vec3 _WorldSpaceCameraPos;
uniform vec4 _ScreenParams;
uniform vec4 hlslcc_mtx4x4unity_WorldToCamera[4];
uniform vec4 hlslcc_mtx4x4_DITHERMATRIX[4];
uniform float _ElementViewEleDrawOn;
uniform float _ElementViewEleID;
uniform vec4 _Color;
uniform float _MainTexAlphaUse;
uniform float _MainTexAlphaCutoff;
uniform float _UseLightMapColorAO;
uniform float _UseVertexColorAO;
uniform float _LightArea;
uniform float _UseCoolShadowColorOrTex;
uniform float _ShadowRampWidth;
uniform float _UseVertexRampWidth;
uniform float _UseShadowTransition;
uniform vec3 _SpecularColor;
uniform float _Shininess;
uniform float _SpecMulti;
uniform float _FaceBlushStrength;
uniform vec3 _FaceBlushColor;
uniform float _EmissionScaler;
uniform float _EmissionScaler1;
uniform vec3 _EmissionColor_MHY;
uniform float _UseMaterial2;
uniform vec3 _Color2;
uniform float _EmissionScaler2;
uniform float _Shininess2;
uniform float _SpecMulti2;
uniform float _UseMaterial3;
uniform vec3 _Color3;
uniform float _EmissionScaler3;
uniform float _Shininess3;
uniform float _SpecMulti3;
uniform float _UseMaterial4;
uniform vec3 _Color4;
uniform float _EmissionScaler4;
uniform float _Shininess4;
uniform float _SpecMulti4;
uniform float _UseMaterial5;
uniform vec3 _Color5;
uniform float _EmissionScaler5;
uniform float _Shininess5;
uniform float _SpecMulti5;
uniform float _UsingDitherAlpha;
uniform float _DitherAlpha;
uniform float _TextureBiasWhenDithering;
uniform float _UseClipPlane;
uniform float _MTMapBrightness;
uniform float _MTMapTileScale;
uniform vec3 _MTMapLightColor;
uniform vec3 _MTMapDarkColor;
uniform vec3 _MTShadowMultiColor;
uniform float _MTShininess;
uniform vec3 _MTSpecularColor;
uniform float _MTSpecularScale;
uniform float _MTSpecularAttenInShadow;
uniform float _ES_CharacterAmbientLightOn;
uniform float _ES_CharacterAmbientBrightness;
uniform vec3 _ES_CharacterMainLightColor;
uniform float _ES_CharacterMainLightBrightness;
uniform vec3 _ES_CharacterAmbientLightColor;
uniform float _ES_CharacterMainLightRatio;
uniform float _ES_CharacterAmbientLightRatio;
uniform float _ES_CharacterColorTone;
uniform float _ES_CharacterPointLightWholeIntensity;
uniform vec4 _LightColor0;
uniform vec4 mhy_AvatarLightDir;
uniform vec4 mhy_CharacterPointLightColor;
uniform vec4 mhy_CharacterOverrideLightDir;
uniform vec4 mhy_CharacterOverrideLightDirInShadow;
uniform vec4 _ElementRimColor;
uniform vec4 _HitColor;
uniform float _HitColorFresnelPower;
uniform float _HitColorScaler;
uniform float _EmissionStrengthLerp;
uniform sampler2D _MainTex;
uniform sampler2D _LightMapTex;
uniform sampler2D _MTMap;
uniform sampler2D _PackedShadowRampTex;

in vec4 vs_COLOR0;
in vec4 vs_TEXCOORD0;
in vec4 vs_TEXCOORD1;
in vec4 vs_TEXCOORD2;
in vec3 vs_TEXCOORD3;
in vec2 vs_TEXCOORD4;
in vec4 vs_TEXCOORD6;//worldPos 

layout(location = 0) out vec4 SV_Target0;
layout(location = 1) out vec4 SV_Target1;
layout(location = 2) out vec4 SV_Target2;
vec3 u_xlat0;
vec4 u_xlat16_0;
uvec2 u_xlatu0;
bool u_xlatb0;
vec4 u_xlat1;
vec4 u_xlat16_1;
vec4 u_xlat10_2;
bool u_xlatb2;
vec3 u_xlat3;
bvec3 u_xlatb3;
vec3 u_xlat4;
float u_xlat16_4;
bvec4 u_xlatb4;
vec4 u_xlat5;
vec3 u_xlat16_5;
vec3 u_xlat16_6;
vec4 u_xlat7;
vec3 u_xlat16_7;
vec3 u_xlat10_7;
bvec3 u_xlatb7;
vec3 u_xlat16_8;
vec3 u_xlat16_9;
vec3 u_xlat16_10;
vec3 u_xlat16_11;
vec3 u_xlat16_12;
vec3 u_xlat13;
vec3 u_xlat16_13;
vec3 u_xlat10_13;
vec3 u_xlat14;
vec3 u_xlat16_14;
vec3 u_xlat10_14;
vec3 u_xlat15;
vec3 u_xlat10_15;
vec3 u_xlat16_16;
vec3 u_xlat16_17;
vec3 u_xlat18;
bool u_xlatb18;
vec3 u_xlat20;
bool u_xlatb20;
vec3 u_xlat21;
vec3 u_xlat16_21;
bool u_xlatb21;
vec3 u_xlat22;
bool u_xlatb22;
vec3 u_xlat16_24;
vec3 u_xlat25;
vec3 u_xlat16_26;
bvec2 u_xlatb36;
vec2 u_xlat39;
float u_xlat10_39;
bool u_xlatb39;
float u_xlat16_42;
bool u_xlatb43;
vec2 u_xlat16_44;
float u_xlat54;
bool u_xlatb54;
float u_xlat57;
float u_xlat16_59;
float u_xlat61;
bool u_xlatb61;
float u_xlat16_62;
void main(){
(ImmCB_0_0_0[0] = vec4(1.0, 0.0, 0.0, 0.0));
(ImmCB_0_0_0[1] = vec4(0.0, 1.0, 0.0, 0.0));
(ImmCB_0_0_0[2] = vec4(0.0, 0.0, 1.0, 0.0));
(ImmCB_0_0_0[3] = vec4(0.0, 0.0, 0.0, 1.0));
(u_xlatb0 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseClipPlane)));
(u_xlat16_1.x = (vs_COLOR0.w + -0.0099999998));
(u_xlatb18 = (u_xlat16_1.x < 0.0));
(u_xlatb0 = (u_xlatb0 && u_xlatb18));
if (((int(u_xlatb0) * -1) != 0))
{
discard;
}
(u_xlatb0 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UsingDitherAlpha)));
if (u_xlatb0)
{
(u_xlatb0 = (_DitherAlpha < 0.94999999));
if (u_xlatb0)
{
(u_xlat0.xy = (vs_TEXCOORD2.yx / vs_TEXCOORD2.ww));
(u_xlat0.xy = (u_xlat0.xy * _ScreenParams.yx));
(u_xlat0.xy = (u_xlat0.xy * vec2(0.25, 0.25)));
(u_xlatb36.xy = greaterThanEqual(u_xlat0.xyxy, (-u_xlat0.xyxy)).xy);
(u_xlat0.xy = fract(abs(u_xlat0.xy)));
(u_xlat0.x = ((u_xlatb36.x) ? (u_xlat0.x) : ((-u_xlat0.x))));
(u_xlat0.y = ((u_xlatb36.y) ? (u_xlat0.y) : ((-u_xlat0.y))));
(u_xlat0.xy = (u_xlat0.xy * vec2(4.0, 4.0)));
(u_xlatu0.xy = uvec2(u_xlat0.xy));
(u_xlat1.x = dot(hlslcc_mtx4x4_DITHERMATRIX[0], ImmCB_0_0_0[int(u_xlatu0.y)]));
(u_xlat1.y = dot(hlslcc_mtx4x4_DITHERMATRIX[1], ImmCB_0_0_0[int(u_xlatu0.y)]));
(u_xlat1.z = dot(hlslcc_mtx4x4_DITHERMATRIX[2], ImmCB_0_0_0[int(u_xlatu0.y)]));
(u_xlat1.w = dot(hlslcc_mtx4x4_DITHERMATRIX[3], ImmCB_0_0_0[int(u_xlatu0.y)]));
(u_xlat0.x = dot(u_xlat1, ImmCB_0_0_0[int(u_xlatu0.x)]));
(u_xlat0.x = ((_DitherAlpha * 17.0) + (-u_xlat0.x)));
(u_xlat0.x = (u_xlat0.x + -0.0099999998));
(u_xlatb0 = (u_xlat0.x < 0.0));
if (((int(u_xlatb0) * -1) != 0))
{
discard;
}
}
}
(u_xlat0.xyz = ((-vs_TEXCOORD3.xyz) + _WorldSpaceCameraPos.xyz));
(u_xlat54 = dot(u_xlat0.xyz, u_xlat0.xyz));
(u_xlat54 = inversesqrt(u_xlat54));
(u_xlatb2 = (0.0 < mhy_CharacterOverrideLightDir.w));
(u_xlatb20 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(vs_TEXCOORD4.y)));
(u_xlat20.xyz = ((bool(u_xlatb20)) ? (mhy_CharacterOverrideLightDirInShadow.xyz) : (mhy_CharacterOverrideLightDir.xyz)));
(u_xlat3.xyz = ((bool(u_xlatb2)) ? (u_xlat20.xyz) : (mhy_AvatarLightDir.xyz)));
(u_xlat0.xyz = ((u_xlat0.xyz * vec3(u_xlat54)) + u_xlat3.xyz));
(u_xlat54 = dot(u_xlat0.xyz, u_xlat0.xyz));
(u_xlat54 = inversesqrt(u_xlat54));
(u_xlat0.xyz = (vec3(u_xlat54) * u_xlat0.xyz));
(u_xlat54 = (_TextureBiasWhenDithering + -1.0));
(u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy, u_xlat54));
(u_xlatb3.xyz = equal(vec4(_MainTexAlphaUse), vec4(3.0, 1.0, 2.0, 0.0)).xyz);
(u_xlat57 = (u_xlat1.w * _FaceBlushStrength));
(u_xlat4.xyz = ((-u_xlat1.xyz) + _FaceBlushColor.xyz));
(u_xlat4.xyz = ((vec3(u_xlat57) * u_xlat4.xyz) + u_xlat1.xyz));
(u_xlat16_5.xyz = ((u_xlatb3.x) ? (u_xlat4.xyz) : (u_xlat1.xyz)));
(u_xlat16_59 = (u_xlat1.w + (-_MainTexAlphaCutoff)));
(u_xlatb3.x = (u_xlat16_59 < 0.0));
(u_xlatb3.x = (u_xlatb3.y && u_xlatb3.x));
if (((int(u_xlatb3.x) * -1) != 0))
{
discard;
}
(u_xlat10_2 = texture(_LightMapTex, vs_TEXCOORD0.xy, u_xlat54));
(u_xlatb3.xy = notEqual(vec4(0.0, 0.0, 0.0, 0.0), vec4(_UseLightMapColorAO, _UseVertexColorAO, _UseLightMapColorAO, _UseLightMapColorAO)).xy);
(u_xlat16_6.x = ((u_xlatb3.x) ? (u_xlat10_2.y) : (0.5)));
(u_xlatb54 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseMaterial2)));
(u_xlatb4 = greaterThanEqual(u_xlat10_2.wwww, vec4(0.80000001, 0.40000001, 0.2, 0.60000002)));
(u_xlatb54 = (u_xlatb54 && u_xlatb4.x));
(u_xlat54 = ((u_xlatb54) ? (2.0) : (1.0)));
(u_xlatb3.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseMaterial3)));
(u_xlatb3.x = (u_xlatb4.y && u_xlatb3.x));
(u_xlatb7.xyz = lessThan(u_xlat10_2.wwww, vec4(0.60000002, 0.40000001, 0.80000001, 0.0)).xyz);
(u_xlatb3.x = (u_xlatb3.x && u_xlatb7.x));
(u_xlat54 = ((u_xlatb3.x) ? (3.0) : (u_xlat54)));
(u_xlatb3.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseMaterial4)));
(u_xlatb3.x = (u_xlatb4.z && u_xlatb3.x));
(u_xlatb3.x = (u_xlatb7.y && u_xlatb3.x));
(u_xlat54 = ((u_xlatb3.x) ? (4.0) : (u_xlat54)));
(u_xlatb3.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseMaterial5)));
(u_xlatb3.x = (u_xlatb4.w && u_xlatb3.x));
(u_xlatb3.x = (u_xlatb7.z && u_xlatb3.x));
(u_xlat54 = ((u_xlatb3.x) ? (5.0) : (u_xlat54)));
(u_xlatb4 = equal(vec4(u_xlat54), vec4(2.0, 3.0, 4.0, 5.0)));
(u_xlat16_24.xyz = ((u_xlatb4.x) ? (_Color2.xyz) : (_Color.xyz)));
(u_xlat16_24.xyz = ((u_xlatb4.y) ? (_Color3.xyz) : (u_xlat16_24.xyz)));
(u_xlat16_24.xyz = ((u_xlatb4.z) ? (_Color4.xyz) : (u_xlat16_24.xyz)));
(u_xlat16_24.xyz = ((u_xlatb4.w) ? (_Color5.xyz) : (u_xlat16_24.xyz)));
(u_xlat16_24.xyz = (u_xlat16_5.xyz * u_xlat16_24.xyz));
(u_xlatb3.x = (0.0099999998 < u_xlat1.w));
(u_xlatb3.x = (u_xlatb3.x && u_xlatb3.z));
(u_xlat16_8.x = ((u_xlatb3.x) ? (u_xlat1.w) : (0.0)));
(u_xlat16_26.x = (u_xlat16_6.x * vs_COLOR0.x));
(u_xlat16_6.x = ((u_xlatb3.y) ? (u_xlat16_26.x) : (u_xlat16_6.x)));
(u_xlatb21 = (u_xlat16_6.x < 0.050000001));
(u_xlatb39 = (0.94999999 < u_xlat16_6.x));
(u_xlat57 = (u_xlat16_6.x + vs_TEXCOORD1.w));
(u_xlat57 = (u_xlat57 * 0.5));
(u_xlat16_6.x = ((u_xlatb39) ? (1.0) : (u_xlat57)));
(u_xlat16_6.x = ((u_xlatb21) ? (0.0) : (u_xlat16_6.x)));
(u_xlatb21 = (u_xlat16_6.x < _LightArea));
(u_xlat16_6.x = ((-u_xlat16_6.x) + _LightArea));
(u_xlat16_6.x = (u_xlat16_6.x / _LightArea));
(u_xlatb39 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseVertexRampWidth)));
(u_xlat16_26.x = (vs_COLOR0.y + vs_COLOR0.y));
(u_xlat16_26.x = max(u_xlat16_26.x, 0.0099999998));
(u_xlat57 = (u_xlat16_26.x * _ShadowRampWidth));
(u_xlat39.x = ((u_xlatb39) ? (u_xlat57) : (_ShadowRampWidth)));
(u_xlat39.x = (u_xlat16_6.x / u_xlat39.x));
(u_xlat39.x = min(u_xlat39.x, 1.0));
(u_xlat39.x = ((-u_xlat39.x) + 1.0));
(u_xlat7.z = ((u_xlatb21) ? (u_xlat39.x) : (1.0)));
(u_xlat7.x = ((u_xlatb21) ? (1.0) : (0.0)));
(u_xlatb39 = (9.9999997e-06 < vs_TEXCOORD4.x));
(u_xlat1.xy = ((bool(u_xlatb39)) ? (vec2(1.0, 0.0)) : (u_xlat7.xz)));
(u_xlatb39 = (0.5 < mhy_AvatarLightDir.w));
(u_xlat16_26.x = (_LightColor0.w + _LightColor0.w));
(u_xlat16_26.x = min(u_xlat16_26.x, 1.0));
(u_xlat16_26.x = ((u_xlatb39) ? (u_xlat16_26.x) : (1.0)));
(u_xlatb39 = (0.89999998 < u_xlat10_2.x));
if (u_xlatb39)
{
(u_xlat21.x = ((u_xlatb21) ? (u_xlat16_6.x) : (0.0)));
(u_xlat39.xy = (vs_TEXCOORD1.yy * hlslcc_mtx4x4unity_WorldToCamera[1].xy));
(u_xlat39.xy = ((hlslcc_mtx4x4unity_WorldToCamera[0].xy * vs_TEXCOORD1.xx) + u_xlat39.xy));
(u_xlat7.yz = ((hlslcc_mtx4x4unity_WorldToCamera[2].xy * vs_TEXCOORD1.zz) + u_xlat39.xy));
(u_xlat7.x = (u_xlat7.y * _MTMapTileScale));
(u_xlat16_44.xy = ((u_xlat7.xz * vec2(0.5, 0.5)) + vec2(0.5, 0.5)));
(u_xlat10_39 = texture(_MTMap, u_xlat16_44.xy).x);
(u_xlat16_6.x = (u_xlat10_39 * _MTMapBrightness));
(u_xlat16_6.x = clamp(u_xlat16_6.x, 0.0, 1.0));
(u_xlat16_9.xyz = (_MTMapLightColor.xyz + (-_MTMapDarkColor.xyz)));
(u_xlat16_9.xyz = ((u_xlat16_6.xxx * u_xlat16_9.xyz) + _MTMapDarkColor.xyz));
(u_xlat16_9.xyz = (u_xlat16_24.xyz * u_xlat16_9.xyz));
(u_xlat16_6.x = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz));
(u_xlat16_6.x = max(u_xlat16_6.x, 0.001));
(u_xlat16_6.x = log2(u_xlat16_6.x));
(u_xlat16_6.x = (u_xlat16_6.x * _MTShininess));
(u_xlat16_6.x = exp2(u_xlat16_6.x));
(u_xlat16_6.x = (u_xlat16_6.x * _MTSpecularScale));
(u_xlat16_6.x = clamp(u_xlat16_6.x, 0.0, 1.0));
(u_xlat16_10.xyz = (u_xlat16_6.xxx * _MTSpecularColor.xyz));
(u_xlat16_10.xyz = (u_xlat10_2.zzz * u_xlat16_10.xyz));
(u_xlatb39 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(vs_TEXCOORD4.x)));
(u_xlat16_11.xyz = (u_xlat16_10.xyz * vec3(_MTSpecularAttenInShadow)));
(u_xlat16_10.xyz = ((bool(u_xlatb39)) ? (u_xlat16_11.xyz) : (u_xlat16_10.xyz)));
(u_xlatb39 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseShadowTransition)));
(u_xlat16_7.xyz = (_MTShadowMultiColor.xyz + vec3(-1.0, -1.0, -1.0)));
(u_xlat7.xyz = ((u_xlat21.xxx * u_xlat16_7.xyz) + vec3(1.0, 1.0, 1.0)));
(u_xlat16_11.xyz = ((bool(u_xlatb39)) ? (u_xlat7.xyz) : (_MTShadowMultiColor.xyz)));
(u_xlatb21 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(u_xlat1.x)));
(u_xlat16_12.xyz = (u_xlat16_9.xyz * u_xlat16_11.xyz));
(u_xlat7.xyz = (u_xlat16_9.xyz * vec3(vec3(_ES_CharacterMainLightBrightness, _ES_CharacterMainLightBrightness, _ES_CharacterMainLightBrightness))));
(u_xlat21.xyz = ((bool(u_xlatb21)) ? (u_xlat16_12.xyz) : (u_xlat7.xyz)));
(u_xlatb7.x = (u_xlat16_26.x < 1.0));
(u_xlat25.xyz = (((-u_xlat16_9.xyz) * u_xlat16_11.xyz) + u_xlat21.xyz));
(u_xlat25.xyz = ((u_xlat16_26.xxx * u_xlat25.xyz) + u_xlat16_12.xyz));
(u_xlat16_9.xyz = ((u_xlatb7.x) ? (u_xlat25.xyz) : (u_xlat21.xyz)));
(u_xlat21.xyz = (u_xlat16_10.xyz * vec3(vec3(_ES_CharacterMainLightBrightness, _ES_CharacterMainLightBrightness, _ES_CharacterMainLightBrightness))));
(u_xlat16_21.xyz = u_xlat21.xyz);
}
else
{
(u_xlat7.x = (u_xlat54 + -1.0));
(u_xlat7.y = ((u_xlat7.x * 0.1) + 0.050000001));
(u_xlatb43 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseCoolShadowColorOrTex)));
if (u_xlatb43)
{
(u_xlat7.w = ((u_xlat7.x * 0.1) + 0.55000001));
(u_xlat1.zw = vec2(((-u_xlat7.y) + 1.0), ((-u_xlat7.w) + 1.0)));
(u_xlat10_13.xyz = texture(_PackedShadowRampTex, u_xlat1.yz).xyz);
(u_xlat10_14.xyz = texture(_PackedShadowRampTex, u_xlat1.yw).xyz);
(u_xlat16_13.xyz = (u_xlat10_13.xyz + (-u_xlat10_14.xyz)));
(u_xlat13.xyz = ((vec3(vec3(_ES_CharacterColorTone, _ES_CharacterColorTone, _ES_CharacterColorTone)) * u_xlat16_13.xyz) + u_xlat10_14.xyz));
(u_xlat16_13.xyz = u_xlat13.xyz);
}
else
{
(u_xlat14.y = ((-u_xlat7.y) + 1.0));
(u_xlat14.x = u_xlat1.y);
(u_xlat10_14.xyz = texture(_PackedShadowRampTex, u_xlat14.xy).xyz);
(u_xlat16_13.xyz = u_xlat10_14.xyz);
}
(u_xlatb61 = (u_xlat16_26.x < 1.0));
if (u_xlatb61)
{
if (u_xlatb43)
{
(u_xlat7.x = ((u_xlat7.x * 0.1) + 0.55000001));
(u_xlat5.yw = ((-u_xlat7.yx) + vec2(1.0, 1.0)));
(u_xlat5.x = 0.0);
(u_xlat5.z = 0.0);
(u_xlat10_14.xyz = texture(_PackedShadowRampTex, u_xlat5.xy).xyz);
(u_xlat10_15.xyz = texture(_PackedShadowRampTex, u_xlat5.zw).xyz);
(u_xlat16_14.xyz = (u_xlat10_14.xyz + (-u_xlat10_15.xyz)));
(u_xlat14.xyz = ((vec3(vec3(_ES_CharacterColorTone, _ES_CharacterColorTone, _ES_CharacterColorTone)) * u_xlat16_14.xyz) + u_xlat10_15.xyz));
(u_xlat16_14.xyz = u_xlat14.xyz);
}
else
{
(u_xlat7.y = ((-u_xlat7.y) + 1.0));
(u_xlat7.x = 0.0);
(u_xlat10_7.xyz = texture(_PackedShadowRampTex, u_xlat7.xy).xyz);
(u_xlat16_14.xyz = u_xlat10_7.xyz);
}
}
else
{
(u_xlat16_14.x = 0.0);
(u_xlat16_14.y = 0.0);
(u_xlat16_14.z = 0.0);
}
(u_xlatb7.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(u_xlat1.x)));
(u_xlat16_10.xyz = (u_xlat16_24.xyz * u_xlat16_13.xyz));
(u_xlat15.xyz = (u_xlat16_24.xyz * vec3(vec3(_ES_CharacterMainLightBrightness, _ES_CharacterMainLightBrightness, _ES_CharacterMainLightBrightness))));
(u_xlat7.xyz = ((u_xlatb7.x) ? (u_xlat16_10.xyz) : (u_xlat15.xyz)));
(u_xlat16_10.xyz = (u_xlat16_24.xyz * u_xlat16_14.xyz));
(u_xlat15.xyz = (((-u_xlat16_24.xyz) * u_xlat16_14.xyz) + u_xlat7.xyz));
(u_xlat15.xyz = ((u_xlat16_26.xxx * u_xlat15.xyz) + u_xlat16_10.xyz));
(u_xlat16_9.xyz = ((bool(u_xlatb61)) ? (u_xlat15.xyz) : (u_xlat7.xyz)));
(u_xlatb7.x = (u_xlat54 == 1.0));
(u_xlat16_6.x = ((u_xlatb4.z) ? (_Shininess4) : (_Shininess5)));
(u_xlat16_44.x = ((u_xlatb4.z) ? (_SpecMulti4) : (_SpecMulti5)));
(u_xlat16_6.x = ((u_xlatb4.y) ? (_Shininess3) : (u_xlat16_6.x)));
(u_xlat16_44.x = ((u_xlatb4.y) ? (_SpecMulti3) : (u_xlat16_44.x)));
(u_xlat16_6.x = ((u_xlatb4.x) ? (_Shininess2) : (u_xlat16_6.x)));
(u_xlat16_44.x = ((u_xlatb4.x) ? (_SpecMulti2) : (u_xlat16_44.x)));
(u_xlat16_6.x = ((u_xlatb7.x) ? (_Shininess) : (u_xlat16_6.x)));
(u_xlat16_44.x = ((u_xlatb7.x) ? (_SpecMulti) : (u_xlat16_44.x)));
(u_xlat16_62 = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz));
(u_xlat16_62 = max(u_xlat16_62, 0.001));
(u_xlat16_62 = log2(u_xlat16_62));
(u_xlat16_6.x = (u_xlat16_6.x * u_xlat16_62));
(u_xlat16_6.x = exp2(u_xlat16_6.x));
(u_xlat0.x = ((-u_xlat10_2.z) + 1.0));
(u_xlatb0 = (u_xlat0.x < u_xlat16_6.x));
(u_xlat16_10.xyz = (u_xlat16_44.xxx * _SpecularColor.xyz));
(u_xlat16_10.xyz = (u_xlat10_2.xxx * u_xlat16_10.xyz));
(u_xlat7.xyz = (u_xlat16_10.xyz * vec3(vec3(_ES_CharacterMainLightBrightness, _ES_CharacterMainLightBrightness, _ES_CharacterMainLightBrightness))));
(u_xlat21.xyz = mix(vec3(0.0, 0.0, 0.0), u_xlat7.xyz, vec3(bvec3(u_xlatb0))));
(u_xlat16_21.xyz = u_xlat21.xyz);
}
(u_xlat16_6.xyz = (u_xlat16_24.xyz * _EmissionColor_MHY.xyz));
(u_xlat16_6.xyz = (u_xlat16_6.xyz * vec3(_EmissionScaler)));
(u_xlatb0 = (u_xlat54 == 1.0));
(u_xlat16_10.xyz = (u_xlat16_6.xyz * vec3(vec3(_EmissionScaler1, _EmissionScaler1, _EmissionScaler1))));
(u_xlat16_11.xyz = (u_xlat16_6.xyz * vec3(vec3(_EmissionScaler2, _EmissionScaler2, _EmissionScaler2))));
(u_xlat16_12.xyz = (u_xlat16_6.xyz * vec3(vec3(_EmissionScaler3, _EmissionScaler3, _EmissionScaler3))));
(u_xlat16_16.xyz = (u_xlat16_6.xyz * vec3(vec3(_EmissionScaler4, _EmissionScaler4, _EmissionScaler4))));
(u_xlat16_17.xyz = (u_xlat16_6.xyz * vec3(vec3(_EmissionScaler5, _EmissionScaler5, _EmissionScaler5))));
(u_xlat16_6.xyz = ((u_xlatb4.w) ? (u_xlat16_17.xyz) : (u_xlat16_6.xyz)));
(u_xlat16_6.xyz = ((u_xlatb4.z) ? (u_xlat16_16.xyz) : (u_xlat16_6.xyz)));
(u_xlat16_6.xyz = ((u_xlatb4.y) ? (u_xlat16_12.xyz) : (u_xlat16_6.xyz)));
(u_xlat16_6.xyz = ((u_xlatb4.x) ? (u_xlat16_11.xyz) : (u_xlat16_6.xyz)));
(u_xlat16_6.xyz = ((bool(u_xlatb0)) ? (u_xlat16_10.xyz) : (u_xlat16_6.xyz)));
(u_xlatb0 = (u_xlat16_26.x < 1.0));
(u_xlat18.xyz = (u_xlat16_26.xxx * u_xlat16_21.xyz));
(u_xlat16_26.xyz = ((bool(u_xlatb0)) ? (u_xlat18.xyz) : (u_xlat16_21.xyz)));
(u_xlat16_26.xyz = (u_xlat16_26.xyz + u_xlat16_9.xyz));
(u_xlat16_6.xyz = (u_xlat16_6.xyz + (-u_xlat16_26.xyz)));
(u_xlat16_6.xyz = ((u_xlat16_8.xxx * u_xlat16_6.xyz) + u_xlat16_26.xyz));
(u_xlat16_6.xyz = ((u_xlatb3.x) ? (u_xlat16_6.xyz) : (u_xlat16_26.xyz)));
(u_xlat0.xyz = ((-vs_TEXCOORD6.xyz) + _WorldSpaceCameraPos.xyz));
(u_xlat54 = dot(u_xlat0.xyz, u_xlat0.xyz));
(u_xlat54 = inversesqrt(u_xlat54));
(u_xlat0.xyz = (vec3(u_xlat54) * u_xlat0.xyz));
(u_xlat54 = dot(vs_TEXCOORD1.xyz, vs_TEXCOORD1.xyz));
(u_xlat54 = inversesqrt(u_xlat54));
(u_xlat4.xyz = (vec3(u_xlat54) * vs_TEXCOORD1.xyz));
(u_xlat0.x = dot(u_xlat0.xyz, u_xlat4.xyz));
(u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0));
(u_xlat0.x = ((-u_xlat0.x) + 1.0));
(u_xlat0.x = max(u_xlat0.x, 9.9999997e-05));
(u_xlat0.x = log2(u_xlat0.x));
(u_xlat0.x = (u_xlat0.x * _HitColorFresnelPower));
(u_xlat0.x = exp2(u_xlat0.x));
(u_xlat18.xyz = max(_ElementRimColor.xyz, _HitColor.xyz));
(u_xlat0.xyz = (u_xlat0.xxx * u_xlat18.xyz));
(u_xlat0.xyz = ((u_xlat0.xyz * vec3(vec3(_HitColorScaler, _HitColorScaler, _HitColorScaler))) + u_xlat16_6.xyz));
(u_xlat16_6.x = ((-u_xlat16_8.x) + 1.0));
(u_xlat16_6.x = ((_EmissionStrengthLerp * u_xlat16_6.x) + u_xlat16_8.x));
(u_xlatb54 = (0.0099999998 < u_xlat16_6.x));
(u_xlatb4.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_ES_CharacterAmbientLightOn)));
(u_xlat22.xyz = (u_xlat0.xyz * vec3(vec3(_ES_CharacterAmbientBrightness, _ES_CharacterAmbientBrightness, _ES_CharacterAmbientBrightness))));
(u_xlat7.xyz = (vec3(vec3(_ES_CharacterPointLightWholeIntensity, _ES_CharacterPointLightWholeIntensity, _ES_CharacterPointLightWholeIntensity)) * mhy_CharacterPointLightColor.xyz));
(u_xlat15.xyz = _ES_CharacterMainLightColor.xyz);
(u_xlat15.xyz = clamp(u_xlat15.xyz, 0.0, 1.0));
(u_xlat61 = ((-mhy_CharacterPointLightColor.w) + 1.0));
(u_xlat7.xyz = ((u_xlat15.xyz * vec3(u_xlat61)) + u_xlat7.xyz));
(u_xlat7.xyz = (u_xlat7.xyz + vec3(-1.0, -1.0, -1.0)));
(u_xlat7.xyz = ((vec3(vec3(_ES_CharacterMainLightRatio, _ES_CharacterMainLightRatio, _ES_CharacterMainLightRatio)) * u_xlat7.xyz) + vec3(1.0, 1.0, 1.0)));
(u_xlat22.xyz = (u_xlat22.xyz * u_xlat7.xyz));
(u_xlat7.xyz = (_ES_CharacterAmbientLightColor.xyz * vec3(_ES_CharacterAmbientLightRatio)));
(u_xlat16_24.xyz = (u_xlat22.xyz * vec3(10.0, 10.0, 10.0)));
(u_xlat16_24.xyz = clamp(u_xlat16_24.xyz, 0.0, 1.0));
(u_xlat22.xyz = ((u_xlat7.xyz * u_xlat16_24.xyz) + u_xlat22.xyz));
(u_xlat16_24.xyz = ((u_xlatb4.x) ? (u_xlat22.xyz) : (u_xlat0.xyz)));
(u_xlat16_8.xyz = (u_xlat0.xyz + (-u_xlat16_24.xyz)));
(u_xlat16_8.xyz = ((u_xlat16_6.xxx * u_xlat16_8.xyz) + u_xlat16_24.xyz));
(u_xlat16_0.xyz = ((bool(u_xlatb54)) ? (u_xlat16_8.xyz) : (u_xlat16_24.xyz)));
(u_xlat16_24.x = max(u_xlat16_0.z, u_xlat16_0.y));
(u_xlat16_1.w = max(u_xlat16_0.x, u_xlat16_24.x));
(u_xlatb4.x = (1.0 < u_xlat16_1.w));
(u_xlat16_1.xyz = (u_xlat16_0.xyz / u_xlat16_1.www));
(u_xlat16_0.w = 1.0);
(u_xlat16_0 = ((u_xlatb4.x) ? (u_xlat16_1) : (u_xlat16_0)));
(u_xlat16_24.x = min(u_xlat16_0.w, 1.1));
(u_xlat16_42 = (u_xlat16_0.w + (-u_xlat16_24.x)));
(u_xlat16_6.x = ((u_xlat16_6.x * u_xlat16_42) + u_xlat16_24.x));
(u_xlat16_4 = (u_xlat16_6.x * 0.050000001));
(u_xlat16_4 = clamp(u_xlat16_4, 0.0, 1.0));
(u_xlat16_4 = sqrt(u_xlat16_4));
(SV_Target0.xyz = ((vs_TEXCOORD1.xyz * vec3(0.5, 0.5, 0.5)) + vec3(0.5, 0.5, 0.5)));
(u_xlatb22 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_ElementViewEleDrawOn)));
(u_xlat16_6.x = (_ElementViewEleID * 0.0039215689));
(SV_Target2.z = ((u_xlatb22) ? (u_xlat16_6.x) : (0.0)));
(SV_Target0.w = vs_TEXCOORD4.x);
(SV_Target1.xyz = u_xlat16_0.xyz);
(SV_Target1.w = u_xlat16_4);
(SV_Target2.xyw = vec3(0.0, 0.0, 0.015686275));
return ;
}
