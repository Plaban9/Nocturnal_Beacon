using System;
using System.Collections;
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

    [Header("Node List")]
    [SerializeField] List<MapRow> nodes2DList = new List<MapRow>();

    private void Start()
    {
        CreateNodes();

        ConnectNodes();

        SetSelectedNode(bossNode);
    }

    private void SetSelectedNode(MapNode node)
    {
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
                node.name = $"{currentDepth} : {currentSplit}";
                node.transform.position = pos;
                nodes2DList[currentDepth].nodesList.Add(node);

                // Adding Boss Node to the first row, so all will point to boss node
                if (currentDepth == 0)
                {
                    node.AddNode(bossNode);
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
        for (int i = 0; i < nodes2DList[^1].nodesList.Count; i++)
        {
            connectedNodes.Add(new());

            // Get a new line to connect all nodes
            int depth = depthLevel - 1;
            var curr = nodes2DList[depth].nodesList[i];
            var line = Instantiate(lineRendererPrefab, bossNode.transform);
            line.endColor = line.startColor = Random.ColorHSV();
            line.positionCount = depthLevel + 1;

            while (curr != null)
            {
                line.SetPosition(depth + 1, curr.transform.position);
                connectedNodes[i].nodesList.Add(curr);
                if (depth < 0) break;

                int luckyIndex = Random.Range(0, curr.GetConnectedNodesList().Count); // Get random node from list to connect
                var next = curr.GetConnectedNodesList()[luckyIndex].GetComponent<MapNode>();
                Debug.Log("Next is " + next.name);
                curr.ConnectNode(next); // Add selected node to connected list // [Internal list] for each node
                curr = next;

                depth--;
            }
        }

        nodes2DList.ForEach(x => x.nodesList.ForEach(y => y.CleanupDisconnectedNodes()));
    }

    public void SelectNode(MapNode selectedNode)
    {
        Debug.Log(selectedNode.name + " is selected");

        SetSelectedNode(selectedNode);

        List<MapRow> selectedLinesList = new();
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            MapRow row = connectedNodes[i];
            if (row.nodesList.Exists(node => node == selectedNode))
            {
                var line = bossNode.transform.GetChild(i).GetComponent<LineRenderer>();
                var color = line.startColor;
                color.a = 1;
                line.startColor = line.endColor = color;
                selectedLinesList.Add(row);
            }
            else
            {
                var line = bossNode.transform.GetChild(i).GetComponent<LineRenderer>();
                var color = line.startColor;
                color.a = .25f;
                line.startColor = line.endColor = color;

                row.nodesList.ForEach(x => x.DeselectNode());
            }
        }

        selectedLinesList.ForEach(row =>
        {
            row.nodesList.ForEach(node => node.SelectNode());
        });
    }



    #region Utility Methods

    [ContextMenu("Create new connections")]
    private void ConnectNewLines()
    {
        foreach (Transform child in bossNode.transform)
        {
            Destroy(child.gameObject);
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