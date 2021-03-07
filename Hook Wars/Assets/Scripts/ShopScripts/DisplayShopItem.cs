using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayShopItem : MonoBehaviour
{
    private Image Image;
    private Material currentMaterial 
    { 
        get { return Image.material; }
        set { Image.material = value; } 
    }

    private Material material;
    private ShopItem shopItem;

    private State currentState;
    private State previousState;


    private Dictionary<State, Color> colors = new Dictionary<State, Color>()
    {
        { State.CantChoose,  new Color(1, 0, 0, 1) },
        { State.NotSelected, new Color(0.9372549f, 0.9411765f, 0.9372549f, 1) },
        { State.Selected,    new Color(0, 1, 0, 1) },
    };

    void Start()
    {
        Image = GetComponent<Image>();
        material = new Material(GamePrefabs.instance.ShaderStaticOutline);
        material.SetFloat("_OutlineThickness", 1);
        GetComponent<Image>().material = material;
        shopItem = GetComponent<ShopItem>();
        previousState = State.None;
    }

    void Update()
    {
        if (currentMaterial != material)
        {
            currentMaterial = material;
        }

        currentState = 
            ! shopItem.EnoughMoney 
            ? State.CantChoose
            : shopItem.Selected
            ? State.Selected
            : State.NotSelected;
        if(currentState != previousState)
        {
            SwichColor();
        }
    }

    private void SwichColor()
    {
        previousState = currentState;
        material.SetColor("_OutlineColor", colors[currentState]);
    }

    enum State
    {
        None,
        CantChoose,
        NotSelected,
        Selected,
    }
}
