using UnityEngine;

public class ChainMove : MonoBehaviour
{
    private float       speed;
    private Vector3     direction;
    private GameObject  target;
    private Rigidbody2D chainRigidbody2D;

    private void Awake() => chainRigidbody2D = gameObject.GetComponent<Rigidbody2D>();

    public void SetSpeed(float speed) => this.speed = speed;

    public void SwichTarget(GameObject newTarget) => target = newTarget;

    private void FixedUpdate()
    {
        chainRigidbody2D.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        direction = (target.transform.position - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, MathAsset.GetAngleFromDir(direction));
    }
}
