using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField : Singleton<GameField>
{
    [SerializeField] private int level = 1;
    [SerializeField] private int wordsNumber = 40;

    private List<FieldWords> fieldWords = new List<FieldWords>();

    private List<Cell> cells = new List<Cell>();

    private void Start()
    {
        FieldGenerator.Instance.GenerateField(ref cells);
        CellPositioner.Instance.InitCellPositions(cells);
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
                        if (field.name == cat.name)
                        {
                            checkField = true;
                            break;
                        }
                        checkField = false;
                    }
                    if (!checkField)
                    {
                        fieldWords.Add(new FieldWords() { name = cat.name });
                    }

                    foreach (var word in cat.words)
                    {
                        if (currentCell >= wordsNumber)
                        {
                            CheckWordsNumberUp();

                            return;
                        }

                        FieldWords findingField = FindFieldCategory(cat.name);

                        findingField.words.Add(word);
                        if (currentCell < cells.Count)
                        {
                            findingField.words.Last().isUsing = true;
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
            cell.SetChoosen(false);
        }
        foreach (var cell in cells)
        {
            if (cell.Category != category)
            {
                HitPoints.Instance.GetDamage();
                cells.Clear();
                return;
            }
        }

        CellPositioner.Instance.UpdateCellPositions();

        foreach (var field in fieldWords)
        {
            Cell lastCell = cells.Last();

            if (field.name == category)
            {
                for (int i = 0; i < cells.Count - 1; i++)
                {
                    lastCell.Weird += cells[i].Weird;
                }

                lastCell.ChengeState(lastCell.Weird, field.words.Count);

                if (field.words.Count == lastCell.Weird)
                {
                    field.words.Clear();

                    Words word = GetNonUsedWord(out string name);

                    if (word != null)
                    {
                        lastCell.InitCell(name, word.word);
                    }
                    else
                    {
                        lastCell.SetNonInteractable();
                    }
                }

                break;
            }
        }

        CheckWordsAndInitCells(cells);

        cells.Clear();

        bool checkEndGame = false;

        foreach (var cell in this.cells)
        {
            if (cell.Enabled)
            {
                checkEndGame = false;
                return;
            }
            checkEndGame = true;
        }

        GameStateController.Instance.EndGame(checkEndGame);
    }

    // Методы для упрощённого взаимодействия
    private void CheckWordsNumberUp ()
    {
        for (int f = 0; f < fieldWords.Count; f++)
        {
            if (fieldWords[f].words.Count < 2)
            {
                fieldWords.RemoveAt(f);
                break;
            }
        }
    }
    private FieldWords FindFieldCategory(string name)
    {
        foreach (var field in fieldWords)
        {
            if (field.name == name)
            {
                return field;
            }
        }

        return null;
    }


    private void CheckWordsAndInitCells (List<Cell> cells)
    {
        int changeCells = 0;

        foreach (var cell in cells)
        {
            if (cell == cells.Last())
            {
                break;
            }

            cell.SetNonInteractable();

            CellPositioner.Instance.UpdateCellPositions();
        }

        foreach (var field in fieldWords)
        {
            foreach (var word in field.words)
            {
                if (!word.isUsing)
                {
                    word.isUsing = true;
                    cells[changeCells].InitCell(field.name, word.word);

                    changeCells++;

                    if (changeCells >= cells.Count - 1)
                    {
                        return;
                    }
                }
            }
        }
    }

    private Words GetNonUsedWord (out string category)
    {
        category = null;
        foreach (var field in fieldWords)
        {
            foreach (var word in field.words)
            {
                if (!word.isUsing)
                {
                    category = field.name;
                    return word;
                }
            }
        }

        return null;
    }
}

[System.Serializable]
public class FieldWords
{
    public string name;
    public List<Words> words = new List<Words>();
}
