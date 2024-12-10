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
        // Player �ʱ�ȭ
        Player = FindObjectOfType<Player>();
        if (Player == null)
        {
            Debug.LogError("������ Player�� ã�� �� �����ϴ�.");
        }
    }
}