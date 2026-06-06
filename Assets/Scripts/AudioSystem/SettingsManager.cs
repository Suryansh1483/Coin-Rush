using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;

    public Slider menuMusicSlider;
    public Slider gameSoundSlider;
    public Slider sensitivitySlider;

    private void Start()
    {
        settingsPanel.SetActive(false);

        menuMusicSlider.value =
            PlayerPrefs.GetFloat("MusicVolume", 1f);

        gameSoundSlider.value =
            PlayerPrefs.GetFloat("SFXVolume", 1f);

        sensitivitySlider.value =
            PlayerPrefs.GetFloat("MouseSensitivity", 3f);

        ApplyMenuMusicVolume(menuMusicSlider.value);
        ApplyGameSoundVolume(gameSoundSlider.value);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ApplyMenuMusicVolume(float value)
    {
        AudioManager.instance?.SetMenuMusicVolume(value);

        PlayerPrefs.SetFloat(
            "MusicVolume",
            value
        );
    }

    public void ApplyGameSoundVolume(float value)
    {
        AudioManager.instance?.SetGameAudioVolume(value);

        PlayerPrefs.SetFloat(
            "SFXVolume",
            value
        );
    }

    public void ApplySensitivity(float value)
    {
        PlayerPrefs.SetFloat(
            "MouseSensitivity",
            value
        );
    }
}