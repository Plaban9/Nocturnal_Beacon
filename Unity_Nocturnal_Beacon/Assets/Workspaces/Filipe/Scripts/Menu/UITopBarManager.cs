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
    }

    [Header("Text Objects")]
    [SerializeField] TextMeshProUGUI floorTxt;
    [SerializeField] TextMeshProUGUI goldTxt;
    [SerializeField] TextMeshProUGUI hpTxt;

    private NoctBeaconRunData   _beaconRunData;
    private Animator            _animator;
    // Start is called before the first frame update
    void Start()
    {
        _beaconRunData = NoctBeaconRunData.Instance;
        _animator = GetComponent<Animator>();




    }

    private void OnLevelWasLoaded(int level)
    {
        floorTxt.text = $"Floor {_beaconRunData.GetHeight()}";
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

    // Update is called once per frame
    void Update()
    {
        
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
}
