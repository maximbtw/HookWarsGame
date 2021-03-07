using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InformationDebug : MonoBehaviour
{
    private static InformationDebug i;
    public Text DebagLog;

    private void Awake() => i = this;


    float timer1 = 0;
    double timer2 = 0;
    private void Update()
    {
        timer1 += Time.deltaTime;
        timer2 += PhotonNetwork.Time;
        DebagLog.text = timer1 + "\n" + PhotonNetwork.Time;
    }

    public static void AddLog(string text)
    {
        i.DebagLog.text += "\n" + text;
    }
}
