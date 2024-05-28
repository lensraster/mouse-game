using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BossFightController : MonoBehaviour
{
    Transform child;
    Animator animator;
    int quantity;
    void Start()
    {
        child = transform.GetChild(0);
        quantity = 0;
        animator = GetComponent<Animator>();
        Reset();
    }


    public void Reset()
    {
        Shader.SetGlobalColor("_GColor", Color.white);
    }
    private void OnEnable()
    {
        StartCoroutine(BossFightTimer(60f));
    }

    IEnumerator BossFight()
    {
        Color redCol = new Color(1f, 0.6f, 0.6f, 1f);
        StartCoroutine(setColor(redCol, 2.5f));
        yield return new WaitForSeconds(2.5f);
        child.gameObject.SetActive(true);
        float bossFightTime = 10f + (float)quantity;
        yield return new WaitForSeconds(bossFightTime);
        quantity++;
        StartCoroutine(setColor(Color.white, 1.5f));
        child.gameObject.SetActive(false);
        StartCoroutine(BossFightTimer(40f));
    }

    IEnumerator BossFightTimer(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(BossFight());
    }

    IEnumerator setColor(Color toColor, float time)
    {
        Color color = Shader.GetGlobalColor("_GColor");
        float timer = time;
        while (timer > 0f)
        {
            Shader.SetGlobalColor("_GColor", Color.Lerp(toColor, color, timer / time));
            timer -= Time.deltaTime;
            yield return null;
        }
        if (timer <= 0f)
        {
            Shader.SetGlobalColor("_GColor", toColor);
            yield break;
        }
    }
}
