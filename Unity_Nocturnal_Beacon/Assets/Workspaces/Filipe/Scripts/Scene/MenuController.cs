using Minimalist.Audio;
using Minimalist.Audio.Music;
using Minimalist.Audio.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] public PlayerUnitData _DEBUG_DATA_FOR_PROTOTYPE1;

    [SerializeField] public Slider masterVolume;
    [SerializeField] public Slider musicVolume;
    [SerializeField] public Slider sfxVolume;

    // Start is called before the first frame update
    void Start()
    {
        masterVolume.value = 1f;
        musicVolume.value = 1f;
        sfxVolume.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        NoctBeaconRunData.Instance.SetPlayer(_DEBUG_DATA_FOR_PROTOTYPE1);
        SceneController.Instance.ToMap(); 
    }

    public void SetMasterVolume()
    {
        AudioManager.SetMasterVolume(masterVolume.value);

    }

    public void SetMusicVolume()
    {
        AudioManager.SetMusicVolume(musicVolume.value);
    }

    public void SetSFXVolume()
    {
        AudioManager.SetSFXVolume(sfxVolume.value);
    }
}
