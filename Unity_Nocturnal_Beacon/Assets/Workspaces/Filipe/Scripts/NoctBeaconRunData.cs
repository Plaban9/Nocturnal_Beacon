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


    // Start is called before the first frame update


    public static NoctBeaconRunData Instance { get; private set; }
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

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetPlayer(PlayerUnitData playerUnitData)
    {
        _playerInformation = playerUnitData;
    }

    public PlayerUnitData GetPlayerInformation()
    {
        return _playerInformation;
    }

    public int GetHeight()
    {
        return _currentHeight;
    }

    public void SetHeight(int height)
    {
        _currentHeight = height;
    }

    public void GetSelectedNodeList(string nodeId)
    {
        _selectedNodeList.Add(nodeId);
    }

    public List<string> GetNodeId()
    {
        return _selectedNodeList;
    }

   

    

    

}