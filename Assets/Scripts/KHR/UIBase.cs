//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UIBase : MonoBehaviour
//{
//    // ui  ��ҵ��� ������ ����
//    public GameObject panel1;
//    public GameObject panel2;
//    public Text scoreText;
//    // �г� 1 Ȱ��ȭ
//    public virtual void ActivationPanel1()
//    {
//        panel1.SetActive(false);
//    }
//    //�г� 2 Ȱ��ȭ
//    public virtual void ActivationPanel2()
//    {
//        panel1.SetActive(false);
//    }
//    //�г� 1 ��Ȱ��ȭ
//    public virtual void DeactivatePanel1()
//    {
//        panel1.SetActive(false);
//    }
//    //�г� 2 ��Ȱ��ȭ
//    public virtual void DeactivatePanel2()
//    {
//        panel1.SetActive(false);
//    }

//    // ���ھ� �ؽ�Ʈ ������Ʈ
//    public virtual void UpdateScoreText(int score)
//    {
//        scoreText.text = "Score: " + score.ToString();
//    }
//}
