using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static int HighestLevel
    {
        get => UtilGame.GetDataInt(UtilGame.KEY_HIGHEST_LEVEL, 0);
        set => UtilGame.SetDataInt(UtilGame.KEY_HIGHEST_LEVEL, value);
    }
}
