using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class Shop : MonoBehaviour, IPointerClickHandler
{
    public GameObject ShopMenu;
    private Animator animator;

    public Side Side;
    [SerializeField] private Sprite myTeamSprite = null;
    [SerializeField] private Sprite anotherTeamSprite = null;

    private bool playerTriggired;
    private bool shopOpened;

    public static event Action Open;
    public static event Action Close;

    private void Start()
    {
        animator = ShopMenu.GetComponent<Animator>();
        playerTriggired = false;
        shopOpened = false;
        ShopCloseClick.Start += CloseShop;
        WaitingRoomServer.LoadGameEvent += SetSprite;
    }

    private void SetSprite()
    {
        if (Side == GameAssets.Player.Side) 
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = myTeamSprite;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = anotherTeamSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerTriggired = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerTriggired = false;
            CloseShop();
        }
    }

    public void OpenShop()
    {
        if (!shopOpened && playerTriggired)
        {
            animator.Play("ShopMenuOpen");
            Open?.Invoke();
            shopOpened = true;
        }
    }

    public void CloseShop()
    {
        if (shopOpened)
        {
            animator.Play("ShopMenuClose");
            Close?.Invoke();
            shopOpened = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (shopOpened) CloseShop();
        else OpenShop();
    }
}
