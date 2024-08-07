using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public  struct OperandsAnswer
    { 
       public int A, B, answer;
    }

public class MathPopup : MonoBehaviour
{
    public CharacterInputController controller;
    [SerializeField] private CanvasGroup questionHolder;
    [SerializeField] private Text[] operands;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Text timerText;
    [SerializeField] private CanvasGroup resultHolder;
    [SerializeField] private GameObject[] results;
    private int correctAnswer = 0;
    private OperandsAnswer GenerateAddOp()
    {
        int A = UnityEngine.Random.Range(10, 50);
        int B = UnityEngine.Random.Range(5, 30);
        OperandsAnswer answer1 = new OperandsAnswer();
        answer1.A = A;
        answer1.B = B;
        answer1.answer = A + B;
        return answer1;
    }

    private OperandsAnswer GenerateSubOp()
    {
        int A = UnityEngine.Random.Range(30, 60);
        int B = UnityEngine.Random.Range(2, 25);
        int answer = A - B;
        if(answer <= 0)
        {
            B -= answer;
        }
        answer = A - B;
        OperandsAnswer answer1 = new OperandsAnswer();
        answer1.A = A;
        answer1.B = B;
        answer1.answer = answer;
        return answer1;
    }

    private OperandsAnswer GenerateMultOp()
    {
        int A = UnityEngine.Random.Range(1, 9);
        int B = UnityEngine.Random.Range(1, 9);
        OperandsAnswer answer1 = new OperandsAnswer();
        answer1.A = A;
        answer1.B = B;
        answer1.answer = A * B;
        return answer1;
    }
    private OperandsAnswer GenerateDivOp()
    {
        int answer = UnityEngine.Random.Range(1, 9);
        int divider = UnityEngine.Random.Range(1, 9);
        int mult = answer * divider;
        OperandsAnswer answer1 = new OperandsAnswer();
        answer1.A = mult;
        answer1.B = divider;
        answer1.answer = answer;
        return answer1;
    }

    private void GenerateQuestion () {

        OperandsAnswer[] questions = new OperandsAnswer[3]; 
        float rand = Random.Range(0.0f, 1.0f);
        if(rand < 0.25)
        {
            questions = generateQuestions(0);
                operands[2].text = "+";
        }
        else if(rand < 0.5)
        {
            questions = generateQuestions(1);
                operands[2].text = "-";
        }
        else if(rand < 0.75)
        {
            questions = generateQuestions(2);
                operands[2].text = "*";
        }
        else
        {
            questions = generateQuestions(3);
            operands[2].text = "/";
        }
        correctAnswer = Random.Range(0, 2);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.GetComponentInChildren<Text>().text = questions[i].answer.ToString();
        }
        operands[0].text = questions[correctAnswer].A.ToString();
        operands[1].text = questions[correctAnswer].B.ToString();
    }


    OperandsAnswer[] generateQuestions(int op)
    {
        List<OperandsAnswer> operands = new List<OperandsAnswer>();
        List<int> answers = new List<int>();
        while(operands.Count < 3)
        {
            OperandsAnswer check = new OperandsAnswer();
            switch(op) {
                case 0:
                    check = GenerateAddOp(); break;
                case 1:
                    check = GenerateSubOp(); break;
                case 2:
                    check = GenerateMultOp(); break;
                case 3:
                    check = GenerateDivOp(); break;
            }
            if (!answers.Contains(check.answer)) {
                answers.Add(check.answer);
                operands.Add(check);
            }
        }

        return operands.ToArray();
    }

    public void GiveAnswer(int answer)
    {
        StopAllCoroutines();
        float waitTime = 0.125f;
        StartCoroutine(alphaSmoothTime(questionHolder, 0.0f, waitTime));
        if (answer == correctAnswer) StartCoroutine(showResult(0));
        else StartCoroutine(showResult(1));
    }

    private void Finish(bool win) {
        if (!win) { 
            controller.currentLife--;
            controller.bossFightController.Reset();
                }
        controller.trackManager.StartMove(!win); 
        AudioListener.pause = false;
        Time.timeScale = 1;
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        resultHolder.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);

        controller.trackManager.StopMove();
        AudioListener.pause = true;
        Time.timeScale = 0;
        GenerateQuestion();
        StartCoroutine(startQuestion());

        /*
        OperandsAnswer[] questions = new OperandsAnswer[3];
        for (int i = 0; i < questions.Length; i++)
        {
            questions[i] = GenerateSubOp();
            Debug.Log(i + "A " + questions[i].A);
            Debug.Log(i + "B " + questions[i].B);
            Debug.Log("----__----" + i + "res " + questions[i].answer);

        }
         */
    }
    IEnumerator alphaSmoothTime(CanvasGroup cg, float to, float time) {
        AudioListener.pause = true;
        Time.timeScale = 0;
        float aTimer = time;
        float startAlpha = cg.alpha;
        while (aTimer > 0) {
            cg.alpha = Mathf.Lerp(to, startAlpha,  aTimer/time);
            aTimer -= Time.unscaledDeltaTime;
            yield return null;
        }
        if(aTimer <= 0) {
            cg.alpha = to;
            yield break;
        }
    }

    IEnumerator startQuestion()
    {
        float waitTime = 0.125f;
        questionHolder.alpha = 0;
        StartCoroutine(alphaSmoothTime(questionHolder,1.0f, waitTime));
        yield return new WaitForSecondsRealtime(waitTime);

        float qTimer = 7;
        while (qTimer > 0)
        {
            if(qTimer > 2) this.timerText.color = Color.white;
            else this.timerText.color = Color.red;
            string timerText = string.Format("{0:00}", qTimer);
            this.timerText.text = timerText;
            qTimer -= Time.unscaledDeltaTime;
            yield return null;
        }
        if(qTimer <= 0)
        {
            StartCoroutine(alphaSmoothTime(questionHolder, 0.0f, waitTime));
            StartCoroutine(showResult(2));
            yield break;
        }

    }

    IEnumerator showResult(int result) {
        resultHolder.gameObject.SetActive(true);
        foreach (var res in results)
        {
            res.SetActive(false);
        }
        switch (result)
        {
            case 0:
                results[0].SetActive(true); break;
            case 1:
                results[1].SetActive(true); break;
            case 2:
                results[2].SetActive(true); break;
        }
        float resTimerStart = 0.125f;
        resultHolder.alpha = 0;
        StartCoroutine(  alphaSmoothTime(resultHolder,1.0f, resTimerStart));
        yield return new WaitForSecondsRealtime(resTimerStart + 1.0f);
        StartCoroutine(  alphaSmoothTime(resultHolder,0.0f, resTimerStart));
        yield return new WaitForSecondsRealtime(resTimerStart);
        switch (result)
        {
            case 0:
                Finish(true); break;
            case 1:
                Finish(false); break;
            case 2:
                Finish(false); break;
        }

    }
}
