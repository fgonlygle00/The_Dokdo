using UnityEngine;

public class StartSceneLoad : MonoBehaviour
{
    private void Start()
    {
        PlayerController.instance.ToggleCursor(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            LoadingSceneManager.LoadScene("StartScene");
    }
}
