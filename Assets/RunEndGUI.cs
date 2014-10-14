using UnityEngine;
using System.Collections;

public class RunEndGUI : VRGUI
{

		public GUISkin skin;
		public int timeLeft;
		public bool playbackNext;
		public bool endOfPlayback;
		public bool runTimeEnd;
		public bool gameTimeEnd;
		private string labelText;

		public override void OnVRGUI ()
		{
				GUI.skin = skin;
		
				GUI.Box (new Rect (Screen.width / 4 - 300, 170, 1200f, 420f), "");
		
				GUI.Label (new Rect (Screen.width / 4 - 300, 200, 1200f, 100f), "WELL DONE!");

				labelText = "";

				if (gameTimeEnd) {
						labelText = "You have reached the end of this game! The game will close in:";
				} else if (endOfPlayback) {
						labelText = "You have reached the end of the playback. Your next run will begin in:";
				} else if (playbackNext && !runTimeEnd) {
						labelText = "You reached the bottom of the mountain. You will be shown a playback of your run in:";
				} else if (playbackNext && runTimeEnd) {
						labelText = "Your run timed out. You will be shown a playback of your run in:";
				} else if (runTimeEnd) {
						labelText = "Your run timed out. The next run will begin in:";
				} else {
						labelText = "You reached the bottom of the mountain. The next run will begin in:";
				}

				GUI.Label (new Rect (Screen.width / 4 + 50, 250, 500f, 120f), labelText);

				GUI.Label (new Rect (Screen.width / 4 - 300, 350, 1200, 100), timeLeft.ToString ());        
		}

		public void resetBools ()
		{
				playbackNext = false;
				endOfPlayback = false;
				runTimeEnd = false;
				gameTimeEnd = false;
		}
}
