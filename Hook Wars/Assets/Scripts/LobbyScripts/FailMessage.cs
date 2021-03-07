using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class FailMessage : MonoBehaviour
{
    [SerializeField] private Text failLog = null;
    [SerializeField] private double timeOfShowingFail = 4;

    private bool isEnabled = false;
    private double startTime;

    void Start() => SwichLog();

    void Update()
    {
        if (isEnabled) return;

        if(PhotonNetwork.Time - startTime > timeOfShowingFail)
        {
            failLog.text = string.Empty;
            SwichLog();
        }
    }

    public void Log(string failMessage)
    {
        SwichLog();
        failLog.text = failMessage;
        SetStartTime();
    }

    private void SwichLog()
    {
        isEnabled = !isEnabled;
        failLog.gameObject.SetActive(!isEnabled);
    }

    private void SetStartTime() => startTime = PhotonNetwork.Time;
}
