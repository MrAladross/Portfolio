using UnityEngine;
using UnityEngine.SceneManagement;

public class AllStories : MonoBehaviour
{
    public string[] stories;
    public GameObject[] buttons;
    void Start()
    {
        AllTheStories.stories = stories;
        AllTheStories.storyIndex = 0;
        for (int i=0;i<stories.Length;++i)
        {
            buttons[i].SetActive(true);
        }
    }
    public void LoadStory(int index)
    {
        AllTheStories.storyIndex = index;
        SceneManager.LoadScene(1);
    }
}
public static class AllTheStories
{
    public static string[] stories;
    public static int storyIndex;
}
