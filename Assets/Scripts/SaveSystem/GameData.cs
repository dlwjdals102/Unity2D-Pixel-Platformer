using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public string lastSceneName;
    public Vector3 playerPosition;

    public GameData()
    {
        lastSceneName = "Level_0";
        playerPosition = Vector3.zero;
    }

}
