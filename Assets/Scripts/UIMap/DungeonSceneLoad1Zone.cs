using UnityEngine;

public class DungeonSceneLoad1Zone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene("DungeonScene_1Zone");
        }
    }
}
