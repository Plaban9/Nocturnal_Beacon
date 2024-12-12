using DG.Tweening;
using Minimalist.Audio;
using Minimalist.Audio.Music;
using Minimalist.Audio.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] public MenuPrototype menu;

    [SerializeField] public Slider masterVolume;
    [SerializeField] public Slider musicVolume;
    [SerializeField] public Slider sfxVolume;

    [SerializeField] public GameObject heroSelectMenu;

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

    public void OpenSelectionMenu()
    {
        Tutorial.Instance.TryShowTutorialCharacterSelection();
        menu.DisableMainMenu();
        heroSelectMenu.SetActive(true);
        heroSelectMenu.transform.DOScaleY(1f, 0.5f);
    }

    public void Exit()
    {
        Application.Quit();
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
