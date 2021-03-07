using UnityEngine;

public class CheckPasswordBox : MonoBehaviour
{
    [SerializeField] private CheckBox   checkBox = null;
    [SerializeField] private GameObject passwordBox = null;

    private Animator passwordBoxAnimator;

    void Start()
    {
        HidePasswordBox();
        checkBox.SwichState += UpdateChek;
        passwordBoxAnimator = passwordBox.GetComponent<Animator>();
        passwordBox.GetComponent<AnimatorController>().AnimationComplite += HidePasswordBox;
    }

    private void UpdateChek()
    {
        if (checkBox.isSelected)
        {
            passwordBox.SetActive(checkBox.isSelected);
            passwordBoxAnimator.Play("OpenInputPasswordBox");
        }
        else
        {
            passwordBoxAnimator.Play("CloseInputPasswordBox");
        }
    }


    private void HidePasswordBox() => passwordBox.SetActive(false);
}
