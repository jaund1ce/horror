using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectricalPuzzleController : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform player;
    public Transform hinge;
    public float openAngle = -90f;
    public float closeAngle = 0f;
    public float openSpeed = 5f;
    public float interactDistance = 3f;

    private bool isDoorOpen = false;
    private bool isLookingAtPuzzle = false;

    [Header("Puzzle Settings")]
    public GameObject puzzleCamera; // 버추얼 카메라
    public GameObject playerCamera; // 플레이어 기본 카메라
    public Transform puzzleUI;
    public UnityEvent onPuzzleComplete;

    public int gridSize = 3;
    public PuzzleTile[,] tiles;

    private bool puzzleSolved = false;

    [Header("Player Settings")]
    public bool disablePlayerMovement = true;

    void Start()
    {
        // Initialize puzzle tiles
        tiles = new PuzzleTile[gridSize, gridSize];
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int childIndex = y * gridSize + x;
                Transform child = puzzleUI.GetChild(childIndex);

                if (child == null)
                {
                    Debug.LogError($"Child at index {childIndex} is missing!");
                    continue;
                }

                PuzzleTile tile = child.GetComponent<PuzzleTile>();
                if (tile == null)
                {
                    Debug.LogError($"Child at ({x}, {y}) is missing a PuzzleTile component!");
                    continue;
                }

                tiles[x, y] = tile;
                tile.Initialize(this, x, y);
            }
        }

        // Ensure cameras are set to default states
        if (puzzleCamera == null || playerCamera == null)
        {
            Debug.LogError("PuzzleCamera or PlayerCamera is not assigned in the Inspector!");
            return;
        }

        puzzleCamera.SetActive(false);
        playerCamera.SetActive(true);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // 플레이어와 문의 거리 체크
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            // Raycast 디버깅용 시각적 표시
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                Debug.Log($"Hit Object: {hit.transform.name}"); // Ray가 무엇을 감지했는지 확인

                // 퍼즐 타일 확인 (PuzzleTile 컴포넌트가 있는지 확인)
                PuzzleTile tile = hit.transform.GetComponent<PuzzleTile>();
                if (tile != null)
                {
                    Debug.Log($"Puzzle Tile hit detected: {hit.transform.name}");
                    EnterPuzzleView();
                }
                // 문 영역 확인
                else if (hit.transform == hinge)
                {
                    Debug.Log("Toggling door!"); // 디버깅 로그 추가
                    if (!isDoorOpen)
                    {
                        ToggleDoor(true); // 문 열기
                    }
                    else
                    {
                        ToggleDoor(false); // 문 닫기
                    }
                }
                else
                {
                    Debug.Log($"Hit object is not a puzzle tile or the door: {hit.transform.name}, Layer: {hit.transform.gameObject.layer}");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }

        // 퍼즐 뷰에서 Q키로 나가기
        if (isLookingAtPuzzle && Input.GetKeyDown(KeyCode.Q))
        {
            ExitPuzzleView();
        }
    }

    void ToggleDoor(bool open)
    {
        isDoorOpen = open;
        StartCoroutine(RotateDoor(open ? openAngle : closeAngle));
    }

    IEnumerator RotateDoor(float targetAngle)
    {
        float currentAngle = hinge.localEulerAngles.y;
        if (currentAngle > 180) currentAngle -= 360;

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * openSpeed);
            hinge.localEulerAngles = new Vector3(0, currentAngle, 0);
            yield return null;
        }

        hinge.localEulerAngles = new Vector3(0, targetAngle, 0);
    }

    void EnterPuzzleView()
    {
        if (puzzleCamera == null || playerCamera == null)
        {
            Debug.LogError("PuzzleCamera or PlayerCamera is not assigned in the Inspector!");
            return;
        }

        isLookingAtPuzzle = true;

        // Priority 설정
        var puzzleCam = puzzleCamera.GetComponent<CinemachineVirtualCamera>();
        var playerCam = playerCamera.GetComponent<CinemachineVirtualCamera>();

        if (puzzleCam != null && playerCam != null)
        {
            puzzleCam.Priority = 20;
            playerCam.Priority = 5;
            Debug.Log("Puzzle camera activated.");
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera component is missing on one of the cameras!");
        }
    }

    void ExitPuzzleView()
    {
        if (puzzleCamera == null || playerCamera == null)
        {
            Debug.LogError("PuzzleCamera or PlayerCamera is not assigned in the Inspector!");
            return;
        }

        isLookingAtPuzzle = false;

        // Priority 설정
        var puzzleCam = puzzleCamera.GetComponent<CinemachineVirtualCamera>();
        var playerCam = playerCamera.GetComponent<CinemachineVirtualCamera>();

        if (puzzleCam != null && playerCam != null)
        {
            puzzleCam.Priority = 5;
            playerCam.Priority = 20;
            Debug.Log("Player camera activated.");
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera component is missing on one of the cameras!");
        }
    }
    public void CheckPuzzleCompletion()
    {
        if (puzzleSolved) return;

        bool isComplete = true;
        foreach (var tile in tiles)
        {
            if (!tile.CheckAlignment())
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
        {
            puzzleSolved = true;
            onPuzzleComplete.Invoke();
        }
    }
}






