using UnityEngine;
using System.Collections;

public class StateMech : MonoBehaviour {

	private Hashtable saved = new Hashtable();

	private int position;

	private GameObject camera3rdPerson;
	private GameObject camera1stPerson;

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		//if (Camera.main.name == "1stPersonCamera" || Camera.main.name == "3rdPersonCamera") {
			camera1stPerson = findGameObject("1stPersonCamera", gameObject);
			camera3rdPerson = findGameObject("3rdPersonCamera", gameObject);
		//} else if (Camera.main.name == "1stPersonOVRCameraController" || Camera.main.name == "3rdPersonOVRCameraController"){
			//camera1stPerson = findGameObject("1stPersonOVRCameraController", gameObject);
			//camera3rdPerson = findGameObject("3rdPersonOVRCameraController", gameObject);
		//}
		camera3rdPerson.camera.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Time: " + Time.time);
		if (Time.time > 10.0) { // Seconds since start of game
			Debug.Log("Stopped recording");
			if(position == 0) {
				// Component script;
				//string name = "Key Board Input";
				//gameObject.GetComponent("Key Board Input");
				//script.active = false;
				//name = "Zig Skeleton";
				//gameObject.GetComponentInChildren(name).active = false;
				switch3rdPerson(true);

			}
			setFromHash(gameObject.transform);
			position++;
		} else {
			Debug.Log("Atleast Im here?");
			updateHash(gameObject.transform);
		}
	}

	void setFromHash(Transform transform) {
		if (saved.Contains (transform.name)) {
			GOReference reference = (GOReference)((ArrayList)saved[transform.name])[position];
			Vector3 gamePosition = reference.position;
			Quaternion gamerotation = reference.rotation;
			transform.position = gamePosition;
			transform.rotation = gamerotation;
			foreach(Transform child in transform) {
				if(!child.name.Contains("camera")){
					setFromHash(child);
				}
			}
		}
	}

	void updateHash(Transform transform) {
		if (saved [transform.name] == null) {
			saved [transform.name] = new ArrayList();
		}
		GOReference reference = new GOReference ();
		reference.position = transform.position;
		reference.rotation = transform.rotation;
		((ArrayList)saved [transform.name]).Add (reference);

		foreach (Transform child in transform) {
			if(!child.name.Contains("camera")){
				updateHash(child);
			}
		}
	}

	void switch3rdPerson(bool to) {
		Camera[] cameras = new Camera[Camera.allCamerasCount];
		Camera.GetAllCameras (cameras);
		if (camera1stPerson != null) {
			if (to) {
				camera1stPerson.camera.enabled = false;
				camera3rdPerson.camera.enabled = true;
			} else {

			}
		}
	}

	GameObject findGameObject(string name, GameObject go) {
		if (go.name == name) {
			return go;
		}
		foreach (Transform child in go.transform) {
			GameObject hope = findGameObject(name, child.gameObject);
			if(hope != null) {
				return hope;
			}
		}
		return null;
	}

	public void Save() {

	}
}

class GOReference {
	public Vector3 position;
	public Quaternion rotation;
}

class PlayerData {

}