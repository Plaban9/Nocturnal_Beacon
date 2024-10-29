using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapBuilderTD : MonoBehaviour
{
    // Mapbuilder with top->down approch
    [SerializeField] private MapNode bossNode;
    [SerializeField] private MapNode mapNodePrefab;
    [SerializeField] private LineRenderer lineRendererPrefab;
    [SerializeField] private Transform selectedNodeEffectTrans;

    [Header("Node Grid")]
    [SerializeField] private int maxSplitsAllowed;
    [SerializeField] private int depthLevel;

    [Header("Node Spacing")]
    [SerializeField] private float horizontalSpacing = 3f;
    [SerializeField] private float verticalSpacing = 3f;

    [Header("Textures for Lines")]
    [SerializeField] private Material unavailablePathMaterial;
    [SerializeField] private Material walkablePathMaterial;
    [SerializeField] private Material walkedPathMaterial;

    [Header("Node List")]
    [SerializeField, Range(0f, 1f)] private float disabledLinesAlpha;
    [SerializeField] private List<MapRow> nodes2DList = new List<MapRow>();

    [Header("Scriptable Objects")]
    [SerializeField] private MapNodeListSO mapNodeListSO;

    private List<LineRenderer> linesList = new();
    private LineRenderer selectedLine;
    [SerializeField] private MapRow selectedLineList = new();

    [SerializeField] private MapNode currentSelectedNode;
    
    [SerializeField] private List<MapRow> connectedNodesList = new();

    [Header("Gradients")]
    [SerializeField] private List<Gradient> _selectedPathGradient = new List<Gradient>();

    private void Start()
    {
        GetDisabledAlphaGradient();

        CreateNodes();

        ConnectNodes();

        selectedLineList.nodesList = new();
        nodes2DList.ForEach(list => list.nodesList.Clear());
        nodes2DList.Clear();

        SetSelectedLineParams();
    }

    private void OnEnable()
    {
        MapNode.OnMapNodeSelected += SelectNode;
    }

    private void OnDisable()
    {
        MapNode.OnMapNodeSelected -= SelectNode;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Proceed();
        }
    }

    private void SetSelectedLineParams()
    {
        selectedLine = Instantiate(lineRendererPrefab, bossNode.transform);
        selectedLine.name = "Selected Line";
        selectedLine.material = walkedPathMaterial;
        selectedLine.startWidth = selectedLine.endWidth = .1f;
        selectedLine.sortingOrder = 1;

        if (NoctBeaconRunData.Instance.GetHeight() != -1)
        {
            LoadNodeList();

        }




    }

    private Gradient GetDisabledAlphaGradient()
    {
        var disabledLineGradient = new Gradient()
        {
            alphaKeys = new GradientAlphaKey[2]
                    {
                new(disabledLinesAlpha, 0),
                new(disabledLinesAlpha, 1)
                    },
            colorKeys = new GradientColorKey[2]
                    {
                new (Color.white, 0),
                new (Color.white, 1)
                    }
        };
        return disabledLineGradient;
    }

    private void SetSelectedNode(MapNode node)
    {
        var previousSelectedNode = currentSelectedNode;
        var modifier = node != bossNode ? 1.5f : 3f;
        Vector3 targetScale = Vector3.one * modifier;

        currentSelectedNode = node;
        selectedNodeEffectTrans.SetParent(node.transform);
        selectedNodeEffectTrans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        selectedNodeEffectTrans.localScale = targetScale;

        if (previousSelectedNode != null)
            previousSelectedNode.transform.DOScale(Vector3.one, .25f);

        currentSelectedNode.transform.DOScale(targetScale, .25f);
    }

    private void CreateNodes()
    {
        var pos = bossNode.transform.position;
        bossNode.SetNodeId(0);

        // Columns
        for (int currentDepth = 0; currentDepth < depthLevel; currentDepth++)
        {
            pos.y -= verticalSpacing;
            pos.x = bossNode.transform.position.x + (maxSplitsAllowed / 2 * -horizontalSpacing);
            nodes2DList.Add(new MapRow());

            // Rows
            for (int currentSplit = 0; currentSplit < maxSplitsAllowed; currentSplit++, pos.x += horizontalSpacing)
            {
                // Creating Node
                var node = Instantiate(mapNodePrefab, transform);
                node.name = $"{currentDepth + 1} : {currentSplit + 1}";
                node.transform.position = pos;
                node.height = depthLevel - currentDepth;
                node.OnClick = () => { SaveNodeList(); };
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
        connectedNodesList.Clear();
        linesList.Clear();

        for (int i = 0; i < maxSplitsAllowed; i++)
        {
            connectedNodesList.Add(new());

            // Get a new line to connect all nodes
            int depth = depthLevel - 1;
            var curr = nodes2DList[depth].nodesList[i];
            var line = Instantiate(lineRendererPrefab, bossNode.transform);
            line.name = "Line " + (i + 1);
            line.colorGradient = GetDisabledAlphaGradient();
            line.positionCount = depthLevel + 1;
            line.SetPosition(0, bossNode.transform.position);
            linesList.Add(line);

            curr.UnlockNode();

            while (curr != null)
            {
                line.SetPosition(depth + 1, curr.transform.position);
                connectedNodesList[i].nodesList.Add(curr);
                if (depth < 0) break;

                var upperConnectableNodes = curr.GetNodesList();
                int luckyIndex = Random.Range(0, upperConnectableNodes.Count); // Get random node from list to connect
                var next = upperConnectableNodes[luckyIndex].GetComponent<MapNode>();
                //Debug.Log("Next is " + next.name);
                curr.ConnectNode(next); // Add selected node to connected list // [Internal list] for each node
                curr = next;

                depth--;
            }
        }

        nodes2DList.ForEach(x => x.nodesList.ForEach(y => y.CleanupDisconnectedNodes()));
        
    }

    private void Proceed()
    {
        // Lock all nodes
        foreach (var lines in connectedNodesList)
        {
            foreach (var node in lines.nodesList)
            {
                node.LockNode();
            }
        }

        // Unlock Nodes Which we have taken
        currentSelectedNode.UnlockNode();
        currentSelectedNode.ConnectedNodeList.ForEach(x => x.UnlockNode());

        if (!selectedLineList.nodesList.Contains(currentSelectedNode))
            selectedLineList.nodesList.Add(currentSelectedNode);

        var count = selectedLineList.nodesList.Count;
        selectedLine.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            MapNode node = selectedLineList.nodesList[i];
            selectedLine.SetPosition(i, node.transform.position);
        }
    }

    public void DeselectNode()
    {
        SelectNode(null);
    }

    public void SelectNode(MapNode selectedNode)
    {
        // Reset Line color
        linesList.ForEach(x => x.colorGradient = GetDisabledAlphaGradient());

        SetSelectedNode(selectedNode);

        int selectedNodeDepth = 0;
        List<MapRow> selectableLinesList = new();
        var gradient = _selectedPathGradient[Random.Range(0, _selectedPathGradient.Count)];
        selectedLine.colorGradient = gradient;

        // Colorize the selectable lines
        for (int i = 0; i < connectedNodesList.Count; i++)
        {
            MapRow row = connectedNodesList[i];
            if (row.nodesList.Exists(node => node == selectedNode))
            {
                var line = linesList[i];
                line.colorGradient = gradient;
                selectableLinesList.Add(row);

                // Selected Node's Effect based on gradient
                var renderer = selectedNodeEffectTrans.GetComponent<Renderer>();
                var selectedCircleColor = gradient.Evaluate(1 - ((float)i / depthLevel));
                //renderer.material.color = selectedCircleColor;
                renderer.material.SetColor("_EmissionColor", selectedCircleColor);
                selectedNodeDepth = row.nodesList.IndexOf(selectedNode);
            }
        }

        var walkableLinesList = connectedNodesList.FindAll(line => line.nodesList.Exists(node => node == selectedNode));

        for (int i = 0; i < walkableLinesList.Count; i++)
        {
            var lineToDisableIndex = connectedNodesList.IndexOf(walkableLinesList[i]);
            var lineToColorize = linesList[lineToDisableIndex];
            var disabledGradient = GetDisabledAlphaGradient();
            float time = 1 - (float)selectedNodeDepth / depthLevel;

            Debug.Log(lineToColorize + " : " + time);

            var alphaKeys = new GradientAlphaKey[]
            {
                new (1, 0),
                new (1, time),
                new (disabledGradient.Evaluate(0).a, time + .01f),
                new (disabledGradient.Evaluate(0).a, 1),
            };
            var colorKeys = new GradientColorKey[]
            {
                new (gradient.Evaluate(0), 0),
                new (gradient.Evaluate(time), time),
                new (disabledGradient.Evaluate(0), time + .01f),
                new (disabledGradient.Evaluate(0), 1),
            };
            lineToColorize.colorGradient = new Gradient()
            {
                alphaKeys = alphaKeys,
                colorKeys = colorKeys,
            };

            // TO-DO: Walkable material swap
            //lineToColorize.material = walkablePathMaterial;
        }

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
        var saveNodeList = new NodeDataList();

        foreach (var mapRow in nodes2DList)
        {
            foreach (var node in mapRow.nodesList)
            {
                var nodeData = new NodeData()
                {
                    name = node.name,
                    nodeId = node.Id,
                    isConnected = node.IsConnected(),
                };

                node.ConnectedNodeList.ForEach(x => nodeData.connectedNodes.Add(x.Id));
                saveNodeList.nodeDataList.Add(nodeData);
            }
        }

        mapNodeListSO.MapNodeList = new List<NodeDataList> { saveNodeList };

        var mapNodeListJson = JsonUtility.ToJson(saveNodeList);
        PlayerPrefs.SetString("Map", mapNodeListJson);
        PlayerPrefs.Save();
        Debug.Log(mapNodeListJson);
    }

    [ContextMenu("Load Map Data")]
    private void LoadNodeList()
    {
        var mapJson = PlayerPrefs.GetString("Map");
        var loadedNodeList = JsonUtility.FromJson<NodeDataList>(mapJson);

        // Load node data into map nodes
        int index = 0;
        foreach (var mapRow in nodes2DList)
        {
            foreach (var node in mapRow.nodesList)
            {
                node.SetDataForReconnectingNodes();

                var nodeData = loadedNodeList.nodeDataList[index];
                index++;
                node.SetConnected(nodeData.isConnected);
                foreach (var connectedNodeId in nodeData.connectedNodes)
                {
                    MapNode nextNode = GetNodeById(connectedNodeId);
                    node.ConnectedNodeList.Add(nextNode);
                    //Debug.Log($"{node} -> {nextNode}");
                }
            }
        }

        ReconnectNodeLines();

        void ReconnectNodeLines()
        {
            foreach (var child in linesList)
            {
                Destroy(child.gameObject);
            }

            linesList.Clear();
            connectedNodesList.Clear();

            for (int i = 0; i < maxSplitsAllowed; i++)
            {
                connectedNodesList.Add(new());

                // Get a new line to connect all nodes
                int depth = depthLevel - 1;
                var curr = nodes2DList[depth].nodesList[i];
                var line = Instantiate(lineRendererPrefab, bossNode.transform);
                //line.endColor = line.startColor = Random.ColorHSV();
                line.endColor = line.startColor = Color.grey;
                line.positionCount = depthLevel + 1;
                linesList.Add(line);

                while (curr != null)
                {
                    line.SetPosition(depth + 1, curr.transform.position);
                    connectedNodesList[i].nodesList.Add(curr);
                    if (depth < 0) break;

                    var next = curr.ConnectedNodeList[0];
                    curr.ConnectedNodeList.RemoveAt(0);
                    Debug.Log($"{curr}:{curr.IsConnected()} |||| {curr} -> {next}");
                    curr.ConnectNode(next);
                    curr = next;

                    depth--;
                }

                Debug.Log("==========================");
            }

            nodes2DList.ForEach(x => x.nodesList.ForEach(y => y.CleanupDisconnectedNodes()));
        }
    }

    #region Utility Methods

    [ContextMenu("Create New Connections")]
    private void ConnectNewLines()
    {
        foreach (var line in linesList)
        {
            Destroy(line.gameObject);
        }

        foreach (var nodeRows in nodes2DList)
        {
            foreach (var node in nodeRows.nodesList)
            {
                node.SetDataForReconnectingNodes();
            }
        }

        ConnectNodes();
    }

    #endregion
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
    public List<int> connectedNodes = new();
}

[Serializable]
public class NodeDataList
{
    public List<NodeData> nodeDataList = new();
}