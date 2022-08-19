#version 330
vec4 ImmCB_0_0_0[4];
uniform vec4 _ScreenParams;
uniform float _ElementViewEleDrawOn;
uniform float _ElementViewEleID;
uniform vec4 hlslcc_mtx4x4_DITHERMATRIX[4];
uniform vec4 _Color;
uniform vec4 _OutlineColor;
uniform float _ES_CharacterAmbientLightOn;
uniform float _ES_CharacterAmbientBrightness;
uniform vec3 _ES_CharacterMainLightColor;
uniform vec3 _ES_CharacterAmbientLightColor;
uniform float _ES_CharacterMainLightRatio;
uniform float _ES_CharacterAmbientLightRatio;
uniform float _ES_CharacterPointLightWholeIntensity;
uniform vec4 mhy_CharacterPointLightColor;
uniform float _UseClipPlane;
uniform float _MainTexAlphaUse;
uniform float _MainTexAlphaCutoff;
uniform float _UsingDitherAlpha;
uniform float _DitherAlpha;
uniform float _DeferredLightThreshold;
uniform float _UseMaterial2;
uniform vec3 _Color2;
uniform vec3 _OutlineColor2;
uniform float _UseMaterial3;
uniform vec3 _Color3;
uniform vec3 _OutlineColor3;
uniform float _UseMaterial4;
uniform vec3 _Color4;
uniform vec3 _OutlineColor4;
uniform float _UseMaterial5;
uniform vec3 _Color5;
uniform vec3 _OutlineColor5;
uniform sampler2D _MainTex;
uniform sampler2D _LightMapTex;
in vec4 vs_COLOR0;
in vec2 vs_TEXCOORD0;
in vec3 vs_TEXCOORD1;
in vec4 vs_TEXCOORD2;
layout(location = 0) out vec4 SV_Target0;
layout(location = 1) out vec4 SV_Target1;
layout(location = 2) out vec4 SV_Target2;
vec2 u_xlat0;
float u_xlat10_0;
ivec4 u_xlati0;
uvec2 u_xlatu0;
bvec2 u_xlatb0;
vec4 u_xlat1;
float u_xlat16_1;
bvec4 u_xlatb1;
vec3 u_xlat16_2;
vec3 u_xlat3;
bool u_xlatb3;
vec3 u_xlat16_4;
vec3 u_xlat16_5;
vec3 u_xlat16_6;
vec3 u_xlat16_7;
vec3 u_xlat8;
vec3 u_xlat9;
ivec3 u_xlati9;
bool u_xlatb9;
bvec3 u_xlatb12;
float u_xlat18;
bvec2 u_xlatb18;
bool u_xlatb27;
float u_xlat16_29;
float u_xlat30;

void main(){

(ImmCB_0_0_0[0] = vec4(1.0, 0.0, 0.0, 0.0));
(ImmCB_0_0_0[1] = vec4(0.0, 1.0, 0.0, 0.0));
(ImmCB_0_0_0[2] = vec4(0.0, 0.0, 1.0, 0.0));
(ImmCB_0_0_0[3] = vec4(0.0, 0.0, 0.0, 1.0));
(u_xlatb0.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseClipPlane)));
(u_xlat16_1 = (vs_COLOR0.w + -0.0099999998));
(u_xlatb9 = (u_xlat16_1 < 0.0));
(u_xlatb0.x = (u_xlatb0.x && u_xlatb9));

if (((int(u_xlatb0.x) * -1) != 0))
{
discard;
}

(u_xlatb0.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UsingDitherAlpha)));

if (u_xlatb0.x)
{
(u_xlatb0.x = (_DitherAlpha < 0.94999999));
if (u_xlatb0.x)
{
(u_xlat0.xy = (vs_TEXCOORD2.yx / vs_TEXCOORD2.ww));
(u_xlat0.xy = (u_xlat0.xy * _ScreenParams.yx));
(u_xlat0.xy = (u_xlat0.xy * vec2(0.25, 0.25)));
(u_xlatb18.xy = greaterThanEqual(u_xlat0.xyxy, (-u_xlat0.xyxy)).xy);
(u_xlat0.xy = fract(abs(u_xlat0.xy)));
(u_xlat0.x = ((u_xlatb18.x) ? (u_xlat0.x) : ((-u_xlat0.x))));
(u_xlat0.y = ((u_xlatb18.y) ? (u_xlat0.y) : ((-u_xlat0.y))));
(u_xlat0.xy = (u_xlat0.xy * vec2(4.0, 4.0)));
(u_xlatu0.xy = uvec2(u_xlat0.xy));
(u_xlat1.x = dot(hlslcc_mtx4x4_DITHERMATRIX[0], ImmCB_0_0_0[int(u_xlatu0.y)]));
(u_xlat1.y = dot(hlslcc_mtx4x4_DITHERMATRIX[1], ImmCB_0_0_0[int(u_xlatu0.y)]));
(u_xlat1.z = dot(hlslcc_mtx4x4_DITHERMATRIX[2], ImmCB_0_0_0[int(u_xlatu0.y)]));
(u_xlat1.w = dot(hlslcc_mtx4x4_DITHERMATRIX[3], ImmCB_0_0_0[int(u_xlatu0.y)]));
(u_xlat0.x = dot(u_xlat1, ImmCB_0_0_0[int(u_xlatu0.x)]));
(u_xlat0.x = ((_DitherAlpha * 17.0) + (-u_xlat0.x)));
(u_xlat0.x = (u_xlat0.x + -0.0099999998));
(u_xlatb0.x = (u_xlat0.x < 0.0));
if (((int(u_xlatb0.x) * -1) != 0))
{
discard;
}
}
}
(u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w);
(u_xlatb9 = (_MainTexAlphaUse == 1.0));
(u_xlat16_2.x = (u_xlat10_0 + (-_MainTexAlphaCutoff)));
(u_xlatb0.x = (u_xlat16_2.x < 0.0));
(u_xlatb0.x = (u_xlatb9 && u_xlatb0.x));
if (((int(u_xlatb0.x) * -1) != 0))
{
discard;
}
(u_xlatb0.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseMaterial2)));
(u_xlatb0.y = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseMaterial3)));
(u_xlatb18.x = (u_xlatb0.y || u_xlatb0.x));
(u_xlatb27 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseMaterial4)));
(u_xlatb18.x = (u_xlatb27 || u_xlatb18.x));
(u_xlatb3 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_UseMaterial5)));
(u_xlatb18.x = (u_xlatb18.x || u_xlatb3));
if (u_xlatb18.x)
{
(u_xlat18 = texture(_LightMapTex, vs_TEXCOORD0.xy).w);
(u_xlatb1 = greaterThanEqual(vec4(u_xlat18), vec4(0.80000001, 0.40000001, 0.2, 0.60000002)));
(u_xlat16_2.xyz = (_Color2.xyz * _OutlineColor2.xyz));
(u_xlati0.xy = ivec2(((uvec2(u_xlatb0.xy) * 4294967295u) & (uvec2(u_xlatb1.xy) * 4294967295u))));
(u_xlatb12.xyz = lessThan(vec4(u_xlat18), vec4(0.60000002, 0.40000001, 0.80000001, 0.80000001)).xyz);
(u_xlat16_4.xyz = (_Color3.xyz * _OutlineColor3.xyz));
(u_xlati0.z = int(((uint(u_xlatb27) * 4294967295u) & (uint(u_xlatb1.z) * 4294967295u))));
(u_xlat16_5.xyz = (_Color4.xyz * _OutlineColor4.xyz));
(u_xlati0.w = int(((uint(u_xlatb1.w) * 4294967295u) & (uint(u_xlatb3) * 4294967295u))));
(u_xlati9.xyz = ivec3(((uvec3(u_xlatb12.xyz) * 4294967295u) & uvec3(u_xlati0.yzw))));
(u_xlat16_6.xyz = (_Color5.xyz * _OutlineColor5.xyz));
(u_xlat16_7.xyz = (_Color.xyz * _OutlineColor.xyz));
(u_xlat16_6.xyz = (((u_xlati9.z != 0)) ? (u_xlat16_6.xyz) : (u_xlat16_7.xyz)));
(u_xlat16_5.xyz = (((u_xlati9.y != 0)) ? (u_xlat16_5.xyz) : (u_xlat16_6.xyz)));
(u_xlat16_4.xyz = (((u_xlati9.x != 0)) ? (u_xlat16_4.xyz) : (u_xlat16_5.xyz)));
(u_xlat16_2.xyz = (((u_xlati0.x != 0)) ? (u_xlat16_2.xyz) : (u_xlat16_4.xyz)));
}
else
{
(u_xlat16_2.xyz = vs_COLOR0.xyz);
}
(u_xlatb0.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_ES_CharacterAmbientLightOn)));
(u_xlat9.xyz = (u_xlat16_2.xyz * vec3(_ES_CharacterAmbientBrightness)));
(u_xlat3.xyz = (vec3(vec3(_ES_CharacterPointLightWholeIntensity, _ES_CharacterPointLightWholeIntensity, _ES_CharacterPointLightWholeIntensity)) * mhy_CharacterPointLightColor.xyz));
(u_xlat8.xyz = vec3(_ES_CharacterMainLightColor.x, _ES_CharacterMainLightColor.y, _ES_CharacterMainLightColor.z));
(u_xlat8.xyz = clamp(u_xlat8.xyz, 0.0, 1.0));
(u_xlat30 = ((-mhy_CharacterPointLightColor.w) + 1.0));
(u_xlat3.xyz = ((u_xlat8.xyz * vec3(u_xlat30)) + u_xlat3.xyz));
(u_xlat3.xyz = clamp(u_xlat3.xyz, 0.0, 1.0));
(u_xlat3.xyz = (u_xlat3.xyz + vec3(-1.0, -1.0, -1.0)));
(u_xlat3.xyz = ((vec3(vec3(_ES_CharacterMainLightRatio, _ES_CharacterMainLightRatio, _ES_CharacterMainLightRatio)) * u_xlat3.xyz) + vec3(1.0, 1.0, 1.0)));
(u_xlat9.xyz = (u_xlat9.xyz * u_xlat3.xyz));
(u_xlat3.xyz = (_ES_CharacterAmbientLightColor.xyz * vec3(_ES_CharacterAmbientLightRatio)));
(u_xlat16_4.xyz = (u_xlat9.xyz * vec3(10.0, 10.0, 10.0)));
(u_xlat16_4.xyz = clamp(u_xlat16_4.xyz, 0.0, 1.0));
(u_xlat9.xyz = ((u_xlat3.xyz * u_xlat16_4.xyz) + u_xlat9.xyz));
(u_xlat16_2.xyz = ((u_xlatb0.x) ? (u_xlat9.xyz) : (u_xlat16_2.xyz)));
(SV_Target0.xyz = ((vs_TEXCOORD1.xyz * vec3(0.5, 0.5, 0.5)) + vec3(0.5, 0.5, 0.5)));
(u_xlatb0.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_ElementViewEleDrawOn)));
(u_xlat16_29 = (_ElementViewEleID * 0.0039215689));
(SV_Target2.z = ((u_xlatb0.x) ? (u_xlat16_29) : (u_xlat16_2.z)));
(SV_Target0.w = _DeferredLightThreshold);
(SV_Target1.xyz = u_xlat16_2.xyz);
(SV_Target1.w = 0.2236068);
(SV_Target2.xy = u_xlat16_2.xy);
(SV_Target2.w = 0.019607844);
return ;
}
