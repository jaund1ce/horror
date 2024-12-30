using System; 
using System.Collections; 
using UnityEngine; 
using UHFPS.Input; 
using UHFPS.Tools; 

namespace UHFPS.Runtime 
{
    public class LockpickComponent : MonoBehaviour 
    {
        public AudioSource AudioSource; 
        public Transform BobbyPin; // �ٺ��� ������Ʈ�� Transform
        public Transform LockpickKeyhole; // ���豸���� Transform
        public Transform KeyholeCopyLocation; // ���豸�� ���� ��ġ

        public Axis BobbyPinForward; // �ٺ����� ȸ�� ��
        public Axis KeyholeForward; // ���豸���� ȸ�� ��

        public float BobbyPinRotateSpeed = 20; // �ٺ��� ȸ�� �ӵ�
        public float BobbyPinResetTime = 1; // �ٺ��� �缳�� �ð�
        public float BobbyPinShakeAmount = 3; // �ٺ��� ��鸲 ����

        public float KeyholeUnlockAngle = -90; // ��� ���� �� ���豸���� �����ؾ� �� ����
        public float KeyholeRotateSpeed = 2; // ���豸�� ȸ�� �ӵ�

        public SoundClip Unlock; // ��� ���� ���� �� ����� ����
        public SoundClip BobbyPinBreak; // �ٺ����� �η��� �� ����� ����

        private PlayerManager playerManager; // �÷��̾� ���� ��ü
        private LockpickInteract lockpick; // ���� ��� ���� �������̽��� ������ ��ü
        private GameManager gameManager; // ���� ������ �����ϴ� �Ŵ��� ��ü

        private bool isActive; // ��� ���� ���� Ȱ��ȭ ����
        private bool isUnlocked; // ����� �����Ǿ����� ����

        private MinMax keyholeLimits; // ���豸�� ȸ�� ���� ����
        private float bobbyPinAngle; // ���� �ٺ����� ȸ�� ����
        private float keyholeAngle; // ���� ���豸���� ȸ�� ����
        private float keyholeTarget; // ���豸���� �����ؾ� �� ��ǥ ����

        private float keyholeTestRange; // ���豸���� ��� �׽�Ʈ ����
        private float bobbyPinLifetime; // �ٺ����� ���� �ð�
        private float bobbyPinUnlockDistance; // ��� ���� ������ ���� �ٺ��� �ּ� �Ÿ�
        private float keyholeUnlockTarget; // ���豸�� ��� ���� ��ǥ �Ÿ�

        private int bobbyPins; // ���� �ٺ��� ����
        private float bobbyPinTime; // ���� �ٺ����� ���� ���� �ð�
        private bool canUseBobbyPin; // �ٺ��� ��� ���� ����

        // ��� ���� �������̽��� �����ϴ� �޼���
        public void SetLockpick(LockpickInteract lockpick)
        {
            this.lockpick = lockpick; // lockpick �����͸� �ʵ忡 ����
            gameManager = lockpick.GameManager; // ���� �Ŵ��� ����
            playerManager = lockpick.PlayerManager; // �÷��̾� �Ŵ��� ����

            keyholeLimits = new MinMax(0, KeyholeUnlockAngle); // ���豸�� ȸ�� ���� ����
            keyholeTestRange = lockpick.KeyholeMaxTestRange; // ��� �׽�Ʈ ���� ����
            bobbyPinLifetime = lockpick.BobbyPinLifetime; // �ٺ��� ���� �ʱ�ȭ
            bobbyPinUnlockDistance = lockpick.BobbyPinUnlockDistance; // ��� ���� �Ÿ� ����
            keyholeUnlockTarget = lockpick.KeyholeUnlockTarget; // ���豸�� ��� ���� ��ǥ ����

            bobbyPinTime = bobbyPinLifetime; // �ٺ��� �ʱ� ���� ����
            bobbyPins = lockpick.BobbyPinItem.Quantity; // �ٺ��� ���� �ʱ�ȭ
            BobbyPin.gameObject.SetActive(bobbyPins > 0); // �ٺ��� UI Ȱ��ȭ

            UpdateLockpicksText(); // ���� �ٺ��� ���� UI ������Ʈ
            lockpick.LockpickUI.SetActive(true); // ��� ���� UI Ȱ��ȭ

            canUseBobbyPin = true; // �ٺ��� ��� ���� ���� Ȱ��ȭ
            isActive = true; // ��� ���� ���� ����
        }

        private void Update()
        {
            if (!isActive) // Ȱ��ȭ���� �ʾҴٸ� ���� ����
                return;

            if (KeyholeCopyLocation != null) // ���豸�� ��ġ�� �����Ǿ��ٸ�
            {
                Vector3 copyPosiiton = KeyholeCopyLocation.position; // ���� ��ġ ��������
                Vector3 bobbyPinPosition = new Vector3(copyPosiiton.x, copyPosiiton.y, copyPosiiton.z); // �ٺ��� ��ġ ����
                BobbyPin.position = bobbyPinPosition; // �ٺ��� ��ġ ����
            }

            if (InputManager.ReadButtonOnce(GetInstanceID(), Controls.EXAMINE)) // ��� Ű �Է� ��
            {
                UnuseLockpick(); // ��� ���� ����
                return;
            }

            bool tryUnlock = InputManager.ReadButton(Controls.JUMP); // ��� �õ� ��ư �Է� Ȯ��
            keyholeTarget = tryUnlock ? KeyholeUnlockAngle : 0; // ��ư �Է¿� ���� ���豸�� ��ǥ ���� ����

            float bobbyPinDiff = Mathf.Abs(lockpick.UnlockAngle - bobbyPinAngle); // ��� ���� ������ ���� �ٺ��� ���� ����
            float bobbyPinNormalized = 0; // �ٺ��� ��� ����ȭ ��
            float bobbyPinShake = 0; // �ٺ��� ��鸲 ��

            if (bobbyPins > 0 && canUseBobbyPin && !isUnlocked) // �ٺ��� ��� ���� ���� Ȯ��
            {
                bool damageBobbyPin = false; // �ٺ��� �ջ� ���� �ʱ�ȭ
                if (!tryUnlock) // ������� �õ��� ���� ���� ��
                {
                    Vector2 bobbyPinMove = InputManager.ReadInput<Vector2>(Controls.POINTER_DELTA); // ����� �Է°� �б�
                    bobbyPinAngle += bobbyPinMove.x * BobbyPinRotateSpeed * Time.deltaTime; // �ٺ��� ȸ��
                }
                else // ������� �õ� ���� ��
                {
                    damageBobbyPin = true; // �ٺ��� �ջ� ����
                    float randomShake = UnityEngine.Random.insideUnitCircle.x; // ������ ��鸲 �� ����
                    bobbyPinShake = UnityEngine.Random.Range(-randomShake, randomShake) * BobbyPinShakeAmount; // ��鸲 �ݿ�

                    if (bobbyPinDiff <= keyholeTestRange) // �ٺ��ɰ� ��� ���� ���̰� ���� ���� ���
                    {
                        bobbyPinNormalized = 1 - (bobbyPinDiff / keyholeTestRange); // ����ȭ �� ���
                        bobbyPinNormalized = (float)Math.Round(bobbyPinNormalized, 2); // �Ҽ��� 2�ڸ��� �ݿø�
                        float targetDiff = Mathf.Abs(keyholeTarget - keyholeAngle); // ���豸�� ���� ���� ���
                        float targetNormalized = targetDiff / keyholeTestRange; // ���豸�� ��ǥ ����ȭ ��

                        Debug.Log($"pinNormalized : {bobbyPinNormalized}  ��� : {1 - bobbyPinUnlockDistance}");
                        if (bobbyPinNormalized >= (1 - bobbyPinUnlockDistance)) // ��� ���� ���� ����
                        {
                            bobbyPinNormalized = 1; // ����ȭ �� �ִ�ȭ
                            damageBobbyPin = false; // �ջ� ���
                            bobbyPinShake = 0; // ��鸲 �ʱ�ȭ

                            if (targetNormalized <= keyholeUnlockTarget) // ���豸�� ���� ���� ��
                            {
                                StartCoroutine(OnUnlock()); // ��� ���� ���� ó��
                                isUnlocked = true; // ��� ���� ����
                            }
                        }
                    }
                }

                if (damageBobbyPin && !lockpick.UnbreakableBobbyPin) // �ٺ��� �ջ� ó��
                {
                    if (bobbyPinTime > 0) // �ٺ��� ������ ���� ������
                    {
                        bobbyPinTime -= Time.deltaTime; // �ð� ����
                    }
                    else // ������ �� �Ǿ��� ���
                    {
                        bobbyPins = Inventory.Instance.RemoveItem(lockpick.BobbyPinItem, 1); // �ٺ��� ���� ����
                        BobbyPin.gameObject.SetActive(false); // �ٺ��� ��Ȱ��ȭ
                        bobbyPinTime = bobbyPinLifetime; // �ٺ��� ���� �ʱ�ȭ
                        UpdateLockpicksText(); // UI ������Ʈ

                        StartCoroutine(ResetBobbyPin()); // �ٺ��� �ʱ�ȭ �ڷ�ƾ ����
                        AudioSource.PlayOneShotSoundClip(BobbyPinBreak); // �η��� �Ҹ� ���

                        canUseBobbyPin = false; // �ٺ��� ��� �Ұ��� ���·� ����
                        bobbyPinAngle = 0; // �ٺ��� ���� �ʱ�ȭ
                    }
                }

                bobbyPinAngle = Mathf.Clamp(bobbyPinAngle, -90, 90); // �ٺ��� ������ ���� ������ Ŭ����
                BobbyPin.localRotation = Quaternion.AngleAxis(bobbyPinAngle + bobbyPinShake, BobbyPinForward.Convert()); // �ٺ��� ȸ�� �ݿ�
            }

            if (isUnlocked) // ��� ���� ���� ��
            {
                keyholeTarget = KeyholeUnlockAngle; // ���豸�� ��ǥ ���� ����
                bobbyPinNormalized = 1f; // ����ȭ �� ����
            }

            keyholeTarget *= bobbyPinNormalized; // ���豸�� ��ǥ ������ ����ȭ �� �ݿ�
            keyholeAngle = Mathf.MoveTowardsAngle(keyholeAngle, keyholeTarget, Time.deltaTime * KeyholeRotateSpeed * 100); // ���豸�� ���� ����
            keyholeAngle = Mathf.Clamp(keyholeAngle, keyholeLimits.RealMin, keyholeLimits.RealMax); // ���豸�� ������ ���� ������ Ŭ����     
            LockpickKeyhole.localRotation = Quaternion.AngleAxis(keyholeAngle, KeyholeForward.Convert()); // ���豸�� ȸ�� �ݿ�
        }        // ��� ���� ���� �� �ʱ� ���·� ����
        private void UnuseLockpick()
        {
            playerManager.PlayerItems.IsItemsUsable = true; // �÷��̾� ������ ��� ���� ���·� ����
            gameManager.FreezePlayer(false); // �÷��̾� ������ ����
            gameManager.SetBlur(false, true); // ȭ�� �� ȿ�� ����
            gameManager.ShowPanel(GameManager.PanelType.MainPanel); // ���� �г� ǥ��
            gameManager.ShowControlsInfo(false, null); // ��Ʈ�� ���� ����
            lockpick.LockpickUI.SetActive(false); // ��� ���� UI ��Ȱ��ȭ
            Destroy(gameObject); // ���� ��ü ����
        }

        // �����ִ� �ٺ��� ���� UI�� ������Ʈ
        private void UpdateLockpicksText()
        {
            string text = lockpick.LockpicksText; // �ؽ�Ʈ ���� ��������
            text = text.RegexReplaceTag('[', ']', "count", bobbyPins.ToString()); // �ٺ��� �������� ��ü
            lockpick.LockpickText.text = text; // UI �ؽ�Ʈ ������Ʈ
        }

        // �ٺ��� �ʱ�ȭ�� ó���ϴ� �ڷ�ƾ
        IEnumerator ResetBobbyPin()
        {
            yield return new WaitForSeconds(BobbyPinResetTime); // ������ �ð���ŭ ���

            if (bobbyPins > 0) // �ٺ����� ���� �ִٸ�
            {
                BobbyPin.gameObject.SetActive(true); // �ٺ��� Ȱ��ȭ
                canUseBobbyPin = true; // �ٺ��� ��� ���� ���·� ����
            }
            else UnuseLockpick(); // �ٺ����� ������ ��� ���� ����
        }

        // ��� ���� ���� ó�� �ڷ�ƾ
        IEnumerator OnUnlock()
        {
            lockpick.Unlock(); // ��� ���� ó�� ����

            if (Unlock.audioClip != null) // ��� ���� ���尡 �ִٸ�
                AudioSource.PlayOneShotSoundClip(Unlock); // ���� ���

            yield return new WaitForSeconds(1f); // 1�� ���
            yield return new WaitUntil(() => !AudioSource.isPlaying); // ���� ����� ���� ������ ���

            UnuseLockpick(); // ��� ���� ���� ó��
        }
    }
}

