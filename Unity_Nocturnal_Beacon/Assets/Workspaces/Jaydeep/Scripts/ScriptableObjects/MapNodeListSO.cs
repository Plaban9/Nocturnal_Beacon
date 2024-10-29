using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map Node SO/ New MapNode List")]
public class MapNodeListSO : ScriptableObject
{
    [field:SerializeField] public List<NodeDataList> MapNodeList { get; set; } = new();

    public void SaveNodeList()
    {
        foreach (var row in MapNodeList)
        {
            var rowJson = JsonUtility.ToJson(row);
            Debug.Log(rowJson);
        }
    }
}
