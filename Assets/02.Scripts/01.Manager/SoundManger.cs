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
    public AudioSource PlayerBreathe;
    public AudioSource Enviroment;
    public AudioSource BGM;
    public AudioSource TEMBGM;

    [SerializeField] private AudioClip[] playerstepSource_cement;
    [SerializeField] private AudioClip[] playerstepSource_concrete;
    [SerializeField] private AudioClip[] playerstepSource_dirt;
    [SerializeField] private AudioClip[] playerstepSource_grass;
    [SerializeField] private AudioClip[] playerstepSource_wood;

    [SerializeField] private AudioClip[] playerheartbeatSource;
    [SerializeField] private AudioClip[] playerBreatheSource;
    [SerializeField] private AudioClip[] bgmSource;
    [SerializeField] private AudioClip[] temBgmSource;
    [SerializeField] private AudioClip[] enviromentsource;

    private Dictionary<string, AudioClip[]> audioClipsDictionary = new Dictionary<string, AudioClip[]>(); //반드시 초기화 해주자
    private Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();

    private float lastSoundChangeTime;
    [SerializeField]private float interval;
    [SerializeField]private int stageNum = -1;
    private float lastCheckTime;
    private AudioClip[] stepAudioClips;
    private Coroutine playerStepCoroutine;

    private int index = 0;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {

    }

    protected override void Update()
    {
        GetSceneSource(SceneManager.GetActiveScene().name);//##ToDo : 씬이 로드 할때 호출이 필요

        if (Time.time - lastCheckTime < interval) return;
        if (TEMBGM.clip != null)
        {
            lastCheckTime = Time.time;
            TEMBGM.Play();
        }
    }

    public void GetSceneSource(string stagename)//특정 씬에서 필요한 사운드를 로드
    {
        if (stagename == "StartScene" && bgmSource.Length == 0)
        {
            bgmSource = Resources.LoadAll<AudioClip>("Sounds/BGMs");
            temBgmSource = Resources.LoadAll<AudioClip>("Sounds/TempBGMs");
            enviromentsource = Resources.LoadAll<AudioClip>("Sounds/Enviroments");

            AddToDictionary(bgmSource);
            AddToDictionary(temBgmSource);
            AddToDictionary(enviromentsource);
            ChangeBGMSound(0);
            ChangeTemBGMSound(0);
        }

        else if (stagename == "MainScene" && (playerstepSource_cement.Length == 0 || playerheartbeatSource.Length == 0 || enviromentsource.Length == 0))
        {
            GetPlayerStepSources();

            playerstepSource_cement = Resources.LoadAll<AudioClip>("Sounds/PlayerSteps/Cement");
            playerheartbeatSource = Resources.LoadAll<AudioClip>("Sounds/PlayerHeartBeats");
            playerBreatheSource = Resources.LoadAll<AudioClip>("Sounds/PlayerBreathes");
            

            AddToDictionary(playerheartbeatSource);
            AddToDictionary(playerBreatheSource);
            

            ChangeBGMSound(1);
            ChangeTemBGMSound(-1);
            ChangeStepSound(GroundType.Cement);
        }

        //else //나중에 필요하면 추가
        //{

        //}
    }

    private void GetPlayerStepSources()
    {
        playerstepSource_cement = Resources.LoadAll<AudioClip>("Sounds/PlayerSteps/Cement");
        playerstepSource_concrete = Resources.LoadAll<AudioClip>("Sounds/PlayerSteps/Concrete");
        playerstepSource_dirt = Resources.LoadAll<AudioClip>("Sounds/PlayerSteps/Dirt");
        playerstepSource_grass = Resources.LoadAll<AudioClip>("Sounds/PlayerSteps/Grass");
        playerstepSource_wood = Resources.LoadAll<AudioClip>("Sounds/PlayerSteps/Wood");

        AddToDictionarys("Cement", playerstepSource_cement);
        AddToDictionarys("Concrete", playerstepSource_concrete);
        AddToDictionarys("Dirt", playerstepSource_dirt);
        AddToDictionarys("Grass", playerstepSource_grass);
        AddToDictionarys("Wood", playerstepSource_wood);
    }

    public void AdjustSoundVolume(AudioSourceType audioSourceType, float volumePercentage)
    {
        switch (audioSourceType)
        {
            case AudioSourceType.STEP: PlayerStep.volume = volumePercentage; break;
            case AudioSourceType.HEARTHBEAT: PlayerHeartBeat.volume = volumePercentage; break;
            case AudioSourceType.BREATHE: PlayerBreathe.volume = volumePercentage; break;
            case AudioSourceType.BGM: BGM.volume = volumePercentage; break;
            case AudioSourceType.ENVIROMENT: Enviroment.volume = volumePercentage; break;
            default: Debug.Log("Index Error"); break;
        }
    }

    public float GetVolume(AudioSourceType audioSourceType)
    {
        switch (audioSourceType)
        {
            case AudioSourceType.STEP: return PlayerStep.volume;
            case AudioSourceType.HEARTHBEAT: return PlayerHeartBeat.volume;
            case AudioSourceType.BREATHE: return PlayerBreathe.volume;
            case AudioSourceType.BGM: return BGM.volume;
            case AudioSourceType.ENVIROMENT: return Enviroment.volume;
            default: Debug.Log("Index Error"); return -1;
        }
    }

    private void AddToDictionary(AudioClip[] audioClips)
    {
        foreach (AudioClip clip in audioClips)
        {
            audioClipDictionary.Add(clip.name, clip);//호출할 bgm이름과 audioclip의 이름을 동일하게 설정해줘야한다.
        }
    }

    private void AddToDictionarys(string name, AudioClip[] audioClips)
    {
            audioClipsDictionary.Add(name, audioClips);//호출할 bgm이름과 audioclip의 이름을 동일하게 설정해줘야한다.
    }

    public void ChangeStepSound(GroundType groundType)//발소리 생성은 어떻게?
    {
        if (audioClipsDictionary.TryGetValue(groundType.ToString(), out AudioClip[] values))
        {
            stepAudioClips = values;
        }
        else
        {
            stepAudioClips = null;
            Debug.Log("No Step Clips!");
        }
    }

    public void PlayPlayrtStepSound(bool OnOff)
    {
        if (OnOff)
        {
            playerStepCoroutine =  StartCoroutine(StartStepSound());
        }
        else
        {
            if (playerStepCoroutine == null) return;

            StopCoroutine(playerStepCoroutine);
            PlayerStep.pitch = 1f;
            playerStepCoroutine = null;
        }
    }

    public void PlayPlayrtStepSound(bool OnOff, int pitch)
    {
        if (OnOff)
        {
            PlayerStep.pitch = pitch;
            playerStepCoroutine = StartCoroutine(StartStepSound());
        }
        else
        {
            if (playerStepCoroutine == null) return;

            StopCoroutine(playerStepCoroutine);
            PlayerStep.pitch = 1f;
            playerStepCoroutine = null;
        }
    }

    private IEnumerator StartStepSound()//코루티의 조건을 외부에서 결정
    {
        if (stepAudioClips == null) yield break;

        while (true)
        {
            index = (int)((index) % stepAudioClips.Length);//다른 소리의 리스트의 길이는 서로 다르기때문에
            PlayerStep.clip = stepAudioClips[index];
            PlayerStep.PlayOneShot(PlayerStep.clip);

            yield return new WaitForSeconds(stepAudioClips[index].length);
            index = (int)((index + 1) % stepAudioClips.Length);
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

        if (audioClipDictionary.TryGetValue(hearthbeatname, out AudioClip value))
        {
            PlayerHeartBeat.clip = value;
            PlayerHeartBeat.pitch = (float)playerState/2 + 0.5f;
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

        switch (stagenum)//이름을 지정, -1은 default이다.
        {
            case 0: bgmname = "StartSceneBGM"; break;
            case 1: bgmname = "Stage1BGM"; break;
            case 2: bgmname = "Stage2BGM"; break;
            case 3: bgmname = "Stage3BGM"; break;
            case 4: bgmname = "Stage4BGM"; break;
            default: Debug.Log($"StageNum : {stagenum} / out of index"); break;
        }

        if(audioClipDictionary.TryGetValue(bgmname, out AudioClip value))
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

    public void ChangeTemBGMSound(int stagenum)
    {
        stageNum = stagenum;
        string tembgmname = "";

        switch (stagenum)//이름을 지정, -1은 default이다.
        {
            case 0: tembgmname = "StartSceneTemBGM"; break;
            case 1: tembgmname = "Stage1TemBGM"; break;
            case 2: tembgmname = "Stage2TemBGM"; break;
            case 3: tembgmname = "Stage3TemBGM"; break;
            case 4: tembgmname = "Stage4TemBGM"; break;
            default: Debug.Log($"TemBGM StageNum : {stagenum} / out of index"); break;
        }

        if (audioClipDictionary.TryGetValue(tembgmname, out AudioClip value))
        {
            TEMBGM.clip = value;
            TEMBGM.loop = false;
        }
        else
        {
            TEMBGM.clip = null;
            Debug.Log($"No {tembgmname} TemBGMName Sound Clip!");
        }
    }

    public void MakeEnviormentSound(string enviormentName)
    {
        if (audioClipDictionary.TryGetValue(enviormentName, out AudioClip value))
        {
            Enviroment.PlayOneShot(value);//오브젝트의 소리는 한번만 생성된다.
        }
        else
        {
            Debug.Log($"No {enviormentName} Enviorment Sound Clip!");
        }        
    }

    public void MakeEnviormentSound(string enviormentName, float amount)
    {
        if (audioClipDictionary.TryGetValue(enviormentName, out AudioClip value))
        {
            Enviroment.PlayOneShot(value);//오브젝트의 소리는 한번만 생성된다.
            MainGameManager.Instance.MakeSoundAction(amount + Enviroment.volume);
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

    public void ChangeBreatheBeatSound(PlayerBreatheType playerBreatheType)
    {
        if (playerBreatheType == PlayerBreatheType.Normal)
        {
            PlayerBreathe.clip = null;
            return;
        }
        string breathename = "PlayerBreathe";

        if (audioClipDictionary.TryGetValue(breathename, out AudioClip value))
        {
            PlayerBreathe.clip = value;
            PlayerBreathe.pitch = (float)playerBreatheType / 2;
            PlayerBreathe.loop = true;
            PlayerBreathe.Play();
        }
        else
        {
            Debug.Log("No Sound Clip!");
        }
    }
}