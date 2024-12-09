using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : mainSingleton<UIManager>
{
    private List<BaseUI> uiList = new List<BaseUI>(); // �Ϲ� UI �� �˾� UI ������ ����

    protected override void Awake()
    {
        base.Awake();
        LoadAllUIs(); // Resources �������� ��� UI ������ �ε�
        SceneManager.sceneLoaded += OnSceneLoaded; // �� �ε� �̺�Ʈ ���
    }

    private void LoadAllUIs()
    {
        // Resources ������ "UI/" ���丮���� ��� BaseUI�� �ε�
        BaseUI[] loadedUIs = Resources.LoadAll<BaseUI>("UI/");
        uiList.AddRange(loadedUIs);
        Debug.Log($"{uiList.Count}���� UI�� �ε�Ǿ����ϴ�.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� ���� ������ UI ǥ��
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

    // �Ϲ� UI ǥ��
    public void Show<T>() where T : BaseUI
    {

        string uiName = typeof(T).ToString();
        // �Ϲ� UI (PopupUI ����)�� �˻��Ͽ� ǥ��
        BaseUI ui = uiList.Find(x => x is T && !(x is PopupUI));
        if (ui == null)
        {
            Debug.LogError($"{typeof(T).Name} UI�� ã�� �� �����ϴ�.");
            return;
        }
        InstantiateUI<T>(ui);
    }


    public void Hide<T>() where T : BaseUI
    {
        // �Ϲ� UI (PopupUI ����)�� �˻��Ͽ� ����
        BaseUI ui = uiList.Find(x => x is T && !(x is PopupUI));
        if (ui == null)
        {
            Debug.LogError($"{typeof(T).Name} UI�� ã�� �� �����ϴ�.");
            return;
        }

        Destroy(ui.canvas.gameObject);
        Debug.Log($"{typeof(T).Name} UI�� ���ŵǾ����ϴ�.");
    }

    // UI�� �ν��Ͻ�ȭ�ϴ� �޼���
    private T InstantiateUI<T>(BaseUI uiPrefab) where T : BaseUI
    {
        // �� ĵ���� ���� �� ����
        GameObject newCanvasObject = new GameObject(typeof(T).Name + " Canvas");
        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        newCanvasObject.AddComponent<GraphicRaycaster>();

        // UI ������ �ν��Ͻ�ȭ
        BaseUI uiInstance = Instantiate(uiPrefab, newCanvasObject.transform);
        uiInstance.name = uiPrefab.name.Replace("(Clone)", "");
        uiInstance.canvas = canvas;

        return (T)uiInstance;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoaded; // �� �ε� �̺�Ʈ ����
    }

    public void Hide(string uiName)
    {
        BaseUI ui = uiList.Find(x => x.name == uiName); // �̸����� UI �˻�
        if (ui != null)
        {
            uiList.Remove(ui);                   // UI ����Ʈ���� ����
            Destroy(ui.canvas.gameObject);       // UI ĵ���� ����
        }
    }
}
