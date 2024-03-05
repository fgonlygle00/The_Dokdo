using UnityEngine;

public class MainSceneLoad : MonoBehaviour
{
    private void Awake()
    {
        InfoManager.instance.LoadGame2();
        DataManager.Instance.SetPlayerData();
        DataManager.Instance.SetItemData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene("MainScene");
        }
    }
}