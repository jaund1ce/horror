using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : mainSingleton<UIManager>
{
    private List<BaseUI> uiList = new List<BaseUI>(); // 일반 UI 및 팝업 UI 프리팹 저장

    protected override void Awake()
    {
        base.Awake();
        LoadAllUIs(); // Resources 폴더에서 모든 UI 프리팹 로드
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트 등록
    }

    private void LoadAllUIs()
    {
        // Resources 폴더의 "UI/" 디렉토리에서 모든 BaseUI를 로드
        BaseUI[] loadedUIs = Resources.LoadAll<BaseUI>("UI/");
        uiList.AddRange(loadedUIs);
        Debug.Log($"{uiList.Count}개의 UI가 로드되었습니다.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에 따라 적절한 UI 표시
        if (scene.buildIndex == 0)
        {
            Show<StartUI>();
        }
        else if (scene.buildIndex == 1)
        {
            Show<MainUI>();
        }
        else if (scene.buildIndex == 2)
        {
            Show<EndUI>();
        }
    }

    // 일반 UI 표시
    public void Show<T>() where T : BaseUI
    {

        string uiName = typeof(T).ToString();
        // 일반 UI (PopupUI 제외)를 검색하여 표시
        BaseUI ui = uiList.Find(x => x is T && !(x is PopupUI));
        if (ui == null)
        {
            Debug.LogError($"{typeof(T).Name} UI를 찾을 수 없습니다.");
            return;
        }
        InstantiateUI<T>(ui);
    }


    public void Hide<T>() where T : BaseUI
    {
        // 일반 UI (PopupUI 제외)를 검색하여 제거
        BaseUI ui = uiList.Find(x => x is T && !(x is PopupUI));
        if (ui == null)
        {
            Debug.LogError($"{typeof(T).Name} UI를 찾을 수 없습니다.");
            return;
        }

        Destroy(ui.canvas.gameObject);
        Debug.Log($"{typeof(T).Name} UI가 제거되었습니다.");
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
        SceneManager.sceneLoaded -= OnSceneLoaded; // 씬 로드 이벤트 해제
    }

    public void Hide(string uiName)
    {
        BaseUI ui = uiList.Find(x => x.name == uiName); // 이름으로 UI 검색
        if (ui != null)
        {
            uiList.Remove(ui);                   // UI 리스트에서 제거
            Destroy(ui.canvas.gameObject);       // UI 캔버스 제거
        }
    }
}
