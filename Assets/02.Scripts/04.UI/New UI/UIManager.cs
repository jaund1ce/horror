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
    }

    protected override void Start()
    {
        // 현재 활성화된 씬 가져오기
        Scene currentScene = SceneManager.GetActiveScene();

        // 씬 빌드 인덱스 확인
        if (currentScene.buildIndex == 0)
        {
            Debug.Log("Start Scene에서 실행 중"); 
            Show<StartUI>();
        }

        else if (currentScene.buildIndex == 1)
        {
            Debug.Log("Main Scene에서 실행 중");
            Show<MainUI>();
        }

        else if (currentScene.buildIndex == 2)
        {
            Debug.Log("End Scene에서 실행 중");
            Show<EndUI>();
        }

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
        if (ui == null)
        {
            Debug.LogError($"{typeof(T).Name} UI를 찾을 수 없습니다.");
            return null;
        }

      
        if (ui is PopupUI popup)
        {
            popupStack.Push(popup);
        }

        return InstantiateUI<T>(ui); 
    }

    public void Hide<T>() where T : BaseUI
    {
       
        if (typeof(T) == typeof(PopupUI) && popupStack.Count > 0)
        {
            PopupUI topPopup = popupStack.Pop();
            Destroy(topPopup.canvas.gameObject); 
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
