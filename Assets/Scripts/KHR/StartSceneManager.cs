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
        
            // 이제 infoManager를 사용하여 InfoManager의 메소드를 호출하거나 변수를 설정
        }
        else
        {
            Debug.LogError("InfoManager prefab could not be loaded. Make sure it is placed in a Resources folder.");
        }
    }

  
}
