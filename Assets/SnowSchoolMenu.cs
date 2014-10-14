using UnityEngine;
using System.Collections;

public class SnowSchoolMenu : VRGUI
{
		public GUISkin skin;
		public Texture2D lineImage;
		public bool showBreak = false;

		public override void OnVRGUI ()
		{
				GUI.skin = skin;
	
				GUI.Box (new Rect (Screen.width / 4 - 300, 170, 1200f, 420f), "");

				GUI.Label (new Rect (Screen.width / 4 - 300, 200, 1200f, 100f), "SNOW SCHOOL VR");
				
				string promptStr = showBreak ? "Good job! You have completed the first half of this demo. Take a break, grab some chocolate and we'll tell you what will happen next." : "Use the balance board to control your movement as if you are snowboarding.\nThe aim of this exercise is to move left and right to touch the green lines on the snow ";
				GUI.Label (new Rect (Screen.width / 4 + 50, 250, 500f, 120f), promptStr);

				GUI.DrawTexture (new Rect (Screen.width / 2 - 125, 380, 230f, 128f), lineImage, ScaleMode.StretchToFill);

				string keyStr = showBreak ? "Press Space To Continue" : "Press Space To Start";
				GUI.Label (new Rect (Screen.width / 4 - 300, Screen.height - 200, 1200, 100), keyStr);        
		}
	
}