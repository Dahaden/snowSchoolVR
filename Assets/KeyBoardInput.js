#pragma strict

private var motor : CharacterMotor;

public var lastDir : Vector3;

function Start () {
    motor = GetComponent(CharacterMotor);
}

function Update () {
    var direction = getVectorInput();

    Debug.Log("Direction: "+ transform.rotation.ToString());
    Debug.Log("Final Vector: "+ direction.ToString());

    motor.inputMoveDirection = transform.rotation * direction;

    motor.inputJump = Input.GetButton("Jump");

    lastDir = direction;

}

function getVectorInput() {
    var left_toe = Input.GetKey(KeyCode.W);
    var left_heel = Input.GetKey(KeyCode.S);

    var right_toe = Input.GetKey(KeyCode.E);
    var right_heel = Input.GetKey(KeyCode.D);

    var x = 0;
    var y = 0;

    if (left_toe) {
        //Debug.Log("left_toe");
        x -= 0.5;
        y += 0.5;
    }

    if (left_heel) {
        //Debug.Log("left_heel");
        x -= 0.5;
        y -= 0.5;
    }

    if (right_toe) {
        //Debug.Log("right_toe");
        x += 0.5;
        y += 0.5;
    }

    if (right_heel) {
        //Debug.Log("right_heel");
        x += 0.5;
        y -= 0.5;
    }

    //Debug.Log("X: " + x + ", Y: " + y);

    var directionVector = new Vector3(x, 0, y);

    //Debug.Log("Initial Vector: "+ directionVector.ToString());

    if (directionVector != Vector3.zero) {
        // Get the length of the directon vector and then normalize it
        // Dividing by the length is cheaper than normalizing when we already have the length anyway
        var directionLength = directionVector.magnitude;
        directionVector = directionVector / directionLength;

        // Make sure the length is no bigger than 1
        directionLength = Mathf.Min(1, directionLength);

        // Make the input vector more sensitive towards the extremes and less sensitive in the middle
        // This makes it easier to control slow speeds when using analog sticks
        directionLength = directionLength * directionLength;

        // Multiply the normalized direction vector by the modified length
        directionVector = directionVector * directionLength;
    }

    //Debug.Log("Returned Vector: "+ directionVector.ToString());

    return directionVector;
}

@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/Snow Input Controller")
