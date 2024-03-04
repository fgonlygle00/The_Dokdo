using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartSceneUI : MonoBehaviour
{

    [SerializeField] private Button LoadButton;

    // Start is called before the first frame update
    void Start()
    {
        LoadButton.onClick.AddListener(() => InfoManager.instance.LoadGame());
        //로드 버튼을 코드로 연결해줌

    }


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
