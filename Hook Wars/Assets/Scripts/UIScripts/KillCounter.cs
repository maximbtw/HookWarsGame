using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (GameAssets.TeamManager.isTeamCreated)
        {
            text.text = GameAssets.TeamManager.GetTeam(GameAssets.Player).CountKill.ToString();
        }
    }
}
