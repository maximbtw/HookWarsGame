using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    private Text text;
    private double time;

    void Start()
    {
        text = GetComponent<Text>();
        WaitingRoomServer.LoadGameEvent += SetTime;
    }

    private void SetTime()
    {
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;

        double minutes = Mathf.FloorToInt((float)time / 60);
        double seconds = Mathf.FloorToInt((float)time % 60);

        string minNul = (minutes < 10) ? "0" : "";
        string secNul = (seconds < 10) ? "0" : "";

        text.text = $"{minNul}{minutes}:{secNul}{seconds}";
    }
}
