using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour, IPointerClickHandler
{
    [Header("Assets")]
    [SerializeField] private CanvasGroup _tutorialCanvasGroup;
    [SerializeField] private CanvasGroup _tutorialFrameCanvasGroup;
    [SerializeField] private GameObject _tutorialFrame;
    [SerializeField] private CanvasGroup _clickAnywhereToContinue;
    [SerializeField] private GameObject currentShownCanvas;

    [Header("Hackjob Tutorial")]
    [SerializeField] private GameObject _characterSelection;
    [SerializeField] private List<GameObject> _combatTutorials;

    private bool canClick = false;


    public static Tutorial Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    [ContextMenu("Reset Tutorials")]
    [ExecuteInEditMode]
    public void EraseTutorial()
    {
        PlayerPrefs.SetInt("Tutorial_CharacterSelection", 0);
        PlayerPrefs.SetInt("Tutorial_Combat", 0);
        PlayerPrefs.Save();
    }

    public void TryShowTutorialCharacterSelection()
    {
        string TUT_KEY = "Tutorial_CharacterSelection";

        Debug.Log("Checking if can open tutorial....");

        if (PlayerPrefs.GetInt(TUT_KEY, 0) == 0)
        {
            Debug.Log("Opening tutorial....");
            ActivateTutorial();
            PlayerPrefs.SetInt(TUT_KEY, 1);
            PlayerPrefs.Save();

            GameObject newObject = Instantiate(_characterSelection, _tutorialFrame.transform);
            _tutorialCanvasGroup.DOFade(1f, 2f);
            currentShownCanvas = newObject;
            StartCoroutine(ShowContinueButton(1f));
        }
    }

    //This is bad.
    private int combatTutorialIndex = 0;

    public void ShowFrameTutorialCombat()
    {
        string TUT_KEY = "Tutorial_Combat";
        Debug.Log("Attempting to show combat tutorial");
        if (PlayerPrefs.GetInt(TUT_KEY, 0) == 0 || (combatTutorialIndex > 0  && combatTutorialIndex < _combatTutorials.Count))
        {
            ActivateTutorial();
            PlayerPrefs.SetInt(TUT_KEY, 1);
            PlayerPrefs.Save();
            _tutorialCanvasGroup.DOFade(1f, 2f);
            
            GameObject newObject = Instantiate(_combatTutorials[combatTutorialIndex], _tutorialFrame.transform);
            currentShownCanvas = newObject;
            combatTutorialIndex++;
            StartCoroutine(ShowContinueButton(1f));
        }
    }

    private IEnumerator ShowContinueButton(float i)
    {
        yield return new WaitForSeconds(i);
        _clickAnywhereToContinue.DOFade(1f, 0.5f);
        canClick = true;
    }

    private void ActivateTutorial()
    {
        if (_tutorialCanvasGroup.alpha == 1f) return;
        _tutorialCanvasGroup.interactable = true;
        _tutorialCanvasGroup.blocksRaycasts = true;
        _tutorialCanvasGroup.DOFade(1f, 1f);
    }

    private void FuckOffTutorial()
    {
        _tutorialCanvasGroup.DOFade(0f, 1f).onComplete = () => {
            Destroy(currentShownCanvas);
            currentShownCanvas = null;
            _tutorialCanvasGroup.interactable = false;
            _tutorialCanvasGroup.blocksRaycasts = false;
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
        {
            if (combatTutorialIndex > 0 && combatTutorialIndex < _combatTutorials.Count)
            {
                currentShownCanvas.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).onComplete = () =>
                {
                    Destroy(currentShownCanvas);
                    currentShownCanvas = null;
                    ShowFrameTutorialCombat();
                };
            }
            else 
            {
                FuckOffTutorial();
            }
        }
    }
}
