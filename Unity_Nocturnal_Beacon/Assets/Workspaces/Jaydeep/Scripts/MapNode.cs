using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public static event Action<MapNode> OnMapNodeSelected;

    [Header("Node Settings")]
    [SerializeField] private NodeType nodeType;
    [SerializeField] private SpriteRenderer lockSprite;
    [SerializeField] private SpriteRenderer typeSprite;
    [SerializeField] private GameObject selectedEffect;
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private float disabledAlphaValue = .25f;

    [Header("Connected Map Nodes")]
    [SerializeField] private List<MapNode> mapNodesList;
    [SerializeField] private List<LineRenderer> connectedLines;

    // For Debug
    [field:SerializeField] public List<MapNode> UpwardNodeList { get; set; }
    [field:SerializeField] public List<MapNode> DownwardNodeList { get; set; }

    private bool isConnected;
    private bool isLocked;
    private SpriteRenderer spriteRenderer;
    private Collider2D nodeCollider;
    private Gradient defaultColorGradient;

    public int Id { get; private set; }

    private int height = -1;

    private void Awake()
    {
        UpwardNodeList = new();
        DownwardNodeList = new();
        spriteRenderer = GetComponent<SpriteRenderer>();
        nodeCollider = GetComponent<Collider2D>();
        defaultColorGradient = linePrefab.colorGradient;
        DisableSelectableEffect();
        SetAsUnavailableNode();
        gameObject.SetActive(false); // This is required as we are enabling only the connected nodes later
    }

    private void OnMouseDown()
    {
        SetMapNodeSelected();
    }

    public int GetHeight() => height;

    public void SetHeight(int height)
    {
        this.height = height;
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

    public void SetSelectedEffectColor(Gradient gradient)
    {
        var particleColor = selectedEffect.GetComponent<Renderer>().material;
        particleColor.SetColor("_EmissionColor", gradient.colorKeys[0].color * 5f);
    }

    public void ConnectNode(MapNode node)
    {
        if (UpwardNodeList.Contains(node))
            return;

        UpwardNodeList.Add(node);
        node.DownwardNodeList.Add(this);
        SetConnected(true);

        var line = Instantiate(linePrefab, transform);
        line.positionCount = 2;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, node.transform.position);
        connectedLines.Add(line);
    }

    public void ResetNode()
    {
        SetConnected(false);
        UpwardNodeList.Clear();
        gameObject.SetActive(true);
    }

    public void SetAsUnavailableNode()
    {
        //var color = spriteRenderer.color;
        //color.a = disabledAlphaValue;
        //spriteRenderer.color = color;

        spriteRenderer.color = Color.grey;

        DisableConnectedLines();
    }

    public void SetAsAvailableNode()
    {
        //var color = spriteRenderer.color;
        //color.a = 1f;
        //spriteRenderer.color = color;

        spriteRenderer.color = Color.white;
    }

    public void EnableConnectedLines()
    {
        foreach (var line in connectedLines)
        {
            var gradient = new Gradient()
            {
                alphaKeys = new GradientAlphaKey[]
                {
                    new() { alpha = 1, time = 0 },
                    new() { alpha = 1, time = 1 },
                },
                colorKeys = new GradientColorKey[]
                {
                    new() { color = Color.white, time = 0 },
                    new() { color = Color.white, time = 1 },
                },
            };

            line.colorGradient = gradient;
            Debug.Log("Node: " + transform.name + "  Line:" + line.name);
        }
    }

    public void DisableConnectedLines()
    {
        foreach (var line in connectedLines)
        {
            line.colorGradient = defaultColorGradient;
            Debug.Log("Node: " + transform.name + "  Line:" + line.name);
        }
    }

    public void UnlockNode()
    {
        isLocked = false;
        //lockSprite.gameObject.SetActive(isLocked);
        MakeClickable();

        EnableSelectableEffect();

        SetAsAvailableNode();
    }

    public void LockNode()
    {
        isLocked = true;
        //lockSprite.gameObject.SetActive(isLocked);
        MakeUnclickable(); // If it is locked then disable collider so, click won't work on node

        DisableSelectableEffect();

        SetAsUnavailableNode();
    }

    public void SetNodeType(Sprite type, NodeType nodeType)
    {
        typeSprite.sprite = type;
        this.nodeType = nodeType;
    }

    public void MakeClickable() => nodeCollider.enabled = true;

    public void MakeUnclickable() => nodeCollider.enabled = false;

    public void EnableSelectableEffect() => selectedEffect.SetActive(true);

    public void DisableSelectableEffect() => selectedEffect.SetActive(false);

    public NodeType GetNodeType() => nodeType;

    public int GetNodeTypeInt() => (int)nodeType;
}
