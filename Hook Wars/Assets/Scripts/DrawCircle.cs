using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public Sprite CircleSprite;
    public float Radius;

    public Color Color;

    private GameObject circle;
    private SpriteRenderer render;

    void Start()
    {
        circle = new GameObject("Radius");
        circle.transform.SetParent(this.gameObject.transform);
        render = circle.AddComponent<SpriteRenderer>();
        render.sprite = CircleSprite;
        render.sortingLayerName = "Player Items";
        Color = new Color(0.75f, 0.95f, 0.75f, 0.2f);
    }

    void Update()
    {
        if (circle.transform.localScale.x != Radius)
        {
            circle.transform.localScale = new Vector3(Radius, Radius);
        }
        if (render.color != Color)
        {
            render.color = Color;
        }
        circle.transform.position = transform.position;
    }
}
