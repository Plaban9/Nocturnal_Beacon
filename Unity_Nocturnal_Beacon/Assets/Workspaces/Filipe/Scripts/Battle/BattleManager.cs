using DG.Tweening;
using Minimalist.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class BattleManager : MonoBehaviour
{

    [Header("Enemies")]
    [SerializeField] Transform _enemyHolder;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] public EnemyEncounter _encounter;
    List<BattleUnit> _enemies = new List<BattleUnit>();

    [Header("UI")]
    [SerializeField] public TextMeshProUGUI _battleStateText;
    [SerializeField] public TextMeshProUGUI _manaText;

    [SerializeField] public GameObject _endScreenCanvas;
    [SerializeField] public TextMeshProUGUI _scoreText;

    [SerializeField] public GameObject _noTargetReticule;

    /*
     * Player Info
     */
    public CardManager _cardManager;
    [SerializeField] BattleUnit _player;

    int _mana;
    int _maxMana;

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
        AudioManager.PlayMusic(Minimalist.Audio.Music.MusicType.Menu, 0.5f, true);
        AudioManager.SetMusicVolume(1f);

        _cardManager = GetComponent<CardManager>();
        var encounter = NoctBeaconRunData.Instance.GetCurrentEncounter();
        if(encounter != null) 
            _encounter = encounter;
        SetupBattle();

    }

    public void StartBattle()
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

    public void SetupBattle()
    {
        _cardManager.SetDeck(NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck());
        _player.SetupPlayerUnit(NoctBeaconRunData.Instance.GetPlayerInformation());
        SetupEnemies();
        _maxMana = NoctBeaconRunData.Instance.GetPlayerInformation().GetMaxMana();
        _mana = _maxMana;
    }

    public void SetupEnemies()
    {
        //int height = NoctBeaconRunData.Instance.GetHeight();
        //// Used when we start making multiple floors stuff

        List<MonsterData> enemiesData = _encounter.enemies;

        for(int i = 0; i < enemiesData.Count; i++)
        {
            GameObject enemyFrame = Instantiate(_enemyPrefab,
                _enemyHolder);
            enemyFrame.transform.position = new Vector3(
                    _encounter.GetX(i),
                    2.5f,
                    _encounter.GetZ(i));
            BattleUnit newUnit = enemyFrame.GetComponent<BattleUnit>();
            _enemies.Add(newUnit);
            newUnit.SetupUnit(enemiesData[i]);
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
            ModifyMana(NoctBeaconRunData.Instance.GetPlayerInformation().GetMaxMana() - _mana);
            _player.GetUnitStatusData().OnTurnEnd();
            if (_currentTurn != 0)
            {
                foreach (BattleUnit enemy in _enemies)
                {
                    enemy.GetHPData().EndTurnFlushShield();
                }
            }

        }
        if (state == BATTLE_STATE.ENEMY_TURN)
        {
            _currentTurn++;
            _player.GetHPData().EndTurnFlushShield();
            foreach(BattleUnit enemy in _enemies)
            {
                enemy.GetUnitStatusData().OnTurnEnd();
            }

        }
    }

    private void OnStart(BATTLE_STATE state)
    {
        if (state == BATTLE_STATE.ENEMY_TURN)
        {
            foreach (BattleUnit enemy in _enemies)
            {
                enemy.GetUnitStatusData().OnTurnStart();
            }
            CheckIfBattleIsOver();
            StartCoroutine(RunEnemyActions());
        }
        else if(state == BATTLE_STATE.PLAYER_TURN)
        {
            _player.GetUnitStatusData().OnTurnStart();
            SetupEnemiesIntent();

            int defaultDrawAmount = 5;
            int a = _player.GetUnitStatusData().OnDraw(defaultDrawAmount);

            _cardManager.DrawCard(a,_player.GetUnitStatusData());
        }
    }

    public void EndTurn()
    {
        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Transition_Clap);

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


    public void ModifyMana(int value)
    {
        var newMana = _mana + value;
        var _currentMana = _mana;
        if (newMana < 0) { newMana = 0; }
        _mana = newMana;
        _manaText.GetComponent<Animator>().Play("ManaJump");
        DOTween.To(() => _currentMana,
            x => _currentMana = x, newMana, 0.5f).OnUpdate(() =>
            {
                _manaText.text = $"{_currentMana}/{_maxMana}";
            }
        );
    }

    public int GetMana()
    {
        return _mana;
    }

    private void CheckIfBattleIsOver()
    {
        if (_player.IsDead() || EnemiesAlive() == 0)
        {
            ChangeBattleState(BATTLE_STATE.BATTLE_OVER);
            BattleEnd();
        }
    }

    private void BattleEnd()
    {
        PlayerUnitData data = NoctBeaconRunData.Instance.GetPlayerInformation();
        data.SetCurrentHp(_player.GetHPData().GetCurrentHP());
        NoctBeaconRunData.Instance.ModifyGold(15);
        _endScreenCanvas.SetActive(true);
        Animator anim = _endScreenCanvas.GetComponent<Animator>();
        if (_player.IsDead())
            anim.Play("BattleEndScreenLose");
        else
        {
            if (NoctBeaconRunData.Instance.GetHeight() == 0)
            {
                AudioManager.PlayMusic(Minimalist.Audio.Music.MusicType.Gameplay, 0.5f, true);
                float healthPct = 1.0f + ((float)_player.GetHPData().GetCurrentHP()) / ((float)_player.GetUnitData().maxHp);
                float goldAmassed = NoctBeaconRunData.Instance.GetGold();
                float bonusRare = 1.0f + NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck().Export().FindAll(it => it.rarity == CardAttribute.Rarity.Rare).Count * 0.1f;
                float bonusLeg = 1.0f + NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck().Export().FindAll(it => it.rarity == CardAttribute.Rarity.Legendary).Count * 0.2f;
                anim.Play("BattleGameEndScreen");
                _scoreText.text = String.Format(
                    "Gold Amassed: {0}\n" +
                    "Health Bonus: {1}\n" +
                    "Card Bonus: Rare Cards: {2}, Legendary Cards: {3}\n" +
                    "<size=80><b>FINAL SCORE: {4}</b></size>",
                    goldAmassed,
                    healthPct,
                    bonusRare,
                    bonusLeg,
                    100f + goldAmassed * healthPct * bonusRare * bonusLeg
                    );
            }
            else
            {
                anim.Play("BattleEndScreenWin");
            }

        }
    }

    public void ToMap()
    {
        SceneController.Instance.ToMap();
    }

    public void ToMain()
    {
        SceneController.Instance.ToMain();
    }
    #region ENEMY ACTIONS

    private void SetupEnemiesIntent()
    {
        int index = 1;
        foreach(BattleUnit enemy in _enemies)
        {
            if (!enemy.IsDead())
            {
                enemy.ShowIntent(0);
                EnemyBehavior behavior = (enemy.GetUnitData() as MonsterData).behavior;
                List<Card> enemyCard = behavior.GetCardsUsed(enemy, _currentTurn);
                for(int i = 0; i < enemyCard.Count; i++)
                {
                    enemy.SetNextTurnIntent(i, enemyCard[i], index);

                }
                index += 1;
            }
        }
    }

    private int EnemiesAlive()
    {
        int enemiesAlive = 0;
        foreach(BattleUnit enemy in _enemies)
        {
            if (!enemy.IsDead())
                enemiesAlive++;
        }
        return enemiesAlive;
    }

    private IEnumerator RunEnemyActions()
    {
        yield return new WaitForSeconds(0.5f);
        foreach(BattleUnit enemy in _enemies)
        {
            if (!enemy.IsDead())
            {
                List<Card> cards = (enemy.GetUnitData() as MonsterData).behavior.GetCardsUsed(enemy, _currentTurn);
                for(int i = 0; i < cards.Count; i++) {
                    enemy.HighlightIntent(i);
                    yield return new WaitForSeconds(0.4f);
                    PlayAnimation(enemy, cards[i]);
                    yield return new WaitForSeconds(0.1f);
                    PerformEnemyBehavior(enemy, cards[i]);
                    yield return new WaitForSeconds(0.3f);
                    enemy.HideIntent(0);
                }
            }
        }
        /*
         * All enemies acted, can return to a player turn if the player is still alive
         */
        CheckIfBattleIsOver();
        ChangeBattleState(BATTLE_STATE.PLAYER_TURN);
    }

    private void PlayAnimation(BattleUnit unit, Card card)
    {
        if (card.cardType == CardAttribute.CardType.Skill)
        {
            unit.PlaySkillAnimation();
        }
        else
        {
            unit.PlayAttackAnimation();
        }
    }

    private void PerformEnemyBehavior(BattleUnit enemy, Card card)
    {
        UseCard(enemy, _player, card);
    }

    

    #endregion

    #region CARD USE

    public bool PlayerTryToUseCard( Card card, BattleUnit target)
    {
        return TryToUseCard(_player, target, card);
    }

    private bool TryToUseCard(BattleUnit owner, BattleUnit target, Card card)
    {
        if (!CheckIfCanCast(owner, card)) return false;

        /**
         * If silenced, etc, add here to prevent use.
         */

        return UseCard(owner, target, card);
    }

    private bool CheckIfCanCast(BattleUnit owner, Card card)
    {
        if (owner.GetUnitData() is PlayableData)
        {
            if(card.GetManaCost() > _mana)
            {
                _manaText.GetComponent<Animator>().Play("ManaBad");
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
            bool effectResult = RunEffect(owner, target, card, effect);
            if (!effectResult) return false;
        }

        if (owner.GetUnitData() is PlayableData)
        {
            ModifyMana(-card.GetManaCost());
        }

        SetupEnemiesIntent();
        CheckIfBattleIsOver();
        PlayAnimation(owner, card);
        return true;
    }

    public bool RunEffect(BattleUnit owner, BattleUnit target, Card card, CardEffect effect)
    {
        if (_currentState == BATTLE_STATE.BATTLE_OVER) return false;
        CheckIfBattleIsOver();
        if (effect.GetTarget() == CardAttribute.EffectTarget.Self)
        {
            effect.OnUse(card, owner, new List<BattleUnit> { owner });
        }
        else
        {
            switch (effect.GetTarget())
            {
                case CardAttribute.EffectTarget.OpponentSingle:
                    if (target == null) return false;
                    effect.OnUse(card, owner, new List<BattleUnit> { target });
                    break;
                case CardAttribute.EffectTarget.OpponentAll:
                    effect.OnUse(card, owner, _enemies);
                    break;
                case CardAttribute.EffectTarget.OpponentRandom:
                    List<BattleUnit> viableUnits = _enemies.FindAll(it => !it.IsDead());
                    BattleUnit chosen = viableUnits.GetRandom();
                    effect.OnUse(card, owner, _enemies);
                    break;
                case CardAttribute.EffectTarget.Both:
                    effect.OnUse(card, owner, new List<BattleUnit> { target });
                    effect.OnUse(card, owner, new List<BattleUnit> { owner });
                    break;
                case CardAttribute.EffectTarget.Global:
                    List<BattleUnit> enemiesPlusPlayer = _enemies;
                    enemiesPlusPlayer.Add(owner);
                    effect.OnUse(card, owner, enemiesPlusPlayer);
                    break;
            }
        }
        CheckIfBattleIsOver();
        return true;
    }

    #endregion

    public BattleUnit GetPlayerbattleUnit()
    {
        return _player;
    }

    #region Targetting

    public void SetNoTargetReticule(bool enabled)
    {
        _noTargetReticule.SetActive(enabled);
    }

    public void OutlinePlayer()
    {
        _player.Outline();
    }

    public void ShowEffectivenessPlayer(Card card)
    {
        _player.ShowEffectivity(card);
    }

    public void HideEffectivenessPlayer()
    {
        _player.HideEffectivity();
    }

    public void HideOutlinePlayer()
    {
        _player.HideOutline();
    }

    public void OutlineEnemies()
    {
        foreach(BattleUnit enemy in _enemies)
        {
            if (!enemy.IsDead())
                enemy.Outline();
        }
    }

    public void ShowEffectivityEnemies(Card card)
    {
        foreach (BattleUnit enemy in _enemies)
        {
            if (!enemy.IsDead())
                enemy.ShowEffectivity(card);
        }
    }

    public void HideOutlineEnemies()
    {
        foreach (BattleUnit enemy in _enemies)
        {
            if (!enemy.IsDead())
                enemy.HideOutline();
        }
    }

    public void HideEffectivityEnemies()
    {
        foreach (BattleUnit enemy in _enemies)
        {
            if (!enemy.IsDead())
                enemy.HideEffectivity();
        }
    }
    #endregion

}
