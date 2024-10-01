using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    public static SceneManagement Instance { get; private set; }
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
        }
    }

    [Serializable]
    public enum GAME_SCENE 
    {
        MAIN_MENU = 0,
        MAP,
        BATTLE
    }

    public Dictionary<GAME_SCENE, string> sceneNames;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        sceneNames = new Dictionary<GAME_SCENE, string>();
        sceneNames.Add(GAME_SCENE.MAIN_MENU, "MainMenu_InProgress");
        sceneNames.Add(GAME_SCENE.MAP, "MapPrototype");
        sceneNames.Add(GAME_SCENE.BATTLE, "CombatTest");
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

   

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
