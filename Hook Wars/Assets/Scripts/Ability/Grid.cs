using UnityEngine;
using Photon.Pun;

public class Grid : Ability
{
    public float TimeDuration;
    [Range(1f, 50)]
    public float Speed;
    [Range(25, 175)]
    public float Length;

    public GameObject GridPrefab;
    private GameObject gridPrefab;

    protected override void StartAbility()
    {
        if (!AbilityStartCheck()) return;
        // -- Select Grid

        SetSelected();
        Speed *= GameAssets.Player.PlayerStat.PrecentAbilitySpeed;
        Length *= GameAssets.Player.PlayerStat.PrecentAbilityLength;
    }

    protected override void SelectedAbility()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Throw(Input.mousePosition);
            SetCooldown();
        }

        //--Mobile

        //if (Input.touchCount == 0) return;

        //var touch = (Input.touchCount == 1) ? Input.GetTouch(0) : Input.GetTouch(1);
        //if (touch.phase == TouchPhase.Began)
        //{
        //    Throw(touch.position);
        //    SetCooldown();
        //}
    }

    private void Throw(Vector2 position)
    {
        // -- Throw grid
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(position);

        gridPrefab = PhotonNetwork.Instantiate(GridPrefab.name, 
            GameAssets.Player.transform.position, Quaternion.identity);
        var gridMove = gridPrefab.GetComponent<GridMove>();
        gridMove.SetStartProperty(TimeDuration, Speed, Length);
        gridMove.Throw(touchPos);
    }
}
