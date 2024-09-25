using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    [SerializeField] BattleUnit _player;
    [SerializeField] List<BattleUnit> _enemies;
    // Start is called before the first frame update

    private BATTLE_STATE _currentState = BATTLE_STATE.PLAYER_TURN;

    [SerializeField] int _mana = 5;

    [Header("Debugging")]
    [SerializeField] public TextMeshProUGUI _battleStateText;
    [SerializeField] public TextMeshProUGUI _manaText;

    int _currentTurn = 0;


    public enum BATTLE_STATE
    {
        PLAYER_TURN,
        ENEMY_TURN,
        BATTLE_OVER
    }


    void Start()
    {
        ChangeBattleState(BATTLE_STATE.PLAYER_TURN);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case BATTLE_STATE.PLAYER_TURN:
                break;
            case BATTLE_STATE.ENEMY_TURN:
                break;
            case BATTLE_STATE.BATTLE_OVER:
                break;
        }
    }

    private void ChangeBattleState(BATTLE_STATE state)
    {
        if (_currentState == BATTLE_STATE.BATTLE_OVER) return;
        OnExit(_currentState);
        _currentState = state;
        OnStart(state);
        switch (_currentState)
        {
            case BATTLE_STATE.PLAYER_TURN:
                _battleStateText.text = "PLAYER TURN";
                break;
            case BATTLE_STATE.ENEMY_TURN:
                _battleStateText.text = "ENEMY TURN";
                break;
            case BATTLE_STATE.BATTLE_OVER:
                _battleStateText.text = "BATTLE END";
                break;
        }
    }

    private void OnExit(BATTLE_STATE state)
    {
        if (state == BATTLE_STATE.PLAYER_TURN)
        {
            ModifyMana(5 - _mana);
        }
        if(state == BATTLE_STATE.ENEMY_TURN)
        {
            _currentTurn++; 
        }
    }

    private void OnStart(BATTLE_STATE state)
    {
        if (state == BATTLE_STATE.ENEMY_TURN)
        {
            CheckIfBattleIsOver();
            RunEnemyActions(); 
        }
    }

    public void EndTurn()
    {
        if (_currentState == BATTLE_STATE.PLAYER_TURN)
        {
            ChangeBattleState(BATTLE_STATE.ENEMY_TURN);
        }
    }

    public void dev_UseHealCard()
    {
        if (_currentState == BATTLE_STATE.PLAYER_TURN)
        {
            if (!TryToUseCard(_player, _enemies, _devHealCard))
            {
                Debug.Log("Failed to use heal card!");
            }
        }
    }

    public void dev_UseShieldCard()
    {
        if (_currentState == BATTLE_STATE.PLAYER_TURN)
        {
            if (!TryToUseCard(_player, new List<BattleUnit>{ _player}, _devShieldCard))
            {
                Debug.Log("Failed to use shield card!");
            }
        }
    }

    public void dev_UseAttackCard()
    {
        if (_currentState == BATTLE_STATE.PLAYER_TURN)
        {
            if (!TryToUseCard(_player, _enemies, _devDamageCard))
            {
                Debug.Log("Failed to use attack card!");
            }
        }
        CheckIfBattleIsOver();
    }

    private void ModifyMana(int value)
    {
        _mana += value;
        _manaText.text = $"{_mana}";
    }

    private void CheckIfBattleIsOver()
    {
        if (_player.GetHPData().IsDead() || EnemiesAlive() == 0)
        {
            ChangeBattleState(BATTLE_STATE.BATTLE_OVER);
        }
    }

    #region ENEMY ACTIONS

    private int EnemiesAlive()
    {
        int enemiesAlive = 0;
        foreach(BattleUnit enemy in _enemies)
        {
            if (!enemy.GetHPData().IsDead())
                enemiesAlive++;
        }
        return enemiesAlive;
    }

    private void RunEnemyActions()
    {
        foreach(BattleUnit enemy in _enemies)
        {
            PerformEnemyBehavior(enemy); 
        }
        /*
         * All enemies acted, can return to a player turn if the player is still alive
         */
        CheckIfBattleIsOver();
        ChangeBattleState(BATTLE_STATE.PLAYER_TURN);
    }

    private void PerformEnemyBehavior(BattleUnit enemy)
    {
        EnemyBehavior behavior = (enemy.GetUnitData() as MonsterData).behavior;
        Card enemyCard = behavior.GetCardUsed(enemy, _currentTurn);
        UseCard(enemy, _player, enemyCard);
    }

    

    #endregion

    #region CARD USE

    [SerializeField] public Card _devDamageCard;
    [SerializeField] public Card _devShieldCard;
    [SerializeField] public Card _devHealCard;


    private bool TryToUseCard(BattleUnit owner, List<BattleUnit> targets, Card card)
    {
        if (!CheckIfCanCast(owner, card)) return false;
        if (targets.Count == 0) return false;

        /**
         * If silenced, etc, add here to prevent use.
         */

        return UseCard(owner, targets[0], card);
    }

    private bool CheckIfCanCast(BattleUnit owner, Card card)
    {
        if (owner.GetUnitData() is PlayableData)
        {
            if(card.manaCost > _mana)
            {
                return false;
            }
        }
        return true;
    }

    private bool UseCard(BattleUnit owner, BattleUnit target, Card card)
    {
        List<CardEffect> cardEffects = card.effects;

        foreach (CardEffect effect in cardEffects)
        {
            CheckIfBattleIsOver();
            if (_currentState == BATTLE_STATE.BATTLE_OVER) return true;
            if (effect.GetTargetting() == CardAttribute.EffectTarget.Self)
            {
                effect.OnUse(effect.GetTargetting(), new List<BattleUnit> { owner });
            }
            else
            {
                effect.OnUse(effect.GetTargetting(), new List<BattleUnit> { target });
            }
        }
        
        if(owner.GetUnitData() is PlayableData)
        {
            ModifyMana(-card.manaCost);
        }

        return true;
    }

    #endregion
}
