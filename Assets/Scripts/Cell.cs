using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private TMP_Text wordText;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider boxCollider;

    private string category;

    private bool choosen = false;

    public int Weird { get; set; } = 1;
    public string Category => category;
    public bool Choosen => choosen;
    public bool Enabled => boxCollider.enabled;

    public void InitCell (string category, string word)
    {
        Weird = 1;
        this.category = category;

        wordText.text = word;

        boxCollider.enabled = true;
        SetChoosen(false);
    }

    public void SetChoosen (bool value)
    {
        choosen = value;

        if (value)
        {
            gameObject.layer = 6;
            sprite.color = Color.green;
        }
        else
        {
            gameObject.layer = 7;
            sprite.color = Color.red;
        }
    }

    public void ChengeState(int current, int max)
    {
        choosen = false;

        wordText.text = current + "/" + max + " " + category;
    }

    public void SetNonInteractable ()
    {
        Color color = Color.green;
        color.a = 0.4f;
        sprite.color = color;
        wordText.text = "";
        boxCollider.enabled = false;
    }
}
