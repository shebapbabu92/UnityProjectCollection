
using UnityEngine;



public class DelayAnimationStart : MonoBehaviour
{
    public Animation anim;
    public float delay = 10f;

    void Start()
    {
        Invoke("PlayAnimation", delay);
    }

    void PlayAnimation()
    {
        anim.Play();
    }
}
