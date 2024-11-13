using Minimalist.Audio;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MenuPrototype : MonoBehaviour
{

    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject settingsMenuUI;
    [SerializeField] GameObject creditsMenuUI;

    [Header("Animation Details")]
    [SerializeField] float timeToAppear = 0.5f;

    private GameObject currentMenu;

    List<GameObject> menus = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.PlayMusic(Minimalist.Audio.Music.MusicType.Menu);

        /*
         * Don't mind me
         */
        menus.Add(mainMenuUI); 
        menus.Add(settingsMenuUI);
        menus.Add(creditsMenuUI);
        DisableAllMenuInstantly();
        currentMenu = mainMenuUI;
        EnableMenu(mainMenuUI.GetComponent<CanvasGroup>());
        /*
         * Don't worry about it
         */
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenMainMenu()
    {
        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.UI_Click);

        DisableMenu(currentMenu.GetComponent<CanvasGroup>(),
            () =>
            {
                EnableMenu(mainMenuUI.GetComponent<CanvasGroup>());
            });
    }

    public void OpenSettingsMenu()
    {
        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.UI_Click);

        DisableMenu(currentMenu.GetComponent<CanvasGroup>(),
            () =>
            {
                EnableMenu(settingsMenuUI.GetComponent<CanvasGroup>());
            });
    }

    public void OpenCreditsMenu()
    {
        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.UI_Click);

        DisableMenu(currentMenu.GetComponent<CanvasGroup>(),
            () =>
            {
                EnableMenu(creditsMenuUI.GetComponent<CanvasGroup>());
            });
    }


    private void DisableAllMenuInstantly()
    {
        foreach (GameObject menu in menus)
        {
            if (menu.TryGetComponent<CanvasGroup>(out CanvasGroup group))
            {
                Debug.Log($"Disabling {menu.name}...");
                group.alpha = 0f;
                group.interactable = false;
                group.blocksRaycasts = false;
            }
            
        }
    }

    void EnableMenu(CanvasGroup menu)
    {
        Debug.Log(menu.alpha); 
        StartCoroutine(EnableMenuAnimation(menu));
        currentMenu = menu.gameObject;
    }

    IEnumerator EnableMenuAnimation(CanvasGroup menu)
    {
        for(float i = 0f; i <= 1f; i+= Time.deltaTime / timeToAppear)
        {
            //Debug.Log($"Enabling... {i}");
            //Debug.Log($"Menu name {menu.name} and alpha {menu.alpha}");
            menu.gameObject.GetComponent<CanvasGroup>().alpha = i;
            if (i >= 0.5f)
            {
                menu.interactable = true;
                menu.blocksRaycasts = true;
            }
                yield return null;
        }
        menu.alpha = 1f; 
    }

    public void DisableMainMenu()
    {
        DisableMenu(mainMenuUI.GetComponent<CanvasGroup>(), () => { });
    }

    public void DisableMenu(CanvasGroup menu, Action doOnFinish)
    {
        StartCoroutine(DisableMenuAnimation(menu, doOnFinish));
    }

    IEnumerator DisableMenuAnimation(CanvasGroup menu, Action doOnFinish)
    {
        menu.interactable = false;
        menu.blocksRaycasts = false;
        for (float i = 0; i <= 1f; i += Time.deltaTime / timeToAppear)
        {
            menu.alpha = 1 - i;
            if (i >= 0.5f)
            {
                menu.interactable = false;
                menu.blocksRaycasts = false;
            }
            yield return null;
        }
        menu.alpha = 0f; 
        doOnFinish.Invoke();
    }



}
