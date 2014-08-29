﻿#pragma strict


var maxLean = 10;
var friction = 0.2;
var Rsc = 8; // Meters

var slopeAngle : float ;

var buffer = 0.01;

var isColliding = false;

var terrain : TerrainData;

function Start () {
    var ter : Terrain = GameObject.Find("Terrain").GetComponent(Terrain);
    terrain = ter.terrainData;
    slopeAngle = terrain.GetSteepness(transform.position.x, transform.position.z);
    Debug.Log("Angle: " + slopeAngle);
}

function Update () {
    if(isColliding) {
        Debug.Log("IsColliding");
        // Get Angle of slope
        slopeAngle = terrain.GetSteepness(transform.position.x, transform.position.z);

        // Get input from keyboard or wiiBoard
        var direction = getVectorInput();

        // Find relative lean angle from -10 -> 10
        var lean : float = maxLean * direction.z;

        // Rotation performed of x axis to make snowboarder appear to lean toe or heel
        // Needs fixing, needs x and y values
        var deltaRotation : Quaternion = Quaternion.Euler(Vector3(lean*slopeAngle, 1, 1));

        // Applies lean to board
        // puts interms of slopeAngle to prevent falling over
        transform.rotation =  deltaRotation;

        // the direction of a (0, 0, 1) in terms of the global direction
        var forward = transform.forward;
        Debug.Log("Velocity X: " + rigidbody.velocity.x);
        Debug.Log("forward: "+ forward +" and lean: " + lean);


        // Applies for in direction forward at strength of maxLean
        // Applies more torque around center of gravity dependent on speed
        //rigidbody.AddForceAtPosition(forward * lean/10000000*-1, new Vector3(rigidbody.velocity.x, 0, 0));

        // Get velocity in terms of local rotation
        //var velocity = transform.InverseTransformDirection( rigidbody.velocity );//Vector3.RotateTowards(rigidbody.velocity, new Vector3(transform.rotation.x,transform.rotation.y ,transform.rotation.z), 3.0, 0.0);

        // Straighten up velocity system to align with character

        // If Leaning
        // if (Mathf.Abs(lean) < buffer) {
        //     // Change velocity from z to x by 50%
        //     rigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x + velocity.z * 0.5, 0, velocity.z * 0.5));
        // } else {
        //     // Change velocity from z to x by 10%
        //     rigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x + velocity.z * 0.1, 0, velocity.z * 0.9));
        // }
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
    var left_toe = Input.GetKey(KeyCode.W);
    var left_heel = Input.GetKey(KeyCode.S);

    var right_toe = Input.GetKey(KeyCode.E);
    var right_heel = Input.GetKey(KeyCode.D);

    var x = 0.0;
    var z = 0.0;

    if (left_toe) {

        x -= 0.5;
        z += 0.5;
        //Debug.Log("left_toe: x-" + x +", y-" + y);
    }

    if (left_heel) {

        x -= 0.5;
        z -= 0.5;
        //Debug.Log("left_heel: x-" + x +", y-" + y);
    }

    if (right_toe) {

        x += 0.5;
        z += 0.5;
        //Debug.Log("right_toe: x-" + x +", y-" + y);
    }

    if (right_heel) {

        x += 0.5;
        z -= 0.5;
        //Debug.Log("right_heel: x-" + x +", y-" + y);
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
