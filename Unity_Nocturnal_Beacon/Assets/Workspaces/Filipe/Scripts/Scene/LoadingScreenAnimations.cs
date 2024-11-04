using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenAnimations : MonoBehaviour
{
    // Start is called before the first frame update

    public static LoadingScreenAnimations Instance { get; private set; }

    [SerializeField] Image loadingScreen;
    float _currentProgress = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);

    }




    public void ToLoading()
    {
        StartCoroutine(ToLoading(1f));
    }

    public void ToScene()
    {
        StartCoroutine(ToScene(1f));
    }

    IEnumerator ToLoading(float time)
    {
        float amountToMove = 0.5f - _currentProgress;

        while(_currentProgress <= 0.5f)
        {
            loadingScreen.material.SetFloat("_Transition", _currentProgress);
            _currentProgress += Time.deltaTime*amountToMove/time;
            yield return new WaitForFixedUpdate();
        }

        DoneLoading();
    }

    IEnumerator ToScene(float time)
    {
        float amountToMove = 1f - _currentProgress;

        while (_currentProgress <= 1f)
        {
            loadingScreen.material.SetFloat("_Transition", _currentProgress);
            _currentProgress += Time.deltaTime * amountToMove / time;
            yield return new WaitForFixedUpdate();
        }

        DoneLoading();
    }


    public void DoneLoading()
    {

    }
}
