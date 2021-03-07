using UnityEngine;
using UnityEngine.UI;

public class PlayerGold : MonoBehaviour
{
    private Text gold;

    void Start()
    {
        gold = GetComponent<Text>();
    }

    void Update()
    {
        if (GameAssets.TeamManager.isTeamCreated)
        {
            gold.text = GameAssets.Player.PlayerStat.Gold.ToString();
        }
    }
}
