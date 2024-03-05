using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject infoManagerPrefab = Resources.Load<GameObject>("InfoManager");
        if (infoManagerPrefab != null)
        {
            GameObject infoManagerInstance = Instantiate(infoManagerPrefab);
        
            // ���� infoManager�� ����Ͽ� InfoManager�� �޼ҵ带 ȣ���ϰų� ������ ����
        }
        else
        {
            Debug.LogError("InfoManager prefab could not be loaded. Make sure it is placed in a Resources folder.");
        }
    }

  
}
