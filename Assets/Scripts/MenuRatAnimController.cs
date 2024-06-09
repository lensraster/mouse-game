using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRatAnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    int poses;

    private void Awake()
    {
        poses = 0;
        Shader.SetGlobalColor("_GColor", Color.white);
       StartCoroutine(animatorCoroutine(poses));
    }
    public void SetIdleTimer()
    {
        poses++;
        if (poses > 2) { poses = 0; }
       StartCoroutine(animatorCoroutine(poses));
    }

    private IEnumerator animatorCoroutine(int pose)
    {
        yield return new WaitForSeconds(0.5f);
        string name = pose.ToString();
        animator.SetTrigger(name);
    }
}
