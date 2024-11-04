using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{

    [SerializeField] AudioClip _music;
    [SerializeField] AudioClip _clickSound;
    [SerializeField] AudioClip _winSound;
    [SerializeField] AudioClip _loseSound;
    [SerializeField] AudioClip _rainAmbiance;

    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _sfxSource;
    [SerializeField] AudioSource _ambienceSource;

    public static SfxManager Instance { get; private set; }

    private float _mainVolume = 1.0f;
    private float _sfxVolume = 1.0f;
    private float _musicVolume = 1.0f;
    private float _ambienceVolume = 1.0f;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this); 
        }
        DontDestroyOnLoad(this);
    }

    public void SetMasterVolume(float volume)
    {
        _mainVolume = volume;
        _musicSource.volume = _musicVolume * _mainVolume;
        _sfxSource.volume = _sfxVolume * _mainVolume; 
        _ambienceSource.volume = _ambienceVolume * _mainVolume;
    }
    public float GetMasterVolume()
    {
        return _mainVolume;
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
        _musicSource.volume = _musicVolume * _mainVolume; 
    }

    public float GetSfxVolume()
    {
        return _sfxVolume;
    }

    public void SetSfxVolume(float volume)
    {
        _sfxVolume = volume;
        _sfxSource.volume = _sfxVolume * _mainVolume;
    }

    public float GetAmbienceVolume()
    {
        return _ambienceVolume;
    }

    public void SetAmbienceVolume(float volume)
    {
        _ambienceVolume = volume;
        _ambienceSource.volume = _ambienceVolume * _mainVolume;
    }

    public void PlayAmbience()
    {
        _ambienceSource.clip = _rainAmbiance;
        _ambienceSource.loop = true;
        _ambienceSource.Play();
    }

    public void PlayClickSound()
    {
        _sfxSource.clip = _clickSound;
        _sfxSource.time = 0.1f;
        _sfxSource.Play();
    }

    public void PlayMainMenuMusic()
    {
        _musicSource.clip = _music;
        _musicSource.loop = true;
        _musicSource.Play(); ;
    }

    public void PlayWinMusic()
    {
        _musicSource.clip = _winSound;
        _musicSource.loop = false;
        _musicSource.Play();
    }

    public void PlayLossMusic()
    {
        _musicSource.clip = _loseSound; 
        _musicSource.loop = false;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.clip = null; 
        _musicSource.loop = false;
    }

    public void StopAmbience()
    {
        _ambienceSource.clip = null;
        _ambienceSource.loop = false;
    }
}
