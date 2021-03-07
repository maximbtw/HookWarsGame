using System;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Range(0, 99999)]
    public int Gold = 0;
    [Range(0,300)]
    public float Speed = 38;
    public KDA KDAStat;
    public int Streak = 0;

    public float PrecentAbilitySpeed = 1;
    public float PrecentAbilityLength = 1;
    public float PrecentAbilityCooldown = 1;

    private void Start()
    {
        KDAStat = new KDA();
    }

    [Serializable]
    public class KDA
    {
        public int Kill = 0;
        public int Death = 0;
        public int Assist = 0;

        public override string ToString()
        {
            return $"{Kill}/{Death}/{Assist}";
        }
    }
}
