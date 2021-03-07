using UnityEngine;

public class DireRotate : MonoBehaviour
{
    void Start()
    {
        WaitingRoomServer.LoadGameEvent += Reverse;
    }

    private void Reverse()
    {
        if (GameAssets.Player.Side == Side.Dire)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
    }
}
