using UnityEngine;
using System.Collections;

public class RightTrigger : MonoBehaviour
{
	private Boundaries bounds;
	
	// Use this for initialization
	void Start ()
	{
		bounds = transform.parent.gameObject.GetComponent<Boundaries> ();
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.name == "GamePlayer") {
			Debug.Log ("HIT Right PLANE");
			//hide red line and show right line
			bounds.hideRight ();
			bounds.showLeft ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
