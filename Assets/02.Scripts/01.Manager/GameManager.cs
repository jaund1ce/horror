using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public Player Player;

    public Player player;

    protected override void Awake()
    {
        // Player 초기화
        Player = FindObjectOfType<Player>();
        if (Player == null)
        {
            Debug.LogError("씬에서 Player를 찾을 수 없습니다.");
        }
    }
}