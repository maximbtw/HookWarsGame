using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class PlayerList : MonoBehaviour
{
    [SerializeField] private RectTransform rectContent = null;
    [SerializeField] private GameObject contentItem    = null;
    [SerializeField] private Text countPlayerLog       = null;

    public static Dictionary<PlayerProfile, Model> Players { get; private set; }
    private Dictionary<PlayerProfile, Model> players;

    public Side GetSidePlayer(Photon.Realtime.Player player)
    => players.FirstOrDefault(x => x.Key.Player.UserId == player.UserId).Key.Side;
    public int[] GetSides()
        => players.OrderBy(x => x.Key.Player.ActorNumber).Select(x => (int)x.Key.Side).ToArray();

    private void Awake()
    {
        players = new Dictionary<PlayerProfile, Model>(RoomSettings.CountPlayer);
        Players = players;
    }

    private void Update()
    {
        if (countPlayerLog == null) return;
        countPlayerLog.text =
                PhotonNetwork.CurrentRoom?.Players.Count + " / " +
                RoomSettings.CountPlayer;
    }

    public void AddPlayer(Photon.Realtime.Player player)
    {
        var model = new Model(contentItem, rectContent);
        var profile = new PlayerProfile(player);
        model.OnReceivedModel(profile);
        players.Add(profile, model);
    }

    public void RemovePlayer(Photon.Realtime.Player player)
    {
        var item = players.FirstOrDefault(x => x.Key.Player.UserId == player.UserId);
        Destroy(item.Value.Content);
        players.Remove(item.Key);
    }

    public void UpdatePlayers()
    {
        foreach(var player in players)
        {
            player.Value.OnReceivedModel(player.Key);
        }
    }

    public void SetSides(int[] sides)
    {
        int index = 0;
        foreach(var player in players)
        {
            player.Key.Side = (Side)sides[index];
            player.Value.OnReceivedModel(player.Key);
            index++;
        }
    }

    public int[] GetShufleSide()
    {
        var sides = new int[PhotonNetwork.CurrentRoom.PlayerCount];
        var d = sides.Length / 2;
        for (int i = 0; i < sides.Length; i++)
            sides[i] = (int)((i < d) ? Side.Dire : Side.Radiant);
        MathAsset.Shufle(sides);
        return sides;
    }

    public class PlayerProfile
    {
        public Side Side;
        public Photon.Realtime.Player Player;

        public PlayerProfile(Photon.Realtime.Player player)
        {
            Side = Side.None;
            Player = player;
            Debug.LogWarning(player.NickName);
        }
    }

    public class Model
    {
        public GameObject Content { get; private set; }

        private Text contentText;
        private Image contentImage;

        public Model(GameObject contentPrefab, RectTransform parent)
        {
            Content = Instantiate(contentPrefab);
            Content.transform.SetParent(parent, false);
            contentText = Content.GetComponentInChildren<Text>();
            contentImage = Content.GetComponentInChildren<Image>();
        }

        public void OnReceivedModel(PlayerProfile profile)
        {
            contentText.text = profile.Player.NickName;

            Color color;
            switch (profile.Side)
            {
                case Side.Dire:
                    color = new Color(0.5f, 0.17f, 0.17f);
                    break;
                case Side.Radiant:
                    color = new Color(0.4f, 0.60f, 0.50f);
                    break;
                default:
                    color = new Color(1, 1, 1);
                    break;
            }
            contentImage.color = color;
        }
    }
}
