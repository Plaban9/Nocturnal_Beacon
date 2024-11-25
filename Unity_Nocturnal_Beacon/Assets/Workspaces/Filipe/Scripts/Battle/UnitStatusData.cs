using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusData : MonoBehaviour
{
    private List<UnitBattleStatusEffect> activeEffects = new();
    [SerializeField] private GameObject statusEffectHolder;
    [SerializeField] private GameObject statusEffectPrefab;
    public void AddStatusEffect(BattleStatusEffect battleStatusEffect)
    {
        StatusEffect effectType = battleStatusEffect._status.statusEffect;
        UnitBattleStatusEffect unitBattleStatusEffect = activeEffects.Find(it => it.bstf._status.statusEffect == effectType);
        if(unitBattleStatusEffect == null)
        {
            GameObject _newIcon = Instantiate(statusEffectPrefab, statusEffectHolder.transform.position, Quaternion.identity, statusEffectHolder.transform);
            if(_newIcon.TryGetComponent<UnitBattleStatusEffect>(out UnitBattleStatusEffect ubstf))
            {
                ubstf.SetBattleStatusEffect(battleStatusEffect);
                ubstf.UpdateInformation();
                activeEffects.Add(ubstf);
            }
        }
        else
        {
            unitBattleStatusEffect.bstf._intensity += battleStatusEffect._intensity;
            unitBattleStatusEffect.bstf._duration += battleStatusEffect._duration;
            unitBattleStatusEffect.UpdateInformation();
        }
    }
    public void UpdateStatusEffects()
    {
        List<UnitBattleStatusEffect> toRemove = activeEffects.FindAll(it => it.bstf._duration <= 0);

        foreach (var rm in toRemove)
            RemoveStatusEffect(rm);

        activeEffects.RemoveAll(it => it.bstf._duration <= 0);
        foreach (UnitBattleStatusEffect effect in activeEffects)
        {
            effect.UpdateInformation();

        }
    }

    public List<BattleStatusEffect> GetStatusEffects()
    {
        UpdateStatusEffects();
        List<BattleStatusEffect> list = new List<BattleStatusEffect>();
        foreach (var item in activeEffects)
        {
            list.Add(item.bstf);
        }
        return list;
    }

    private void RemoveStatusEffect(UnitBattleStatusEffect battleStatusEffect)
    {
        battleStatusEffect.transform.parent = null;
        Destroy(battleStatusEffect);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        UnitBattleStatusEffect status = activeEffects.Find(it => it.bstf._status.statusEffect == statusEffect);
        if (status != null)
        {
            status.transform.parent = null;
            activeEffects.Remove(status);
            Destroy(status);
        }
    }

    public void OnTurnEnd()
    {
        foreach(var active in activeEffects)
        {
            active.bstf.OnTurnEnd();
        }
        UpdateStatusEffects();
    }

    public void OnTurnStart()
    {
        foreach (var active in activeEffects)
        {
            active.bstf.OnTurnStart();
        }
        UpdateStatusEffects();
    }

    public int OnDraw(int i)
    {
        int final = i;
        foreach (var effect in activeEffects)
        {
            final = effect.bstf.OnDraw(final);
        }
        UpdateStatusEffects();

        return final;
    }

    public int OnDealDamage(int i)
    {
        int final = i;
        foreach (var effect in activeEffects)
        {
            final = effect.bstf.OnDealDamage(final);
        }
        UpdateStatusEffects();
        return final;
    }

    public int OnGainBlock (int i)
    {
        int final = i;
        foreach (var effect in activeEffects)
        {
            final = effect.bstf.OnGainBlock(final);
        }
        UpdateStatusEffects();
        return final;
    }

    public int OnLoseBlock(int i)
    {
        int final = i;
        foreach (var effect in activeEffects)
        {
            final = effect.bstf.OnLoseBlock(final);
        }
        UpdateStatusEffects();
        return final;
    }
}
