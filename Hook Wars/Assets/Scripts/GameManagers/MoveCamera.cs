using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public int BoundX = 50;

    private void Start() => WaitingRoomServer.LoadGameEvent += ReverseCamera;

    private void ReverseCamera()
    {
        if(GameAssets.Player.Side == Side.Dire)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
    }

    void Update()
    {
        if (GameAssets.Player == null) return;

        var X = GameAssets.Player.transform.position.x;
        var Y = GameAssets.Player.transform.position.y;

        if (X > BoundX) X = BoundX;
        else if (X < -BoundX) X = -BoundX;

        if (GameAssets.Player.Side == Side.Dire)
        {
            if (Y < 85) Y = 85 - (85 - Y) * 3f;
            else if (Y > 85) Y = 85 - (85 - Y) * -0.2f;
        }
        else
        {
            if (Y > 0) Y *= 3f;
            else if (Y < 0) Y *= -0.2f;
        }
        transform.position = new Vector3(X, Y, transform.position.z);
    }
}
