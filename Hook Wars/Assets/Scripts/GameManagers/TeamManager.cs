using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeamManager : MonoBehaviour
{
    public List<Player> Players { get; private set; }
    public bool isTeamCreated { get; private set; }
    public Team TeamRadiant { get; private set; }
    public Team TeamDire { get; private set; }

    void Start()=> isTeamCreated = false;

    public Team GetTeam(Player player) => (player.Side == Side.Radiant) ? TeamRadiant : TeamDire;

    public void AddPlayers() => Players = GamePrefabs
        .instance.PlayersPlace
        .GetComponentsInChildren<Player>()
        .ToList();

    public void SetMainPlayer(string userID) => GameAssets.Player = Players
        .FirstOrDefault(x => x.PhotonView.Owner.UserId == userID);

    public Player GetPlayer(int id) => Players.FirstOrDefault(p => p.PhotonView.OwnerActorNr == id);

    public Vector3 GetPosition(Player player)
    {
        var team = GetTeam(player);

        Vector3 position = Vector3.zero;
        do
        {
            float y = (player.Side == Side.Radiant) ? 0 : 85;
            float x = UnityEngine.Random.Range(-100, 100);

            position = new Vector3(x, y);
        }
        while (team.Players
                   .Select(p => p.transform.position)
                   .Any(p => (p - position).magnitude < 7.5f));

        return position;
    }

    public void SetSidesToPlayers(int[] sides)
    {
        int index = 0;
        foreach(var player in Players.OrderBy(x => x.PhotonView.OwnerActorNr))
        {
            player.Side = (Side)sides[index];
            index++;
        }
    }

    public void CreateTeam()
    {
        TeamRadiant = new Team(Side.Radiant, RoomSettings.CountPlayer / 2);
        TeamDire = new Team(Side.Dire, RoomSettings.CountPlayer / 2);

        foreach (var player in Players)
        {
            if (player.Side == Side.Radiant)
            {
                TeamRadiant.AddPlayer(player);
            }
            else
            {
                TeamDire.AddPlayer(player);
            }
        }

        isTeamCreated = true;
    }

    public class Team 
    {
        public readonly int MaxPlayers;
        public Side Side { get; private set; }
        public List<Player> Players { get; private set; }
        public int CountKill { get; private set; }

        public void AddPlayer(Player player) => Players.Add(player);

        public Team(Side side, int maxPlayers)
        {
            Side = side;
            CountKill = 0;
            this.MaxPlayers = maxPlayers;
            Players = new List<Player>(maxPlayers);
        }

        public void AddKill()
        {
            CountKill++;
        }

        public override string ToString()
        {
            return $"Team {Side}\n " +
                   $"Count playrs: {Players.Count}\n" +
                   $"Count kills: {CountKill}";
        }
    }
}
