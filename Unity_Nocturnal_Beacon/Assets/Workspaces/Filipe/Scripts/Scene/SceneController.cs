using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Serializable]
    public enum GAME_SCENE 
    {
        MAIN_MENU = 0,
        MAP,
        BATTLE
    }

    private LoadingScreenAnimations _loadingScreenAnimation; 

    public Dictionary<GAME_SCENE, string> sceneNames;

    // Start is called before the first frame update
    void Start()
    {
        _loadingScreenAnimation = GetComponent<LoadingScreenAnimations>();
        sceneNames = new Dictionary<GAME_SCENE, string>();
        sceneNames.Add(GAME_SCENE.MAIN_MENU, "MainMenu_InProgress");
        sceneNames.Add(GAME_SCENE.MAP, "MapPrototype");
        sceneNames.Add(GAME_SCENE.BATTLE, "CombatTest");
    }

    public void ToMain()
    {
        ChangeToScene(GAME_SCENE.MAIN_MENU);
    }


    public void ToMap()
    {
        ChangeToScene(GAME_SCENE.MAP);
    }

    public void ToBattle()
    {
        ChangeToScene(GAME_SCENE.BATTLE);
    }

    public void ChangeToScene(GAME_SCENE gameScene)
    {
        StartCoroutine(LoadSceneAsync(sceneNames[gameScene]));
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        _loadingScreenAnimation.ToLoading();

        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.completed += (AsyncOperation) => {
            _loadingScreenAnimation.ToScene();
        };



        while (!asyncLoad.isDone)
        {
            
            yield return null;
        }

        Debug.Log("Aie!");
    }
}
