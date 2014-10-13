using UnityEngine;
using System.Collections;
using System.Linq;

public class StateMech : MonoBehaviour
{

		private Hashtable saved = new Hashtable ();
		private int position;
		private int max = 0;
		public float timeWithoutFeedForward = (float)120.0;
		private float timeOffset = (float)0;
		private GameObject camera3rdPerson;
		private GameObject camera1stPerson;
		private ArrayList spheres = new ArrayList ();
		public bool OVRActive = false;
		public float timeLoop = (float)300.0;
		public Boundaries boundaries;
		private string numHits = "";
		private SnowSchoolMenu initialGUI;

		void Awake ()
		{
				DontDestroyOnLoad (gameObject);
		}

		public void returnToStart ()
		{
				if (camera1stPerson.camera.enabled == true) {
						//save number of hits 
						numHits = numHits + "," + ((boundaries.leftHits + boundaries.rightHits).ToString ());
						boundaries.leftHits = 0;
						boundaries.rightHits = 0;
				}
				if (Time.time > timeWithoutFeedForward) {
						//Debug.Log("Stopped recording");
						if (position == 0) {
								//switch3rdPerson (true);
				
						}
			
						// Destroy the last frame of spheres
						foreach (GameObject sphere in spheres) {
								Destroy (sphere);
						}
						spheres.Clear ();
			
						setFromHash (gameObject.transform);
			
						position++;
						if (position == max) {
								timeOffset = Time.time;
								switch3rdPerson (false);
								//gameObject.GetComponent("Key Board Input").active = true;
								//findGameObject("Dana", gameObject).GetComponent("Zig Skeleton").active = true;
								gameObject.transform.position = ((GOReference)((ArrayList)saved [gameObject.name]) [0]).position;
								gameObject.transform.rotation = ((GOReference)((ArrayList)saved [gameObject.name]) [0]).rotation;
								gameObject.rigidbody.velocity = new Vector3 (0, 0, 0);
								saved = new Hashtable ();
								position = 0;
								max = 0;
				
								// Destroy the last frame of spheres
								foreach (GameObject sphere in spheres) {
										Destroy (sphere);
								}
								spheres.Clear ();
						}
	
				} else {
						gameObject.transform.position = ((GOReference)((ArrayList)saved [gameObject.name]) [0]).position;
						gameObject.transform.rotation = ((GOReference)((ArrayList)saved [gameObject.name]) [0]).rotation;
						gameObject.rigidbody.velocity = new Vector3 (0, 0, 0);
						saved = new Hashtable ();
						position = 0;
						max = 0;
				}
		}
		// Use this for initialization
		void Start ()
		{
		
				//Debug.Log("Start");
				if (!OVRActive) {
						camera1stPerson = findGameObject ("1stPersonCamera", gameObject);
						camera3rdPerson = findGameObject ("3rdPersonCamera", gameObject);
						//turnOff(true, findGameObject ("1stPersonOVRCameraController", gameObject));
						//turnOff(true, findGameObject ("3rdPersonOVRCameraController", gameObject));
				} else {
						camera1stPerson = findGameObject ("1stPersonOVRCameraController", gameObject);
						camera3rdPerson = findGameObject ("3rdPersonOVRCameraController", gameObject);
						//turnOff(true, findGameObject ("3rdPersonCamera", gameObject));
						//turnOff(true, findGameObject ("1stPersonCamera", gameObject));
				}

				turnOff (true, camera3rdPerson);

				initialGUI = gameObject.GetComponentInChildren<SnowSchoolMenu>();
				//gameObject.SetActive (false);
				// hide the cursor
				Screen.lockCursor = true;
				Screen.showCursor = false;
		
		}
	
		// Update is called once per frame
		void Update ()
		{

				//hide initial screen if showing and press space
				if (Input.GetKeyDown (KeyCode.Space) && initialGUI.enabled) {
					initialGUI.enabled = false;
					gameObject.rigidbody.constraints = RigidbodyConstraints.None;
				}else if(Input.GetKeyDown (KeyCode.R)){
					//recenter rift
					OVRCamera.ResetCameraPositionOrientation (Vector3.one, Vector3.zero, Vector3.up, Vector3.zero);
				}
//				//Debug.Log ("Time: " + Time.time);
//				if (Time.time > timeLoop + timeOffset) { // Seconds since start of game
//						//Debug.Log("Stopped recording");
//						if (position == 0) {
//								// Component script;
//								//string name = "Key Board Input";
//								//gameObject.GetComponent("Key Board Input");
//								//script.active = false;
//								//name = "Zig Skeleton";
//								//gameObject.GetComponentInChildren(name).active = false;
//								switch3rdPerson (true);
//								//gameObject.GetComponent("Key Board Input").active = false;
//								//findGameObject("Dana", gameObject).GetComponent("Zig Skeleton").active = false;
//
//						}
//
//						// Destroy the last frame of spheres
//						foreach (GameObject sphere in spheres) {
//								Destroy (sphere);
//						}
//						spheres.Clear ();
//
//						setFromHash (gameObject.transform);
//
//						position++;
//						if (position == max) {
//								timeOffset = Time.time;
//								switch3rdPerson (false);
//								//gameObject.GetComponent("Key Board Input").active = true;
//								//findGameObject("Dana", gameObject).GetComponent("Zig Skeleton").active = true;
//								gameObject.transform.position = ((GOReference)((ArrayList)saved [gameObject.name]) [0]).position;
//								gameObject.transform.rotation = ((GOReference)((ArrayList)saved [gameObject.name]) [0]).rotation;
//								gameObject.rigidbody.velocity = new Vector3 (0, 0, 0);
//								saved = new Hashtable ();
//								position = 0;
//								max = 0;
//
//								// Destroy the last frame of spheres
//								foreach (GameObject sphere in spheres) {
//										Destroy (sphere);
//								}
//								spheres.Clear ();
//						}
//				} else {
				if (camera1stPerson.camera.enabled == true) {
						updateHash (gameObject.transform);
						max++;
				}

				if (Time.time > timeLoop) {
						//save and quit
						System.IO.File.WriteAllText (@"C:\Users\Sara\Desktop\SnowSchoolData.csv", numHits);
						Application.Quit ();
				}
		}

		void setFromHash (Transform transform)
		{
				if (saved.Contains (transform.name)) {
						GOReference reference = (GOReference)((ArrayList)saved [transform.name]) [position];
						Vector3 gamePosition = reference.position;
						Quaternion gamerotation = reference.rotation;
						transform.position = gamePosition;
						transform.rotation = gamerotation;

						// Create red highlighted portions for all transforms that contain a certain string
						if (transform.name.Contains ("Leg")) {
				
								GameObject mySphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
								spheres.Add (mySphere);
								mySphere.collider.enabled = false;
								mySphere.transform.localScale = new Vector3 (1, 1, 1) / 5;
								mySphere.transform.position = transform.position;
								Color color = mySphere.renderer.material.color;

								color = Color.red;
								color.a = 1 - reference.score;
								mySphere.renderer.material.color = color;
								mySphere.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
								//Debug.Log ("Shader - " + mySphere.renderer.material.shader);
						}

						foreach (Transform child in transform) {
								if (!child.name.Contains ("Camera")) {
										setFromHash (child);
								}
						}
				}
		}

		void updateHash (Transform transform)
		{
				if (saved [transform.name] == null) {
						saved [transform.name] = new ArrayList ();
				}
				GOReference reference = new GOReference ();
				reference.position = transform.position;
				reference.rotation = transform.rotation;
				((ArrayList)saved [transform.name]).Add (reference);

				foreach (Transform child in transform) {
						if (!child.name.Contains ("Camera")) {
								updateHash (child);
						}
				}
		}

		void switch3rdPerson (bool to)
		{
				Camera[] cameras = new Camera[Camera.allCamerasCount];
				Camera.GetAllCameras (cameras);
				if (camera1stPerson != null) {
						if (to) {
								turnOff (true, camera1stPerson);
								turnOff (false, camera3rdPerson);
						} else {
								turnOff (false, camera1stPerson);
								turnOff (true, camera3rdPerson);
						}
				}
		}

		void turnOff (bool off, GameObject camera)
		{
				if (camera.name == "1stPersonCamera" || camera.name == "3rdPersonCamera") {
						camera.camera.enabled = !off;
				} else if (camera.name == "1stPersonOVRCameraController" || camera.name == "3rdPersonOVRCameraController") {
						foreach (Transform child in camera.transform) {
								child.gameObject.camera.enabled = !off;
						}
				}
		}

		GameObject findGameObject (string name, GameObject go)
		{
				if (go.name == name) {
						return go;
				}
				foreach (Transform child in go.transform) {
						GameObject hope = findGameObject (name, child.gameObject);
						if (hope != null) {
								return hope;
						}
				}
				return null;
		}

		public void Save ()
		{

		}

}

class GOReference
{
		public Vector3 position;
		public Quaternion rotation;
		public float score = 0.5f;
}

class PlayerData
{

}