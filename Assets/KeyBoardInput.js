#pragma strict


var maxLean = 10;
var friction = 0.2;
var Rsc = 8; // Meters

var slopeAngle = Quaternion.Euler(Vector3(0, 0, 0));

var buffer = 0.01;

var isColliding = false;

function Start () {
    var hitCast : RaycastHit;

    if (Physics.Raycast(transform.position, Vector3.down, hitCast))
    {
        Debug.DrawRay(hitCast.point,hitCast.normal * 100,Color.blue);

        var dir = Vector3.up;

        transform.rotation = Quaternion.FromToRotation(dir, hitCast.normal);
        slopeAngle = transform.rotation;
    }
}

function Update () {
    if(isColliding) {
        var direction = getVectorInput();
        var lean = maxLean * direction.z;

        var deltaRotation : Quaternion = Quaternion.Euler(Vector3(lean, 0, 0));
        transform.rotation = slopeAngle * deltaRotation;

        var forward = transform.forward;
        Debug.Log("Velocity X: " + rigidbody.velocity.x);
        Debug.Log("forward: "+ forward +" and lean: " + lean);
        rigidbody.AddForceAtPosition(forward * lean/10000000*-1, new Vector3(rigidbody.velocity.x, 0, 0));

        var velocity = transform.InverseTransformDirection( rigidbody.velocity );//Vector3.RotateTowards(rigidbody.velocity, new Vector3(transform.rotation.x,transform.rotation.y ,transform.rotation.z), 3.0, 0.0);

        // Straighten up velocity system to align with character

        if (Mathf.Abs(lean) < buffer) {

            rigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x + velocity.z * 0.5, 0, velocity.z * 0.5));
        } else {

            rigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x + velocity.z * 0.1, 0, velocity.z * 0.9));
        }
    }

}
var counter = 0;
function OnCollisionStay (col : Collision)
{
    if (col.gameObject.name == "Terrain" || col.gameObject.name == "Plane") {
        isColliding = true;
        // Debug.Log("Counter: " + counter++);
        var direction = getVectorInput();
        var lean = maxLean * direction.z;

        if (Mathf.Abs(lean) < buffer ) {
            slopeAngle = rigidbody.rotation;
            Debug.Log("Reset SlopeAngle");
            return;
        }

        // var resistiveForce = rigidbody.mass * Mathf.Cos(lean);
        // var centripetalF = Mathf.Cos(lean) * resistiveForce;
        // var centripetalA = centripetalF * rigidbody.mass * Mathf.Pow(rigidbody.velocity.z, 2);

    } else {
        isColliding = false;
    }
}

function getVectorInput() {
    var left_toe = Input.GetKey(KeyCode.W);
    var left_heel = Input.GetKey(KeyCode.S);

    var right_toe = Input.GetKey(KeyCode.E);
    var right_heel = Input.GetKey(KeyCode.D);

    var x = 0.0;
    var y = 0.0;

    if (left_toe) {

        x -= 0.5;
        y += 0.5;
        //Debug.Log("left_toe: x-" + x +", y-" + y);
    }

    if (left_heel) {

        x -= 0.5;
        y -= 0.5;
        //Debug.Log("left_heel: x-" + x +", y-" + y);
    }

    if (right_toe) {

        x += 0.5;
        y += 0.5;
        //Debug.Log("right_toe: x-" + x +", y-" + y);
    }

    if (right_heel) {

        x += 0.5;
        y -= 0.5;
        //Debug.Log("right_heel: x-" + x +", y-" + y);
    }

    //Debug.Log("X: " + x + ", Y: " + y);

    var directionVector = new Vector3(x, 0, y);

    //Debug.Log("Initial Vector: "+ directionVector.ToString());

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
