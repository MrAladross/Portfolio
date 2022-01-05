using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginnerMathQuestions : MonoBehaviour
{
    public Text questionText;
    public InputField answerField;
    public Text answerText;
    public Text responseText;
    public Text buttonText;
    public Image backgroundSprite;
    public Color backgroundColor;

    private TouchScreenKeyboard numpad = null;

    public string rewardText;

    public int random1;
    public int random2;
    public int answer;

    public Transform visualsLayoutGroup1;
    public Transform visualsLayoutGroup2;
    public Color32[] colorChoices;


    public GameObject visualPlaceholder;

    public int score;

    public bool nextQuestion;
    public bool addingOrSubtracting;//false = adding, true = subtracting
    public bool visualNumerical; //false = numerical, true = visual

    // Start is called before the first frame update
    void Start()
    {
        EnableNumpad();
        cd = 0.5f;
        score = 7;
        if (visualNumerical)
        {
            RemoveVisualItemsChildren();
            if (!addingOrSubtracting)
            {
                GenerateVisualAddingQuestion();
            }
            else
            {
                GenerateVisualSubtractingQuestion();
            }
        }
        else
        {
            if (!addingOrSubtracting)
            {
                GenerateAddingQuestion();
            }
            else
            {
                GenerateSubtractingQuestion();
            }
        }

    }

    public void EnableNumpad()
    {
        answerField.keyboardType = TouchScreenKeyboardType.NumberPad;
    }
    public void RedoRewardText()
    {
        int rando = UnityEngine.Random.Range(0, 5);
        if (rando == 0)
        {
            rewardText = "Great Job!";
        }
        else if (rando == 1)
        {
            rewardText = "Fantastic!";
        }
        else if (rando == 2)
        {
            rewardText = "You Rock!";
        }
        else if (rando == 3)
        {
            rewardText = "You are smart!";
        }
        else if (rando == 4)
        {
            rewardText = "Awesome Work!";
        }
    }
    public void GenerateVisualAddingQuestion()
    {
        if (score < 10)
        {
            random1 = UnityEngine.Random.Range(0, score);
            random2 = UnityEngine.Random.Range(0, score);
        }
        else if (score < 50)
        {
            random1 = UnityEngine.Random.Range(0, 15);
            random2 = UnityEngine.Random.Range(7, 15);
        }
        else
        {
            random1 = UnityEngine.Random.Range(7, 20);
            random2 = UnityEngine.Random.Range(8, 20);
        }

        questionText.text = random1 + " + " + random2 + " = ";
        answer = random2 + random1;
        buttonText.text = "Check Answer";
        nextQuestion = false;
        responseText.text = "";
        answerField.text = "";

        int[] nums = GetRandomNumbers(0, colorChoices.Length);
        for (int i=0;i<random1;++i)
        {
            GameObject nextVisual = (GameObject)Instantiate(visualPlaceholder, visualsLayoutGroup1);
            nextVisual.GetComponent<Image>().color = colorChoices[nums[0]];
        }
        for (int i = 0; i < random2; ++i)
        {
            GameObject nextVisual = (GameObject)Instantiate(visualPlaceholder, visualsLayoutGroup2);
            nextVisual.GetComponent<Image>().color = colorChoices[nums[1]];
        }

    }
    public void GenerateVisualSubtractingQuestion()
    {
        if (score < 10)
        {
            random1 = UnityEngine.Random.Range(score/2, score+1);
            random2 = UnityEngine.Random.Range(0, score/2);
        }
        else if (score < 50)
        {
            random1 = UnityEngine.Random.Range(8, 15);
            random2 = UnityEngine.Random.Range(2, 8);
        }
        else
        {
            random1 = UnityEngine.Random.Range(12, 20);
            random2 = UnityEngine.Random.Range(3, 12);
        }

        questionText.text = random1 + " - " + random2 + " = ";
        answer = random1 - random2;
        buttonText.text = "Check Answer";
        nextQuestion = false;
        responseText.text = "";
        answerField.text = "";

        int[] nums = GetRandomNumbers(0, colorChoices.Length);
        for (int i = 0; i < random1; ++i)
        {
            GameObject nextVisual = (GameObject)Instantiate(visualPlaceholder, visualsLayoutGroup1);
            nextVisual.GetComponent<Image>().color = colorChoices[nums[0]];
        }
        for (int i = 0; i < random2; ++i)
        {
            GameObject nextVisual = (GameObject)Instantiate(visualPlaceholder, visualsLayoutGroup2);
            nextVisual.GetComponent<Image>().color = colorChoices[nums[1]];
        }
    }
    public int[] GetRandomNumbers(int min, int max)
    {
        int[] nums = new int[max-min+1];

        for (int i=0;i<max-min+1;++i)
        {
            nums[i] = i+min;
        }

        for (int i = 0; i < max - min+1; ++i)
        {
            int a = UnityEngine.Random.Range(min, max);
            int b = UnityEngine.Random.Range(min, max);


            int c = nums[a];
            nums[a] = nums[b];
            nums[b] = c;
        }

        return nums;
    }
    public void GenerateAddingQuestion()
    {
        if (score < 25)
        {
            random1 = UnityEngine.Random.Range(0, score);
            random2 = UnityEngine.Random.Range(0, score);
        }
        else if (score < 105)
        {
            random1 = UnityEngine.Random.Range(10, score);
            random2 = UnityEngine.Random.Range(10, score);
        }
        else
        {
            random1 = UnityEngine.Random.Range(score/2, score);
            random2 = UnityEngine.Random.Range(score/2, score);
        }

        questionText.text = random1 + " + " + random2 + " = ";
            answer = random2 + random1;
            buttonText.text = "Check Answer";
            nextQuestion = false;
        responseText.text = "";
        answerField.text = "";
        
    }
    public void ChooseLightGreen()
    {
        backgroundColor = new Color32(237, 255, 208, 255);
    }
    public void ChooseLightPink()
    {
        backgroundColor = new Color32(234, 200, 220, 255);
    }
    public void SetColor()
    {
        backgroundSprite.color = backgroundColor;
    }
    public void GenerateSubtractingQuestion()
    {
        if (score < 25)
        {
            random1 = UnityEngine.Random.Range(score, score*2);
            random2 = UnityEngine.Random.Range(0, score);
        }
        else if (score < 105)
        {
            random1 = UnityEngine.Random.Range(score, score*2);
            random2 = UnityEngine.Random.Range(10, score);
        }
        else
        {
            random1 = UnityEngine.Random.Range(score / 2, score*2);
            random2 = UnityEngine.Random.Range(10, score/2);
        }
        questionText.text = random1 + " - " + random2 + " = ";
        answer = random1 - random2;
        buttonText.text = "Check Answer";
        nextQuestion = false;
        responseText.text = "";
        answerField.text = "";
    }
    public void EnableAdd()
    {
        addingOrSubtracting = false;
        score = 7;
        if (visualNumerical)
        {
            RemoveVisualItemsChildren();
            if (!addingOrSubtracting)
            {
                GenerateVisualAddingQuestion();
            }
            else
            {
                GenerateVisualSubtractingQuestion();
            }
        }
        else
        {
            if (!addingOrSubtracting)
            {
                GenerateAddingQuestion();
            }
            else
            {
                GenerateSubtractingQuestion();
            }
        }
    }
    public void EnableSubtract()
    {

        addingOrSubtracting = true;
        score = 5;
        if (visualNumerical)
        {
            RemoveVisualItemsChildren();
            if (!addingOrSubtracting)
            {
                GenerateVisualAddingQuestion();
            }
            else
            {
                GenerateVisualSubtractingQuestion();
            }
        }
        else
        {
            if (!addingOrSubtracting)
            {
                GenerateAddingQuestion();
            }
            else
            {
                GenerateSubtractingQuestion();
            }
        }
    }
    public void ChangeDifficulty(int index)
    {
        if (index==0)
        {
            if (addingOrSubtracting)
            {
                score = 5;
            }
            else
            {
                score = 7;
            }
        }
        else if (index ==1)
        {
            score = 25; 
        }
        else { score = 105; }
        if (visualNumerical)
        {
            RemoveVisualItemsChildren();
            if (!addingOrSubtracting)
            {
                GenerateVisualAddingQuestion();
            }
            else
            {
                GenerateVisualSubtractingQuestion();
            }
        }
        else
        {
            if (!addingOrSubtracting)
            {
                GenerateAddingQuestion();
            }
            else
            {
                GenerateSubtractingQuestion();
            }
        }
    }
    public void VisualsEnableDisable()
    {
        visualNumerical = !visualNumerical;

        if (!visualNumerical)
        {
            RemoveVisualItemsChildren();
        }

        if (visualNumerical)
        {
            if (!addingOrSubtracting)
            {
                GenerateVisualAddingQuestion();
            }
            else
            {
                GenerateVisualSubtractingQuestion();
            }
        }
        else
        {
            if (!addingOrSubtracting)
            {
                GenerateAddingQuestion();
            }
            else
            {
                GenerateSubtractingQuestion();
            }
        }

    }
    public void RemoveVisualItemsChildren()
    {
        if (visualsLayoutGroup1.childCount > 0)
        {
            for (int i = visualsLayoutGroup1.childCount - 1; i >= 0; --i)
            {
                GameObject vlgchild = visualsLayoutGroup1.GetChild(i).gameObject;
                vlgchild.transform.parent = null;
                Destroy(vlgchild);
            }
        }
        if (visualsLayoutGroup2.childCount > 0)
        {
            for (int i = visualsLayoutGroup2.childCount - 1; i >= 0; --i)
            {
                GameObject vlgchild = visualsLayoutGroup2.GetChild(i).gameObject;
                vlgchild.transform.parent = null;
                Destroy(vlgchild);
            }
        }
    }
    public void EvaluateAnswer()
    {
        if (nextQuestion)
        {
            if (visualNumerical)
            {
                RemoveVisualItemsChildren();
                if (!addingOrSubtracting)
                {
                    GenerateVisualAddingQuestion();
                }
                else
                {
                    GenerateVisualSubtractingQuestion();
                }
            }
            else
            {
                if (!addingOrSubtracting)
                {
                    GenerateAddingQuestion();
                }
                else
                {
                    GenerateSubtractingQuestion();
                }
            }
        }
        else
        {
            int currentAnswer = -1;
            if (int.TryParse(answerText.text, out currentAnswer))
            {
                if (currentAnswer == answer)
                {
                    RedoRewardText();
                    responseText.text = rewardText;
                    nextQuestion = true;
                    buttonText.text = "Next Question";
                    score += answer / 5;
                }
                else
                {
                    responseText.text = "Try Again";
                }
            }
            else
            {
                responseText.text = "You must type a number";
            }
        }
    }

    private float timer;
    private float cd;
    private bool growingShrinking;
    // Update is called once per frame
    void Update()
    {
        if (nextQuestion)
        {
            timer += Time.deltaTime;
            if (timer>cd)
            {
                timer = 0;
                growingShrinking = !growingShrinking;
            }
            if (growingShrinking)
            {
                buttonText.transform.parent.localScale *= 1.003f;
            }
            else
            {
                buttonText.transform.parent.localScale *= 0.997f;
            }
        }
        else
        {
            buttonText.transform.parent.localScale = new Vector3(1,1,1);
        }
    }
}
