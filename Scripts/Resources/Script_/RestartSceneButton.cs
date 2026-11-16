using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSceneButton : MonoBehaviour
{

    public void RestartCurrentScene()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void RestartToMainScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
