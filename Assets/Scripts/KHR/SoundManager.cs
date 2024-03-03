using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public string audioFolderPath; // ����� ������ ����� ���� ���

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // ���ҽ� �������� ������ ����� ���� ���
    public void PlayRandomAudioFromResources()
    {
        Object[] audioFiles = Resources.LoadAll(audioFolderPath, typeof(AudioClip));

        if (audioFiles.Length > 0)
        {
            int randomIndex = Random.Range(0, audioFiles.Length);
            AudioClip audioClip = audioFiles[randomIndex] as AudioClip;

            audioSource.clip = audioClip;
            audioSource.Play();
        }
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
