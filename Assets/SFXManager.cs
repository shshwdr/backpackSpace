using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    private AudioSource sfxSource;
    // Start is called before the first frame update
    void Start()
    {
         sfxSource = GetComponent<AudioSource>();
    }
public void PlaySFX(AudioClip clip)
{
    sfxSource.PlayOneShot(clip);
}

public void PlaySFX(string str)
{
    var clip = Resources.Load<AudioClip>("sfx/" + str);
    if (clip == null)
    {
        Debug.LogError("SFX not found: " + str);
        return;
    }
    sfxSource.PlayOneShot(clip);
}
}
