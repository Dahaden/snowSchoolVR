/* Code provided by Chris Morris of Six Times Nothing (http://www.sixtimesnothing.com) */
/* Free to use and modify  */


Shader "Hidden/TerrainEngine/Splatmap/Lightmap-FirstPass" {
Properties {
	_Control ("Control (RGBA)", 2D) = "red" {}
	_Splat3 ("Layer 3 (A)", 2D) = "white" {}
	_Splat2 ("Layer 2 (B)", 2D) = "white" {}
	_Splat1 ("Layer 1 (G)", 2D) = "white" {}
	_Splat0 ("Layer 0 (R)", 2D) = "white" {}
	// used in fallback on old cards
	_MainTex ("BaseMap (RGB)", 2D) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
}

SubShader {
	Tags {
		"SplatCount" = "4"
		"Queue" = "Geometry-100"
		"RenderType" = "Opaque"
	}
CGPROGRAM
#pragma surface surf BlinnPhong vertex:vert
#pragma target 3.0
#include "UnityCG.cginc"

struct Input {
	float3 worldPos;
	float2 uv_Control : TEXCOORD0;
	float2 uv_Splat0 : TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
	float2 uv_Splat2 : TEXCOORD3;
	float2 uv_Splat3 : TEXCOORD4;
	
	///
	
	float distance;
	
	
};

void vert (inout appdata_full v, out Input o) {
	// Supply the shader with tangents for the terrain
	// A general tangent estimation	
	float3 T1 = float3(1, 0, 1);
	float3 Bi = cross(T1, v.normal);
	float3 newTangent = cross(v.normal, Bi);
	normalize(newTangent);
	v.tangent.xyz = newTangent.xyz;
	if (dot(cross(v.normal,newTangent),Bi) < 0)
		v.tangent.w = -1.0f;
	else
		v.tangent.w = 1.0f;
	
	float distanceVar = distance(_WorldSpaceCameraPos, mul(_Object2World, v.vertex));	
	o.distance = distanceVar;
	
}
sampler2D _Control;
sampler2D _CustomColorMap, _TerrainNormalMap;
sampler2D _BumpMap0, _BumpMap1, _BumpMap2, _BumpMap3;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
float _Spec0, _Spec1, _Spec2, _Spec3;
float3 _ColTex0, _ColTex1, _ColTex2, _ColTex3;
float4 _v4CameraPos;
float _SplattingDistance;

void surf (Input IN, inout SurfaceOutput o) {
	
	half3 col;
	half3 finalCol;
	
	// do all texture fetches outside the if
	half3 colorMap = tex2D(_CustomColorMap, IN.uv_Control);
	half4 splat_control = tex2D(_Control, IN.uv_Control);
	half3 splatcol0 = tex2D (_Splat0, IN.uv_Splat0).rgb;
	half3 splatcol1 = tex2D (_Splat1, IN.uv_Splat1).rgb;
	half3 splatcol2 = tex2D (_Splat2, IN.uv_Splat2).rgb;
	half3 splatcol2_2nd = tex2D (_Splat2, IN.uv_Splat2* -.5).rgb;
	half3 splatcol3 = tex2D (_Splat3, IN.uv_Splat3).rgb;
	float3 normalsplat0 = UnpackNormal(tex2D(_BumpMap0, IN.uv_Splat0));
	float3 normalsplat1 = UnpackNormal(tex2D(_BumpMap1, IN.uv_Splat1));
	float3 normalsplat2 = UnpackNormal(tex2D(_BumpMap2, IN.uv_Splat2));
	float3 normalsplat2_2nd = UnpackNormal(tex2D(_BumpMap2, IN.uv_Splat2* -.5));
	float3 normalsplat3 = UnpackNormal(tex2D(_BumpMap3, IN.uv_Splat3));
	float3 farnormal = UnpackNormal(tex2D(_TerrainNormalMap, IN.uv_Control));
	
	if (IN.distance < _SplattingDistance)
	{
		
		//// 4 splats, normals, and specular settings
		// use the built in splat map --> make sure you have copied your custom made to the built in via script
		
		//// first detail texture: red = ground
		col = splat_control.r * splatcol0;
		o.Normal = splat_control.r * normalsplat0;

		//// second detail texture: green = grass
		col += splat_control.g * splatcol1;
		o.Normal += splat_control.g * normalsplat1;
	
		//// third detail texture rock: blue = rock
		// uses uv mixing
		col += splat_control.b * (splatcol2*.65 + splatcol2_2nd*.35);
		o.Normal += splat_control.b * (normalsplat2*.5 + normalsplat2_2nd*.5);
		
		//// forth detail texture: alpha
		col += splat_control.a * splatcol3;
		o.Normal += splat_control.a * normalsplat3;
		
		/// final composition
		
		// fade out detail normal maps
		float fadeout = pow(IN.distance/_SplattingDistance, 4.0);
		o.Normal = lerp(normalize(o.Normal + farnormal), farnormal, fadeout);
		
		// apply colorcorrection to detail maps
		// see: http://blog.wolfire.com/2009/12/Detail-texture-color-matching
		half3 color_correction = splat_control.r*_ColTex0 + splat_control.g*_ColTex1 + splat_control.b*_ColTex2 + splat_control.a*_ColTex3;
		finalCol = col * (colorMap/color_correction);
		
		// fade out detail maps
		finalCol = lerp(finalCol, colorMap, fadeout);
	}
    else {
    	finalCol = colorMap;
    	o.Normal = farnormal;
    }
        
	o.Albedo = finalCol;
	o.Alpha = 0.0;
}
ENDCG  
}

// Fallback to Diffuse
Fallback "Diffuse"
}