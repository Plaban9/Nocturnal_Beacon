using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitBattleStatusEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _intensityTxt;
    [SerializeField] private TextMeshProUGUI _durationTxt;
    [SerializeField] Image _image;
    public BattleStatusEffect bstf;

    // Start is called before the first frame update
    void Start()
    {
        if (bstf != null) UpdateInformation();        
    }
    
    public void SetBattleStatusEffect(BattleStatusEffect bstf)
    {
        this.bstf = bstf;
        if(_intensityTxt != null)
        {
            UpdateInformation();
        }
    }

    public void UpdateInformation()
    {
        if (bstf == null || _image == null) return;
        _image.sprite = bstf._status.icon;
        _intensityTxt.text = bstf._intensity.ToString();
        _durationTxt.text = bstf._duration.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
