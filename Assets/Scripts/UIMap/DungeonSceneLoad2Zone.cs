using UnityEngine;

public class DungeonSceneLoad2Zone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene("DungeonScene_2Zone");
        }
    }
}
