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

    private Dictionary<string, AudioClip[]> AudioClipsDictionary = new Dictionary<string, AudioClip[]>(); //�ݵ�� �ʱ�ȭ ������
    private Dictionary<string, AudioClip> AudioClipDictionary = new Dictionary<string, AudioClip>();

    private float lastSoundChangeTime;
    [SerializeField]private float interval;
    [SerializeField]private int stageNum = -1;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        GetSceneSource(SceneManager.GetActiveScene().name);//##ToDo : ���� �ε� �Ҷ� ȣ���� �ʿ�
    }

    public void GetSceneSource(string stagename)//Ư�� ������ �ʿ��� ���带 �ε�
    {
        if (stagename == "StartScene" && bgmSource.Length == 0)
        {
            Debug.Log("BGM load complete");
            bgmSource = Resources.LoadAll<AudioClip>("Sounds/BGMs");

            AddToDictionary(bgmSource);
            ChangeBGMSound(0);
        }

        else if (stagename == "MainScene" && (playerstepSource1.Length == 0 || playerheartbeatSource.Length == 0 || enviromentsource.Length == 0))
        {
            ChangeBGMSound(1);
            Debug.Log("MainSceneSound load complete");

            playerstepSource1 = Resources.LoadAll<AudioClip>("Sounds/PlayerSteps");
            playerheartbeatSource = Resources.LoadAll<AudioClip>("Sounds/PlayerHeartBeats");
            enviromentsource = Resources.LoadAll<AudioClip>("Sounds/Enviroments");

            AddToDictionarys("playerStep1", playerstepSource1);
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

    public void ChangeStepSound(GroundType groundType)//�߼Ҹ� ������ ���?
    {
        if (groundType == GroundType.Cement)
        {
            if (AudioClipsDictionary.TryGetValue(groundType.ToString(), out AudioClip[] values))
            {
                for (int i = 0; i < values.Length; i++)
                {
                    PlayerStep.PlayOneShot(values[i]);
                }
            }
            else
            {
                Debug.Log("No Step Clips!");
            }
        }
    }

    public void ChangeHearthBeatSound(PlayerHeartState playerState)
    {
        if (playerState == PlayerHeartState.Normal)
        {
            PlayerHeartBeat.clip = null;
            return;
        }
        string hearthbeatname = "PlayerHearthBeat";

        if (AudioClipDictionary.TryGetValue(hearthbeatname, out AudioClip value))
        {
            PlayerHeartBeat.clip = value;
            PlayerHeartBeat.pitch = (int)playerState;
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
            case 0: bgmname = "StartSceneBGM"; break;
            case 1: bgmname = "Stage1BGM"; break;
            case 2: bgmname = "Stage2BGM"; break;
            case 3: bgmname = "Stage3BGM"; break;
            case 4: bgmname = "Stage4BGM"; break;
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