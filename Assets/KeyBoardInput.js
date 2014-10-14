#pragma strict

var maxLean = 10;
var maxAngle = 45;

var isColliding = false;

var terrain : TerrainData;
var count = 0;

function Start () {
	count = 0;
}

function Update () {
    if(isColliding) {

    	// Debug.Log("AngleSteep: " + slopeAngleSteep);
        var direction = getVectorInput();
		// Debug.Log(lean);
		
		var rotate = getRotate(gameObject.transform);
		var target = null;
		
		if(rotate != null && count%20) {
			var tiltAroundX = (direction.z/15) * maxLean;
			target = Quaternion.Euler (tiltAroundX, 0, 0);
			
			//var defaultRotate =  gameObject.transform.rotation;
			//defaultRotate.x = rotate.transform.rotation.x;
			//rotate.transform.localRotation  = defaultRotate;

			// Dampen towards the target rotation
			rotate.transform.localRotation  = Quaternion.Slerp(rotate.transform.localRotation , target,
		                               Time.deltaTime * 2.0);
		}
		count++;
		
		var eulerRotation = transform.rotation.eulerAngles;
		
		if( eulerRotation.x > maxAngle && eulerRotation.x < 360 - maxAngle && count%20) {
			//transform.rotation.eulerAngles.x = maxAngle;
			target = Quaternion.Euler (maxAngle, transform.rotation.y, transform.rotation.z);
			transform.rotation  = Quaternion.Slerp(transform.rotation , target,
		                               Time.deltaTime * 2.0);
			Debug.Log("Large X Angle");
		}
		
		if( eulerRotation.z > maxAngle && eulerRotation.z < 360 - maxAngle  && count%20) {
			//transform.rotation.eulerAngles.z = maxAngle;
			target = Quaternion.Euler (transform.rotation.x, transform.rotation.y, maxAngle);
			transform.rotation  = Quaternion.Slerp(transform.rotation , target,
		                               Time.deltaTime * 2.0);
			Debug.Log("Large Z Angle");
		}

        //transform.rotation = slopeAngle * deltaRotation;
        var forward = transform.forward;
        //var oldVelocity = transform.InverseTransformDirection( rigidbody.velocity );
        
        
        // Find x and z velocity of snoboader (dont care about up and down) (x through the length of snowboard, z through width
        
        var oldVelocity = transform.InverseTransformDirection( rigidbody.velocity );
        //Debug.Log("Velocity X: " + oldVelocity.x + ", Velocity Z: " + oldVelocity.z);
        
        
        var newVelocity = new Vector3();
        
        // Leaning *should* be in opposite direction to velocity z
        
        var count = 0;
        if (count%2 == 0) {	
        	//Debug.Log("Z Velocity: " + oldVelocity.z);
        }
        
        if(direction.z * oldVelocity.z < 0 || oldVelocity.z < 5) {
        
        
        	// Leaning increases friction in z movement and only reduces that TODO(Skidding??)
        	
        		// Reduce by a propotion of lean
        	newVelocity.z = oldVelocity.z / Mathf.Abs(20/direction.z);
        	
        	newVelocity.x = oldVelocity.x;
        	
        	var turn = 1;
        	if (newVelocity.x > 0) {
        		turn = -1;
        	}
        	
        	// As leaning occurs, small rotation of board occurs, also rotating velocity of player
        		//rotate around y axis a fraction of lean z and velocity x
        	rigidbody.AddTorque(new Vector3(0, (direction.z) / 50 * turn, 0));
        
        	rigidbody.velocity = transform.TransformDirection(newVelocity);
        
        
        }
        
    }		
}
function getRotate(transform) {
	var t : Transform = transform;
	for(var child : Transform in t) {
		if(child.name == "Rotate") {
			return child.gameObject;
		}
	}
	return null;
}

function OnCollisionEnter (col : Collision)
{
	if (col.collider.gameObject.name.Equals("Terrain")) {
    	isColliding = true;
    }
}

function OnCollisionExit (col : Collision)
{
	if (col.collider.gameObject.name.Equals("Terrain")) {
    	isColliding = false;
    }
}

function getVectorInput() {
	var steps = 10.0;
	var x : float = Input.GetAxis("Mouse X") * steps;
	var z : float = Input.GetAxis("Mouse Y") * steps;
    var directionVector = new Vector3(x, 0, z);
	//Debug.Log("Lean X: " + directionVector.x + ", Lean Z: " + directionVector.z);
    return directionVector;
}
