using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] List<BattleUnit> _enemies;
    // Start is called before the first frame update

    [SerializeField] public List<UnitData> _possibleEnemies;

    [Header("UI")]
    [SerializeField] public TextMeshProUGUI _battleStateText;
    [SerializeField] public TextMeshProUGUI _manaText;

    /*
     * Player Info
     */
    public CardManager _cardManager;
    [SerializeField] BattleUnit _player;

    int _mana;

    /*
     * Battle Info
     */
    int _currentTurn = 0;
    private BATTLE_STATE _currentState = BATTLE_STATE.SETUP;


    public static BattleManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }



    public enum BATTLE_STATE
    {
        SETUP,
        PLAYER_TURN,
        ENEMY_TURN,
        BATTLE_OVER
    }


    void Start()
    {
        _cardManager = GetComponent<CardManager>();
        SetupBattle();

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

    public void SetupBattle()
    {
        _cardManager.SetDeck(NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck());
        _player.SetupPlayerUnit(NoctBeaconRunData.Instance.GetPlayerInformation());
        SetupEnemies();
        _mana = NoctBeaconRunData.Instance.GetPlayerInformation().GetMaxMana();
    }

    public void SetupEnemies()
    {
        int height = NoctBeaconRunData.Instance.GetHeight();
        // Used when we start making multiple floors stuff

        UnitData unit = _possibleEnemies[ (int) Mathf.Floor(UnityEngine.Random.RandomRange(0, _possibleEnemies.Count))];
        _enemies[0].SetupUnit(unit);
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
            ModifyMana(NoctBeaconRunData.Instance.GetPlayerInformation().GetMaxMana() - _mana);
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
        } else if(state == BATTLE_STATE.PLAYER_TURN)
        {
            _cardManager.DrawCard(5 /*TODO: Change it to variable. */);
        }
    }

    public void EndTurn()
    {
        StartCoroutine(PerformEndTurn());
    }

    IEnumerator PerformEndTurn()
    {
        yield return StartCoroutine(_cardManager.DiscardHandZoneCard());

        if (_currentState == BATTLE_STATE.PLAYER_TURN)
        {
            ChangeBattleState(BATTLE_STATE.ENEMY_TURN);
        }
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
            StartCoroutine(BattleEnd());
        }
    }


    IEnumerator BattleEnd()
    {
        yield return new WaitForSeconds(2);
        SceneController.Instance.ToMap(); 
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

    public bool PlayerTryToUseCard( Card card)
    {
        return TryToUseCard(_player, _enemies, card);
    }

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
            if (_currentState == BATTLE_STATE.BATTLE_OVER) return false;
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
        CheckIfBattleIsOver();
        return true;
    }

    #endregion
}
