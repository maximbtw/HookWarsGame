using System;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public event Action AnimationComplite;
    public event Action AnimationStart;

    public void Close() => AnimationComplite?.Invoke();
    public void Open() => AnimationStart?.Invoke(); 

}
