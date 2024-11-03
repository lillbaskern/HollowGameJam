using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<AudioClip> AudioFiles;

    public AudioSource SFXSource;
    public AudioSource UISFXSource;

    void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else Instance = this;
    }
    public void PlaySound(string clip)
    {
        switch (clip.ToLower())
        {
            case "win":
                PlaySFX(AudioFiles[7]);
                break;
            case "no":
                PlaySFX(AudioFiles[2]);
                break;
            case "yes":
                PlaySFX(AudioFiles[3]);
                break;
            case "lose":
                PlaySFX(AudioFiles[4]);
                break;
            case "ding":
                PlaySFX(AudioFiles[5]);
                break;
            case "placecard":
                PlaySFX(AudioFiles[0]);
                break;
            default:
                break;
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayUI(AudioClip clip)
    {

    }

}
