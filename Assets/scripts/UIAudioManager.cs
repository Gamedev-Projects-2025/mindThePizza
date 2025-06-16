using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        Instance = this;
    }

    public void PlayClick()
    {
        Debug.Log("PlayClick called");
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
