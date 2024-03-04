using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public string audioFolderPath; // 오디오 파일이 저장된 폴더 경로

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

  

    // 리소스 폴더에서 특정 오디오 파일 재생
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
