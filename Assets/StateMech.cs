using UnityEngine;
using System.Collections;

public class StateMech : MonoBehaviour {

	public ArrayList positions = new ArrayList();

	public ArrayList rotations = new ArrayList();

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		positions.Add(gameObject.transform.position);
		rotations.Add (gameObject.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Time: " + Time.time);
		if (Time.time > 10.0) { // Seconds since start of game
			Debug.Log("Stopped recording");
		} else {
			Debug.Log("Atleast Im here?");
			positions.Add (gameObject.transform.position);
			rotations.Add (gameObject.transform.rotation);
		}
	}

	public void Save() {

	}
}

class PlayerData {

}