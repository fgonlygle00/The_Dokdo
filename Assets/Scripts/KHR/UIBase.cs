//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UIBase : MonoBehaviour
//{
//    // ui  요소들을 연결할 변수
//    public GameObject panel1;
//    public GameObject panel2;
//    public Text scoreText;
//    // 패널 1 활성화
//    public virtual void ActivationPanel1()
//    {
//        panel1.SetActive(false);
//    }
//    //패널 2 활성화
//    public virtual void ActivationPanel2()
//    {
//        panel1.SetActive(false);
//    }
//    //패널 1 비활성화
//    public virtual void DeactivatePanel1()
//    {
//        panel1.SetActive(false);
//    }
//    //패널 2 비활성화
//    public virtual void DeactivatePanel2()
//    {
//        panel1.SetActive(false);
//    }

//    // 스코어 텍스트 업데이트
//    public virtual void UpdateScoreText(int score)
//    {
//        scoreText.text = "Score: " + score.ToString();
//    }
//}
