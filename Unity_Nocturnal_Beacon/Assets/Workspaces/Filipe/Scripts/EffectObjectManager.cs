using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectObjectManager : MonoBehaviour
{
    
    void Deactivate()
    {
        this.gameObject.SetActive(false);
        if(TryGetComponent<Animation>(out Animation anim)) 
        {
            anim.Stop();
        }
        EffectManager.Instance.PushNumberEffect(this.gameObject.GetComponent<TextMeshProUGUI>());
    }
}
