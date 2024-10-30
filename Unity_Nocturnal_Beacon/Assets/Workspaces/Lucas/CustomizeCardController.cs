using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class CustomizeCardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<GameObject> editableNotices = new List<GameObject>();
    [SerializeField] List<EffectSlot> effectSlots = new List<EffectSlot>();

    ReactiveProperty<EffectSlot> selectingSlot = new ReactiveProperty<EffectSlot>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveAllEditableNotice(bool set)
    {
        editableNotices.ForEach(x => x.SetActive(set));
        
    }

    public void SetSelectingSlot(EffectSlot slot = null)
    {
        selectingSlot.Value = slot;

        foreach(var s in effectSlots)
        {
            s.SetSelecting(s == slot);
        }

        SetActiveAllEditableNotice(false);
    }

    public void Reset()
    {
        SetSelectingSlot(null);
        SetActiveAllEditableNotice(true);
    }


}
