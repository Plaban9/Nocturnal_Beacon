using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    Dictionary<int, Card> cardDict = new Dictionary<int, Card>();
    Card card;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Init();

        if (GUILayout.Button("Generate Basic Info"))
        {
            GenerateId();
            GenerateName();
        }

    }

    private void Init()
    {
        card = (Card)target;

        cardDict.Clear();
        var cardObjects = Resources.LoadAll<Card>("CardObject/PlayerCards");

        foreach (var cardObject in cardObjects)
        {
            if (cardObject.name == card.name) continue;
            cardDict.TryAdd(cardObject.id, cardObject);
        }
    }

    void GenerateId()
    {
        int initId = 10001;

        while (cardDict.ContainsKey(initId))
        {
            initId++;
        }

        card.id = initId;
    }

    void GenerateName()
    {
        string assetPath = AssetDatabase.GetAssetPath(card.GetInstanceID());
        card.name = Path.GetFileNameWithoutExtension(assetPath);
        card.UpdateDebugDescription();
    }

}
