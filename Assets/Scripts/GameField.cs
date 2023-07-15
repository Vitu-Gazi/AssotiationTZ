using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField : Singleton<GameField>
{
    [SerializeField] private List<Cell> cells;
    [SerializeField] private int level = 1;
    [SerializeField] private int wordsNumber = 40;

    [SerializeField] private List<FieldWords> fieldWords = new List<FieldWords>();

    private void Start()
    {
        FieldGenerator.Instance.GenerateField(ref cells);
        WordInit();
    }

    private void WordInit ()
    {
        CategoriesList categoriesList = JSONReader.GetCategories();

        int currentCell = 0;

        if (wordsNumber < cells.Count)
        {
            wordsNumber = cells.Count;
        }

        while (currentCell < wordsNumber)
        {
            foreach (var cat in categoriesList.categories)
            {
                if (cat.level == level)
                {
                    bool checkField = false;
                    foreach (var field in fieldWords)
                    {
                        if (field.category == cat.name)
                        {
                            checkField = true;
                            break;
                        }
                        checkField = false;
                    }
                    if (!checkField)
                    {
                        fieldWords.Add(new FieldWords() { category = cat.name });
                    }

                    foreach (var word in cat.words)
                    {
                        if (currentCell >= wordsNumber)
                        {
                            for (int f = 0; f < fieldWords.Count; f++)
                            {
                                if (fieldWords[f].words.Count < 2)
                                {
                                    fieldWords.RemoveAt(f);
                                    break;
                                }
                            }

                            return;
                        }

                        fieldWords.Last().words.Add(word);
                        if (currentCell < cells.Count)
                        {
                            fieldWords.Last().words.Last().isUsing = true;
                            cells[currentCell].InitCell(cat.name, word.word);
                        }

                        currentCell++;
                    }
                }
            }
        }
    }

    public void CheckWords (ref List<Cell> cells)
    {
        string category = cells[0].Category;

        foreach (var cell in cells)
        {
            cell.CellState = CellState.NonInterctable;
            cell.SetChoosen(false);
        }
        foreach (var cell in cells)
        {
            if (cell.Category != category)
            {
                cells.Clear();
                return;
            }
        }
        foreach (var cell in cells)
        {
            cell.SetChoosen(false);
            if (cell != cells.Last())
            {
                foreach (var field in fieldWords)
                {
                    foreach (var word in field.words)
                    {
                        if (!word.isUsing && cell.CellState != CellState.Update)
                        {
                            cell.InitCell(field.category, word.word);
                            word.isUsing = true;
                            cell.gameObject.SetActive(true);
                            cell.CellState = CellState.Update;
                        }
                    }
                }

            }
        }
        foreach (var cell in cells)
        {
            if (cell.CellState == CellState.NonInterctable && cell != cells.Last())
            {
                cell.SetNonInteractable();
                //cell.gameObject.SetActive(false);
            }
        }
        foreach (var field in fieldWords)
        {
            Cell lastCell = cells.Last();

            if (field.category == category)
            {
                for (int i = 0; i < cells.Count - 1; i++)
                {
                    lastCell.Weird += cells[i].Weird;
                }

                lastCell.ChengeState(lastCell.Weird, field.words.Count);

                if (field.words.Count == lastCell.Weird)
                {
                    field.words.Clear();
                    lastCell.SetNonInteractable();
                    //lastCell.gameObject.SetActive(false);
                }

                break;
            }
        }

        cells.Clear();

        bool checkEndGame = false;

        foreach (var cell in this.cells)
        {
            if (cell.gameObject.activeSelf)
            {
                checkEndGame = false;
                return;
            }
            checkEndGame = true;
        }

        GameStateController.Instance.EndGame(checkEndGame);
    }
}

[System.Serializable]
public class FieldWords
{
    public string category;
    public List<Words> words = new List<Words>();
}
