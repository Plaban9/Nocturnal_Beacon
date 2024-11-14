using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITopBarManager : MonoBehaviour
{
    
    public static UITopBarManager Instance;

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

        _animator = GetComponent<Animator>();

    }

    [Header("Text Objects")]
    [SerializeField] TextMeshProUGUI floorTxt;
    [SerializeField] TextMeshProUGUI goldTxt;
    [SerializeField] TextMeshProUGUI hpTxt;

    private NoctBeaconRunData   _beaconRunData;
    private Animator            _animator;

    void Start()
    {
    }

    private void OnLevelWasLoaded(int level)
    {
        _beaconRunData = NoctBeaconRunData.Instance;
        floorTxt.text = $"Floor to Summit: {_beaconRunData.GetHeight()}";
        hpTxt.text = $"{_beaconRunData.GetPlayerInformation().GetCurrentHP()}";
        goldTxt.text = $"{_beaconRunData.GetGold()}";
        if (level == 2) // if not map level
        {
            PushUp();
        }
        else
        {
            PullDown();
        }
    }

    public void PullDown()
    {
        _animator.SetBool("isVisible", true); 
    }

    public void PushUp()
    {
        _animator.SetBool("isVisible", false);
    }

    public bool IsDown()
    {
        return _animator.GetBool("isVisible");
    }

    public void SetHP(int hp)
    {
        hpTxt.text = hp.ToString();
    }

    public void SetGold(int gold)
    {
        goldTxt.text = gold.ToString();
    }

    public void OnClickDeckBtn()
    {
        var dp = UIManager.Instance.ShowPage(GamePage.DeckPage).GetComponent<DeckPage>();

        dp.Setup();
        dp.Show();
    }
}
