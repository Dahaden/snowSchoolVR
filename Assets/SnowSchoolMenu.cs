using UnityEngine;
using System.Collections;

public class SnowSchoolMenu : VRGUI
{
		public GUISkin skin;
		public Texture2D lineImage;

		public override void OnVRGUI ()
		{
				GUI.skin = skin;
	
				GUI.Box (new Rect (Screen.width / 4 - 300, 170, 1200f, 420f), "");

				GUI.Label (new Rect (Screen.width / 4 - 300, 200, 1200f, 100f), "SNOW SCHOOL VR");
				GUI.Label (new Rect (Screen.width / 4 + 50, 250, 500f, 120f), "Use the balance board to control your movement as if you are snowboarding.\nThe aim of this exercise is to move left and right to touch the green lines on the snow ");

				GUI.DrawTexture (new Rect (Screen.width / 2 - 125, 380, 230f, 128f), lineImage, ScaleMode.StretchToFill);
		
				GUI.Label (new Rect (Screen.width / 4 - 300, Screen.height - 200, 1200, 100), "Press Space To Start");        
		}
	
}