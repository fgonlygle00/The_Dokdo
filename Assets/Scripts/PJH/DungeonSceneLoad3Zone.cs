using UnityEngine;

public class DungeonSceneLoad3Zone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene("DungeonScene_3Zone");
        }
    }
}
