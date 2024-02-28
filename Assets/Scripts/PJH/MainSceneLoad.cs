using UnityEngine;

public class MainSceneLoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene("MainScene");
        }
    }
}
