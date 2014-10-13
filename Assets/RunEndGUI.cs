using UnityEngine;
using System.Collections;

public class RunEndGUI : VRGUI {

	public GUISkin skin;
	public int timeLeft;

	public override void OnVRGUI ()
	{
		GUI.skin = skin;
		
		GUI.Box (new Rect (Screen.width / 4 - 300, 170, 1200f, 420f), "");
		
		GUI.Label (new Rect (Screen.width / 4 - 300, 200, 1200f, 100f), "WELL DONE!");
		GUI.Label (new Rect (Screen.width / 4 + 50, 250, 500f, 120f), "You reached the bottom of the mountain. The next run will begin in:");

		GUI.Label (new Rect (Screen.width / 4 - 300, 350, 1200, 100), timeLeft.ToString());        
	}
}
