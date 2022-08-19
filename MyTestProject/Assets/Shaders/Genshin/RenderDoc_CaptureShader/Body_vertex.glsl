#version 330

//World Camera Position
uniform vec3 _WorldSpaceCameraPos;

// x = 1 or -1 (-1 if projection is flipped)
// y = near plane
// z = far plane
// w = 1/far plane
uniform vec4 _ProjectionParams;

//object space to world space 
uniform vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
//world space to object space
uniform vec4 hlslcc_mtx4x4unity_WorldToObject[4];
//
uniform vec4 hlslcc_mtx4x4unity_MatrixVPZero[4];

uniform vec4 _MainTex_ST;
uniform float _CharacterAmbientSensorShadowOn;
uniform float _CharacterAmbientSensorForceShadowOn;
uniform vec4 _AmbientSensorUVs;
uniform float _UseClipPlane;
uniform float _ClipPlaneWorld;
uniform vec4 _ClipPlane;
uniform vec4 mhy_AvatarLightDir;
uniform vec4 mhy_CharacterOverrideLightDir;
uniform vec4 mhy_CharacterOverrideLightDirInShadow;
uniform sampler2D _CharacterAmbientSensorTex;

// position
// normal
// TEXCOORD0
// TEXCOORD1
// color
in vec4 in_POSITION0;
in vec3 in_NORMAL0;
in vec2 in_TEXCOORD0;
in vec2 in_TEXCOORD1;
in vec4 in_COLOR0;

// color
// TEXCOORD0~6
out vec4 vs_COLOR0;
out vec4 vs_TEXCOORD0;
out vec4 vs_TEXCOORD1;
out vec4 vs_TEXCOORD2;
out vec3 vs_TEXCOORD3;
out vec2 vs_TEXCOORD4;
out vec3 vs_TEXCOORD5;
//worldPos
out vec4 vs_TEXCOORD6;

float u_xlat0;
bool u_xlatb0;
vec4 u_xlat1;
bool u_xlatb1;
vec4 u_xlat2;
vec4 u_xlat3;
vec4 u_xlat4;
float u_xlat16_5;
vec3 u_xlat6;
bvec3 u_xlatb6;
vec3 u_xlat7;
float u_xlat8;
bool u_xlatb8;
bool u_xlatb13;
float u_xlat18;
float u_xlat19;

void main(){
//u_xlatb0 = _CharacterAmbientSensorShadowOn //角色环境阴影探照
(u_xlatb0 = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_CharacterAmbientSensorShadowOn)));


if (u_xlatb0)
{

(u_xlat0 = textureLod(_CharacterAmbientSensorTex, _AmbientSensorUVs.xy, 0.0).x);
(u_xlatb0 = (0.5 < u_xlat0));
(u_xlat0 = ((u_xlatb0) ? (1.0) : (0.0)));
}
else
{
(u_xlatb6.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(_CharacterAmbientSensorForceShadowOn)));
(u_xlat0 = ((u_xlatb6.x) ? (1.0) : (0.0)));
}

//角色是否在太阳光阴影中
(u_xlatb6.x = (0.5 < mhy_CharacterOverrideLightDirInShadow.w));

(u_xlat0 = ((u_xlatb6.x) ? (1.0) : (u_xlat0)));
(u_xlatb6.xy = lessThan(vec4(0.5, 0.0, 0.0, 0.0), mhy_CharacterOverrideLightDir.wwww).xy);

(vs_TEXCOORD4.x = ((u_xlatb6.x) ? (0.0) : (u_xlat0)));

//worldPos
(u_xlat1.xyz = (in_POSITION0.yyy * hlslcc_mtx4x4unity_ObjectToWorld[1].xyz));
(u_xlat1.xyz = ((hlslcc_mtx4x4unity_ObjectToWorld[0].xyz * in_POSITION0.xxx) + u_xlat1.xyz));
(u_xlat1.xyz = ((hlslcc_mtx4x4unity_ObjectToWorld[2].xyz * in_POSITION0.zzz) + u_xlat1.xyz));
(vs_TEXCOORD6.xyz = ((hlslcc_mtx4x4unity_ObjectToWorld[3].xyz * in_POSITION0.www) + u_xlat1.xyz));

(u_xlatb6.xz = notEqual(vec4(0.0, 0.0, 0.0, 0.0), vec4(_UseClipPlane, _UseClipPlane, _ClipPlaneWorld, _ClipPlaneWorld)).xz);
(u_xlatb1 = (abs(_ClipPlane.w) < 0.001));
(u_xlat7.xyz = (_ClipPlane.www * _ClipPlane.xyz));
(u_xlat1.xyz = ((bool(u_xlatb1)) ? (vec3(0.0, 0.0, 0.0)) : (u_xlat7.xyz)));
(u_xlat2 = (u_xlat1.yyyy * hlslcc_mtx4x4unity_WorldToObject[1]));
(u_xlat2 = ((hlslcc_mtx4x4unity_WorldToObject[0] * u_xlat1.xxxx) + u_xlat2));
(u_xlat1 = ((hlslcc_mtx4x4unity_WorldToObject[2] * u_xlat1.zzzz) + u_xlat2));
(u_xlat1 = (u_xlat1 + hlslcc_mtx4x4unity_WorldToObject[3]));
(u_xlat1.xyz = (u_xlat1.xyz / u_xlat1.www));
(u_xlat2.xyz = (hlslcc_mtx4x4unity_WorldToObject[1].xyz * _ClipPlane.yyy));
(u_xlat2.xyz = ((hlslcc_mtx4x4unity_WorldToObject[0].xyz * _ClipPlane.xxx) + u_xlat2.xyz));
(u_xlat2.xyz = ((hlslcc_mtx4x4unity_WorldToObject[2].xyz * _ClipPlane.zzz) + u_xlat2.xyz));
(u_xlat1.x = dot(u_xlat1.xyz, u_xlat2.xyz));
(u_xlat7.x = dot(in_POSITION0.xyz, u_xlat2.xyz));
(u_xlatb13 = (u_xlat7.x < u_xlat1.x));
(u_xlat1.x = ((-u_xlat1.x) + u_xlat7.x));
(u_xlat2.xyz = (((-u_xlat1.xxx) * u_xlat2.xyz) + in_POSITION0.xyz));
(u_xlat2.w = 0.0);
(u_xlat3.xyz = in_POSITION0.xyz);
(u_xlat3.w = in_COLOR0.w);
(u_xlat1 = ((bool(u_xlatb13)) ? (u_xlat2) : (u_xlat3)));
(u_xlat2.x = dot(in_POSITION0.xyz, _ClipPlane.xyz));
(u_xlat8 = (_ClipPlane.w + -0.0099999998));
(u_xlatb8 = (u_xlat2.x < u_xlat8));
(u_xlat2.x = (u_xlat2.x + (-_ClipPlane.w)));
(u_xlat4.xyz = (((-u_xlat2.xxx) * _ClipPlane.xyz) + in_POSITION0.xyz));
(u_xlat4.w = 0.0);
(u_xlat2 = ((bool(u_xlatb8)) ? (u_xlat4) : (u_xlat3)));
(u_xlat1.xyz = ((u_xlatb6.z) ? (u_xlat1.xyz) : (u_xlat2.xyz)));
(u_xlat16_5 = ((u_xlatb6.z) ? (u_xlat1.w) : (u_xlat2.w)));
(u_xlat1.xyz = ((u_xlatb6.x) ? (u_xlat1.xyz) : (in_POSITION0.xyz)));
(vs_COLOR0.w = ((u_xlatb6.x) ? (u_xlat16_5) : (in_COLOR0.w)));
(u_xlat2.xyw = ((-_WorldSpaceCameraPos.xyz) + hlslcc_mtx4x4unity_ObjectToWorld[3].xyz));
(u_xlat3.x = hlslcc_mtx4x4unity_ObjectToWorld[0].x);
(u_xlat3.y = hlslcc_mtx4x4unity_ObjectToWorld[1].x);
(u_xlat3.z = hlslcc_mtx4x4unity_ObjectToWorld[2].x);
(u_xlat3.w = u_xlat2.x);
(u_xlat1.w = 1.0);
(u_xlat6.x = dot(u_xlat3, u_xlat1));
(u_xlat3.x = hlslcc_mtx4x4unity_ObjectToWorld[0].y);
(u_xlat3.y = hlslcc_mtx4x4unity_ObjectToWorld[1].y);
(u_xlat3.z = hlslcc_mtx4x4unity_ObjectToWorld[2].y);
(u_xlat3.w = u_xlat2.y);
(u_xlat18 = dot(u_xlat3, u_xlat1));
(u_xlat2.x = hlslcc_mtx4x4unity_ObjectToWorld[0].z);
(u_xlat2.y = hlslcc_mtx4x4unity_ObjectToWorld[1].z);
(u_xlat2.z = hlslcc_mtx4x4unity_ObjectToWorld[2].z);
(u_xlat2.x = dot(u_xlat2, u_xlat1));
(u_xlat3.x = hlslcc_mtx4x4unity_ObjectToWorld[0].w);
(u_xlat3.y = hlslcc_mtx4x4unity_ObjectToWorld[1].w);
(u_xlat3.z = hlslcc_mtx4x4unity_ObjectToWorld[2].w);
(u_xlat3.w = hlslcc_mtx4x4unity_ObjectToWorld[3].w);
(u_xlat19 = dot(u_xlat3, u_xlat1));


(u_xlat3 = (vec4(u_xlat18) * hlslcc_mtx4x4unity_MatrixVPZero[1]));
(u_xlat3 = ((hlslcc_mtx4x4unity_MatrixVPZero[0] * u_xlat6.xxxx) + u_xlat3));
(u_xlat2 = ((hlslcc_mtx4x4unity_MatrixVPZero[2] * u_xlat2.xxxx) + u_xlat3));
(u_xlat2 = ((hlslcc_mtx4x4unity_MatrixVPZero[3] * vec4(u_xlat19)) + u_xlat2));
(u_xlat3 = (u_xlat1.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1]));


(u_xlat3 = ((hlslcc_mtx4x4unity_ObjectToWorld[0] * u_xlat1.xxxx) + u_xlat3));
(u_xlat1 = ((hlslcc_mtx4x4unity_ObjectToWorld[2] * u_xlat1.zzzz) + u_xlat3));
(u_xlat1 = ((hlslcc_mtx4x4unity_ObjectToWorld[3] * in_POSITION0.wwww) + u_xlat1));
(vs_TEXCOORD3.xyz = (u_xlat1.xyz / u_xlat1.www));
(vs_TEXCOORD0.xy = ((in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw));
(vs_TEXCOORD0.zw = ((in_TEXCOORD1.xy * _MainTex_ST.xy) + _MainTex_ST.zw));
(u_xlat1.xz = (u_xlat2.xw * vec2(0.5, 0.5)));
(u_xlat6.x = (u_xlat2.y * _ProjectionParams.x));
(u_xlat1.w = (u_xlat6.x * 0.5));
(vs_TEXCOORD2.xy = (u_xlat1.zz + u_xlat1.xw));
(u_xlat1.xyz = (in_NORMAL0.yyy * hlslcc_mtx4x4unity_ObjectToWorld[1].xyz));
(u_xlat1.xyz = ((hlslcc_mtx4x4unity_ObjectToWorld[0].xyz * in_NORMAL0.xxx) + u_xlat1.xyz));
(u_xlat1.xyz = ((hlslcc_mtx4x4unity_ObjectToWorld[2].xyz * in_NORMAL0.zzz) + u_xlat1.xyz));
(u_xlat6.x = dot(u_xlat1.xyz, u_xlat1.xyz));
(u_xlat6.x = inversesqrt(u_xlat6.x));
(u_xlat1.xyz = (u_xlat6.xxx * u_xlat1.xyz));
(u_xlatb6.x = (vec4(0.0, 0.0, 0.0, 0.0) != vec4(u_xlat0)));
(u_xlat3.xyz = ((u_xlatb6.x) ? (mhy_CharacterOverrideLightDirInShadow.xyz) : (mhy_CharacterOverrideLightDir.xyz)));
(u_xlat6.xyz = ((u_xlatb6.y) ? (u_xlat3.xyz) : (mhy_AvatarLightDir.xyz)));
(u_xlat16_5 = dot(u_xlat1.xyz, u_xlat6.xyz));
(vs_TEXCOORD1.w = ((u_xlat16_5 * 0.4975) + 0.5));

(gl_Position = u_xlat2);
(vs_COLOR0.xyz = in_COLOR0.xyz);
(vs_TEXCOORD1.xyz = u_xlat1.xyz);
(vs_TEXCOORD2.zw = u_xlat2.zw);
(vs_TEXCOORD4.y = u_xlat0);
(vs_TEXCOORD5.xyz = vec3(0.0, 0.0, 0.0));
(vs_TEXCOORD6.w = 0.0);
return ;
}
