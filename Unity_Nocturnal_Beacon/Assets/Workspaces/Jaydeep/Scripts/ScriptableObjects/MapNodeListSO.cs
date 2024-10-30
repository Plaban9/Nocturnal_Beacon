using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Map Node SO/ New MapNode List")]
public class MapNodeListSO : ScriptableObject
{
    [SerializeField] private NodeDataList nodeList;

    [field: SerializeField] public NodeDataList SelectedLine { get; set; } = new();
    public NodeDataList MapNodeList
    {
        get
        {
            RetriveNodeList();
            return nodeList;
        }
        private set
        {
            nodeList = value;
        }
    }

    public void SaveNodeList(NodeDataList list)
    {
        SelectedLine.nodeDataList.Clear();
        nodeList = list;
        var nodeDataListJson = JsonUtility.ToJson(list);
        Debug.Log(nodeDataListJson);

        PlayerPrefs.SetString("Map", nodeDataListJson);
        PlayerPrefs.Save();
    }

    public void SetSelectedLine()
    {

    }

    private void RetriveNodeList()
    {
        var mapJson = PlayerPrefs.GetString("Map");
        nodeList = JsonUtility.FromJson<NodeDataList>(mapJson);
    }
}
