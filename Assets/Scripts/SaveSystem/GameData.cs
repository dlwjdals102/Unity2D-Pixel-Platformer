using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public string lastSceneName;

    public Vector3 playerPosition;
    public float playerCurrentHealth;

    public GameData()
    {
        lastSceneName = "Level_0";
        playerPosition = Vector3.zero;
        playerCurrentHealth = 0;
    }

}
