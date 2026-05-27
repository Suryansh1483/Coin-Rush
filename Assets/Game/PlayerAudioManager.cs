using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioManager : MonoBehaviour
{
    public static PlayerAudioManager instance;

    [Header("Audio Clips")]
    public AudioClip walkClip;
    public AudioClip idleClip;
    public AudioClip coinPickupClip;
    public AudioClip fireHitClip;
    public AudioClip deathClip;
    public AudioClip resetClip;

    [Header("Settings")]
    public float volume = 1f;

    private AudioSource audioSource;
    private bool isWalking;
    private bool isDead;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    void Update()
    {
        if (isDead || audioSource == null) return;

        bool currentlyWalking = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (currentlyWalking && !isWalking)
        {
            PlayLoop(walkClip);
            isWalking = true;
        }
        else if (!currentlyWalking && isWalking)
        {
            PlayLoop(idleClip);
            isWalking = false;
        }
    }

    private void PlayLoop(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }

    private void PlayOnce(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayCoinSound() => PlayOnce(coinPickupClip);
    public void PlayFireHitSound() => PlayOnce(fireHitClip);

    public void PlayDeathSound()
    {
        StopAllSounds();
        PlayOnce(deathClip);
        isDead = true;
    }

    public void PlayResetSound()
    {
        StopAllSounds();
        PlayOnce(resetClip);
        isDead = false;
    }

    public void StopAllSounds()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        isWalking = false;
    }

    public void ResetAudioState()
    {
        StopAllSounds();
        isDead = false;
        isWalking = false;
    }
}
