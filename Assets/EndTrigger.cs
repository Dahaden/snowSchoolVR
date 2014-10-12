using UnityEngine;
using System.Collections;

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
