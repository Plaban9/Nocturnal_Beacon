using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{

    public static event Action<MapNode> OnMapNodeSelected;

    [Header("Node Settings")]
    [SerializeField] private SpriteRenderer lockSprite;

    [Header("Connected Map Nodes")]
    [SerializeField] private List<MapNode> mapNodesList;

    // For Debug
    [field:SerializeField] public List<MapNode> ConnectedNodeList { get; set; }

    private bool isConnected;
    private bool isLocked;
    private SpriteRenderer spriteRenderer;

    public int Id { get; private set; }
    public int Depth { get; private set; }
    public int Split { get; private set; }

    /// <summary>
    /// Difficulty/Encounter related
    /// </summary>
    public Action OnClick;
    public int height = -1;

    private void Awake()
    {
        ConnectedNodeList = new();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        SetMapNodeSelected();
    }

    public void SetMapNodeSelected()
    {
        OnMapNodeSelected?.Invoke(this);
        OnClick.Invoke(); 
        NoctBeaconRunData.Instance.SetHeight(height);
        SceneController.Instance.ToBattle();
    }

    public void SetNodeId(int nodeId) { Id = nodeId; }

    public bool IsConnected() { return isConnected; }

    public void SetConnected(bool isConnected) { this.isConnected = isConnected; }

    public void AddNode(MapNode newNode)
    {
        mapNodesList.Add(newNode);
    }

    public List<MapNode> GetNodesList()
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

    public void SetAsUnavailableNode()
    {
        var color = spriteRenderer.color;
        //color.a = .25f;
        spriteRenderer.color = color;
    }

    public void SetAsSelectableNode()
    {
        var color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }

    public void UnlockNode()
    {
        isLocked = false;
        lockSprite.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = true;
    }

    public void LockNode()
    {
        isLocked = true;
        lockSprite.gameObject.SetActive(true);
        GetComponent<Collider2D>().enabled = false;
    }
}
