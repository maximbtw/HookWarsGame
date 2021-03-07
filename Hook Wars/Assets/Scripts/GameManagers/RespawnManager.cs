using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private static Queue<Respawn> respawns;

    public static void AddRespawn(Player player, float time)=> respawns.Enqueue(new Respawn(player, time));

    void Start() => respawns = new Queue<Respawn>();

    void Update()
    {
        if (respawns.Count == 0) return;

        foreach(var respawn in respawns)
        {
            respawn.UpdateTime();
        }
        if (respawns.Peek().Timer <= 0)
        {
            var player = respawns.Dequeue().Player;
            player.Spawn();
        }
    }

    class Respawn
    {
        public Player Player { get; private set; }
        public float Timer { get; private set; }

        public Respawn(Player player, float time)
        {
            Player = player;
            Timer = time;
        }

        public void UpdateTime() => Timer -= Time.deltaTime;
    }
}
