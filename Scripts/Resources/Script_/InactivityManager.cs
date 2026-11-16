using UnityEngine;
using UnityEngine.SceneManagement;

public class InactivityManager : MonoBehaviour
{
    public static InactivityManager Instance;

    [Tooltip("상호작용이 없을 때 메인으로 돌아가는 시간(초)")]
    public float timeoutSeconds = 60f;

    [Tooltip("초기화해서 돌아갈 메인 씬 이름")]
    public string mainSceneName = "Main";

    private float timeSinceLastInteraction = 0f;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

        if (HasUserInput())
        {
            ResetTimer();
        }

        timeSinceLastInteraction += Time.deltaTime;

        if (timeSinceLastInteraction >= timeoutSeconds)
        {
            Debug.Log("[InactivityManager] Timeout reached, loading main scene...");
            LoadMainScene();
        }
    }

    private bool HasUserInput()
    {

        if (Input.anyKeyDown)
            return true;

        if (Input.GetMouseButtonDown(0))
            return true;

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Began)
                    return true;
            }
        }

        return false;
    }

    public void ResetTimer()
    {
        timeSinceLastInteraction = 0f;

    }

    private void LoadMainScene()
    {
        ResetTimer();

        if (string.IsNullOrEmpty(mainSceneName))
        {
            Debug.LogError("[InactivityManager] mainSceneName이 비어 있습니다!");
            return;
        }

        Debug.Log("[InactivityManager] Loading scene: " + mainSceneName);
        SceneManager.LoadScene(mainSceneName);
    }
}
