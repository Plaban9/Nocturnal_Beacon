using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONCardReader : MonoBehaviour
{
    private void Start()
    {
        ReadModdedCards();
    }

    public void TryCreateFolders()
    {
        if (!Directory.Exists(Application.dataPath + "/modding"))
        {
            Directory.CreateDirectory(Application.dataPath + "/modding");
        }
        if (!Directory.Exists(Application.dataPath + "/modding/cards"))
        {
            Directory.CreateDirectory(Application.dataPath + "/modding/cards");
        }
        if (!Directory.Exists(Application.dataPath + "/modding/characters"))
        {
            Directory.CreateDirectory(Application.dataPath + "/modding/characters");
        }
        if (!Directory.Exists(Application.dataPath + "/modding/sprites"))
        {
            Directory.CreateDirectory(Application.dataPath + "/modding/sprites");
        }
    }

    public void ReadModdedCards()
    {
        Debug.Log("Starting loading player created cards...");

        //No modding folder, ignore.
        if (!Directory.Exists(Application.dataPath + "/modding"))
        {
            Debug.Log("No modding folder found, created folders...");
            TryCreateFolders();
            return;
        }

        var info = new DirectoryInfo(Application.dataPath + "/modding/cards");
        var moddedCards = info.GetFiles("*.json");
        foreach(var card in moddedCards)
        {
            Debug.Log(card.FullName);
            var content = File.ReadAllText(card.FullName);
            Debug.Log(content);
            /*Card importedCard =*/ ReadCard(content);
        }
        Debug.Log("Done loading player created cards...");

    }

    public Card ReadCard(string savedCardsJson)
    {
        Card newCard = new Card();
        JsonUtility.FromJsonOverwrite(savedCardsJson, newCard);
        return newCard;
    }

    public void SaveCardToJson(Card card)
    {
        string strOutput = JsonUtility.ToJson(card);

        TryCreateFolders();


        File.WriteAllText(Application.dataPath + $"/modding/cards/{card.name}.json", strOutput);
    }



    //public void ReadModdedCharacters()
    //{

    //    //No modding folder, ignore.
    //    if (!Directory.Exists(Application.dataPath + "/modding"))
    //    {
    //        return;
    //    }

    //    var info = new DirectoryInfo(Application.dataPath + "/modding/characters");
    //    var moddedCards = info.GetFiles();
    //    foreach (var card in moddedCards)
    //    {

    //        //Card importedCard = ReadCard(File.ReadAllLines(card.DirectoryName+card.FullName));
    //    }

    //}

    //public void ReadCharacter(TextAsset savedPlayersJson)
    //{

    //}

    //public void SaveCharacterToJson(Card card)
    //{
    //    string strOutput = JsonUtility.ToJson(card);

    //    if (!Directory.Exists(Application.dataPath + "/modding"))
    //    {
    //        Directory.CreateDirectory(Application.dataPath + "/modding");
    //    }
    //    if (!Directory.Exists(Application.dataPath + "/modding/characters"))
    //    {
    //        Directory.CreateDirectory(Application.dataPath + "/modding/characters");
    //    }

    //    File.WriteAllText(Application.dataPath + $"/modding/characters/{card.name}.json", strOutput);
    //}

}
