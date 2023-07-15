using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class JSONReader
{
    public static CategoriesList GetCategories ()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("words", typeof(TextAsset));

        CategoriesList categoriesList = JsonUtility.FromJson<CategoriesList>(textAsset.text);

        return categoriesList;
    }
}


[System.Serializable]
public class CategoriesList
{
    public Categories[] categories;
}

[System.Serializable]
public class Categories
{
    public int index;
    public string name;
    public string description;
    public string icon;
    public int level;
    public Words[] words;
}

[System.Serializable]
public class Words
{
    public string word;
    public string description;

    public bool isUsing = false;
}

