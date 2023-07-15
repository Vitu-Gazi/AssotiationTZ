using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : Singleton<FieldGenerator>
{
    [SerializeField] private Cell cell;
    [SerializeField] private Transform parent;
    [SerializeField] private Vector3 localPosiotions;
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 8;

    public void GenerateField (ref List<Cell> cells)
    {
        float stepRows = localPosiotions.x * 2.5f * -1;
        stepRows /= rows;

        float stepColumn = localPosiotions.y * 1.3f;
        stepColumn /= columns;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Cell newCell = Instantiate(cell);
                newCell.transform.SetParent(parent);
                newCell.transform.localPosition = new Vector3(localPosiotions.x + (stepRows * r), (stepColumn * c));

                cells.Add(newCell);
            }
        }

        List<Cell> newCells = new List<Cell>(cells);

        for (int i = 0; i < cells.Count; i++)
        {
            Cell randomCell = newCells.GetRandom();
            cells[i] = randomCell;
            newCells.Remove(randomCell);
        }
    }
}
