using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public static LevelLoader Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToMainMenu()
    {
        StartCoroutine(LoadScene("MainMenu"));
    }

    public void ToLevel1()
    {
        StartCoroutine(LoadScene("Level 1"));
    }

    public void ToSettings()
    {
        StartCoroutine(LoadScene("Settings"));
    }

    public void ToWinScene()
    {
        StartCoroutine(LoadScene("WinScene"));
    }

    public void ToLoseScene()
    {
        StartCoroutine(LoadScene("LoseScene"));
    }

    public void LoseScene()
    {
        StartCoroutine(LoadScene("LoseScene"));
    }

    


}
