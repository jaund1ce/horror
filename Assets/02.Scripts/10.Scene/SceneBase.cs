using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase : MonoBehaviour
{
    public Transform uiTransform; // UI 오브젝트가 배치될 부모 Transform을 저장합니다.
                                  // UIManager가 이를 참조하여 동적으로 생성된 UI를 적절한 위치에 배치합니다
                                  // 씬 전환 시에도 동일한 구조를 유지하기 위해 사용됩니다.
    public Transform mapTransform;

    protected virtual void Awake()
    {
        UIManager.UITransform = uiTransform; // UIManager의 정적 변수 UITransform에
                                             // 현재 씬의 uiTransform을 할당합니다.
                                             // 이를 통해 UIManager는 새롭게 생성된 UI를 올바르게 배치할 수 있습니다.
        MapManager.MapTransform = mapTransform;

        if (!Main_SceneManager.Instance.isDontDestroy) // Main_SceneManager의 isDontDestroy가 false인 경우
        {
            Main_SceneManager.Instance.ChangeScene("ManagerScene", () => // "ManagerScene" 씬을 Additive 모드로 로드하고
                                                                         // 콜백 함수 안에서 특정 작업을 수행합니다.
                                                                         // 여기서는 isDontDestroy를 true로 설정하고
                                                                         // 다시 "ManagerScene" 씬을 언로드합니다.
                                                                         // `()=>`는 람다 식(Lambda Expression)으로,
                                                                         // 매개변수가 없는 익명 메서드를 정의합니다.
                                                                         // { } 내부의 코드가 실행될 작업을 나타냅니다.
                                                                         // 이 식은 `Action` 타입으로 전달되며,
                                                                         // ChangeScene 호출이 끝난 후 실행됩니다.
            {
                Main_SceneManager.Instance.isDontDestroy = true; // isDontDestroy를 true로 설정하여
                                                                 // DontDestroyOnLoad 상태를 유지합니다.
                                                                 // 이는 특정 오브젝트가 씬 전환 시 삭제되지 않도록 하기 위함입니다.
                Main_SceneManager.Instance.UnLoadScene("ManagerScene"); // ManagerScene 씬을 언로드합니다.
                                                                        // 메모리 효율을 위해 더 이상 필요 없는 임시 씬을 제거합니다. 
            }, UnityEngine.SceneManagement.LoadSceneMode.Additive); // Additive 모드로 씬을 로드합니다.
                                                                    // 현재 씬 위에 추가적으로 로드하여 병행 작업을 수행할 수 있습니다.
        }
    }

    protected virtual void OnDestroy()
    {
        uiTransform = null; // uiTransform을 null로 설정하여 메모리 누수를 방지합니다.
        mapTransform = null;
    }
}
