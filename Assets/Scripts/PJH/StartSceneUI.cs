using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    // 새로시작
    public void NewGame()
    {
        LoadingSceneManager.LoadScene("MainScene");
    }

    // 종료
    public void ExitGame()
    {
        // 해당 코드는 에디터에 재생을 중지하는거
        UnityEditor.EditorApplication.isPlaying = false;
        // 진짜 게임을 종료하는거
        Application.Quit();
    }
}
