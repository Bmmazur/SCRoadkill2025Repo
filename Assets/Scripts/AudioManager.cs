using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource soundFXSource;
    [SerializeField] AudioSource backgroundSource;

    public AudioClip background;
    public AudioClip shovelMiss;
    public AudioClip shovelHit;
    public AudioClip walk;
    public AudioClip sprint;
    public AudioClip carCrash;

    private void Start()
    {
        backgroundSource.clip = background;
        backgroundSource.Play();

    }

    public void PlaySFX(AudioClip clip)
    {
        soundFXSource.PlayOneShot(clip);
    }
}
