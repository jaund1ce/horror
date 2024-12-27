using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Xml.Linq;

public class UIManager : mainSingleton<UIManager>
{

    public static int ScreenWidth = 1920; // 화면 너비 기준 해상도를 설정합니다.
    public static int ScreenHeight = 1080; // 화면 높이 기준 해상도를 설정합니다.

    public static Transform UITransform
    {
        get
        {
            if (uiTransform == null) // uiTransform이 null이면
                uiTransform = FindFirstObjectByType<SceneBase>().uiTransform;  // 현재 활성화된 SceneBase의 uiTransform을 가져옵니다.
            return uiTransform; // uiTransform을 반환합니다.
        }
        set { uiTransform = value; }  // uiTransform을 설정합니다.
    }

    private Dictionary<string, BaseUI> uiList = new Dictionary<string, BaseUI>();

    private MainUI mainUI;
    // 인스펙터에서 넣는거는 프로젝트의 원본의 오브젝트고 실제로 동작하고싶은건 인스턴시에이트
    //된 아이를 컨트롤 하고싶어서 miss 나는것

    private static Transform uiTransform;


    protected override void Awake()
    {
        base.Awake();
    }




    //그럼 직접 코드에서 해당 UI를 가져오는 스크립트 작성
    //해당 스크립트는 씬 매니져에서 씬이 로드될때 동작하는곳에 작성하는게 좋다
    public T GetUI<T>() where T : BaseUI 
    {
        string uiName = typeof(T).ToString();
        uiList.TryGetValue(uiName, out BaseUI existingUI);
        if(existingUI == null)return null;
        return existingUI.GetComponent<T>();
    }


    // 일반 UI 표시
    public T Show<T>() where T : BaseUI
    {
        RemoveNull(); // Dictionary에서 null 레퍼런스를 제거하여 데이터를 정리합니다.
                      // 이는 메모리 누수를 방지하고 UI 관리의 일관성을 유지하기 위함입니다.
        string uiName = typeof(T).ToString();
        uiList.TryGetValue(uiName, out  BaseUI ui);

        if (ui != null) // 동일한 이름의 UI가 이미 활성화된 경우
        {
            Hide<T>(); // 활성화된 UI를 숨깁니다.
            return null; // 이미 활성화된 경우 새로운 UI를 반환하지 않습니다.
        }

        if (ui == null)  // 검색된 UI 객체가 없으면
        {
            uiList.Remove(uiName); // uiList에서 uiName 키를 제거합니다.
            var obj = ResourceManager.Instance.LoadAsset<GameObject>(uiName, eAssetType.UI); // ResourceManager에서 UI Prefab을 로드합니다.
            ui = LoadUI<T>(obj, uiName); // Prefab을 인스턴스화하고 UI 객체를 생성합니다.
            uiList.Add(uiName, ui);  // 생성된 UI 객체를 uiList에 추가합니다.
            ui.OpenUI(); 
        }

        ui.gameObject.SetActive(true); // UI GameObject를 활성화합니다.

        return (T)ui; // 생성된 UI 객체를 반환합니다.                 


    }


    // UI를 인스턴스화하는 메서드
    public T LoadUI<T>(GameObject prefab, string uiName) where T : BaseUI
    {
        var newCanvasObject = new GameObject(uiName + " Canvas"); // 새 Canvas GameObject를 생성합니다.

        newCanvasObject.transform.SetParent(UIManager.UITransform); // 생성된 Canvas를 UI 부모 Transform에 연결합니다.

        var canvas = newCanvasObject.gameObject.AddComponent<Canvas>(); // Canvas 컴포넌트를 추가합니다.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Canvas를 화면 전체에 표시되도록 설정합니다.

        var canvasScaler = newCanvasObject.gameObject.AddComponent<CanvasScaler>(); // CanvasScaler 컴포넌트를 추가합니다.
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; // 화면 크기에 맞춰 UI를 스케일링합니다.
        canvasScaler.referenceResolution = new Vector2(UIManager.ScreenWidth, UIManager.ScreenHeight);  // 기준 해상도를 설정합니다.
        newCanvasObject.gameObject.AddComponent<GraphicRaycaster>();// UI 이벤트 처리를 위한 GraphicRaycaster 컴포넌트를 추가합니다.

        var obj = Instantiate(prefab, newCanvasObject.transform); // Prefab을 인스턴스화하여 Canvas의 자식으로 추가합니다.
        obj.name = obj.name.Replace("(Clone)", ""); // 복사본 이름에서 "(Clone)"을 제거합니다.

        var result = obj.GetComponent<T>(); // 인스턴스화된 객체에서 T 형식의 컴포넌트를 가져옵니다.
        result.canvas = canvas; // 생성된 Canvas를 UI 객체와 연결합니다.
        result.canvas.sortingOrder = uiList.Count; // UI의 정렬 순서를 설정합니다.

        return result; // 생성된 UI 객체를 반환합니다.
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    public void Hide<T>()
    {
        string uiName = typeof(T).ToString(); // T 형식의 이름을 uiName에 저장합니다.

        Hide(uiName); // uiName을 사용하여 UI를 숨깁니다.
    }

    public void Hide(string uiName)
    {
        uiList.TryGetValue(uiName, out BaseUI ui); // uiList에서 uiName 키로 UI 객체를 검색합니다.

        if (ui == null) // 검색된 UI 객체가 없으면
            return; // 메서드를 종료합니다.

        DestroyImmediate(ui.canvas.gameObject); // Canvas GameObject를 즉시 삭제합니다.
        uiList.Remove(uiName); // uiList에서 uiName 키를 제거합니다.
    }

    void RemoveNull()
    {
        List<string> tempList = new List<string>(uiList.Count); // 임시 리스트를 생성하여 null 레퍼런스를 수집합니다.
        foreach (var temp in uiList)
        {
            if (temp.Value == null) // UI 객체가 null인 경우
                tempList.Add(temp.Key); // 키를 임시 리스트에 추가합니다.
        }

        foreach (var temp in tempList)// tempList에 저장된 null 키를 하나씩 순회합니다.
                                      // 각 키는 uiList에서 제거됩니다. 이를 통해 UI 리스트의 정합성을 유지합니다.
        {
            uiList.Remove(temp); // 수집된 null 키를 uiList에서 제거합니다.
        }
    }
}

