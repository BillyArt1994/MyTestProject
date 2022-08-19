#ifndef Math
#define Math

#define PI_Mobile 3.14

//remap form t1-t2 to s1-s2
inline half Remap(half x, half t1, half t2, half s1, half s2)
{
	return (x - t1) / (t2 - t1) * (s2 - s1) + s1;
}

inline half2 Remap(half2 coordinate, half t1, half t2, half s1, half s2)
{
	coordinate -=t1;
	coordinate *= (1/(t2 - t1) * (s2 - s1));
	coordinate +=s1;
	return coordinate;
}

//��ת����
half3 RotateFunction(half3 Target,half RotateValue)      
{
    half rotation_cube = RotateValue * UNITY_PI / 180;
    half cos2 = cos(rotation_cube);
    half sin2 = sin(rotation_cube);
    half2x2 m_rotate_cube = half2x2(cos2, -sin2, sin2, cos2);
    half2 redlect_dir_rotate = mul(Target.xz, m_rotate_cube);    
    Target = half3(redlect_dir_rotate.x, Target.y, redlect_dir_rotate.y);   //��סY��
    return Target;
}

half3 RGBToLuminance(half3 rgb){
    return rgb.x*0.299+rgb.y*0.587+rgb.z*0.114;
}


        //RGB to HSV
float3 RGB2HSV(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

       //HSV to RGB
float3 HSV2RGB( float3 c ){
    float3 rgb = clamp( abs(fmod(c.x*6.0+float3(0.0,4.0,2.0),6)-3.0)-1.0, 0, 1);
    rgb = rgb*rgb*(3.0-2.0*rgb);
    return c.z * lerp( float3(1,1,1), rgb, c.y);
} 

#endif // Math