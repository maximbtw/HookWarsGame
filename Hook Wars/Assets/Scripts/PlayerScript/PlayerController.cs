using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Controll
    {
        Bot,
        User,
        TakeControll
    }

    public Controll Controller;
    public Joystick Joystick;

    private Player player;
    private PhotonView photonView;
    private PlayerStat playerStat;
    private Rigidbody2D playerRigidBody2D;

    private float speed => playerStat.Speed;
    private Vector3 direction;
    private float disabledTime;

    void Start()
    {
        player = GetComponent<Player>();
        photonView = GetComponent<PhotonView>();
        playerStat = GetComponent<PlayerStat>();
        playerRigidBody2D = GetComponent<Rigidbody2D>();
        disabledTime = 0; 
    }

    private void FixedUpdate()
    {
        if (disabledTime > 0)
        {
            disabledTime -= Time.fixedDeltaTime;
            return;
        }
        playerRigidBody2D.MovePosition(transform.position + speed * direction * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        switch (Controller)
        {
            case Controll.User:
                UserControll();
                break;
            case Controll.Bot:
                BotConroll();
                break;
        }
    }

    private float timer = 0;

    private void BotConroll()
    {
        if (direction == null)
            direction = new Vector3(1, 0, 0).normalized;
        timer += Time.deltaTime;
        if (timer > 3)
        {
            timer = 0;
            direction = new Vector3((direction.x) > 0 ? -1 : 1, 0, 0).normalized;
        }
        transform.eulerAngles = new Vector3(0, 0, MathAsset.GetAngleFromDir(direction) + 180);
    }

    private void UserControll() 
    {
        direction = Joystick.Direction;
        if (player.Side == Side.Dire) direction *= -1;
        direction.Normalize();
        if(direction.x != 0 || direction.y != 0)
            transform.eulerAngles = new Vector3(0, 0, MathAsset.GetAngleFromDir(direction) + 180);
    }

    public void Immobilize(float disabledTime)
    {
        this.disabledTime += disabledTime;
    }
}
