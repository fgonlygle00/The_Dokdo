using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    //[SerializeField]
    //Image progressBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        Animation animation = GetComponentInChildren<Animation>();

        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                float target = op.progress;
                float currentValue = animation["circle_half_rotating_3_1"].normalizedTime;

                currentValue = Mathf.Lerp(currentValue, target, timer);
                if (currentValue >= target)
                {
                    timer = 0f;
                }

                animation["circle_half_rotating_3_1"].normalizedTime = currentValue;
            }
            else
            {
                float target = 1f;
                float currentValue = animation["circle_half_rotating_3_1"].normalizedTime;

                currentValue = Mathf.Lerp(currentValue, target, timer);
                if (currentValue >= target)
                {
                    op.allowSceneActivation = true;
                    break;
                }

                animation["circle_half_rotating_3_1"].normalizedTime = currentValue;
            }
        }
    }
}