#pragma strict

function Start () {
}

function Update () {

    var hitCast : RaycastHit;

    if (Physics.Raycast(transform.position, Vector3.down, hitCast))
    {
        Debug.DrawRay(hitCast.point,hitCast.normal * 100,Color.blue);

        var dir = Vector3.up;

        transform.rotation = Quaternion.FromToRotation(dir, hitCast.normal);
    }

}
