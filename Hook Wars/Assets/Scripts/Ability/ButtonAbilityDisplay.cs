using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ButtonAbilityDisplay : MonoBehaviour
{
    private Image Image;
    private Material currentMaterial
    {
        get { return Image.material; }
        set { Image.material = value; }
    }

    private Dictionary<AbilityState, PropertyMaterial> colors;
    private Material material;
    private Ability ability;
    private Text textTimer;
    private AbilityState stateAbility => ability.AbilityState;
    private AbilityState prevStateAbility;

    void Start()
    {
        Image = GetComponent<Image>();
        ability = GetComponent<Ability>();
        CreateMaterial();
        textTimer = GetComponentInChildren<Text>();
        textTimer.gameObject.SetActive(false);
        CreateColors();
        SwichColors();
    }

    private void CreateMaterial()
    {
        material = new Material(GamePrefabs.instance.ShaderAnimationOutline);
        material.SetFloat("_OutlineThickness", 1);
        GetComponent<Image>().material = material;
    }

    private void CreateColors()
    {
        var blackColor = new Color(0.07058824f, 0.07058824f, 0.08235294f, 1);
        var whiteColor = new Color(0.9372549f, 0.9411765f, 0.9372549f, 1);
        var redColorY = new Color(1, 0, 0, 1);
        var redColorT = new Color(1, 0, 0, 0);
        var greenColor = new Color(0, 1, 0, 1);

        colors = new Dictionary<AbilityState, PropertyMaterial>()
        {
            {AbilityState.Selected, new PropertyMaterial(greenColor,blackColor) },
            {AbilityState.NotActive, new PropertyMaterial(whiteColor, blackColor)},
            {AbilityState.Cooldown, new PropertyMaterial(redColorY, blackColor) },
            {AbilityState.CantChoose, new PropertyMaterial(blackColor, redColorT) },
        };
    }

    void Update()
    {
        if (currentMaterial != material)
        {
            currentMaterial = material;
        }

        if (prevStateAbility != stateAbility)
        {
            if (prevStateAbility == AbilityState.Cooldown)
                textTimer.gameObject.SetActive(false);
            else if (stateAbility == AbilityState.Cooldown)
                textTimer.gameObject.SetActive(true);

            SwichColors();
        }
        if(stateAbility == AbilityState.Cooldown)
        {
            textTimer.text = Math.Round(ability.Timer, 1).ToString();
        }
    }

    private void SwichColors()
    {
        prevStateAbility = stateAbility;
        colors[prevStateAbility].SetColors(material);
    }

    //public void SwichOnText()
    //{
    //    textTimer.gameObject.SetActive(true);
    //}

    struct PropertyMaterial
    {
        public readonly string nameProperty1;
        public readonly string nameProperty2;
        public readonly Color Color1;
        public readonly Color Color2;

        public PropertyMaterial(Color color1, Color color2)
        {
            nameProperty1 = "_OutlineColor";
            nameProperty2 = "_OutlineColor2";
            Color1 = color1;
            Color2 = color2;
        }

        public void SetColors(Material material)
        {
            material.SetColor(nameProperty1, Color1);
            material.SetColor(nameProperty2, Color2);
        }
    }
}
