using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BossFightController : MonoBehaviour
{
    [SerializeField] TrackManager trackManager;
    [SerializeField] CharacterInputController charController;
    [SerializeField] Text scoreText;
    [SerializeField] int firstScore = 20;
    [SerializeField] float firstSpeed = 20;
    [SerializeField] int scoreStep = 10;
    [SerializeField] float speedStep = 15;
    [SerializeField] GameObject boss;
    [SerializeField] ParticleSystem poof;

    int scoreNeeded;
    float speedNeeded;
    bool bossFightStarted;

    void Start()
    {
        scoreText.gameObject.SetActive(false);
        ResetStates();
    }


    public void ResetStates()
    {
        Shader.SetGlobalColor("_GColor",Color.white);    
        Shader.SetGlobalFloat("_Inverted",0f);
        scoreNeeded = firstScore;
        speedNeeded = firstSpeed;
        bossFightStarted = false;
        boss.SetActive(false);
    }

    IEnumerator StartBossFight()
    {
        StartCoroutine(setColor(true, 1.2f));
        yield return new WaitForSeconds(1.2f);
        boss.SetActive(true);
        scoreText.gameObject.SetActive(true);
        poof.Play();
        int initScore = charController.coins;
        StartCoroutine(BossFight(initScore));
        yield break;
    }


    IEnumerator BossFight(int initScore) {

      //  Debug.Log("StartBossFight. score: " + initScore);
        while((charController.coins - initScore) < scoreNeeded)
        {
      //      Debug.Log("while shoto" + (charController.coins - initScore).ToString());
            int score = charController.coins - initScore;
            scoreText.text = score + " / " + scoreNeeded;
            poof.transform.position = boss.transform.position;
            yield return null;
        }
        if((charController.coins - initScore) >= scoreNeeded)
        {
        //    Debug.Log("finish coroutine");
            StartCoroutine(FinishBossFight());
            yield break;
        }
    }

    IEnumerator FinishBossFight() {
        scoreText.gameObject.SetActive(false);
        StartCoroutine(setColor(false, 1.2f));
        yield return new WaitForSeconds(1.2f);
        boss.SetActive(false);
        poof.Play();
        scoreNeeded += scoreStep;
        speedNeeded += speedStep;
        trackManager.ContinueAcceleration();
        bossFightStarted = false;
        yield break;
    }

    IEnumerator setColor(bool toRed, float time)
    {
        Color toColor = toRed ? new Color(1f, 0.6f, 0.6f, 1f) : Color.white;
        float toInverted = toRed ? 1f : 0f;
        float inverted = Shader.GetGlobalFloat("_Inverted");
        Color color = Shader.GetGlobalColor("_GColor");
        float timer = time;
        while (timer > 0f) {
            Shader.SetGlobalColor("_GColor", Color.Lerp(toColor, color,  timer / time));
            Shader.SetGlobalFloat("_Inverted", Mathf.Lerp(toInverted,inverted, timer / time ));
            timer -= Time.deltaTime;
            yield return null;
        }
        if(timer <= 0f) {
            Shader.SetGlobalColor("_GColor", toColor);
            Shader.SetGlobalFloat("_Inverted", toInverted);
            yield break;
        }
    }

    private void Update()
    {
        float speed = trackManager.speed;
        if (speed > speedNeeded && !bossFightStarted)
        {
            bossFightStarted = true;
            trackManager.StopAcceleration();
            StartCoroutine(StartBossFight());
        }
    }
}
