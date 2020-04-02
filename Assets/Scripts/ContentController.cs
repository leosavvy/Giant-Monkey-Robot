using System.Collections.Generic;
using UnityEngine;

public class ContentController : MonoBehaviour
{
    public GameObject Row;

    public void AddRow(IEnumerable<string> values)
    {
        GameObject row = Instantiate(Row, this.transform);
        row.layer = 5;
        row.transform.position = Vector3.zero;

        var rowController = row.GetComponent<RowController>();

        foreach (var value in values)
        {
            rowController.AddRowItem(value);
        }

        GameManager.gameManager.AllItems.Add(row);
    }

}
