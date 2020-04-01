using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ContentController : MonoBehaviour
{
    public GameObject Row;

    public void AddRow(IEnumerable<string> values)
    {
        GameObject row = Instantiate(Row, this.transform);
        row.layer = 5;
        row.transform.position = new Vector3(0, 0, 0);

        var rowController = row.GetComponent<RowController>();

        foreach (var value in values)
        {
            rowController.AddRowItem(value);
        }
    }

    public void AddRow(GMRJsonDataItem dataItem)
    {
        GameObject row = Instantiate(Row, this.transform);
        row.layer = 5;
        row.transform.position = new Vector3(0, 0, 0);

        var rowController = row.GetComponent<RowController>();

        rowController.AddRowItem(dataItem.ID);
        rowController.AddRowItem(dataItem.Name);
        rowController.AddRowItem(dataItem.Nickname);
        rowController.AddRowItem(dataItem.Role);
    }
}
