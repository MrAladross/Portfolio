using UnityEngine;
using UnityEngine.UI;
public class ParseStoryAndChoiceNumbers : MonoBehaviour
{

    public string fullStoryWithFormatting;
    public string[] parsedParagraphs;
    public int[] choiceNumbers;

    public void ParseContents()
    {
        if (AllTheStories.stories != null)
            fullStoryWithFormatting = AllTheStories.stories[AllTheStories.storyIndex];
        parsedParagraphs = fullStoryWithFormatting.Split('~');
        choiceNumbers = new int[parsedParagraphs.Length * 2];
        for(int i=0;i<parsedParagraphs.Length;i++)
        {
            int a = parsedParagraphs[i].Length;
            choiceNumbers[2*i] = int.Parse(parsedParagraphs[i].Substring(a - 4,2));
            choiceNumbers[2*i+1] = int.Parse(parsedParagraphs[i].Substring(a - 2,2));
            parsedParagraphs[i] = parsedParagraphs[i].Substring(0, parsedParagraphs[i].Length - 4);
            parsedParagraphs[i] = parsedParagraphs[i]+"_" + "\r\n" + "\r\n" + "\r\n" + "\r\n" + "\r\n" + "\r\n" + "\r\n";
        }


    }
}
