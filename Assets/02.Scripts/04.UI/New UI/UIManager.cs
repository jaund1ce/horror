using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : mainSingleton<UIManager>
{
    private List<BaseUI> uiList = new List<BaseUI>(); // �ν��Ͻ�ȭ�� UI ����
    public int paperInteractionCount; // �ܼ� UI �ر� ���� Count

    private MainUI mainUI; // �ν����Ϳ��� �ִ°Ŵ� ������Ʈ�� ������ ������Ʈ�� ������ �����ϰ������ �ν��Ͻÿ���Ʈ
    //�� ���̸� ��Ʈ�� �ϰ�; miss ���°�


    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded; // �� �ε� �̺�Ʈ ���
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

        paperInteractionCount = 0;

        Time.timeScale = 1f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initalize();
        // ���� ���� ������ UI ǥ��
        if (scene.buildIndex == 0)
        {
            StartCoroutine(DelayShowUI());
        }
        else if (scene.buildIndex == 1)
        {
            Show<MainUI>();
            mainUI = GetUI<MainUI>();
        }
        else if (scene.buildIndex == 2)
        {
            Show<EndUI>();
        }
    }

    //�׷� ���� �ڵ忡�� �ش� UI�� �������� ��ũ��Ʈ �ۼ�
    //�ش� ��ũ��Ʈ�� �� �Ŵ������� ���� �ε�ɶ� �����ϴ°��� �ۼ��ϴ°� ����
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

    // �Ϲ� UI ǥ��
    public void Show<T>() where T : BaseUI
    {
        BaseUI existingUI = uiList.Find(x => x is T);
        if (existingUI != null)
        {
            Debug.LogWarning($"{typeof(T).Name} UI�� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
            Hide<T>();
            return;
        }
        BaseUI uiPrefab = Resources.Load<BaseUI>("UI/" + typeof(T).Name); // ������ �ε�
        if (uiPrefab == null)
        {
            Debug.LogError($"{typeof(T).Name} UI �������� ã�� �� �����ϴ�.");
            return;
        }

        T uiInstance = InstantiateUI<T>(uiPrefab); // UI �ν��Ͻ� ����
        uiList.Add(uiInstance);                   // �ν��Ͻ�ȭ�� UI�� ����Ʈ�� �߰�
        Debug.Log($"{typeof(T).Name} UI�� �����Ǿ����ϴ�.");
    }

    public void Hide<T>() where T : BaseUI
    {
        
        // �ν��Ͻ�ȭ�� UI�� ã��
        BaseUI ui = uiList.Find(x => x is T);
        if (ui == null)
        {
            //Debug.LogError($"{typeof(T).Name} UI�� ã�� �� �����ϴ�.");
            return;
        }

        uiList.Remove(ui); // ����Ʈ���� ����
        Destroy(ui.canvas.gameObject); // ĵ���� �ı�
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

    public void Hide(System.Type type)
    {
        BaseUI ui = uiList.Find(x => x.GetType() == type);
        if (ui == null)
        {
            Debug.LogError($"{type.Name} UI�� ã�� �� �����ϴ�.");
            return;
        }

        uiList.Remove(ui); // ����Ʈ���� ����
        Destroy(ui.canvas.gameObject); // ĵ���� �ı�
        Debug.Log($"{type.Name} UI�� ���ŵǾ����ϴ�.");
    }

    public void ActivePromptUI(IInteractable CurrentInteracteable) 
    {
        mainUI.ShowPromptUI(CurrentInteracteable);
    }
}
