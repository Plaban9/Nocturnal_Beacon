using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Map Node SO/ New MapNode List")]
public class MapNodeListSO : ScriptableObject
{
    [field: SerializeField] public NodeDataList MapNodeList { get; private set; } = new();
    [field: SerializeField] public NodeDataList SelectedLine { get; set; } = new();

    public void SaveNodeList(NodeDataList list)
    {
        //SelectedLine.nodeDataList.Clear();
        MapNodeList = list;
        var nodeDataListJson = JsonUtility.ToJson(list);
        Debug.Log(nodeDataListJson);

        PlayerPrefs.SetString("Map", nodeDataListJson);
        PlayerPrefs.Save();
    }

    public void AddToSelectedLineAndSaveList(NodeData nodeData)
    {
        if (SelectedLine.nodeDataList.Contains(nodeData))
            return;

        SelectedLine.nodeDataList.Add(nodeData);
        var selectedLineListJson = JsonUtility.ToJson(SelectedLine);
        PlayerPrefs.SetString("SelectedLine", selectedLineListJson);
        PlayerPrefs.Save();
    }

    public void RetriveNodeListData()
    {
        var mapJson = PlayerPrefs.GetString("Map");
        var selectedLineJson = PlayerPrefs.GetString("SelectedLine");
        SelectedLine = JsonUtility.FromJson<NodeDataList>(selectedLineJson);
        MapNodeList = JsonUtility.FromJson<NodeDataList>(mapJson);

        Debug.Log(mapJson);

        if (SelectedLine == null)
        {
            SelectedLine = new();
        }
        if (MapNodeList == null)
        {
            MapNodeList = new();
        }
    }

    public void ResetData()
    {
        MapNodeList = new();
        SelectedLine = new();
    }
}
