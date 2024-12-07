using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public Player Player;

    protected override void Start()
    {
        // Player �ʱ�ȭ
        Player = FindObjectOfType<Player>();
        if (Player == null)
        {
            Debug.LogError("������ Player�� ã�� �� �����ϴ�.");
        }
    }
}