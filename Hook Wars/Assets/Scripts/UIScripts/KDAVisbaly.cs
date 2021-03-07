using UnityEngine;
using UnityEngine.UI;

public class KDAVisbaly : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = GameAssets.Player.PlayerStat.KDAStat.ToString();
    }
}
