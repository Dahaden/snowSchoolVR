using UnityEngine;
using System.Collections;

public class Boundaries : MonoBehaviour
{

		private GameObject boundLeft;
		private GameObject boundRight;

		void Awake ()
		{
				DontDestroyOnLoad (gameObject);
		}

		// Use this for initialization
		void Start ()
		{
		
				boundLeft = findGameObject ("BoundaryLineLeft", gameObject);
				boundRight = findGameObject ("BoundaryLineRight", gameObject);
		
				boundRight.SetActive(false);
				//Debug.Log("Boundary: " + boundaryLeft);

		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void hideLeft ()
		{
				boundLeft.SetActive (false);
		}

		public void showLeft ()
		{
				boundLeft.SetActive (true);
		}

		public void hideRight ()
		{
				boundRight.SetActive (false);
		}

		public void showRight ()
		{
				boundRight.SetActive (true);
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
}
