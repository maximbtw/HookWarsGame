using System;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;
    private void Awake() { instance = this; }

    public static TeamManager TeamManager;
    public static Player Player;
    public static Canvas Canvas;

    public SoundAudioClip[] SoundAudioClipArray;
    public PurchaseItem[] PurchaseItemArray;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    [Serializable]
    public class PurchaseItem
    {
        public ItemManager.ItemID ItemID;
        public Item Item;
        [Range(0, 99999)]
        public int Price;
    }
}
