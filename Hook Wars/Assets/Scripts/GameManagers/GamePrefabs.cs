using UnityEngine;

public class GamePrefabs : MonoBehaviour
{
    public static GamePrefabs instance;
    private void Awake() { instance = this; }

    public Shader ShaderAnimationOutline;
    public Shader ShaderStaticOutline;
    public Font   TextFont;

    public GameObject PlayerPrefab;
    public GameObject PlayersPlace;
    public Joystick Joystick;
}
