using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource battleMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBattleMusic()
    {
        
        //battleMusic.Play();

        battleMusic.DOFade(1, 0.5f);
    }
    public void StopBattleMusic()
    {
        //battleMusic.Stop();
        battleMusic.DOFade(0, 0.5f);
    }
}
