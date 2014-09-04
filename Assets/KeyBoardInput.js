#pragma strict


var maxLean = 10;
var friction = 0.2;
var Rsc = 8; // Meters

// var slopeAngle : float ;
var slopeAngleSteep : float ;
var worldPos : Vector3;

var slopeAngle = Quaternion.Euler(Vector3(0, 0, 0));

var buffer = 0.01;

var isColliding = false;

var terrain : TerrainData;

function Start () {
	var hitCast : RaycastHit;

    if (Physics.Raycast(transform.position, Vector3.down, hitCast))
    {
        Debug.DrawRay(hitCast.point,hitCast.normal * 100,Color.blue);

        var dir = Vector3.up;

        transform.rotation = Quaternion.FromToRotation(dir, hitCast.normal);
        slopeAngle = transform.rotation;
    }

    var ter : Terrain = GameObject.Find("Terrain").GetComponent(Terrain);
    terrain = ter.terrainData;
    slopeAngleSteep = terrain.GetSteepness(transform.position.x, transform.position.z);
    Debug.Log("Angle: " + slopeAngle);
    //Debug.Log("AngleSteep: " + slopeAngleSteep);
}

function Update () {
    if(isColliding) {
    	Debug.Log("Angle: " + slopeAngle);
        var direction = getVectorInput();
        var lean : float = maxLean * direction.z;

        var deltaRotation : Quaternion = Quaternion.Euler(Vector3(0, lean, 0));
        transform.rotation = slopeAngle * deltaRotation;
        var forward = transform.forward;
        //Debug.Log("Velocity X: " + rigidbody.velocity.x);
        //Debug.Log("forward: "+ forward +" and lean: " + lean);

        rigidbody.AddForceAtPosition(forward * lean/10000000*-1, new Vector3(rigidbody.velocity.x, 0, 0));
        var velocity = transform.InverseTransformDirection( rigidbody.velocity );//Vector3.RotateTowards(rigidbody.velocity, new Vector3(transform.rotation.x,transform.rotation.y ,transform.rotation.z), 3.0, 0.0);
        
         if (Mathf.Abs(lean) < buffer) {
             // Change velocity from z to x by 50%
             rigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x + velocity.z * 0.5, 0, velocity.z * 0.5));
         } else {
             // Change velocity from z to x by 10%
             rigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x + velocity.z * 0.1, 0, velocity.z * 0.9));
             Debug.Log("test");
        }
        
        rigidbody.AddForce(transform.TransformDirection(new Vector3(-1000, 0, 0)));
        //var eulerAngleVelocity : Vector3 = Vector3 (0, 100, 0);
        
        if (lean >= 0.5) {
        	//var deltaRotation : Quaternion = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        	// rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
        } else if (lean <= -0.5) {
        	// rigidbody.AddForce(transform.TransformDirection(Vector3.left * 10));
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
    var left_toe = Input.GetKey(KeyCode.S);
    var left_heel = Input.GetKey(KeyCode.D);

    var right_toe = Input.GetKey(KeyCode.W);
    var right_heel = Input.GetKey(KeyCode.E);

    var x = 0.0;
    var z = 0.0;

    if (left_toe) {

        x -= 0.5;
        z -= 0.5;
        //Debug.Log("left_toe: x-" + x +", z-" + z);
    }

    if (left_heel) {

        x -= 0.5;
        z += 0.5;
        //Debug.Log("left_heel: x-" + x +", z-" + z);
    }

    if (right_toe) {

        x += 0.5;
        z -= 0.5;
        //Debug.Log("right_toe: x-" + x +", z-" + z);
    }

    if (right_heel) {

        x += 0.5;
        z += 0.5;
        //Debug.Log("right_heel: x-" + x +", z-" + z);
    }

    //Debug.Log("X: " + x + ", Y: " + y);

    var directionVector = new Vector3(x, 0, z);

    //Debug.Log("Initial Vector: "+ directionVector.ToString());

    // Used to make max magnitude = 1
    // May use this later
    // if (directionVector != Vector3.zero) {
    //     // Get the length of the directon vector and then normalize it
    //     // Dividing by the length is cheaper than normalizing when we already have the length anyway
    //     var directionLength = directionVector.magnitude;
    //     directionVector = directionVector / directionLength;
    //
    //     // Make sure the length is no bigger than 1
    //     directionLength = Mathf.Min(1, directionLength);
    //
    //     // Make the input vector more sensitive towards the extremes and less sensitive in the middle
    //     // This makes it easier to control slow speeds when using analog sticks
    //     directionLength = directionLength * directionLength;
    //
    //     // Multiply the normalized direction vector by the modified length
    //     directionVector = directionVector * directionLength;
    // }

    //Debug.Log("Returned Vector: "+ directionVector.ToString());

    return directionVector;
}
