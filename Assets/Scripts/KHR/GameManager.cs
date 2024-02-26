using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 게임 매니저의 싱글톤 인스턴스
    public static GameManager instance;

    private void Awake()
    {
        // 싱글톤 패턴을 위한 코드
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 게임이 시작될 때 호출되는 메소드
    public void StartGame()
    {
        Debug.Log("게임이 시작되었습니다.");
        // 게임 시작에 필요한 추가적인 로직을 여기에 작성하세요.
    }

    // 게임이 끝날 때 호출되는 메소드
    public void EndGame()
    {
        Debug.Log("게임이 종료되었습니다.");
        // 게임 종료에 필요한 추가적인 로직을 여기에 작성하세요.
    }
}
