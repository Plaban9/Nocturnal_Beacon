using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [Header("Connected Map Nodes")]
    [SerializeField] private GameObject lineRendererPrefab;
    [SerializeField] private List<MapNode> mapNodesList;
    [field:SerializeField] public List<MapNode> ConnectedNodeList { get; set; }

    private bool isConnected;

    public int Depth { get; private set; }
    public int Split { get; private set; }

    private void Awake()
    {
        ConnectedNodeList = new();
    }

    public void AddNode(MapNode newNode)
    {
        mapNodesList.Add(newNode);
    }

    public List<MapNode> GetConnectedNodesList()
    {
        return mapNodesList;
    }

    public void ConnectNode(MapNode node)
    {
        if(ConnectedNodeList.Contains(node)) return;

        ConnectedNodeList.Add(node);

        isConnected = true;
    }

    public void CleanupDisconnectedNodes()
    {
        gameObject.SetActive(isConnected);
    }

    public void SetDataForReconnectingNodes()
    {
        isConnected = false;
        ConnectedNodeList.Clear();
        gameObject.SetActive(true);
    }
}
