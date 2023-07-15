using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellPositioner : Singleton<CellPositioner>
{
    private List<Cell> cells = new List<Cell>();

    public void InitCellPositions (List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            this.cells.Add(cell);
        }
    }

    public void UpdateCellPositions()
    {
        Vector3 onePos;
        Vector3 twoPos;

        List<GameObject> newCells = new List<GameObject>();

        foreach (var c in cells)
        {
            if (!c.Enabled)
            {
                newCells.Clear();
                Collider[] colliders = Physics.OverlapSphere(c.transform.position, 10);

                foreach (var col in colliders)
                {
                    if (col.transform.localPosition.y > c.transform.localPosition.y && col.transform.localPosition.x == c.transform.localPosition.x)
                    {
                        newCells.Add(col.gameObject);
                    }
                }

                newCells = newCells.OrderBy(x => x.transform.localPosition.y).ToList();

                foreach (var newCell in newCells)
                {
                    onePos = newCell.transform.localPosition;
                    twoPos = c.transform.localPosition;

                    newCell.transform.localPosition = twoPos;
                    c.transform.localPosition = onePos;
                }
            }
        }
    }
}
