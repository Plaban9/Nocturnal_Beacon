using CardAttribute;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class IfTargetFitsCriteria: CardEffect
{
    [SerializeReference, SubclassSelector]
    public CardEffect UseEffectBasedOnThis = new CardEffect();
    [SerializeReference, SubclassSelector]
    public ITargetCriteriaStrategy TargetCriteriaStrategy;


    public override string LocalizationKey => "CE_DESC_WithManaDoEffect";
    public override string EffectDescription
    {
        get
        {
            
            return string.Format("{0} {1} : {2}",
                target switch
                {
                    EffectTarget.Self => "On user",
                    EffectTarget.OpponentSingle => "On target target",
                    EffectTarget.OpponentRandom => "On randomly chosen target",
                    EffectTarget.OpponentAll => "On any target",
                    EffectTarget.Both => "On user and target",
                    EffectTarget.Global => "On any unit"
                },
                TargetCriteriaStrategy.GetDescription(),
                UseEffectBasedOnThis.EffectDescription);
        }
    }
    public IfTargetFitsCriteria() { }


    public IfTargetFitsCriteria(EffectTarget target, int val1, int val2)
    {
        this.target = target;
        this.val1 = val1;
        this.val2 = val2;
    }


    public override int GetEffectCost()
    {
        int result;
        float multiplier = 1;

        if (val1 > 0)
        {
            int baseVal = 20;
            int scaling = (int) Mathf.Pow(4, 1 + val1);
            result = val1 > 0 ? (int)baseVal + scaling : 0;
            return result;
        }
        else {
            int baseVal = 5;
            int scaling = 3 * val1;
            result = val1 != 0 ? (int)baseVal + scaling : 0;
            return result;
        }
    }


    override public void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {

        foreach (BattleUnit target in targets)
        {
            Debug.Log("aie!");
            if (TargetCriteriaStrategy.GetsIfFitsCriteria(target))
            {
                BattleManager.Instance.RunEffect(owner, target, card, UseEffectBasedOnThis);
            }
        }

        
    }
}


public interface ITargetCriteriaStrategy
{

    public bool GetsIfFitsCriteria(BattleUnit unit);
    public string GetDescription();
}

[Serializable]
public class IfIsElement : ITargetCriteriaStrategy
{
    [SerializeField] ComparisonEnum comparison = ComparisonEnum.IS;
    [SerializeField] Element chosenElement = Element.NONE;
    public bool GetsIfFitsCriteria(BattleUnit unit)
    {
        return comparison == ComparisonEnum.IS ? unit.GetElement() == chosenElement : unit.GetElement() != chosenElement;
    }

    public string GetDescription()
    {
        return string.Format("whose element {0} {1}",
            comparison == ComparisonEnum.IS ? "is" : "is not",
            chosenElement.ToString());
    }
}


[Serializable]
public class HealthPercent : ITargetCriteriaStrategy
{
    [SerializeField] ValueComparator comparator = ValueComparator.LessThan;
    [SerializeField][Range(0f,1f)] float percent = 0.5f;
    public bool GetsIfFitsCriteria(BattleUnit unit)
    {
        float currHpPercent = ((float)unit.GetHPData().GetCurrentHP() / (float) unit.GetUnitData().maxHp);
        switch (comparator)
        {
            case ValueComparator.GreaterThan:
                return percent > currHpPercent ? true : false;
            case ValueComparator.LessThan:
                return percent < currHpPercent ? true :  false;
            case ValueComparator.EqualTo:
                return percent == currHpPercent ? true : false;
            case ValueComparator.NotEqualTo:
                return percent != currHpPercent ? true : false;
            case ValueComparator.GreaterThanOrEqualTo:
                return percent >= currHpPercent ? true : false;
            case ValueComparator.LessThanOrEqualTo:
                return percent <= currHpPercent ? true : false;
        }


        return false;
    }

    public string GetDescription()
    {
        return string.Format("whose health is {0} {1}", comparator switch
        {
            ValueComparator.GreaterThan => "greater than",
            ValueComparator.LessThan => "less than",
            ValueComparator.EqualTo => "equal to", // ... really
            ValueComparator.NotEqualTo => "not equal to", // ....
            ValueComparator.GreaterThanOrEqualTo => "greater than or equal to",
            ValueComparator.LessThanOrEqualTo => "less than or equal to"
        }, percent * 100f + "%");
    }
}
