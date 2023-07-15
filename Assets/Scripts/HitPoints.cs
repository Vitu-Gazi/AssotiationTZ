using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitPoints : Singleton<HitPoints>
{
    [SerializeField] private int startHit;
    [SerializeField] private TMP_Text textHit;

    private int currentHit;

    private void Start()
    {
        currentHit = startHit;
        textHit.text = "Current hits " + currentHit.ToString();
    }

    public void GetDamage (int damage = 1)
    {
        currentHit -= damage;

        textHit.text = "Current hits " + currentHit.ToString();

        if (currentHit <= 0)
        {
            GameStateController.Instance.EndGame(false);
        }
    }
}
