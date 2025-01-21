using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ElectricalPuzzleController
/// - �ڽ��� ���� ���� �� �ִ� ���ͷ��� ����
/// - �ڽ��� ���� ���� ī�޶�� ��ȯ
/// - ������ WirePanel ���� ������Ʈ�� Ŭ���Ͽ� �۵�
/// - ���̾� ���� ���¸� Ȯ���ϰ�, ���� �ϼ� �� ���� ���� ���
/// - Cinemachine�� ����� ī�޶� ��ȯ ����
/// </summary>
public class ElectricalPuzzleController : MonoBehaviour
{
    [Header("�ڽ� �� ���� ����")]
    public Transform boxDoor; // �ڽ� �� Transform
    public float openAngle = 90f; // ���� ���� ����
    public float openDistance = 3f; // �ڽ��� ��ȣ�ۿ� ������ �ִ� �Ÿ�

    [Header("ī�޶� ����")]
    public CinemachineVirtualCamera puzzleCamera; // ���� ����� Cinemachine ī�޶�
    public CinemachineVirtualCamera playerCamera; // �÷��̾� ����� Cinemachine ī�޶�

    [Header("���� ���� ���")]
    public Transform interactiveParent; // WirePanel ������Ʈ���� �θ�
    public GameObject[] outputPanels; // ��� �г�
    public GameObject[] inputPanels; // �Է� �г�

    [Header("����� ����")]
    public AudioClip clickSoundClip; // ���̾� ��� Ŭ�� �� ����Ǵ� ���� Ŭ��
    public AudioClip successSoundClip; // ���� �ϼ� �� ����Ǵ� ���� Ŭ��
    public AudioSource audioSource; // ����� �����

    private bool isBoxOpen = false; // �ڽ��� ���� �ִ��� ����
    private bool isPuzzleActive = false; // ���� ���� Ȱ��ȭ ����
    private bool isPlayerInRange = false; // �÷��̾ ��ȣ�ۿ� ���� ���� �ִ��� ����

    private Quaternion closedRotation; // �ڽ� �� ���� ������ ȸ����
    private Quaternion openRotation; // �ڽ� �� ���� ������ ȸ����

    void Start()
    {
        // �ڽ� �� �ʱ� ȸ���� ����
        closedRotation = boxDoor.rotation;
        openRotation = Quaternion.Euler(boxDoor.eulerAngles.x, boxDoor.eulerAngles.y + openAngle, boxDoor.eulerAngles.z);

        // �ʱ� ī�޶� �켱���� ����
        puzzleCamera.Priority = 0;
        playerCamera.Priority = 1;

        // �ʱ� ���콺 Ŀ�� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // E Ű �Է����� ��ȣ�ۿ� �õ�
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            TryInteract();
        }

        // Q Ű �Է����� ���� ���� ����
        if (isPuzzleActive && Input.GetKeyDown(KeyCode.Q))
        {
            ExitPuzzleView();
        }

        // ���콺 Ŭ������ ���� ������Ʈ ����
        if (isPuzzleActive && Input.GetMouseButtonDown(0))
        {
            TrySelectWirePanel();
        }
    }

    /// <summary>
    /// �÷��̾�� �ڽ� ��ȣ�ۿ� �õ�
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
    /// ���� ī�޶󿡼� WirePanel ������Ʈ ����
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
    /// �ڽ� �� ����/�ݱ�
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
    /// ���� ī�޶�� ��ȯ
    /// </summary>
    void EnterPuzzleView()
    {
        isPuzzleActive = true;
        puzzleCamera.Priority = 2;
        playerCamera.Priority = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f; // �ð��� ��� �帧
    }

    /// <summary>
    /// �÷��̾� ī�޶�� ����
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
    /// WirePanel ������Ʈ ȸ��
    /// </summary>
    /// <param name="panel">ȸ���� �г� ������Ʈ</param>
    public void RotateWirePanel(GameObject panel)
    {
        panel.transform.Rotate(0, 0, 90);
        PlayClickSound();
        CheckPuzzleCompletion();
    }

    /// <summary>
    /// Ŭ�� ���� ���
    /// </summary>
    void PlayClickSound()
    {
        if (clickSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSoundClip);
        }
    }

    /// <summary>
    /// ���� �ϼ� ���� Ȯ��
    /// </summary>
    void CheckPuzzleCompletion()
    {
        bool allConnected = true;

        // ���� ���� ���� �߰�
        foreach (GameObject output in outputPanels)
        {
            // ��: ��� �г� ���� ���� ���� ����
        }

        if (allConnected)
        {
            PlaySuccessSound();
            Debug.Log("Puzzle Solved!");
            // ���� �ϼ� ���� �߰�
        }
    }

    /// <summary>
    /// ���� �ϼ� ���� ���
    /// </summary>
    void PlaySuccessSound()
    {
        if (successSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(successSoundClip);
        }
    }

    /// <summary>
    /// �ݶ��̴� ���� ����
    /// </summary>
    /// <param name="other">Ʈ���ſ� ������ Collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    /// <summary>
    /// �ݶ��̴� ��Ż ����
    /// </summary>
    /// <param name="other">Ʈ���ſ��� ��Ż�� Collider</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}








