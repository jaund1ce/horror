using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class UIManager : mainSingleton<UIManager>
{
    private List<BaseUI> uiList = new List<BaseUI>(); 
    private Stack<PopupUI> popupStack = new Stack<PopupUI>();

    protected override void Awake()
    {
        base.Awake();
        LoadAllUIs();
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 활성화된 씬 가져오기
        Debug.Log($"새로운 씬 로드됨: {scene.name}");

        // 씬 이름 또는 빌드 인덱스를 기반으로 UI 표시
        if (scene.buildIndex == 0)
        {
            Debug.Log("Start Scene에서 실행 중");
            Show<StartUI>();
        }
        else if (scene.buildIndex == 1)
        {
            Debug.Log("Main Scene에서 실행 중");
            Show<MainUI>();
        }
        else if (scene.buildIndex == 2)
        {
            Debug.Log("End Scene에서 실행 중");
            Show<EndUI>();
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        // 씬 로드 이벤트 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void LoadAllUIs()
    {
       
        BaseUI[] loadedUIs = Resources.LoadAll<BaseUI>("UI/");
        uiList.AddRange(loadedUIs);
        Debug.Log($"{uiList.Count}개의 UI가 로드되었습니다.");
    }

    public T Show<T>() where T : BaseUI
    {
       
        BaseUI ui = uiList.Find(x => x is T);
        if (ui is PopupUI popup)
        {
            popupStack.Push(popup);
        }
        else if (ui == null)
        {
            Debug.LogError($"{typeof(T).Name} UI를 찾을 수 없습니다.");
            return null;
        }

      
        

        return InstantiateUI<T>(ui); 
    }

    public void TogglePopup<T>() where T : PopupUI
    {
        // 스택에 팝업이 있고, 가장 위의 팝업이 T 타입이라면 닫음
        if (popupStack.Count > 0 && popupStack.Peek() is T)
        {
            Hide<T>(); // 팝업 닫기
        }
        else
        {
            Show<T>(); // 팝업 열기
        }
    }

    public void Hide<T>() where T : BaseUI
    {
       
        if (typeof(T) == typeof(PopupUI) && popupStack.Count > 0)
        {
            PopupUI topPopup = popupStack.Pop();
            topPopup.CloseUI();                 
            Destroy(topPopup.canvas.gameObject);
            Debug.Log($"스택 상태: {popupStack.Count}개의 팝업 남음");
        }
        else
        {
           
            BaseUI ui = uiList.Find(x => x is T);
            if (ui != null)
            {
                Destroy(ui.canvas.gameObject);
            }
        }
    }

    private void Destroy(object gameObject)
    {
        throw new NotImplementedException();
    }

    private T InstantiateUI<T>(BaseUI uiPrefab) where T : BaseUI
    {
       
        GameObject newCanvasObject = new GameObject(typeof(T).Name + " Canvas");
        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; 
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        newCanvasObject.AddComponent<GraphicRaycaster>();

        // UI 프리팹 인스턴스 생성
        BaseUI uiInstance = Instantiate(uiPrefab, newCanvasObject.transform);
        uiInstance.name = uiPrefab.name.Replace("(Clone)", ""); 
        uiInstance.canvas = canvas;


        return (T)uiInstance; // 생성된 UI 반환
    }
}
