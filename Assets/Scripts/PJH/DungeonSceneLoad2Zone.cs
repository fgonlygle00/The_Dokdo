using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonSceneLoad2Zone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("DungeonScene_2Zone");
        }
    }
}
