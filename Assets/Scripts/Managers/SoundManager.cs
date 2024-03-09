using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _SFX;

    [SerializeField] private AudioClip _EndGame;

    [SerializeField] private AudioClip _DropBall;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioSFX sfx)
    {
        switch(sfx)
        {
            case AudioSFX.DROP:
            {
                _SFX.clip = _DropBall;
                _SFX.Play();
                break;
            }
            case AudioSFX.ENDGAME:
            {
                _SFX.clip = _EndGame;
                _SFX.Play();
                break;
            }
        }
        
    }
}

public enum AudioSFX
{
    DROP,
    ENDGAME
}
