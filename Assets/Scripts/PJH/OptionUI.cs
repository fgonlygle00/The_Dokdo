using UnityEngine;

public class OptionUI : MonoBehaviour
{
    public GameObject OptionWindow;

    // 창 열기
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OptionWindow.SetActive(true);
        }
    }

    // 창 닫기
    public void CloseWindow()
    {
        OptionWindow.SetActive(false);
    }

    // 시작 화면으로 이동
    public void HomeGame()
    {
        LoadingSceneManager.LoadScene("StartScene");
    }

    // 게임 종료
    public void ExitGame()
    {
        // 해당 코드는 에디터에 재생을 중지하는거
        UnityEditor.EditorApplication.isPlaying = false;
        // 진짜 게임을 종료하는거
        Application.Quit();
    }
}
