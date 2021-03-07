using UnityEngine;

public class GipsMove : MoveObject
{
    private Vector3 targetPosition => target.transform.position;

    public override void Throw(GameObject target)
    {
        base.Throw(target);
    }

    protected override void UpdateFly()
    {
        direction = (targetPosition - transform.position).normalized;
        base.UpdateFly();
    }
}
