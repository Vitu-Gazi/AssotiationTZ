using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CellsCollector : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private CollectorVisual collectorVisual;

    private List<Cell> cells = new List<Cell>();

    private void Start()
    {
        InputController.Instance.ChooseCell += ChooseCell;
    }
    private void OnDestroy()
    {
        InputController.Instance.ChooseCell -= ChooseCell;
    }

    private void ChooseCell (Cell newCell)
    {
        foreach(var el in cells)
        {
            if (el == newCell)
            {
                RemovePoints(el);

                collectorVisual.UpdatePoints(GetPositions());

                return;
            }
        }

        if (cells.Count >= 10)
        {
            return;
        }

        if (cells.Count == 0)
        {
            cells.Add(newCell);
            newCell.SetChoosen(true);
        }
        else
        {
            CheckCells(newCell);
        }

        collectorVisual.UpdatePoints(GetPositions());

        if (cells.Count >= 2)
        {
            button.interactable = true;
        }
    }

    private void CheckCells (Cell newCell)
    {
        Vector3 direction = newCell.transform.position - cells.Last().transform.position;
        Ray ray = new Ray(cells.Last().transform.position, direction * direction.magnitude);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Cell cell))
            {
                if (cell.Choosen)
                {
                    RemovePoints(cell);
                }

                cells.Add(cell);
                cell.SetChoosen(true);

                if (cell != newCell)
                {
                    CheckCells(newCell);
                }
            }
        }
    }

    private List<Vector3> GetPositions ()
    {
        List<Vector3> positions = new List<Vector3>();

        foreach (var cell in cells)
        {
            positions.Add(cell.transform.position);
        }

        return positions;
    }

    private void RemovePoints (Cell cell)
    {
        int index = 0;

        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i] == cell)
            {
                index = i;
                break;
            }
        }
        for (int i = index; i < cells.Count; i++)
        {
            cells[i].SetChoosen(false);
        }

        cells.RemoveRange(index, cells.Count - index);
        cell.SetChoosen(false);

        if (cells.Count < 2)
        {
            button.interactable = false;
        }
    }

    public void CombineCells ()
    {
        GameField.Instance.CheckWords(ref cells);
        button.interactable = false;
        collectorVisual.UpdatePoints(GetPositions());
    }
}
