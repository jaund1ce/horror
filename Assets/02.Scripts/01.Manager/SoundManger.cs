using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class SoundManger : mainSingleton<SoundManger>
{
    public AudioSource PlayerStep;
    public AudioSource PlayerHeartBeat;
    public AudioSource BGM;
    public AudioSource Enviroment;

    public AudioClip[] playerstepSource1;
    public AudioClip[] playerheartbeatSource;
    public AudioClip[] bgmSource;
    public AudioClip[] enviromentsource;

    public Dictionary<string, AudioClip[]> AudioClipsDictionary; //�߰��� ���� Ư���� ��� ����
    public Dictionary<string, AudioClip> AudioClipDictionary;

    [SerializeField]private float interval;
    [SerializeField]private int stageNum = -1;

    protected override void Awake()
    {
        base.Awake();
        //GetSceneSource(SceneManager.GetActiveScene().name);
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    { 
        
    }

    public void GetSceneSource(string stagename)
    {
        if (stagename == "StartScene")
        {
            bgmSource = Resources.LoadAll<AudioClip>("Sounds/BGMs");

            AddToDictionary(bgmSource);
        }

        else if (stagename == "MainScene")
        {
            playerstepSource1 = Resources.LoadAll<AudioClip>("Sounds/PlayerSteps");

            AddToDictionarys("playerStep1", playerstepSource1);

            playerheartbeatSource = Resources.LoadAll<AudioClip>("Sounds/PlayerHearthBeats");
            enviromentsource = Resources.LoadAll<AudioClip>("Sounds/Enviroments");

            AddToDictionary(playerheartbeatSource);
            AddToDictionary(enviromentsource);
        }

        //else //���߿� �ʿ��ϸ� �߰�
        //{

        //}
    }

    private void AddToDictionary(AudioClip[] audioClips)
    {
        foreach (AudioClip clip in audioClips)
        {
            AudioClipDictionary.Add(clip.name, clip);//ȣ���� bgm�̸��� audioclip�� �̸��� �����ϰ� ����������Ѵ�.
        }
    }

    private void AddToDictionarys(string name, AudioClip[] audioClips)
    {
            AudioClipsDictionary.Add(name, audioClips);//ȣ���� bgm�̸��� audioclip�� �̸��� �����ϰ� ����������Ѵ�.
    }

    //public void ChangeStepSound(string groundType)
    //{
    //    if (groundType == "Cement")
    //    {
    //        if(AudioClipsDictionary.TryGetValue(groundType, out AudioClip[] values))
    //        {
    //            for(int i =0; i < values.Length; i++)
    //            {
    //                PlayerStep.PlayOneShot(values[i]);
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("No Step Clips!");
    //        }
    //    }
    //}

    public void ChangeHearthBeatSound(PlayerState playerState)
    {
        if (playerState == PlayerState.Normal)
        {
            PlayerHeartBeat.clip = null;
            return;
        }
        //playerstat.normal�� -1 �̱� ������ ���� �ڵ����� ���� ���ڴ� 0 �̾���Ѵ�.
        string hearthbeatname = $"PlayerHearthBeat{(int)playerState}";

        if (AudioClipDictionary.TryGetValue(hearthbeatname, out AudioClip value))
        {
            PlayerHeartBeat.clip = value;
            PlayerHeartBeat.loop = true;
            PlayerHeartBeat.Play();
        }
        else
        {
            Debug.Log("No Sound Clip!");
        }
    }

    public void ChangeBGMSound(int stagenum)
    {
        stageNum = stagenum;
        string bgmname = "";

        switch (stagenum)//�̸��� ����, -1�� default�̴�.
        {
            case 0: bgmname = "Stage1BGM"; break;
            case 1: bgmname = "Stage2BGM"; break;
            case 2: bgmname = "Stage3BGM"; break;
            case 3: bgmname = "Stage4BGM"; break;
            default: Debug.Log($"StageNum : {stagenum} / out of index"); break;
        }

        if(AudioClipDictionary.TryGetValue(bgmname, out AudioClip value))
        {
            BGM.clip = value;
            BGM.loop = true;
            BGM.Play();
        }
        else
        {
            Debug.Log("No Sound Clip!");
        }
    }

    public void MakeEnviormentSound(string enviormentName)
    {
        if (AudioClipDictionary.TryGetValue(enviormentName, out AudioClip value))
        {
            Enviroment.PlayOneShot(value);//������Ʈ�� �Ҹ��� �ѹ��� �����ȴ�.
        }
        else
        {
            Debug.Log($"No {enviormentName} Enviorment Sound Clip!");
        }
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}