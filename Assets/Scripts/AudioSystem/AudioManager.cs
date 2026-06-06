using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource menuMusicSource;
    public AudioSource gameAudioSource;
    public AudioSource sfxSource;

    [Header("Menu")]
    public AudioClip menuMusic;

    [Header("Gameplay Audio")]
    public AudioClip idleClip;
    public AudioClip walkClip;

    [Header("SFX")]
    public AudioClip coinClip;
    public AudioClip hurtClip;
    public AudioClip winClip;
    public AudioClip gameOverClip;
    public AudioClip buttonClickClip;

    private bool isWalking;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetMenuMusicVolume(
            PlayerPrefs.GetFloat("MusicVolume", 1f)
        );

        SetGameAudioVolume(
            PlayerPrefs.GetFloat("SFXVolume", 1f)
        );
    }

    public void PlayMenuMusic()
    {
        if (menuMusicSource == null || menuMusic == null)
            return;

        if (
            menuMusicSource.clip == menuMusic &&
            menuMusicSource.isPlaying
        )
            return;

        menuMusicSource.clip = menuMusic;
        menuMusicSource.loop = true;
        menuMusicSource.Play();
    }

    public void StopMenuMusic()
    {
        if (menuMusicSource != null)
            menuMusicSource.Stop();
    }

    public void PlayIdle()
    {
        if (gameAudioSource == null || idleClip == null)
            return;

        if (
            gameAudioSource.clip == idleClip &&
            gameAudioSource.isPlaying
        )
            return;

        gameAudioSource.clip = idleClip;
        gameAudioSource.loop = true;
        gameAudioSource.Play();

        isWalking = false;
    }

    public void PlayWalk()
    {
        if (gameAudioSource == null || walkClip == null)
            return;

        if (isWalking)
            return;

        gameAudioSource.clip = walkClip;
        gameAudioSource.loop = true;
        gameAudioSource.Play();

        isWalking = true;
    }

    public void StopWalk()
    {
        if (!isWalking)
            return;

        PlayIdle();
    }

    public void StopGameplayAudio()
    {
        if (gameAudioSource != null)
            gameAudioSource.Stop();

        isWalking = false;
    }

    public void StopAllAudio()
    {
        if (menuMusicSource != null)
            menuMusicSource.Stop();

        if (gameAudioSource != null)
            gameAudioSource.Stop();

        if (sfxSource != null)
            sfxSource.Stop();

        isWalking = false;
    }

    public void PlayCoin()
    {
        PlaySFX(coinClip);
    }

    public void PlayHurt()
    {
        PlaySFX(hurtClip);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickClip);
    }

    public void PlayWin()
    {
        if (winClip == null)
            return;

        AudioSource.PlayClipAtPoint(
            winClip,
            Camera.main.transform.position,
            sfxSource != null ? sfxSource.volume : 1f
        );
    }

    public void PlayGameOver()
    {
        if (gameOverClip == null)
            return;

        AudioSource.PlayClipAtPoint(
            gameOverClip,
            Camera.main.transform.position,
            sfxSource != null ? sfxSource.volume : 1f
        );
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetMenuMusicVolume(float value)
    {
        if (menuMusicSource != null)
            menuMusicSource.volume = value;

        PlayerPrefs.SetFloat(
            "MusicVolume",
            value
        );
    }

    public void SetGameAudioVolume(float value)
    {
        if (gameAudioSource != null)
            gameAudioSource.volume = value;

        if (sfxSource != null)
            sfxSource.volume = value;

        PlayerPrefs.SetFloat(
            "SFXVolume",
            value
        );
    }
}