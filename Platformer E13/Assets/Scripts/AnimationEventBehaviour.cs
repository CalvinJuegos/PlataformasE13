using System;
using UnityEngine;

public class AnimationEventBehaviour : MonoBehaviour
{
    public event Action OnAnimationFinish;

    // This method is called by the Animation Event in the Animator
    public void AnimationFinishEvent()
    {
        OnAnimationFinish?.Invoke();
    }
}