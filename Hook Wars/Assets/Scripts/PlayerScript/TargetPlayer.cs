using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    public static Player Target { get; private set; }

    public static void RemoveTarget()
    {
        Target = null;
    }

    public static void SetTarget(Player player)
    {
        Target = player;
    }
}
