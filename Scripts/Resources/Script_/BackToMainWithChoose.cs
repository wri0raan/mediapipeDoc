using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainWithChoose : MonoBehaviour
{
    [Header("Main 씬 이름")]
    public string mainSceneName = "Main";

    public void OnClick()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene(mainSceneName, LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == mainSceneName)
        {

            var canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                var t = canvas.transform;

                var main = t.Find("Main");
                var active = t.Find("Active");
                var choose = t.Find("Choose");

                if (main != null) main.gameObject.SetActive(true);
                if (active != null) active.gameObject.SetActive(true);
                if (choose != null) choose.gameObject.SetActive(true);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
