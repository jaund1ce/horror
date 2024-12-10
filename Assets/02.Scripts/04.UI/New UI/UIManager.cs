using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : mainSingleton<UIManager>
{
    private List<BaseUI> uiList = new List<BaseUI>(); // 인스턴스화된 UI 저장


    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트 등록
    }


    private void Initalize()
    {
        uiList.RemoveAll(item => item == null);
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    } 

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initalize();
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
        Debug.Log($"{typeof(T).Name} UI가 생성되었습니다.");
    }

    public void Hide<T>() where T : BaseUI
    {
        
        // 인스턴스화된 UI를 찾음
        BaseUI ui = uiList.Find(x => x is T);
        if (ui == null)
        {
            Debug.LogError($"{typeof(T).Name} UI를 찾을 수 없습니다.");
            return;
        }

        uiList.Remove(ui); // 리스트에서 제거
        Destroy(ui.canvas.gameObject); // 캔버스 파괴
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
