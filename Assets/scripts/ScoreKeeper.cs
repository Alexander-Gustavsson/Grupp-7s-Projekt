using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class ScoreKeeper
{
    public static List<int> CollectedApples = new List<int>();

    public static void AddApple(int ID)
    {
        if (!CollectedApples.Contains(ID))
        {
            CollectedApples.Add(ID);
        }
    }
}
