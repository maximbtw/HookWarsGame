using Photon.Pun;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    protected State state;
    private ThrowState throwState;

    protected Vector3 direction;
    protected float speed;
    protected float duration;
    protected float length;
    protected GameObject target;
    protected Vector3 startPosition;
    protected Rigidbody2D objectRigidBody2D;

    private Vector3 targetPosition;
    private float timer;

    private void Start() => objectRigidBody2D = gameObject.GetComponent<Rigidbody2D>();


    public virtual void SetStartProperty(float duration, float speed, float length = -1)
    {
        this.duration = duration;
        this.speed = speed;
        this.length = length;
    }

    public virtual void Throw(GameObject target)
    {
        this.target = target;
        throwState = ThrowState.Directed;
        StartThrow();
    }

    public virtual void Throw(Vector3 touchPos)
    {
        throwState = ThrowState.Free;
        StartThrow();
    }

    private void StartThrow()
    {
        startPosition = transform.position = GameAssets.Player.transform.position;
        state = State.Fly;
        timer = 0;
    }

    protected virtual void FixedUpdate()
    {
        objectRigidBody2D.MovePosition(transform.position + speed * direction * Time.fixedDeltaTime);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Fly:
                UpdateFly();
                break;
            case State.Stay:
                UpdateStay();
                break;
        }
    }

    protected virtual void UpdateFly()
    {
        //transform.position = transform.position + direction * speed * Time.fixedDeltaTime;
    }

    protected virtual void UpdateStay()
    {
        if ((targetPosition - transform.position).magnitude < 10)
        {
            speed = 0;
            transform.position = targetPosition;
        }
        else
        {
            direction = (targetPosition - transform.position).normalized;
            //transform.position = transform.position + direction * speed * Time.fixedDeltaTime;
        }

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Stay) return;
        var player = collision.GetComponent<Player>();

        switch (throwState)
        {
            case ThrowState.Directed:
                if (player != null && player == target)
                    EventCollision(player);
                break;
            case ThrowState.Free:
                if (player != null && player != GameAssets.Player)
                    EventCollision(player);
                break;
        }
    }

    protected void EventCollision(Player player)
    {
        GameEventManager.EventInvoke("Immobilize", player.PlayerId, duration);
        targetPosition = player.transform.position;
        speed *= 5;
        state = State.Stay;
    }

    protected enum State
    {
        None,
        Fly,
        Stay
    }

    enum ThrowState
    {
        Directed,
        Free
    }
}
