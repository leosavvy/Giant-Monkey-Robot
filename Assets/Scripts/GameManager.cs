using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Linq;
using SFB;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GMRJson
{
    public string Title { get; set; }
    public List<string> ColumnHeaders { get; set; }
    public List<GMRJsonDataItem> Data { get; set; }
}

public class GMRJsonDataItem
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Nickname { get; set; }
}

public enum ItemType
{
    Structured,
    Raw
}

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public Text Title;
 
    public GameObject Header;
    public GameObject Content;

    private string JSON_PATH;
    private string jsonText;

    public List<GameObject> AllItems = new List<GameObject>();

    

    // Start is called before the first frame update
    void Start()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(this);
        }

        JSON_PATH = Application.dataPath + "/StreamingAssets/JsonChallenge.json";

        jsonText = File.ReadAllText(JSON_PATH);

        if (jsonText.Length == 0)
        {
            LoadJson();
            return;
        }
        else
        {
            StartGame();
        }
    }

    void StartGame()
    {
        var data = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(JSON_PATH));

        Title.text = data["Title"].ToString();

        AddHeaders(data);
        AddRows(data);
    }

    void AddRows(JObject data)
    {
        ContentController content = Content.GetComponent<ContentController>();

        foreach (var item in data["Data"])
        {
            var rowValues = new List<string>();

            foreach (string key in data["ColumnHeaders"])
            {
                rowValues.Add(item[key].ToString());
            }

            content.AddRow(rowValues);
        }
    }

    void AddHeaders(JObject data)
    {
        HeaderController header = Header.GetComponent<HeaderController>();

        foreach (string key in data["ColumnHeaders"])
        {
            header.AddHeader(key.ToString());
        }
    }

    public void ReloadJson()
    {
        ClearItems();

        AllItems = new List<GameObject>();

        StartGame();
    }

    public void LoadJson()
    {
        ClearItems();

        JSON_PATH = StandaloneFileBrowser.OpenFilePanel("Select Json", "", "json", false)[0];

        if (JSON_PATH.Length != 0)
        {
            jsonText = File.ReadAllText(JSON_PATH);
        }

        StartGame();
    }

    private void ClearItems()
    {
        foreach (GameObject go in AllItems)
        {
            GameObject.Destroy(go);
        }
    }
}
