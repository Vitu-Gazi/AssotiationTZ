using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListArrayRandom
{
    public static T GetRandom<T>(this List<T> list)
    {
        T t = list[UnityEngine.Random.Range(0, list.Count)];

        return t;
    }
}

