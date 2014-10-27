using UnityEngine;
using System.Collections;

// Provides trigger for end of the map

public class EndTrigger : MonoBehaviour {

	public StateMech stateM;
	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.name == "GamePlayer") {
			Debug.Log ("HIT END PLANE");
			//return to start
			stateM.returnToStart();

		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
