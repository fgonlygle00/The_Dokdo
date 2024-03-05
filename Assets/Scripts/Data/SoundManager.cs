using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public string audioFolderPath; // ����� ������ ����� ���� ���

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

  

    // ���ҽ� �������� Ư�� ����� ���� ���
    public void PlayAudioFromResources(string audioFileName)
    {
        AudioClip audioClip = Resources.Load(audioFolderPath + "/" + audioFileName) as AudioClip;

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }


}
