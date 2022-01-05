using UnityEngine;
using UnityEngine.SceneManagement;
public class ResetLevel : MonoBehaviour
{

    bool areYouSure = false;
    public void ResetScene()
    {
        if (!areYouSure)
        {
            areYouSure = true;
            Invoke("MakeFalse", 1);
        }
        else
            SceneManager.LoadScene(0);
    }
    void MakeFalse()
    {
        areYouSure = false;
    }
}
