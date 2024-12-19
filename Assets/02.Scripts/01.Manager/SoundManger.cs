using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerState
{
    Normal = -1,
    Chased = 0,
    Danger,
    Chasing,
    Hide
}

public class SoundManger : mainSingleton<SoundManger>
{
    public AudioSource PlayerStep;
    public AudioSource PlayerHeartBeat;
    public AudioSource BGM;
    public AudioSource Enviroment;

    public AudioClip[] playerstepSource;
    public AudioClip[] playerheartbeatSource;
    public AudioClip[] bgmSource;
    public AudioClip[] enviromentsource;

    public float interval;
    public int StageNum = -1;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        if(BGM != null && bgmSource != null && bgmSource.Length != 0 && StageNum != -1)
        {
            BGM.clip = bgmSource[StageNum];
            BGM.loop = true;
            BGM.Play();
        }
    }

    protected override void Update()
    { 
        
    }

    public void ChangeState(PlayerState playerState)
    {
        if (playerState == PlayerState.Normal)
        {
            PlayerHeartBeat.clip = null;
            return;
        }
        if(playerheartbeatSource[(int)playerState] == null) return ;

        PlayerHeartBeat.clip = playerheartbeatSource[(int)playerState];
        PlayerHeartBeat.loop = true;
        PlayerHeartBeat.Play();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}