using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Xml.Linq;

public class UIManager : mainSingleton<UIManager>
{

    public static int ScreenWidth = 1920; // ȭ�� �ʺ� ���� �ػ󵵸� �����մϴ�.
    public static int ScreenHeight = 1080; // ȭ�� ���� ���� �ػ󵵸� �����մϴ�.

    public static Transform UITransform
    {
        get
        {
            if (uiTransform == null) // uiTransform�� null�̸�
                uiTransform = FindFirstObjectByType<SceneBase>().uiTransform;  // ���� Ȱ��ȭ�� SceneBase�� uiTransform�� �����ɴϴ�.
            return uiTransform; // uiTransform�� ��ȯ�մϴ�.
        }
        set { uiTransform = value; }  // uiTransform�� �����մϴ�.
    }

    private Dictionary<string, BaseUI> uiList = new Dictionary<string, BaseUI>();

    private MainUI mainUI;
    // �ν����Ϳ��� �ִ°Ŵ� ������Ʈ�� ������ ������Ʈ�� ������ �����ϰ������ �ν��Ͻÿ���Ʈ
    //�� ���̸� ��Ʈ�� �ϰ�; miss ���°�

    private static Transform uiTransform;


    protected override void Awake()
    {
        base.Awake();
    }




    //�׷� ���� �ڵ忡�� �ش� UI�� �������� ��ũ��Ʈ �ۼ�
    //�ش� ��ũ��Ʈ�� �� �Ŵ������� ���� �ε�ɶ� �����ϴ°��� �ۼ��ϴ°� ����
    public T GetUI<T>() where T : BaseUI 
    {
        string uiName = typeof(T).ToString();
        uiList.TryGetValue(uiName, out BaseUI existingUI);
        if(existingUI == null)return null;
        return existingUI.GetComponent<T>();
    }


    // �Ϲ� UI ǥ��
    public T Show<T>() where T : BaseUI
    {
        RemoveNull(); // Dictionary���� null ���۷����� �����Ͽ� �����͸� �����մϴ�.
                      // �̴� �޸� ������ �����ϰ� UI ������ �ϰ����� �����ϱ� �����Դϴ�.
        string uiName = typeof(T).ToString();
        uiList.TryGetValue(uiName, out  BaseUI ui);

        if (ui != null) // ������ �̸��� UI�� �̹� Ȱ��ȭ�� ���
        {
            Hide<T>(); // Ȱ��ȭ�� UI�� ����ϴ�.
            return null; // �̹� Ȱ��ȭ�� ��� ���ο� UI�� ��ȯ���� �ʽ��ϴ�.
        }

        if (ui == null)  // �˻��� UI ��ü�� ������
        {
            uiList.Remove(uiName); // uiList���� uiName Ű�� �����մϴ�.
            var obj = ResourceManager.Instance.LoadAsset<GameObject>(uiName, eAssetType.UI); // ResourceManager���� UI Prefab�� �ε��մϴ�.
            ui = LoadUI<T>(obj, uiName); // Prefab�� �ν��Ͻ�ȭ�ϰ� UI ��ü�� �����մϴ�.
            uiList.Add(uiName, ui);  // ������ UI ��ü�� uiList�� �߰��մϴ�.
            ui.OpenUI(); 
        }

        ui.gameObject.SetActive(true); // UI GameObject�� Ȱ��ȭ�մϴ�.

        return (T)ui; // ������ UI ��ü�� ��ȯ�մϴ�.                 


    }


    // UI�� �ν��Ͻ�ȭ�ϴ� �޼���
    public T LoadUI<T>(GameObject prefab, string uiName) where T : BaseUI
    {
        var newCanvasObject = new GameObject(uiName + " Canvas"); // �� Canvas GameObject�� �����մϴ�.

        newCanvasObject.transform.SetParent(UIManager.UITransform); // ������ Canvas�� UI �θ� Transform�� �����մϴ�.

        var canvas = newCanvasObject.gameObject.AddComponent<Canvas>(); // Canvas ������Ʈ�� �߰��մϴ�.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Canvas�� ȭ�� ��ü�� ǥ�õǵ��� �����մϴ�.

        var canvasScaler = newCanvasObject.gameObject.AddComponent<CanvasScaler>(); // CanvasScaler ������Ʈ�� �߰��մϴ�.
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; // ȭ�� ũ�⿡ ���� UI�� �����ϸ��մϴ�.
        canvasScaler.referenceResolution = new Vector2(UIManager.ScreenWidth, UIManager.ScreenHeight);  // ���� �ػ󵵸� �����մϴ�.
        newCanvasObject.gameObject.AddComponent<GraphicRaycaster>();// UI �̺�Ʈ ó���� ���� GraphicRaycaster ������Ʈ�� �߰��մϴ�.

        var obj = Instantiate(prefab, newCanvasObject.transform); // Prefab�� �ν��Ͻ�ȭ�Ͽ� Canvas�� �ڽ����� �߰��մϴ�.
        obj.name = obj.name.Replace("(Clone)", ""); // ���纻 �̸����� "(Clone)"�� �����մϴ�.

        var result = obj.GetComponent<T>(); // �ν��Ͻ�ȭ�� ��ü���� T ������ ������Ʈ�� �����ɴϴ�.
        result.canvas = canvas; // ������ Canvas�� UI ��ü�� �����մϴ�.
        result.canvas.sortingOrder = uiList.Count; // UI�� ���� ������ �����մϴ�.

        return result; // ������ UI ��ü�� ��ȯ�մϴ�.
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    public void Hide<T>()
    {
        string uiName = typeof(T).ToString(); // T ������ �̸��� uiName�� �����մϴ�.

        Hide(uiName); // uiName�� ����Ͽ� UI�� ����ϴ�.
    }

    public void Hide(string uiName)
    {
        uiList.TryGetValue(uiName, out BaseUI ui); // uiList���� uiName Ű�� UI ��ü�� �˻��մϴ�.

        if (ui == null) // �˻��� UI ��ü�� ������
            return; // �޼��带 �����մϴ�.

        DestroyImmediate(ui.canvas.gameObject); // Canvas GameObject�� ��� �����մϴ�.
        uiList.Remove(uiName); // uiList���� uiName Ű�� �����մϴ�.
    }

    void RemoveNull()
    {
        List<string> tempList = new List<string>(uiList.Count); // �ӽ� ����Ʈ�� �����Ͽ� null ���۷����� �����մϴ�.
        foreach (var temp in uiList)
        {
            if (temp.Value == null) // UI ��ü�� null�� ���
                tempList.Add(temp.Key); // Ű�� �ӽ� ����Ʈ�� �߰��մϴ�.
        }

        foreach (var temp in tempList)// tempList�� ����� null Ű�� �ϳ��� ��ȸ�մϴ�.
                                      // �� Ű�� uiList���� ���ŵ˴ϴ�. �̸� ���� UI ����Ʈ�� ���ռ��� �����մϴ�.
        {
            uiList.Remove(temp); // ������ null Ű�� uiList���� �����մϴ�.
        }
    }
}

