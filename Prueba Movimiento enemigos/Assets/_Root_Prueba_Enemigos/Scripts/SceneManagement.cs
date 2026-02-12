using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadRandomMap()
    {
        int randomScene = Random.Range(1, 3);
        Time.timeScale = 1;
        SceneManager.LoadScene(randomScene);
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
        Time.timeScale = 1;
    }
}
