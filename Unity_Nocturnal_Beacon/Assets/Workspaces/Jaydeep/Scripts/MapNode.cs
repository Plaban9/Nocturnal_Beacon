using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MapNode : MonoBehaviour
{

    public static event Action<MapNode> OnMapNodeSelected;

    [Header("Node Settings")]
    [SerializeField] private SpriteRenderer lockSprite;
    [SerializeField] private GameObject selectedEffect;

    [Header("Connected Map Nodes")]
    [SerializeField] private List<MapNode> mapNodesList;

    // For Debug
    [field:SerializeField] public List<MapNode> ForwardNodeList { get; set; }
    [field:SerializeField] public List<MapNode> BackwardNodeList { get; set; }

    private bool isConnected;
    private bool isLocked;
    private SpriteRenderer spriteRenderer;
    private Collider2D nodeCollider;

    public int Id { get; private set; }

    /// <summary>
    /// Difficulty/Encounter related
    /// </summary>
    public Action OnClick;
    public int height = -1;

    private void Awake()
    {
        ForwardNodeList = new();
        spriteRenderer = GetComponent<SpriteRenderer>();
        nodeCollider = GetComponent<Collider2D>();
        gameObject.SetActive(false); // This is required
    }

    private void OnMouseDown()
    {
        SetMapNodeSelected();
    }

    public void SetMapNodeSelected()
    {
        OnMapNodeSelected?.Invoke(this);
    }

    public void SetNodeId(int nodeId) { Id = nodeId; }

    public bool IsConnected() { return isConnected; }

    public void SetConnected(bool isConnected)
    {
        this.isConnected = isConnected;
        gameObject.SetActive(isConnected);
    }

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
        //if(ForwardNodeList.Contains(node)) return;

        ForwardNodeList.Add(node);
        node.BackwardNodeList.Add(this);
        SetConnected(true);
    }

    public void ResetNode()
    {
        SetConnected(false);
        ForwardNodeList.Clear();
        gameObject.SetActive(true);
    }

    public void SetAsUnavailableNode()
    {
        var color = spriteRenderer.color;
        color.a = .25f;
        spriteRenderer.color = color;
    }

    public void SetAsAvailableNode()
    {
        var color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }

    public void UnlockNode()
    {
        isLocked = false;
        lockSprite.gameObject.SetActive(isLocked);
        nodeCollider.enabled = !isLocked;

        EnableSelectableEffect();

        SetAsAvailableNode();
    }

    public void EnableSelectableEffect() => selectedEffect.SetActive(true);

    public void DisableSelectableEffect() => selectedEffect.SetActive(false);

    public void LockNode()
    {
        isLocked = true;
        lockSprite.gameObject.SetActive(isLocked);
        nodeCollider.enabled = !isLocked; // If it is locked then disable collider so, click won't work on node

        DisableSelectableEffect();

        SetAsUnavailableNode();
    }
}
