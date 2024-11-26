using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoctBeaconRunData : MonoBehaviour
{
    [SerializeField] private PlayerUnitData _playerInformation;
    //[SerializeField] private Map _map;
    //[SerializeField] private Node _currentNode;
    private List<string> _selectedNodeList;
    [SerializeField] private int _currentHeight = -1;
    [SerializeField] private int _userGold = 100;
    private EnemyEncounter _currentEncounter = null;

    // Start is called before the first frame update


    public static NoctBeaconRunData Instance { get; private set; }

    private List<NoctBeaconListener> noctBeaconListeners = new List<NoctBeaconListener>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetPlayer(PlayerUnitData playerUnitData)
    {
        _playerInformation = playerUnitData;
        _playerInformation.InitDeck();
    }

    public PlayerUnitData GetPlayerInformation()
    {
        return _playerInformation;
    }

    public bool IsNewGameStarted() => _currentHeight == -1;

    public int GetHeight()
    {
        return _currentHeight;
    }

    public void SetHeight(int height)
    {
        _currentHeight = height;
        foreach (var listener in noctBeaconListeners)
        {
            listener.OnFloorChanged();
        }
    }

        public void GetSelectedNodeList(string nodeId)
    {
        _selectedNodeList.Add(nodeId);
    }

    public List<string> GetNodeId()
    {
        return _selectedNodeList;
    }

   public int GetGold()
    {
        return _userGold;
    }

    public bool ModifyGold(int modification)
    {

        if (_userGold + modification < 0) return false;
        _userGold += modification;
        foreach (var listener in noctBeaconListeners)
        {
            listener.OnGoldChanged();
        }
        return true;
    }

    public void SetHp(int modification)
    {
        _playerInformation.SetCurrentHp(modification);
        foreach (var listener in noctBeaconListeners)
        {
            listener.OnHealthChanged();
        }
    }

    public EnemyEncounter GetCurrentEncounter()
    {
        return _currentEncounter;
    }

    public void SetNextEncounter(EnemyEncounter data)
    {
        _currentEncounter = data;
    }

    public void AddListener(NoctBeaconListener nbl)
    {
        noctBeaconListeners.Add(nbl);
    }

    public void RemoveListener(NoctBeaconListener nbl)
    {
        noctBeaconListeners.Remove(nbl);
    }

    public interface NoctBeaconListener
    {
        public void OnHealthChanged();
        public void OnGoldChanged();
        public void OnFloorChanged();
    }
}