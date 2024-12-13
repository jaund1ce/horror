using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ElectricalPuzzleController
/// - 박스를 열고 닫을 수 있는 인터랙션 구현
/// - 박스를 열면 퍼즐 카메라로 전환
/// - 퍼즐은 WirePanel 하위 오브젝트를 클릭하여 작동
/// - 와이어 연결 상태를 확인하고, 퍼즐 완성 시 성공 사운드 재생
/// - Cinemachine을 사용한 카메라 전환 포함
/// </summary>
public class ElectricalPuzzleController : MonoBehaviour
{
    [Header("박스 문 관련 설정")]
    public Transform boxDoor; // 박스 문 Transform
    public float openAngle = 90f; // 문을 여는 각도
    public float openDistance = 3f; // 박스와 상호작용 가능한 최대 거리

    [Header("카메라 설정")]
    public CinemachineVirtualCamera puzzleCamera; // 퍼즐 보기용 Cinemachine 카메라
    public CinemachineVirtualCamera playerCamera; // 플레이어 보기용 Cinemachine 카메라

    [Header("퍼즐 구성 요소")]
    public Transform interactiveParent; // WirePanel 오브젝트들의 부모
    public GameObject[] outputPanels; // 출력 패널
    public GameObject[] inputPanels; // 입력 패널

    [Header("오디오 설정")]
    public AudioClip clickSoundClip; // 와이어 블록 클릭 시 재생되는 사운드 클립
    public AudioClip successSoundClip; // 퍼즐 완성 시 재생되는 사운드 클립
    public AudioSource audioSource; // 오디오 재생기

    private bool isBoxOpen = false; // 박스가 열려 있는지 여부
    private bool isPuzzleActive = false; // 퍼즐 상태 활성화 여부
    private bool isPlayerInRange = false; // 플레이어가 상호작용 범위 내에 있는지 여부

    private Quaternion closedRotation; // 박스 문 닫힌 상태의 회전값
    private Quaternion openRotation; // 박스 문 열린 상태의 회전값

    void Start()
    {
        // 박스 문 초기 회전값 설정
        closedRotation = boxDoor.rotation;
        openRotation = Quaternion.Euler(boxDoor.eulerAngles.x, boxDoor.eulerAngles.y + openAngle, boxDoor.eulerAngles.z);

        // 초기 카메라 우선순위 설정
        puzzleCamera.Priority = 0;
        playerCamera.Priority = 1;

        // 초기 마우스 커서 상태
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // E 키 입력으로 상호작용 시도
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            TryInteract();
        }

        // Q 키 입력으로 퍼즐 보기 종료
        if (isPuzzleActive && Input.GetKeyDown(KeyCode.Q))
        {
            ExitPuzzleView();
        }

        // 마우스 클릭으로 퍼즐 오브젝트 선택
        if (isPuzzleActive && Input.GetMouseButtonDown(0))
        {
            TrySelectWirePanel();
        }
    }

    /// <summary>
    /// 플레이어와 박스 상호작용 시도
    /// </summary>
    void TryInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, openDistance))
        {
            if (hit.transform == boxDoor)
            {
                ToggleBoxDoor();
            }
            else if (isBoxOpen && hit.collider.CompareTag("WirePanel"))
            {
                EnterPuzzleView();
            }
        }
    }

    /// <summary>
    /// 퍼즐 카메라에서 WirePanel 오브젝트 선택
    /// </summary>
    void TrySelectWirePanel()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.IsChildOf(interactiveParent))
            {
                RotateWirePanel(hit.transform.gameObject);
            }
        }
    }

    /// <summary>
    /// 박스 문 열기/닫기
    /// </summary>
    void ToggleBoxDoor()
    {
        if (isBoxOpen)
        {
            boxDoor.rotation = closedRotation;
        }
        else
        {
            boxDoor.rotation = openRotation;
        }
        isBoxOpen = !isBoxOpen;
    }

    /// <summary>
    /// 퍼즐 카메라로 전환
    /// </summary>
    void EnterPuzzleView()
    {
        isPuzzleActive = true;
        puzzleCamera.Priority = 2;
        playerCamera.Priority = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f; // 시간은 계속 흐름
    }

    /// <summary>
    /// 플레이어 카메라로 복귀
    /// </summary>
    void ExitPuzzleView()
    {
        isPuzzleActive = false;
        puzzleCamera.Priority = 0;
        playerCamera.Priority = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// WirePanel 오브젝트 회전
    /// </summary>
    /// <param name="panel">회전할 패널 오브젝트</param>
    public void RotateWirePanel(GameObject panel)
    {
        panel.transform.Rotate(0, 0, 90);
        PlayClickSound();
        CheckPuzzleCompletion();
    }

    /// <summary>
    /// 클릭 사운드 재생
    /// </summary>
    void PlayClickSound()
    {
        if (clickSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSoundClip);
        }
    }

    /// <summary>
    /// 퍼즐 완성 여부 확인
    /// </summary>
    void CheckPuzzleCompletion()
    {
        bool allConnected = true;

        // 퍼즐 검증 로직 추가
        foreach (GameObject output in outputPanels)
        {
            // 예: 출력 패널 연결 상태 검증 로직
        }

        if (allConnected)
        {
            PlaySuccessSound();
            Debug.Log("Puzzle Solved!");
            // 퍼즐 완성 로직 추가
        }
    }

    /// <summary>
    /// 퍼즐 완성 사운드 재생
    /// </summary>
    void PlaySuccessSound()
    {
        if (successSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(successSoundClip);
        }
    }

    /// <summary>
    /// 콜라이더 진입 감지
    /// </summary>
    /// <param name="other">트리거에 진입한 Collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    /// <summary>
    /// 콜라이더 이탈 감지
    /// </summary>
    /// <param name="other">트리거에서 이탈한 Collider</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}








