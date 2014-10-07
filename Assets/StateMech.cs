using UnityEngine;
using System.Collections;

public class StateMech : MonoBehaviour {

	private Hashtable saved = new Hashtable();

	private int position;
	private int max = 0;
	private float timeOffset = (float)0.0;

	private GameObject camera3rdPerson;
	private GameObject camera1stPerson;

	public bool OVRActive = false;
	public float timeLoop = (float)30.0;

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		if (!OVRActive) {
				camera1stPerson = findGameObject ("1stPersonCamera", gameObject);
				camera3rdPerson = findGameObject ("3rdPersonCamera", gameObject);
			//turnOff(true, findGameObject ("1stPersonOVRCameraController", gameObject));
			//turnOff(true, findGameObject ("3rdPersonOVRCameraController", gameObject));
		} else {
			camera1stPerson = findGameObject("1stPersonOVRCameraController", gameObject);
			camera3rdPerson = findGameObject("3rdPersonOVRCameraController", gameObject);
			//turnOff(true, findGameObject ("3rdPersonCamera", gameObject));
			//turnOff(true, findGameObject ("1stPersonCamera", gameObject));
		}
		turnOff(true, camera3rdPerson);

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Time: " + Time.time);
		if (Time.time > timeLoop + timeOffset) { // Seconds since start of game
			//Debug.Log("Stopped recording");
			if(position == 0) {
				// Component script;
				//string name = "Key Board Input";
				//gameObject.GetComponent("Key Board Input");
				//script.active = false;
				//name = "Zig Skeleton";
				//gameObject.GetComponentInChildren(name).active = false;
				switch3rdPerson(true);
				//gameObject.GetComponent("Key Board Input").active = false;
				//findGameObject("Dana", gameObject).GetComponent("Zig Skeleton").active = false;

			}
			setFromHash(gameObject.transform);
			position++;
			if (position == max) {
				timeOffset = Time.time;
				switch3rdPerson(false);
				//gameObject.GetComponent("Key Board Input").active = true;
				//findGameObject("Dana", gameObject).GetComponent("Zig Skeleton").active = true;
				gameObject.transform.position = ((GOReference)((ArrayList)saved[gameObject.name])[0]).position;
				gameObject.transform.rotation = ((GOReference)((ArrayList)saved[gameObject.name])[0]).rotation;
				gameObject.rigidbody.velocity = new Vector3(0, 0, 0);
				saved = new Hashtable();
				position = 0;
			}
		} else {
			//Debug.Log("Atleast Im here?");
			updateHash(gameObject.transform);
			max++;
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
				turnOff(true, camera1stPerson);
				turnOff(false, camera3rdPerson);
			} else {
				turnOff(false, camera1stPerson);
				turnOff(true, camera3rdPerson);
			}
		}
	}

	void turnOff(bool off, GameObject camera) {
		if (camera.name == "1stPersonCamera" || camera.name == "3rdPersonCamera") {
			camera.camera.enabled = !off;
		} else if(camera.name == "1stPersonOVRCameraController" || camera.name == "3rdPersonOVRCameraController"){
			foreach(Transform child in camera.transform) {
				child.gameObject.camera.enabled = !off;
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