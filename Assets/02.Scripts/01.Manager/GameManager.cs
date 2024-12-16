using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public int paperInteractionCount;

    public Player Player;

    public Player player;

    protected override void Awake()
    {
        // Player √ ±‚»≠
        Player = FindObjectOfType<Player>();
        if (Player == null)
        {
            return;
        }
    }
}