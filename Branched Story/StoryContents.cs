using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryContents : MonoBehaviour
{

     string[] storyParagraphs;
    public int choice1Value;
    public int choice2Value;
    public Text button1Text;
    public Text button2Text;
    bool toMain = false;
     int[] choiceValues;
    TextResizer tr;
    ParseStoryAndChoiceNumbers pStory;
    private void Start()
    {
        tr = GetComponent<TextResizer>();
        pStory = GetComponent<ParseStoryAndChoiceNumbers>();
        pStory.ParseContents();
        storyParagraphs = pStory.parsedParagraphs;
        choiceValues = pStory.choiceNumbers;

        choice1Value = choiceValues[0];
        choice2Value = choiceValues[1];
        

        string[] set = storyParagraphs[0].Split('_');
        tr.SetText(set[0]+set[3]);

        button1Text.text = set[1];
        button2Text.text = set[2];
    }

    public void ChooseChoice1()
    {
        toMain = false;
        string[] set = storyParagraphs[choice1Value - 1].Split('_');
        tr.SetText(set[0]+set[3]);
        int a = choice1Value * 2 - 2;
        choice1Value = choiceValues[a];
        choice2Value = choiceValues[a+1];


        button1Text.text = set[1];
        button2Text.text = set[2];
        if (set[1].Contains("Start"))
        {
            button1Text.text = set[1];
            button2Text.text = "Back to main menu";
            toMain = true;
        }

        tr.ClearYDistance();
    }
    public void ChooseChoice2()
    {
        if (toMain)
        {
            toMain = false;
            SceneManager.LoadScene(0);
        }
        string[] set = storyParagraphs[choice2Value - 1].Split('_');
        tr.SetText(set[0]+set[3]);
        int a = choice2Value * 2 - 2;
        choice1Value = choiceValues[a];
        choice2Value = choiceValues[a+1];


        button1Text.text =set[1];
        button2Text.text =set[2];
        if (set[1].Contains("Start"))
        {
            button1Text.text = set[1];
            button2Text.text = "Back to main menu";
            toMain = true;
        }
        tr.ClearYDistance();
    }


}
