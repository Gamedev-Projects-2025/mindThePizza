using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsAudioController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Toggle muteToggle;

    private const string MUSIC_VOLUME_PARAM = "MusicVolume";
    private const string PREF_MUTED = "IsMuted";

    void Start()
    {
        bool isMuted = PlayerPrefs.GetInt(PREF_MUTED, 0) == 1;
        muteToggle.SetIsOnWithoutNotify(isMuted);
        ApplyMute(isMuted);
    }

    public void OnMuteToggled(bool isMuted)
    {
        bool actualMuteState = muteToggle.isOn;
        PlayerPrefs.SetInt(PREF_MUTED, actualMuteState ? 1 : 0);
        ApplyMute(actualMuteState);
    }

    private void ApplyMute(bool isMuted)
    {
        float volume = isMuted ? -80f : 0f;
        audioMixer.SetFloat(MUSIC_VOLUME_PARAM, volume);
    }
}