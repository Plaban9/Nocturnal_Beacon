using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Minimalist.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapBuilderTD : MonoBehaviour
{
    public static MapBuilderTD Instance { get; private set; }

    #region Variables

    #region Tower Creation

    // Mapbuilder with top->down approch
    [SerializeField] private MapNode bossNode;
    [SerializeField] private MapNode mapNodePrefab;
    [SerializeField] private Transform selectedNodeEffectTrans;

    [Header("Node Grid")]
    [SerializeField] private int maxSplitsAllowed;
    [SerializeField] private int depthLevel;

    [Header("Node Types")]
    [SerializeField] private List<NodeTypeData> nodeTypesList;

    [Header("Node Spacing")]
    [SerializeField] private float horizontalSpacing = .5f;
    [SerializeField] private float verticalSpacing = 3f;
    [SerializeField] private float incrementalVerticalSpacing = 0f;

    [Header("Textures for Lines")]
    [SerializeField] private Material unavailablePathMaterial;
    [SerializeField] private Material walkablePathMaterial;
    [SerializeField] private Material walkedPathMaterial;

    [Header("Node List")]
    [SerializeField, Range(0f, 1f)] private float disabledLinesAlpha;
    [SerializeField] private List<MapRow> nodes2DList = new List<MapRow>();

    [Header("Scriptable Objects")]
    [SerializeField] private MapNodeListSO mapNodeListSO;

    [Header("Selected Line")]
    [SerializeField] private LineRenderer selectedLine;
    [SerializeField] private MapRow selectedNodeLineList = new();

    [Header("Current Node And Connections")]
    [SerializeField] private MapNode currentSelectedNode;
    //[SerializeField] private List<MapRow> connectedNodesList = new(); // Configured in a way of How each columns are connected (essentially the vertical lines form)

    [Header("Other script ref")]
    [SerializeField] private NodeInfoCanvas nodeInfoCanvas;

    [Header("Gradients")]
    [SerializeField] private List<Gradient> selectedLineGradientList = new List<Gradient>();
    [field: SerializeField] public Gradient CurrentGradient => selectedLine.colorGradient;

    [Header("NonEventNode Management")]
    [SerializeField] private MapNonBattleNodeManager _nonBattleNodeManager;
    private List<MapQuest> _availableEventList = new List<MapQuest>();
    private float _chanceForEvent = 0.15f;

    [Header("Debug Options")]
    [SerializeField] private bool connectFullGrid;

    public float LastRowPos => -(depthLevel * verticalSpacing);

    #endregion

    #region Tower Configuration

    [Header("Tower Confifuration")]

    [Header("Confiruration SO")]
    [SerializeField] private TowerConfiguration towerConfigurationSO;

    #endregion

    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            return;
        }
        Instance = this;

        if (towerConfigurationSO == null)
        {
            Debug.LogError("Tower Configuration Scriptable Object is attached to map builder");
            return;
        }

        depthLevel = towerConfigurationSO.GetMaxHeight() - 1;
    }

    private void Start()
    {
        AudioManager.PlayMusic(Minimalist.Audio.Music.MusicType.Menu, 0f, true);

        _availableEventList.AddRange(towerConfigurationSO.questList);
        _chanceForEvent = towerConfigurationSO.questChance;

        SetSelectedLineParams();

        CreateNodes();

        if (PlayerPrefs.HasKey("Map"))
            LoadNodeList();
        else if(!connectFullGrid)
            ConnectNodes();
        else
            ConnectFullGrid();

        SaveNodeList();
    }

    private void OnEnable()
    {
        MapNode.OnMapNodeSelected += SelectNode;
    }

    private void OnDisable()
    {
        MapNode.OnMapNodeSelected -= SelectNode;
        mapNodeListSO.ResetData();
    }

    private void ConnectFullGrid()
    {
        for (int i = 0; i < nodes2DList.Count; i++)
        {
            for (int j = 0; j < nodes2DList[i].nodesList.Count; j++)
            {
                var curr = nodes2DList[i].nodesList[j];
                var nodeTypeData = nodeTypesList.GetRandom();
                var upperConnectable = curr.GetNodesList();

                curr.SetNodeType(nodeTypeData.nodeSprite, nodeTypeData.nodeType);
                upperConnectable.ForEach(x => curr.ConnectNode(x));
                nodes2DList[^1].nodesList[j].UnlockNode();
            }
        }
    }

    private void SetSelectedLineParams()
    {
        selectedLine.name = "Selected Line";
        selectedLine.material = walkedPathMaterial;
        selectedLine.sortingOrder = 1;

        var gradient = selectedLineGradientList.GetRandom();
        var particle = selectedNodeEffectTrans.GetComponent<ParticleSystem>().main;

        selectedLine.colorGradient = gradient;
        particle.startColor = gradient.colorKeys[^1].color;
    }

    private void CreateNodes()
    {
        var pos = bossNode.transform.position;
        bossNode.SetNodeId(0);
        bossNode.SetHeight(0);
        bossNode.SetAsAvailableNode();
        bossNode.gameObject.SetActive(true);

        if (towerConfigurationSO.TryGetBossEncounter(out var bossEncounter))
        {
            bossNode.SetEnemyEncounter(bossEncounter);
        }

        var spacing = 1 + horizontalSpacing;

        // Rows
        for (int currentDepth = 0; currentDepth < depthLevel; currentDepth++, spacing += horizontalSpacing)
        {
            pos.y -= verticalSpacing + (incrementalVerticalSpacing * currentDepth);
            nodes2DList.Add(new MapRow());
            var center = (maxSplitsAllowed - 1) * (spacing);
            Debug.Log("For Depth " + (spacing + 1) + " the center is " + center + " and start pos is " + (-center / 2));
            pos.x = bossNode.transform.position.x + -center / 2;

            // Column
            for (int currentSplit = 0; currentSplit < maxSplitsAllowed; currentSplit++, pos.x += (spacing))
            {
                // Creating Node
                var node = Instantiate(mapNodePrefab, transform);
                node.name = $"{currentDepth + 1} : {currentSplit + 1}";
                node.transform.position = pos;
                node.SetHeight(currentDepth + 1);
                nodes2DList[currentDepth].nodesList.Add(node);

                int nodeId = int.Parse((currentDepth + 1).ToString() + (currentSplit + 1));
                node.SetNodeId(nodeId);

                // Adding Boss Node to the first row, so all will point to boss node
                if (currentDepth == 0)
                {
                    node.AddNode(bossNode);
                    continue;
                }

                // Add upper left node
                if (currentSplit > 0)
                    node.AddNode(nodes2DList[currentDepth - 1].nodesList[currentSplit - 1]);

                // Add upper middle node
                node.AddNode(nodes2DList[currentDepth - 1].nodesList[currentSplit]);

                // Add upper right node
                if (currentSplit + 1 < maxSplitsAllowed)
                    node.AddNode(nodes2DList[currentDepth - 1].nodesList[currentSplit + 1]);
            }
        }
    }

    private void ConnectNodes()
    {
        //connectedNodesList.Clear();


        for (int splits = 0; splits < maxSplitsAllowed; splits++)
        {
            var availableNodeTypes = new List<NodeTypeData>
            {
                nodeTypesList.Find(x => x.nodeType == NodeType.Combat)
            };
            //connectedNodesList.Add(new());

            // Get a new line to connect all nodes
            var curr = nodes2DList[depthLevel - 1].nodesList[splits];

            for (int depth = depthLevel - 1; depth >= 0; depth--)
            {
                //connectedNodesList[splits].nodesList.Add(curr);

                if (towerConfigurationSO.GetFloorType(depth) == NodeType.Rest)
                    availableNodeTypes.Add(nodeTypesList.Find(x => x.nodeType == NodeType.Rest));
                
                if (towerConfigurationSO.GetFloorType(depth) == NodeType.Shop)
                    availableNodeTypes.Add(nodeTypesList.Find(x => x.nodeType == NodeType.Shop));


                // Set Node Type


                if (Random.Range(0f, 1f) <= _chanceForEvent && _availableEventList.Count!=0)
                {
                    availableNodeTypes.Add(nodeTypesList.Find(x => x.nodeType == NodeType.Qest));
                }

                var nodeTypeData = availableNodeTypes.GetRandom();
                curr.SetNodeType(nodeTypeData.nodeSprite, nodeTypeData.nodeType);


                if (nodeTypeData.nodeType == NodeType.Qest)
                {
                    if (_availableEventList.Count != 0)
                    {
                        MapQuest q = _availableEventList.Pop();
                        curr.SetQuest(q);
                    }
                }


                //Set node Encounter
                if (nodeTypeData.nodeType == NodeType.Combat && depth > 0)
                {
                    if (towerConfigurationSO.TryGetEncounter(depth, out var encounter))
                        curr.SetEnemyEncounter(encounter);
                    else
                        Debug.LogError("Tower Configuration SO can't find enemy encounter for depth: " + depth);
                }

                // Getting Next node to connect
                var upperConnectableNodes = curr.GetNodesList();
                var next = upperConnectableNodes.GetRandom();
                Debug.Log($"{curr} -> {next}");
                curr.ConnectNode(next); // Add selected node to connected list // [Internal list] for each node
                curr = next;
            }

            //connectedNodesList[splits].nodesList.Add(curr);
        }

        //for (int i = 0; i < connectedNodesList.Count; i++)
        for (int i = 0; i < nodes2DList[0].nodesList.Count; i++)
        {
            // Unlocking very bottom nodes
            //connectedNodesList[i].nodesList[0].UnlockNode();
            nodes2DList[^1].nodesList[i].UnlockNode();
        }
    }

    public void Proceed()
    {
        NoctBeaconRunData.Instance.SetNextEncounter(null);
        if (currentSelectedNode == null)
            return;

        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Player_Spawn);
        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Player_Jump);

        var nodeData = new NodeData()
        {
            name = currentSelectedNode.name,
            nodeId = currentSelectedNode.Id,
            isConnected = currentSelectedNode.IsConnected(),
        };
        mapNodeListSO.AddToSelectedLineAndSaveList(nodeData);

        Debug.Log("This is being reached");

        if (NoctBeaconRunData.Instance)
        {
            var height = currentSelectedNode.GetHeight();
            NoctBeaconRunData.Instance.SetHeight(height);

            switch (currentSelectedNode.GetNodeType())
            {
                case NodeType.Combat:
                    NoctBeaconRunData.Instance.SetNextEncounter(
                        currentSelectedNode.EnemyEncounter
                    );
                    break;
                case NodeType.Shop:
                    _nonBattleNodeManager.SetEvent(NonBattleNodeTypes.SHOP);
                    break;
                case NodeType.Rest:
                    _nonBattleNodeManager.SetEvent(NonBattleNodeTypes.REST);
                    break;
                case NodeType.Upgd:
                    _nonBattleNodeManager.SetEvent(NonBattleNodeTypes.UPGD);
                    break;
                case NodeType.Qest:
                    _nonBattleNodeManager.SetEvent(NonBattleNodeTypes.QEST);
                    _nonBattleNodeManager.SetQuest(currentSelectedNode.MapQuest);
                    break;
            }

            if(currentSelectedNode == bossNode)
            {
                NoctBeaconRunData.Instance.SetNextEncounter(
                    currentSelectedNode.EnemyEncounter
                );
            }



            //NoctBeaconRunData.Instance.IsBoss(currentSelectedNode.GetHeight);
        }

        DeselectNode();

        if (currentSelectedNode == bossNode)
        {
            ResetMap();
            return;
        }

        if (SceneController.Instance)
        {
            if (NoctBeaconRunData.Instance.GetCurrentEncounter() != null)
            {
                SceneController.Instance.ToBattle();
            }
            else
            {
                LoadNodeList();
            }
        }
        else
        {
            LoadNodeList();
        }
    }

    private void ResetMap()
    {
        PlayerPrefs.DeleteKey("Map");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DeselectNode()
    {
        SelectNode(null);
    }

    public void SelectNode(MapNode selectedNode)
    {
        var gradient = selectedLineGradientList.GetRandom();
        selectedLine.colorGradient = gradient;
        var particle = selectedNodeEffectTrans.GetComponent<ParticleSystem>().main;
        particle.startColor = gradient.colorKeys[^1].color;

        SetSelectedNode(selectedNode);

        SetBossNodeOutlineColor();

        if (currentSelectedNode)
        {
            currentSelectedNode.SetSelectedEffectColor(gradient);
        }
    }

    private void SetSelectedNode(MapNode node)
    {

        var previousSelectedNode = currentSelectedNode;
        var modifier = node != bossNode ? 2f : 3f;
        Vector3 targetScale = Vector3.one * modifier;

        currentSelectedNode = node;

        if (previousSelectedNode != null)
        {
            if (previousSelectedNode == bossNode)
                previousSelectedNode.transform.DOScale(Vector3.one * 3f, .25f);
            else
                previousSelectedNode.transform.DOScale(Vector3.one, .25f);
        }

        if (currentSelectedNode != null)
        {
            currentSelectedNode.transform.DOScale(targetScale, .25f);
            Debug.Log($"{currentSelectedNode.GetHeight()} is the height for {currentSelectedNode.name}!");
            nodeInfoCanvas.OnNodeInfoRequestedAtNode(currentSelectedNode);
        }
    }

    public MapNode GetCurrentlyLastProceededNode()
    {
        if (selectedNodeLineList.nodesList.Count > 0)
            return selectedNodeLineList.nodesList[^1];
        else
            return null;
    }

    private MapNode GetNodeById(int id)
    {
        if (id == bossNode.Id)
        {
            return bossNode;
        }

        foreach (var mapRow in nodes2DList)
        {
            foreach (var node in mapRow.nodesList)
            {
                if (node.Id == id)
                {
                    return node;
                }
            }
        }

        return null;
    }

    [ContextMenu("Save Map Data")]
    private void SaveNodeList()
    {
        NodeDataList saveNodeList = GetNodeDataList();
        mapNodeListSO.SaveNodeList(saveNodeList);
    }

    private NodeDataList GetNodeDataList()
    {
        var saveNodeList = new NodeDataList();
        foreach (var mapRow in nodes2DList)
        {
            foreach (var node in mapRow.nodesList)
            {
                var nodeData = new NodeData()
                {
                    name = node.name,
                    nodeId = node.Id,
                    nodeType = node.GetNodeType(),
                    isConnected = node.IsConnected(),
                };

                node.UpwardNodeList.ForEach(x => nodeData.connectedNodes.Add(x.Id));
                saveNodeList.nodeDataList.Add(nodeData);
            }
        }

        return saveNodeList;
    }

    [ContextMenu("Load Map Data")]
    private void LoadNodeList()
    {
        mapNodeListSO.RetriveNodeListData();
        var loadedNodeList = mapNodeListSO.MapNodeList;

        // Load node data into map nodes
        foreach (var nodeData in loadedNodeList.nodeDataList)
        {
            var node = GetNodeById(nodeData.nodeId);
            node.ResetNode();
            node.LockNode();
            node.SetConnected(nodeData.isConnected);

            var typeData = nodeTypesList.Find(x => x.nodeType == nodeData.nodeType);
            node.SetNodeType(typeData.nodeSprite, typeData.nodeType);

            foreach (var connectedNodeId in nodeData.connectedNodes)
            {
                MapNode nextNode = GetNodeById(connectedNodeId);
                node.ConnectNode(nextNode);
            }
        }

        ReconnectNodeLines();

        SetSelectedLine();

        UnlockNodes();

        void UnlockNodes()
        {
            if (selectedNodeLineList.nodesList.Count == 0)
            {
                //for (int i = 0; i < connectedNodesList.Count; i++)
                //{
                //    // Unlocking very bottom nodes
                //    connectedNodesList[i].nodesList[0].UnlockNode();
                //}

                for (int i = 0; i < nodes2DList[0].nodesList.Count; i++)
                {
                    // Unlocking very bottom nodes
                    nodes2DList[^1].nodesList[i].UnlockNode();
                }
            }
            else
            {
                var lastSelectedNode = selectedNodeLineList.nodesList[^1];
                //SetSelectedNode(lastSelectedNode);
                lastSelectedNode.UnlockNode();
                lastSelectedNode.DisableSelectableEffect();
                lastSelectedNode.MakeUnclickable();
                lastSelectedNode.EnableConnectedLines();
                lastSelectedNode.UpwardNodeList.ForEach(nextNode =>
                {
                    nextNode.UnlockNode();
                    if (nextNode == bossNode)
                    {
                        EnableBossSpriteEffect();
                    }
                });

                selectedNodeEffectTrans.SetParent(lastSelectedNode.transform);
                selectedNodeEffectTrans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                selectedNodeEffectTrans.localScale = Vector3.one;
            }
        }

        void SetSelectedLine()
        {
            int index = 0;
            selectedNodeLineList.nodesList.Clear();
            selectedLine.positionCount = mapNodeListSO.SelectedLine.nodeDataList.Count;
            foreach (var nodeData in mapNodeListSO.SelectedLine.nodeDataList)
            {
                var node = GetNodeById(nodeData.nodeId);
                node.SetAsAvailableNode();
                selectedNodeLineList.nodesList.Add(node);
                selectedLine.SetPosition(index, node.transform.position);
                index++;
            }
        }

        void ReconnectNodeLines()
        {
            int depth = 1;

            foreach (var rows in nodes2DList)
            {
                foreach (var node in rows.nodesList)
                {
                    if (node.IsConnected() && node.GetNodeType() == NodeType.Combat)
                    {
                        if (towerConfigurationSO.TryGetEncounter(depth, out var encounter))
                            node.SetEnemyEncounter(encounter);
                        else
                            Debug.LogError("Tower Configuration SO can't find enemy encounter for depth: " + depth);
                    }
                }

                depth++;
            }
        }
    }

    private void EnableBossSpriteEffect()
    {
        var spriteRenderer = bossNode.GetComponent<SpriteRenderer>();
        var mat = spriteRenderer.material;
        mat.SetInt("_IsEnabled", 1);

        SetBossNodeOutlineColor();
    }

    private void SetBossNodeOutlineColor()
    {
        var spriteRenderer = bossNode.GetComponent<SpriteRenderer>();
        var mat = spriteRenderer.material;
        
        var color1 = selectedLine.colorGradient.colorKeys[0].color * 25f;
        var color2 = selectedLine.colorGradient.colorKeys[^1].color * 25f;

        mat.SetColor("_OutlineColor", color1);
        mat.SetColor("_OutlineColor2", color2);
    }
}

public enum NodeType
{
    Combat = 0,
    Shop,
    Rest,
    Upgd,
    Qest
}

[Serializable]
class NodeTypeData
{
    public NodeType nodeType;
    public Sprite nodeSprite;
}

[Serializable]
public class MapRow
{
    public List<MapNode> nodesList = new();
}

[Serializable]
public class NodeData
{
    public string name; // For visualizing in list
    public int nodeId;
    public bool isConnected;
    public NodeType nodeType;
    public List<int> connectedNodes = new();
}

[Serializable]
public class NodeDataList
{
    public List<NodeData> nodeDataList = new();
}