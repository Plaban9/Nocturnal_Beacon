using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapBuilderTD : MonoBehaviour
{
    // Mapbuilder with top->down approch
    [SerializeField] private MapNode bossNode;
    [SerializeField] private MapNode mapNodePrefab;
    [SerializeField] private LineRenderer lineRendererPrefab;
    [SerializeField] private Transform selectedNodeEffectTrans;
    [SerializeField] private int maxSplitsAllowed;
    [SerializeField] private int depthLevel;
    [SerializeField, Range(0f, 100f)] private float nodeDestructionPercent;

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

    #region Gradient
    [SerializeField] private List<Gradient> _selectedPathGradient = new List<Gradient>();
    #endregion

    private void Start()
    {
        GetDisabledAlphaGradient();

        CreateNodes();

        ConnectNodes();

        SelectNode(null);
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
        if (node == null)
        {
            selectedNodeEffectTrans.position = Vector3.zero;
            selectedNodeEffectTrans.localScale = Vector3.zero;
            return;
        }

        selectedNodeEffectTrans.position = node.transform.position;
        selectedNodeEffectTrans.localScale = node.transform.localScale;
    }

    private void OnEnable()
    {
        MapNode.OnMapNodeSelected += SelectNode;
    }

    private void OnDisable()
    {
        MapNode.OnMapNodeSelected -= SelectNode;
    }

    private void CreateNodes()
    {
        var pos = bossNode.transform.position;
        bossNode.SetNodeId(0);

        // Columns
        for (int currentDepth = 0; currentDepth < depthLevel; currentDepth++)
        {
            pos.y -= 3;
            pos.x = bossNode.transform.position.x + (maxSplitsAllowed / 2 * -3);
            nodes2DList.Add(new MapRow());

            // Rows
            for (int currentSplit = 0; currentSplit < maxSplitsAllowed; currentSplit++, pos.x += 3)
            {
                // Creating Node
                var node = Instantiate(mapNodePrefab, transform);
                node.name = $"{currentDepth + 1} : {currentSplit + 1}";
                node.transform.position = pos;
                nodes2DList[currentDepth].nodesList.Add(node);

                int nodeId = int.Parse(node.name.Replace(":", string.Empty).Replace(" ", string.Empty));
                //Debug.Log(nodeId);
                node.SetNodeId(nodeId);

                // Adding Boss Node to the first row, so all will point to boss node
                if (currentDepth == 0)
                {
                    node.AddNode(bossNode);
                    node.SetNodeId(currentSplit + 1);
                    continue;
                }

                // Add upper left node
                if (currentSplit > 0 && nodes2DList[currentDepth - 1].nodesList[currentSplit - 1] != null)
                    node.AddNode(nodes2DList[currentDepth - 1].nodesList[currentSplit - 1]);

                // Add upper middle node
                if (nodes2DList[currentDepth - 1].nodesList[currentSplit] != null)
                    node.AddNode(nodes2DList[currentDepth - 1].nodesList[currentSplit]);

                // Add upper right node
                if (currentSplit + 1 < maxSplitsAllowed && nodes2DList[currentDepth - 1].nodesList[currentSplit + 1] != null)
                    node.AddNode(nodes2DList[currentDepth - 1].nodesList[currentSplit + 1]);
            }
        }
    }

    [SerializeField] List<MapRow> connectedNodes = new();

    private void ConnectNodes()
    {
        connectedNodes.Clear();
        linesList.Clear();

        for (int i = 0; i < maxSplitsAllowed; i++)
        {
            connectedNodes.Add(new());

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
                connectedNodes[i].nodesList.Add(curr);
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

    public void DeselectNode()
    {
        SelectNode(null);
    }

    public void SelectNode(MapNode selectedNode)
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            MapRow row = connectedNodes[i];

            var line = linesList[i];
            //var color = line.startColor;

            line.colorGradient = GetDisabledAlphaGradient();
            line.material = unavailablePathMaterial;

            row.nodesList.ForEach(x => x.SetAsUnavailableNode());
        }

        SetSelectedNode(selectedNode);

        if (!selectedNode)
            return;

        Debug.Log(selectedNode.name + " is selected");

        int selectedNodeDepth = 0;
        List<MapRow> selectedLinesList = new();
        var gradient = _selectedPathGradient[Random.Range(0, _selectedPathGradient.Count)];

        for (int i = 0; i < connectedNodes.Count; i++)
        {
            MapRow row = connectedNodes[i];
            if (row.nodesList.Exists(node => node == selectedNode))
            {
                var line = linesList[i];
                line.colorGradient = gradient;
                selectedLinesList.Add(row);
                // Selected Node's Emmision and color should be based on gradiant that is selected
                Renderer renderer = selectedNodeEffectTrans.GetComponent<Renderer>();
                Color selectedCircleColor = gradient.Evaluate(1 - ((float)i / depthLevel));
                renderer.material.color = selectedCircleColor;
                renderer.material.SetColor("_EmissionColor", selectedCircleColor);
                selectedNodeDepth = row.nodesList.IndexOf(selectedNode);
            }
        }

        var nonWalkableLinesList = connectedNodes.FindAll(line => line.nodesList.Exists(node => node == selectedNode));
        var tempSelectedLine = nonWalkableLinesList[0];
        var index = connectedNodes.IndexOf(tempSelectedLine);
        var selectedLine = linesList[index];

        selectedLine.material = walkablePathMaterial;
        nonWalkableLinesList.Remove(tempSelectedLine);

        for (int i = 0; i < nonWalkableLinesList.Count; i++)
        {
            var nonWalkableNodelist = nonWalkableLinesList[i];
            var lineToDisableIndex = connectedNodes.IndexOf(nonWalkableLinesList[i]);
            var lineToDisable = linesList[lineToDisableIndex];
            var disabledGradient = GetDisabledAlphaGradient();
            float time = 1 - (float)selectedNodeDepth / depthLevel;

            Debug.Log(lineToDisable + " : " + time);

            var alphaKeys = new GradientAlphaKey[]
            {
                new (1, 0),
                new (1, time - .01f),
                new (disabledGradient.alphaKeys[0].alpha, time),
                new (disabledGradient.alphaKeys[0].alpha, 1),
            };
            var colorKeys = new GradientColorKey[]
            {
                new (gradient.colorKeys[0].color, 0),
                new (gradient.Evaluate(time + .1f), time + .01f),
                new (disabledGradient.colorKeys[0].color, time),
                new (disabledGradient.colorKeys[0].color, 1),
            };

            lineToDisable.colorGradient = new Gradient()
            {
                alphaKeys = alphaKeys,
                colorKeys = colorKeys,
            };
            for (int j = 0; j < nonWalkableNodelist.nodesList.Count; j++)
            {
                Debug.Log(nonWalkableLinesList[i].nodesList[j]);

                if (nonWalkableLinesList[i].nodesList[j] == selectedNode)
                {
                    break;
                }
            }
        }

        selectedLinesList.ForEach(row =>
        {
            row.nodesList.ForEach(node => node.SetAsSelectableNode());
        });
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
            foreach (Transform child in bossNode.transform)
            {
                Destroy(child.gameObject);
            }

            connectedNodes.Clear();
            for (int i = 0; i < maxSplitsAllowed; i++)
            {
                connectedNodes.Add(new());

                // Get a new line to connect all nodes
                int depth = depthLevel - 1;
                var curr = nodes2DList[depth].nodesList[i];
                var line = Instantiate(lineRendererPrefab, bossNode.transform);
                //line.endColor = line.startColor = Random.ColorHSV();
                line.endColor = line.startColor = Color.grey;
                line.positionCount = depthLevel + 1;

                while (curr != null)
                {
                    line.SetPosition(depth + 1, curr.transform.position);
                    connectedNodes[i].nodesList.Add(curr);
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