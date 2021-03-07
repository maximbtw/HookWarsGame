using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckBox : MonoBehaviour, IPointerClickHandler
{
    [NonSerialized] public bool isSelected = false;

    private GameObject checkMark = null;

    public event Action SwichState;

    void Start()
    {
        checkMark = transform.Find("Mark").gameObject;
        checkMark.SetActive(isSelected);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        TurnCheckBox();
    }

    private void TurnCheckBox()
    {
        isSelected = !isSelected;
        checkMark.SetActive(isSelected);


        SwichState?.Invoke();
    }
}
