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
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� Ȱ��ȭ�� �� ��������
        Debug.Log($"���ο� �� �ε��: {scene.name}");

        // �� �̸� �Ǵ� ���� �ε����� ������� UI ǥ��
        if (scene.buildIndex == 0)
        {
            Debug.Log("Start Scene���� ���� ��");
            Show<StartUI>();
        }
        else if (scene.buildIndex == 1)
        {
            Debug.Log("Main Scene���� ���� ��");
            Show<MainUI>();
        }
        else if (scene.buildIndex == 2)
        {
            Debug.Log("End Scene���� ���� ��");
            Show<EndUI>();
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        // �� �ε� �̺�Ʈ ��� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void LoadAllUIs()
    {
       
        BaseUI[] loadedUIs = Resources.LoadAll<BaseUI>("UI/");
        uiList.AddRange(loadedUIs);
        Debug.Log($"{uiList.Count}���� UI�� �ε�Ǿ����ϴ�.");
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
            Debug.LogError($"{typeof(T).Name} UI�� ã�� �� �����ϴ�.");
            return null;
        }

      
        

        return InstantiateUI<T>(ui); 
    }

    public void TogglePopup<T>() where T : PopupUI
    {
        // ���ÿ� �˾��� �ְ�, ���� ���� �˾��� T Ÿ���̶�� ����
        if (popupStack.Count > 0 && popupStack.Peek() is T)
        {
            Hide<T>(); // �˾� �ݱ�
        }
        else
        {
            Show<T>(); // �˾� ����
        }
    }

    public void Hide<T>() where T : BaseUI
    {
       
        if (typeof(T) == typeof(PopupUI) && popupStack.Count > 0)
        {
            PopupUI topPopup = popupStack.Pop();
            topPopup.CloseUI();                 
            Destroy(topPopup.canvas.gameObject);
            Debug.Log($"���� ����: {popupStack.Count}���� �˾� ����");
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

        // UI ������ �ν��Ͻ� ����
        BaseUI uiInstance = Instantiate(uiPrefab, newCanvasObject.transform);
        uiInstance.name = uiPrefab.name.Replace("(Clone)", ""); 
        uiInstance.canvas = canvas;


        return (T)uiInstance; // ������ UI ��ȯ
    }
}
