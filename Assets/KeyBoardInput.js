#pragma strict

var maxLean = 10;

var isColliding = false;

var terrain : TerrainData;
var count = 0;

function Start () {
	count = 0;
	/*var hitCast : RaycastHit;

    if (Physics.Raycast(transform.position, Vector3.down, hitCast))
    {
        Debug.DrawRay(hitCast.point,hitCast.normal * 100,Color.blue);

        var dir = Vector3.up;

        //transform.rotation = Quaternion.FromToRotation(dir, hitCast.normal);
        slopeAngle = transform.rotation;	
    }

    var ter : Terrain = GameObject.Find("Terrain").GetComponent(Terrain);
    terrain = ter.terrainData;
    
    // Normalizing the coordinates
    var normX = transform.position.x * 1.0 / (terrain.size.x - 1);
    var normY = transform.position.y * 1.0 / (terrain.size.y - 1);
    Debug.Log(normX);
    Debug.Log(normY);
    slopeAngleSteep = terrain.GetSteepness(normX, normY);
    //Debug.Log("Angle: " + slopeAngle);
    Debug.Log("AngleSteep: " + slopeAngleSteep);*/
}

function Update () {
    if(isColliding) {

    	// Debug.Log("AngleSteep: " + slopeAngleSteep);
        var direction = getVectorInput();
		// Debug.Log(lean);
		
		var rotate = getRotate(gameObject.transform);
		
		if(rotate != null && count%20) {
			var tiltAroundX = (direction.z/15) * maxLean * -1;
			var target = Quaternion.Euler (tiltAroundX, 0, 0);
			
			//var defaultRotate =  gameObject.transform.rotation;
			//defaultRotate.x = rotate.transform.rotation.x;
			//rotate.transform.localRotation  = defaultRotate;

			// Dampen towards the target rotation
			rotate.transform.localRotation  = Quaternion.Slerp(rotate.transform.localRotation , target,
		                               Time.deltaTime * 2.0);
		}
		count++;

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
        	
        	// As leaning occurs, small rotation of board occurs, also rotating velocity of player
        		//rotate around y axis a fraction of lean z and velocity x
        	rigidbody.AddTorque(new Vector3(0, (direction.z) / 50, 0));
        
        	rigidbody.velocity = transform.TransformDirection(newVelocity);
        
        
        }
        
        
        
        
        
        
        
        
        
        
        //Debug.Log("Velocity X: " + rigidbody.velocity.x);
        //Debug.Log("forward: "+ forward +" and lean: " + lean);

        //rigidbody.AddForceAtPosition(forward * lean/10000000*-1, new Vector3(rigidbody.velocity.x, 0, 0));
        //var velocity = transform.InverseTransformDirection( rigidbody.velocity );//Vector3.RotateTowards(rigidbody.velocity, new Vector3(transform.rotation.x,transform.rotation.y ,transform.rotation.z), 3.0, 0.0);

        //if (Mathf.Abs(lean) < buffer) {
             // Change velocity from z to x by 50%
             //rigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x + velocity.z * 0.5, 0, velocity.z * 0.5));
        //} else {
             // Change velocity from z to x by 10%
             //rigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x + velocity.z * 0.1, 0, velocity.z * 0.9));
             // Debug.Log("test");
        //}
        
        //rigidbody.AddForce(transform.TransformDirection(new Vector3(-1000, 0, 0)));
        //var eulerAngleVelocity : Vector3 = Vector3 (0, 100, 0);
        
        //if (lean >= 0.5) {
        	// var deltaRotation : Quaternion = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        	// rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
        //} else if (lean <= -0.5) {
        	// rigidbody.AddForce(transform.TransformDirection(Vector3.left * 10));
        //}
    }		
}
function getRotate(transform) {
	var t : Transform = transform;
	for(var child : Transform in t) {
		if(child.name == "Rotate") {
			return child.gameObject;
		}
	}
}

function OnCollisionEnter (col : Collision)
{
    isColliding = true;
}

function OnCollisionExit (col : Collision)
{
    isColliding = false;
}

function getVectorInput() {
	var steps = 10.0;
	var x : float = Input.GetAxis("Mouse X") * steps;
	var z : float = Input.GetAxis("Mouse Y") * steps;
    var directionVector = new Vector3(x, 0, z);
	//Debug.Log("Lean X: " + directionVector.x + ", Lean Z: " + directionVector.z);
    return directionVector;
}
