using UnityEngine;
using System.Collections;

public class RunEndGUI : VRGUI
{

		public GUISkin skin;
		public int timeLeft;
		public bool playbackNext;
		public bool endOfPlayback;

		public override void OnVRGUI ()
		{
				GUI.skin = skin;
		
				GUI.Box (new Rect (Screen.width / 4 - 300, 170, 1200f, 420f), "");
		
				GUI.Label (new Rect (Screen.width / 4 - 300, 200, 1200f, 100f), "WELL DONE!");

				string text = "";
				if (playbackNext) {
						text = "You reached the bottom of the mountain. You will be shown a playback of your run in:";
				} else if (endOfPlayback) {
						text = "You have reached the end of the playback. Your next run will begin in:";
				} else {
						text = "You reached the bottom of the mountain. The next run will begin in:";
				}
				GUI.Label (new Rect (Screen.width / 4 + 50, 250, 500f, 120f), text);

				GUI.Label (new Rect (Screen.width / 4 - 300, 350, 1200, 100), timeLeft.ToString ());        
		}
}
