using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITopBarManager : MonoBehaviour, NoctBeaconRunData.NoctBeaconListener
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
    [SerializeField] private TextMeshProUGUI hpTxt;

    private NoctBeaconRunData   _beaconRunData;
    private Animator            _animator;

    void Start()
    {
        NoctBeaconRunData.Instance.AddListener(this);
        _beaconRunData = NoctBeaconRunData.Instance;
        hpTxt.text = $"{_beaconRunData.GetPlayerInformation().GetCurrentHP()}";
        goldTxt.text = $"{_beaconRunData.GetGold()}";
    }

    public void SetFloor(int floor)
    {
        floorTxt.text = $"Floor {floor}";
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
        int currentHp = int.Parse(hpTxt.text);
        DOTween.To(() => currentHp,
            x => currentHp = x, hp, 1.5f).OnUpdate(() =>
            {
                hpTxt.text = $"{currentHp}";
            }
        );
    }

    public void SetGold(int gold)
    {
        int currentGold = int.Parse(goldTxt.text);
        DOTween.To(() => currentGold,
            x => currentGold = x, gold, 1.5f).OnUpdate(() =>
            {
                goldTxt.text = $"{currentGold}";
            }
        );
    }

    public void OnClickDeckBtn()
    {
        var dp = UIManager.Instance.ShowPage(GamePage.DeckPage).GetComponent<DeckPage>();

        dp.Setup();
        dp.Show();
    }

    

    public void OnHealthChanged()
    {
        SetHP(NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentHP());
    }

    public void OnGoldChanged()
    {
        SetGold(NoctBeaconRunData.Instance.GetGold());

    }

    public void OnFloorChanged()
    {
        SetFloor(NoctBeaconRunData.Instance.GetHeight());

    }
}
