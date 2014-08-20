/* Code provided by Chris Morris of Six Times Nothing (http://www.sixtimesnothing.com) */
/* Free to use and modify */

using UnityEngine;
using System.Collections;

public class CustomTerrainScriptColorMap : MonoBehaviour {
	public float SplattingDistance = 600;
	
	public Texture2D CustomColorMap;
	public Texture2D TerrainNormalMap;
	
	public Color ColTex0;
	public Color ColTex1;
	public Color ColTex2;
	public Color ColTex3;

	public Texture2D Bump0;
	public Texture2D Bump1;
	public Texture2D Bump2;
	public Texture2D Bump3;
	
	void Start () {
		
		// make sure that we will never see the basemap...
		Terrain.activeTerrain.basemapDistance = 100000;
		
		Shader.SetGlobalFloat("_SplattingDistance", SplattingDistance);
		
		Shader.SetGlobalColor("_ColTex0", ColTex0);
		Shader.SetGlobalColor("_ColTex1", ColTex1);
		Shader.SetGlobalColor("_ColTex2", ColTex2);
		Shader.SetGlobalColor("_ColTex3", ColTex3);
		
		if(CustomColorMap)
			Shader.SetGlobalTexture("_CustomColorMap", CustomColorMap);
				
		if(TerrainNormalMap)
			Shader.SetGlobalTexture("_TerrainNormalMap", TerrainNormalMap);	
		
		if(Bump0)
			Shader.SetGlobalTexture("_BumpMap0", Bump0);
		
		if(Bump1)
			Shader.SetGlobalTexture("_BumpMap1", Bump1);
		
		if(Bump2)
			Shader.SetGlobalTexture("_BumpMap2", Bump2);
		
		if(Bump3)
			Shader.SetGlobalTexture("_BumpMap3", Bump3);
		
		
	}

}
