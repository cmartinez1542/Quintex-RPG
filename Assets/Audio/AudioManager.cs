using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------- Audio Source --------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-------- Audio Clip --------")]
    public AudioClip background;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip death;
    

    public void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlayAttackSound()
    {
        if (attack1 != null )
        {
            SFXSource.clip = attack1;
            SFXSource.pitch = 2f; // Increase pitch
            SFXSource.dopplerLevel = 0; // Prevents unwanted distortion
            SFXSource.Play();
        }
    }

    public void PlayAttackSound2()
    {
        if (attack2 != null )
        {
            SFXSource.clip = attack2;
            SFXSource.Play();
        }
    }

}


