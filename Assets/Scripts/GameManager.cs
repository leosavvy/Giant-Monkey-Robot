using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Linq;
using SFB;
using UnityEngine.UI;

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

    List<string> Values = new List<string>();

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
        jsonText = File.ReadAllText(JSON_PATH);

        var jsonData = JsonMapper.ToObject(jsonText);

        var keys = getKeysFromJson(jsonData);

        if (keys.Any(k => k == "Title"))
        {
            HandleStructuredJson(JsonMapper.ToObject<GMRJson>(File.ReadAllText(JSON_PATH)));
        }
        else
        {
            HandleRawJson(keys, jsonData);
        }
    }

    void HandleStructuredJson(GMRJson structuredData)
    {
        HeaderController header = Header.GetComponent<HeaderController>();
        ContentController content = Content.GetComponent<ContentController>();

        Title.text = structuredData.Title;

        foreach (var headerColum in structuredData.ColumnHeaders)
        {
            header.AddHeader(headerColum, ItemType.Structured);
        }

        foreach (var dataItem in structuredData.Data)
        {
            content.AddRow(dataItem);
        }
    }

    void HandleRawJson(IEnumerable<string> keys, JsonData jsonData)
    {
        HeaderController header = Header.GetComponent<HeaderController>();
        ContentController content = Content.GetComponent<ContentController>();

        foreach (var key in keys)
        {
            header.AddHeader(key, ItemType.Raw);
        }

        foreach (JsonData jsonItem in jsonData)
        {
            content.AddRow(getValues(jsonItem, keys));
        }
    }

    IEnumerable<string> getKeysFromJson(JsonData jsonData)
    {
        List<string> Keys = new List<string>();

        if (jsonData.IsArray)
        {
            foreach (JsonData jsonItem in jsonData)
            {
                foreach (var key in jsonItem.Keys)
                {
                    Keys.Add(key.ToString());
                }
            }

            return Keys.Distinct();
        }
        else
        {
            foreach (var key in jsonData.Keys)
            {
                Keys.Add(key.ToString());
            }

            return Keys.Distinct();
        }
    }

    IEnumerable<string> getValues(JsonData jsonData, IEnumerable<string> Keys)
    {
        List<string> values = new List<string>();

        foreach (var key in Keys)
        {
            values.Add(jsonData[key.ToString()].ToString());
        }

        return values;
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
