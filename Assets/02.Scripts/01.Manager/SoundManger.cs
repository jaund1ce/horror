using System;
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
    public AudioSource Environment;
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
    [SerializeField] private AudioClip[] environmentsource;

    private Dictionary<string, AudioClip[]> audioClipsDictionary = new Dictionary<string, AudioClip[]>(); //반드시 초기화 해주자
    private Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();

    private float lastSoundChangeTime;
    [SerializeField]private float interval;
    [SerializeField]private int stageNum = -1;
    private float lastCheckTime;
    private AudioClip[] stepAudioClips;
    private Coroutine playerStepCoroutine;

    private int index = 0;
    private float soundpitch;

    [Header("Volumes")]
    [SerializeField][Range(0f, 1f)] private float masterVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float stepVolume = 0.5f;
    [SerializeField][Range(0f, 1f)] private float hearthbeatrVolume = 0.9f;
    [SerializeField][Range(0f, 1f)] private float breatherVolume = 0.1f;
    [SerializeField][Range(0f, 1f)] private float bgmVolume = 0.5f;
    [SerializeField][Range(0f, 1f)] private float environmentVolume = 0.5f;

    private WaitForSeconds nullWaitForSeconds = new WaitForSeconds(0.3f);

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    protected override void Start()
    {
        GetSceneSource("StartScene");
    }

    protected override void Update()
    {
        if (Time.time - lastCheckTime < interval) return;
        if (TEMBGM.clip != null)
        {
            lastCheckTime = Time.time;
            TEMBGM.Play();
        }
    }

    private void Init()
    {
        PlayerStep.volume = masterVolume * stepVolume;
        PlayerHeartBeat.volume = masterVolume * hearthbeatrVolume;
        PlayerBreathe.volume = masterVolume * breatherVolume;
        Environment.volume = masterVolume * environmentVolume;
        BGM.volume = masterVolume * bgmVolume;
        TEMBGM.volume = masterVolume * bgmVolume;
    }

    public void GetSceneSource(string stagename)//특정 씬에서 필요한 사운드를 로드해줌으로서 로딩을 줄여준다
    {
        ResetAllSounds();

        if (stagename == "StartScene")
        {
            if (bgmSource.Length == 0 || temBgmSource.Length == 0 || environmentsource.Length == 0)
            {
                bgmSource = Resources.LoadAll<AudioClip>("Sounds/BGMs");
                temBgmSource = Resources.LoadAll<AudioClip>("Sounds/TempBGMs");
                environmentsource = Resources.LoadAll<AudioClip>("Sounds/Enviroments");

                AddToDictionary(bgmSource);
                AddToDictionary(temBgmSource);
                AddToDictionary(environmentsource);
            }
            stageNum = 0;
        }

        else if (stagename == "MainScene1")
        {
            if (playerheartbeatSource.Length == 0 || playerBreatheSource.Length == 0)
            {
                GetPlayerStepSources();
                playerheartbeatSource = Resources.LoadAll<AudioClip>("Sounds/PlayerHeartBeats");
                playerBreatheSource = Resources.LoadAll<AudioClip>("Sounds/PlayerBreathes");

                AddToDictionary(playerheartbeatSource);
                AddToDictionary(playerBreatheSource);
            }
            stageNum = 1;
        }

        else if (stagename == "MainScene2")
        {
            if (playerheartbeatSource.Length == 0 || playerBreatheSource.Length == 0)
            {
                GetPlayerStepSources();
                playerheartbeatSource = Resources.LoadAll<AudioClip>("Sounds/PlayerHeartBeats");
                playerBreatheSource = Resources.LoadAll<AudioClip>("Sounds/PlayerBreathes");

                AddToDictionary(playerheartbeatSource);
                AddToDictionary(playerBreatheSource);
            }
            stageNum = 2;
        }
        else //나중에 필요하면 추가
        {
            stageNum = -1;
        }

        ChangeBGMSound(stageNum);
        ChangeTemBGMSound(stageNum);
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
            case AudioSourceType.MASTER: masterVolume = volumePercentage; break;
            case AudioSourceType.STEP: stepVolume = volumePercentage; break;
            case AudioSourceType.HEARTHBEAT: hearthbeatrVolume = volumePercentage; break;
            case AudioSourceType.BREATHE: breatherVolume = volumePercentage; break;
            case AudioSourceType.BGM: bgmVolume = volumePercentage; break;
            case AudioSourceType.ENVIROMENT: environmentVolume = volumePercentage; break;
            default: Debug.Log("Index Error"); break;
        }
        ChangeAllVolumes();
    }

    private void ChangeAllVolumes()
    {
        PlayerStep.volume = stepVolume * masterVolume;
        PlayerHeartBeat.volume = hearthbeatrVolume * masterVolume;
        PlayerBreathe.volume = bgmVolume * masterVolume;
        BGM.volume = bgmVolume * masterVolume;
        TEMBGM.volume = bgmVolume * masterVolume;
        Environment.volume = environmentVolume * masterVolume;
    }

    public float GetVolume(AudioSourceType audioSourceType)
    {
        switch (audioSourceType)
        {
            case AudioSourceType.MASTER: return masterVolume; ;
            case AudioSourceType.STEP: return stepVolume * masterVolume;
            case AudioSourceType.HEARTHBEAT: return hearthbeatrVolume * masterVolume;
            case AudioSourceType.BREATHE: return breatherVolume * masterVolume;
            case AudioSourceType.BGM: return bgmVolume * masterVolume;
            case AudioSourceType.ENVIROMENT: return environmentVolume * masterVolume;
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

    public void ChangeStepSound(GroundType groundType)
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

    public void PlayPlayerStepSound(bool OnOff, float pitch = 1)
    {
        if (OnOff)
        {
            soundpitch = pitch;
            if(soundpitch >= 1f)
            {
                stepVolume = 1f;
            }
            else
            {
                stepVolume = 0.5f;
            }
            playerStepCoroutine = StartCoroutine(StartStepSound());
        }
        else
        {
            if (playerStepCoroutine == null) return;

            StopCoroutine(playerStepCoroutine);
            playerStepCoroutine = null;
        }
    }

    private IEnumerator StartStepSound()//코루티의 조건을 외부에서 결정
    {
        if (stepAudioClips == null) yield break;

        while (true)
        {
            if (stepAudioClips == null) yield return nullWaitForSeconds;
            else
            {
                index = (int)(((index) % stepAudioClips.Length));//다른 소리의 리스트의 길이는 서로 다르기때문에
                PlayerStep.clip = stepAudioClips[index];
                PlayerStep.PlayOneShot(PlayerStep.clip);
                MainGameManager.Instance.MakeSound(PlayerStep.clip.length);

                yield return new WaitForSeconds(PlayerStep.clip.length / soundpitch);
                index = (int)((index + 1) % stepAudioClips.Length);
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

        if (audioClipDictionary.TryGetValue(hearthbeatname, out AudioClip value))
        {
            PlayerHeartBeat.clip = value;
            PlayerHeartBeat.pitch = ((float)playerState)/2 + 0.5f;
            PlayerHeartBeat.loop = true;
            PlayerHeartBeat.Play();
        }
        else
        {
        }
    }

    public void ChangeBGMSound(int stagenum)
    {
        string bgmname = "";

        switch (stagenum)//이름을 지정, -1은 default이다.
        {
            case 0: bgmname = "StartSceneBGM"; break;
            case 1: bgmname = "Stage1BGM"; break;
            case 2: bgmname = "Stage2BGM"; break;
            case 3: bgmname = "Stage3BGM"; break;
            case 4: bgmname = "DieUI"; break;
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

    public void MakeEnviornmentSound(string environmentName)
    {
        if (audioClipDictionary.TryGetValue(environmentName, out AudioClip value))
        {
            Environment.PlayOneShot(value);//오브젝트의 소리는 한번만 생성된다.
        }
        else
        {
            Debug.Log($"No {environmentName} Enviorment Sound Clip!");
        }        
    }

    public void MakeEnviormentSound(string enviormentName, float amount = 1f)
    {
        if (audioClipDictionary.TryGetValue(enviormentName, out AudioClip value))
        {
            Environment.PlayOneShot(value);
            MainGameManager.Instance.MakeSoundAction(amount + Environment.volume);
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

        if (playerBreatheType == PlayerBreatheType.Damaged)
        {
            if(PlayerBreathe.clip == null)
            {
                MakeEnviornmentSound("PlayerTakeDamage");
            }
            else//코루틴으로
            {
                StartCoroutine( StartDamagedSound());
            }
            return;
        }

        string breathename = $"PlayerBreathe{playerBreatheType.ToString()}";

        if (audioClipDictionary.TryGetValue(breathename, out AudioClip value))
        {
            PlayerBreathe.clip = value;
            PlayerBreathe.loop = true;
            PlayerBreathe.Play();
        }
        else
        {
            Debug.Log("No Sound Clip!");
        }
    }

    private IEnumerator StartDamagedSound()//코루티의 조건을 외부에서 결정
    {
            PlayerBreathe.Stop();
            MakeEnviornmentSound("PlayerTakeDamage2");

            yield return new WaitForSeconds(1f);

            PlayerBreathe.Play();

    }

    public void ChangeAllSounds()
    {
        ChangeBGMSound(stageNum);
        ChangeTemBGMSound(stageNum);
    }

    public void ResetAllSounds()
    {
        PlayerStep.clip = null;
        PlayerHeartBeat.clip = null;
        PlayerBreathe.clip = null;
        Environment.clip = null;
        BGM.clip = null;
        TEMBGM.clip = null;
    }
}