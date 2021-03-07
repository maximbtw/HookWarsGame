using UnityEngine;
using Photon.Pun;

public class GridMove : MoveObject
{
    public override void Throw(Vector3 touchPos)
    {
        base.Throw(touchPos);
        length = Mathf.Min(length, (transform.position - touchPos).magnitude);
        direction = (touchPos - startPosition).normalized;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (State.Fly == state)
        {
            objectRigidBody2D.MoveRotation(objectRigidBody2D.rotation + 115 *Time.fixedDeltaTime);
        }
    }

    protected override void UpdateFly()
    {
        base.UpdateFly();
        if ((transform.position - startPosition).magnitude >= length)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
