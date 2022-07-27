using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : UnityEvent<string>
{

}

public class WeaponAnimationEvents : MonoBehaviour
{
    //---------
    private void Start()
    {
        Invoke("ChangeMode", .01f);
    }
    void ChangeMode()
    {
        Animator anim = GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.Normal;
    }
    //--------- */

    public AnimationEvent WeaponAnimationEvent = new AnimationEvent();

    public void OnAnimationEvent(string eventName)
    {
        WeaponAnimationEvent.Invoke(eventName);
    }
}
