using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RowController : MonoBehaviour
{
    public GameObject RowItem;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRowItem(string TextValue)
    {
        GameObject rowItem = Instantiate(RowItem, this.transform);
        rowItem.layer = 5;

        rowItem.transform.position = new Vector3(0, 0, 0);

        Text textComponent = rowItem.GetComponent<Text>();

        textComponent.text = TextValue;
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.color = new Color(0, 0, 0);
        textComponent.fontSize = 15;
        textComponent.fontStyle = FontStyle.Bold;
        textComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
    }
}
