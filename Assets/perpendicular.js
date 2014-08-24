#pragma strict

private var input : KeyBoardInput;

function Start () {
    input = GetComponent(KeyBoardInput);
}

function Update () {

    var hitCast : RaycastHit;
    var weight = input.lastDir;

    if (Physics.Raycast(transform.position, Vector3.down, hitCast))
    {
        Debug.DrawRay(hitCast.point,hitCast.normal * 100,Color.blue);

        var dir = Vector3.up;

        if (weight.z != 0) {
            dir.z = weight.z * 20;
        }

        transform.rotation = Quaternion.FromToRotation(dir, hitCast.normal);
    }

}


@script RequireComponent (KeyBoardInput)
