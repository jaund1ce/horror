using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : mainSingleton<UIManager>
{
    private List<BaseUI> uiList = new List<BaseUI>(); // 인스턴스화된 UI 저장

    private MainUI mainUI;
    // 인스펙터에서 넣는거는 프로젝트의 원본의 오브젝트고 실제로 동작하고싶은건 인스턴시에이트
    //된 아이를 컨트롤 하고싶어서 miss 나는것


    protected override void Awake()
    {
        base.Awake();
    }


    public void Initalize()
    {
        uiList.RemoveAll(item => item == null);
    }


    //그럼 직접 코드에서 해당 UI를 가져오는 스크립트 작성
    //해당 스크립트는 씬 매니져에서 씬이 로드될때 동작하는곳에 작성하는게 좋다
    public T GetUI<T>() where T : BaseUI 
    {
        BaseUI existingUI = uiList.Find(x => x is T);
        if(existingUI == null)return null;
        return existingUI.GetComponent<T>();
    }

    private IEnumerator DelayShowUI()
    {
        yield return null;
        Show<StartUI>();
    }

    // 일반 UI 표시
    public void Show<T>() where T : BaseUI
    {
        BaseUI existingUI = uiList.Find(x => x is T);
        if (existingUI != null)
        {
            Debug.LogWarning($"{typeof(T).Name} UI는 이미 활성화되어 있습니다.");
            Hide<T>();
            return;
        }
        BaseUI uiPrefab = Resources.Load<BaseUI>("UI/" + typeof(T).Name); // 프리팹 로드
        if (uiPrefab == null)
        {
            Debug.LogError($"{typeof(T).Name} UI 프리팹을 찾을 수 없습니다.");
            return;
        }

        T uiInstance = InstantiateUI<T>(uiPrefab); // UI 인스턴스 생성
        uiList.Add(uiInstance);                   // 인스턴스화된 UI를 리스트에 추가
                                                  //Debug.Log($"{typeof(T).Name} UI가 생성되었습니다.");

        //RemoveNull();

        //string uiName = typeof(T).ToString();
        //uiList.TryGetValue(uiName, out UIBase ui);
        //if (ui == null)
        //{
        //    //ui = Resources.Load<UIBase>($"UI/{uiName}");
        //    uiList.Remove(uiName);
        //    var obj = ResourceManager.Instance.LoadAsset<GameObject>(uiName, eAssetType.UI);
        //    ui = Load<T>(obj, uiName);
        //    uiList.Add(uiName, ui);
        //    ui.Opened(param);
        //}

        //ui.gameObject.SetActive(true);

        //return (T)ui;
    }

    public void Hide<T>() where T : BaseUI
    {
        
        // 인스턴스화된 UI를 찾음
        BaseUI ui = uiList.Find(x => x is T);
        if (ui == null)
        {
            //Debug.LogError($"{typeof(T).Name} UI를 찾을 수 없습니다.");
            return;
        }

        uiList.Remove(ui); // 리스트에서 제거
        Destroy(ui.canvas.gameObject); // 캔버스 파괴
        //Debug.Log($"{typeof(T).Name} UI가 제거되었습니다.");
    }

    // UI를 인스턴스화하는 메서드
    private T InstantiateUI<T>(BaseUI uiPrefab) where T : BaseUI
    {
        // 새 캔버스 생성 및 설정
        GameObject newCanvasObject = new GameObject(typeof(T).Name + " Canvas");
        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        newCanvasObject.AddComponent<GraphicRaycaster>();

        // UI 프리팹 인스턴스화
        BaseUI uiInstance = Instantiate(uiPrefab, newCanvasObject.transform);
        uiInstance.name = uiPrefab.name.Replace("(Clone)", "");
        uiInstance.canvas = canvas;

        return (T)uiInstance;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void Hide(System.Type type)
    {
        BaseUI ui = uiList.Find(x => x.GetType() == type);
        if (ui == null)
        {
            Debug.LogError($"{type.Name} UI를 찾을 수 없습니다.");
            return;
        }

        uiList.Remove(ui); // 리스트에서 제거
        Destroy(ui.canvas.gameObject); // 캔버스 파괴
        Debug.Log($"{type.Name} UI가 제거되었습니다.");
    }


}
