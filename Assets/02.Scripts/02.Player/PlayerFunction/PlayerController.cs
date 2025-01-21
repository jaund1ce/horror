using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs PlayerInputs {  get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions PlayerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��
    public Equipment EquipMent { get; private set; }

    [SerializeField] private Camera parkourCamera;
    [SerializeField] private CinemachineVirtualCamera playercamera;
    public CinemachineBasicMultiChannelPerlin VirtualCameraNoise;
    public GameObject Head;
    [SerializeField] float maxRotateY;

    [SerializeField]private float rotateSencitivity;
    public bool RunningReady = false;
    public bool CrouchingReady = false;
    private float currentYangle = 0f;
    private bool subscribed = false;

    private void Awake()
    {
        PlayerInputs = new PlayerInputs();
        PlayerActions = PlayerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        EquipMent = GetComponent<Equipment>();
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(parkourCamera);
    }

    private void OnEnable()
    {
        VirtualCameraNoise = playercamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        PlayerInputs.Enable();
        InputSubscribe();

        ChangeRotateSencitivity(MainGameManager.Instance.temMouseSensitivity);
        MainGameManager.Instance.temMouseSensitivity = -1f;
    }

    private void OnDisable()
    {
        MainGameManager.Instance.temMouseSensitivity = rotateSencitivity;
        PlayerInputs.Disable();
        InputUnsubscribe();
    }

    public void InputSubscribe()
    {
        if (!subscribed)
        {
            PlayerActions.Look.started += RotateCamera;
            PlayerActions.Run.started += ChangeRunState;
            PlayerActions.Run.canceled += ChangeRunState2;
            PlayerActions.Crouch.started += ChangeCrouchState;
            PlayerActions.Crouch.canceled += ChangeCrouchState2;
            PlayerActions.EquipmentUse.started += EquipMent.OnAttackInput;
            subscribed = true;
        }
    }

    public void InputUnsubscribe()
    {
        if (subscribed)
        {
            PlayerActions.Look.started -= RotateCamera;
            PlayerActions.Run.canceled -= ChangeRunState;
            PlayerActions.Run.canceled -= ChangeRunState2;
            PlayerActions.Crouch.started -= ChangeCrouchState;
            PlayerActions.Crouch.canceled -= ChangeCrouchState2;
            PlayerActions.EquipmentUse.started -= EquipMent.OnAttackInput;
            subscribed = false;
        }
    }

    private void RotateCamera(InputAction.CallbackContext context)//cinemachine�� aim��Ŀ� ���� ȸ����Ű�� ����� �ٸ���.
    {
        Vector2 delta = context.ReadValue<Vector2>();

        if (delta != Vector2.zero)
        {
            float rotatex = Mathf.Clamp(delta.y, -maxRotateY, maxRotateY);
            float rotatey = Mathf.Clamp(delta.x, -60f, 60f);

            transform.Rotate(Vector3.up, rotatey * rotateSencitivity * Time.deltaTime);

            if (currentYangle - rotatex * rotateSencitivity * Time.deltaTime < maxRotateY && currentYangle - rotatex * rotateSencitivity * Time.deltaTime > -maxRotateY)
            {
                currentYangle -= rotatex * rotateSencitivity * Time.deltaTime;
                Head.transform.Rotate(Vector3.right, -rotatex * rotateSencitivity * Time.deltaTime, Space.Self);
            }
        }
    }

    private void ChangeRunState(InputAction.CallbackContext context)
    {
        RunningReady = true; 
    }
    private void ChangeRunState2(InputAction.CallbackContext context)
    {
        RunningReady = false;
    }

    private void ChangeCrouchState(InputAction.CallbackContext context)
    {
        CrouchingReady = true;
    }
    private void ChangeCrouchState2(InputAction.CallbackContext context)
    {
        CrouchingReady = false;
    }

    public void ChangeRotateSencitivity(float amount)
    {
        if (amount <= 0) return;
        rotateSencitivity = amount;
    }

    public float GetRotateSencitivity()
    {
        return rotateSencitivity;
    }
}
