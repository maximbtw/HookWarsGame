using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public LayerMask NeedLayer;
    void Update()
    {
        if (Input.touchCount == 0) return;

        Vector2 touchPos;
        var touch = (Input.touchCount == 1) ? Input.GetTouch(0) : Input.GetTouch(1);
        if (touch.phase == TouchPhase.Began)
        {
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else return;


        //RaycastHit2D rayHit = Physics2D.Raycast(touchPos, Vector2.zero);
        //if (rayHit.transform != null)
        //    Debug.Log("Selected object: " + rayHit.transform.name);

        //RaycastHit2D[] allHits = Physics2D.RaycastAll(touchPos, Vector2.zero);

        //for (int i = 0; i < allHits.Length; i++)
        //{
        //    Debug.Log(allHits[i].transform.name);
        //}

        RaycastHit2D maskHit = Physics2D.Raycast(touchPos, Vector2.zero, 10f, NeedLayer);
        if (maskHit.transform != null)
        {
            TargetPlayer.SetTarget(maskHit.transform.gameObject.GetComponent<Player>());
            //Debug.Log("Layer object: " + maskHit.transform.name);
        }

    }
}
