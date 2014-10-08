using UnityEngine;
using System.Collections;

public class LeftTrigger : MonoBehaviour
{
		private Boundaries bounds;

		// Use this for initialization
		void Start ()
		{
		bounds =  transform.parent.gameObject.GetComponent<Boundaries> ();
		}

		void OnTriggerEnter (Collider other)
		{
				if (other.gameObject.name == "GamePlayer") {
						Debug.Log ("HIT LEFT PLANE");
						//hide red line and show right line
						bounds.hideLeft ();
						bounds.showRight ();
				}
		}

		// Update is called once per frame
		void Update ()
		{

		}
}
