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
        public Transform BobbyPin; // 바비핀 오브젝트의 Transform
        public Transform LockpickKeyhole; // 열쇠구멍의 Transform
        public Transform KeyholeCopyLocation; // 열쇠구멍 복사 위치

        public Axis BobbyPinForward; // 바비핀의 회전 축
        public Axis KeyholeForward; // 열쇠구멍의 회전 축

        public float BobbyPinRotateSpeed = 20; // 바비핀 회전 속도
        public float BobbyPinResetTime = 1; // 바비핀 재설정 시간
        public float BobbyPinShakeAmount = 3; // 바비핀 흔들림 정도

        public float KeyholeUnlockAngle = -90; // 잠금 해제 시 열쇠구멍이 도달해야 할 각도
        public float KeyholeRotateSpeed = 2; // 열쇠구멍 회전 속도

        public SoundClip Unlock; // 잠금 해제 성공 시 재생할 사운드
        public SoundClip BobbyPinBreak; // 바비핀이 부러질 때 재생할 사운드

        private PlayerManager playerManager; // 플레이어 관리 객체
        private LockpickInteract lockpick; // 현재 잠금 해제 인터페이스와 연관된 객체
        private GameManager gameManager; // 게임 전반을 관리하는 매니저 객체

        private bool isActive; // 잠금 해제 동작 활성화 여부
        private bool isUnlocked; // 잠금이 해제되었는지 여부

        private MinMax keyholeLimits; // 열쇠구멍 회전 범위 제한
        private float bobbyPinAngle; // 현재 바비핀의 회전 각도
        private float keyholeAngle; // 현재 열쇠구멍의 회전 각도
        private float keyholeTarget; // 열쇠구멍이 도달해야 할 목표 각도

        private float keyholeTestRange; // 열쇠구멍의 잠금 테스트 범위
        private float bobbyPinLifetime; // 바비핀의 수명 시간
        private float bobbyPinUnlockDistance; // 잠금 해제 성공을 위한 바비핀 최소 거리
        private float keyholeUnlockTarget; // 열쇠구멍 잠금 해제 목표 거리

        private int bobbyPins; // 남은 바비핀 개수
        private float bobbyPinTime; // 현재 바비핀의 남은 수명 시간
        private bool canUseBobbyPin; // 바비핀 사용 가능 여부

        // 잠금 해제 인터페이스를 설정하는 메서드
        public void SetLockpick(LockpickInteract lockpick)
        {
            this.lockpick = lockpick; // lockpick 데이터를 필드에 저장
            gameManager = lockpick.GameManager; // 게임 매니저 참조
            playerManager = lockpick.PlayerManager; // 플레이어 매니저 참조

            keyholeLimits = new MinMax(0, KeyholeUnlockAngle); // 열쇠구멍 회전 범위 설정
            keyholeTestRange = lockpick.KeyholeMaxTestRange; // 잠금 테스트 범위 설정
            bobbyPinLifetime = lockpick.BobbyPinLifetime; // 바비핀 수명 초기화
            bobbyPinUnlockDistance = lockpick.BobbyPinUnlockDistance; // 잠금 성공 거리 설정
            keyholeUnlockTarget = lockpick.KeyholeUnlockTarget; // 열쇠구멍 잠금 해제 목표 설정

            bobbyPinTime = bobbyPinLifetime; // 바비핀 초기 수명 설정
            bobbyPins = lockpick.BobbyPinItem.Quantity; // 바비핀 수량 초기화
            BobbyPin.gameObject.SetActive(bobbyPins > 0); // 바비핀 UI 활성화

            UpdateLockpicksText(); // 남은 바비핀 개수 UI 업데이트
            lockpick.LockpickUI.SetActive(true); // 잠금 해제 UI 활성화

            canUseBobbyPin = true; // 바비핀 사용 가능 여부 활성화
            isActive = true; // 잠금 해제 동작 시작
        }

        private void Update()
        {
            if (!isActive) // 활성화되지 않았다면 동작 중지
                return;

            if (KeyholeCopyLocation != null) // 열쇠구멍 위치가 설정되었다면
            {
                Vector3 copyPosiiton = KeyholeCopyLocation.position; // 복사 위치 가져오기
                Vector3 bobbyPinPosition = new Vector3(copyPosiiton.x, copyPosiiton.y, copyPosiiton.z); // 바비핀 위치 조정
                BobbyPin.position = bobbyPinPosition; // 바비핀 위치 설정
            }

            if (InputManager.ReadButtonOnce(GetInstanceID(), Controls.EXAMINE)) // 취소 키 입력 시
            {
                UnuseLockpick(); // 잠금 해제 종료
                return;
            }

            bool tryUnlock = InputManager.ReadButton(Controls.JUMP); // 잠금 시도 버튼 입력 확인
            keyholeTarget = tryUnlock ? KeyholeUnlockAngle : 0; // 버튼 입력에 따라 열쇠구멍 목표 각도 설정

            float bobbyPinDiff = Mathf.Abs(lockpick.UnlockAngle - bobbyPinAngle); // 잠금 성공 각도와 현재 바비핀 각도 차이
            float bobbyPinNormalized = 0; // 바비핀 잠금 정규화 값
            float bobbyPinShake = 0; // 바비핀 흔들림 값

            if (bobbyPins > 0 && canUseBobbyPin && !isUnlocked) // 바비핀 사용 가능 상태 확인
            {
                bool damageBobbyPin = false; // 바비핀 손상 여부 초기화
                if (!tryUnlock) // 잠금해제 시도를 하지 않을 때
                {
                    Vector2 bobbyPinMove = InputManager.ReadInput<Vector2>(Controls.POINTER_DELTA); // 사용자 입력값 읽기
                    bobbyPinAngle += bobbyPinMove.x * BobbyPinRotateSpeed * Time.deltaTime; // 바비핀 회전
                }
                else // 잠금해제 시도 중일 때
                {
                    damageBobbyPin = true; // 바비핀 손상 설정
                    float randomShake = UnityEngine.Random.insideUnitCircle.x; // 무작위 흔들림 값 생성
                    bobbyPinShake = UnityEngine.Random.Range(-randomShake, randomShake) * BobbyPinShakeAmount; // 흔들림 반영

                    if (bobbyPinDiff <= keyholeTestRange) // 바비핀과 잠금 각도 차이가 범위 내일 경우
                    {
                        bobbyPinNormalized = 1 - (bobbyPinDiff / keyholeTestRange); // 정규화 값 계산
                        bobbyPinNormalized = (float)Math.Round(bobbyPinNormalized, 2); // 소수점 2자리로 반올림
                        float targetDiff = Mathf.Abs(keyholeTarget - keyholeAngle); // 열쇠구멍 각도 차이 계산
                        float targetNormalized = targetDiff / keyholeTestRange; // 열쇠구멍 목표 정규화 값

                        Debug.Log($"pinNormalized : {bobbyPinNormalized}  대상 : {1 - bobbyPinUnlockDistance}");
                        if (bobbyPinNormalized >= (1 - bobbyPinUnlockDistance)) // 잠금 해제 성공 조건
                        {
                            bobbyPinNormalized = 1; // 정규화 값 최대화
                            damageBobbyPin = false; // 손상 취소
                            bobbyPinShake = 0; // 흔들림 초기화

                            if (targetNormalized <= keyholeUnlockTarget) // 열쇠구멍 조건 충족 시
                            {
                                StartCoroutine(OnUnlock()); // 잠금 해제 성공 처리
                                isUnlocked = true; // 잠금 상태 설정
                            }
                        }
                    }
                }

                if (damageBobbyPin && !lockpick.UnbreakableBobbyPin) // 바비핀 손상 처리
                {
                    if (bobbyPinTime > 0) // 바비핀 수명이 남아 있으면
                    {
                        bobbyPinTime -= Time.deltaTime; // 시간 감소
                    }
                    else // 수명이 다 되었을 경우
                    {
                        bobbyPins = Inventory.Instance.RemoveItem(lockpick.BobbyPinItem, 1); // 바비핀 수량 감소
                        BobbyPin.gameObject.SetActive(false); // 바비핀 비활성화
                        bobbyPinTime = bobbyPinLifetime; // 바비핀 수명 초기화
                        UpdateLockpicksText(); // UI 업데이트

                        StartCoroutine(ResetBobbyPin()); // 바비핀 초기화 코루틴 시작
                        AudioSource.PlayOneShotSoundClip(BobbyPinBreak); // 부러짐 소리 재생

                        canUseBobbyPin = false; // 바비핀 사용 불가능 상태로 설정
                        bobbyPinAngle = 0; // 바비핀 각도 초기화
                    }
                }

                bobbyPinAngle = Mathf.Clamp(bobbyPinAngle, -90, 90); // 바비핀 각도를 제한 범위로 클램핑
                BobbyPin.localRotation = Quaternion.AngleAxis(bobbyPinAngle + bobbyPinShake, BobbyPinForward.Convert()); // 바비핀 회전 반영
            }

            if (isUnlocked) // 잠금 해제 성공 시
            {
                keyholeTarget = KeyholeUnlockAngle; // 열쇠구멍 목표 각도 설정
                bobbyPinNormalized = 1f; // 정규화 값 설정
            }

            keyholeTarget *= bobbyPinNormalized; // 열쇠구멍 목표 각도에 정규화 값 반영
            keyholeAngle = Mathf.MoveTowardsAngle(keyholeAngle, keyholeTarget, Time.deltaTime * KeyholeRotateSpeed * 100); // 열쇠구멍 각도 조정
            keyholeAngle = Mathf.Clamp(keyholeAngle, keyholeLimits.RealMin, keyholeLimits.RealMax); // 열쇠구멍 각도를 제한 범위로 클램핑     
            LockpickKeyhole.localRotation = Quaternion.AngleAxis(keyholeAngle, KeyholeForward.Convert()); // 열쇠구멍 회전 반영
        }        // 잠금 해제 종료 및 초기 상태로 복귀
        private void UnuseLockpick()
        {
            playerManager.PlayerItems.IsItemsUsable = true; // 플레이어 아이템 사용 가능 상태로 복원
            gameManager.FreezePlayer(false); // 플레이어 움직임 해제
            gameManager.SetBlur(false, true); // 화면 블러 효과 해제
            gameManager.ShowPanel(GameManager.PanelType.MainPanel); // 메인 패널 표시
            gameManager.ShowControlsInfo(false, null); // 컨트롤 정보 숨김
            lockpick.LockpickUI.SetActive(false); // 잠금 해제 UI 비활성화
            Destroy(gameObject); // 현재 객체 삭제
        }

        // 남아있는 바비핀 수를 UI에 업데이트
        private void UpdateLockpicksText()
        {
            string text = lockpick.LockpicksText; // 텍스트 형식 가져오기
            text = text.RegexReplaceTag('[', ']', "count", bobbyPins.ToString()); // 바비핀 수량으로 대체
            lockpick.LockpickText.text = text; // UI 텍스트 업데이트
        }

        // 바비핀 초기화를 처리하는 코루틴
        IEnumerator ResetBobbyPin()
        {
            yield return new WaitForSeconds(BobbyPinResetTime); // 설정된 시간만큼 대기

            if (bobbyPins > 0) // 바비핀이 남아 있다면
            {
                BobbyPin.gameObject.SetActive(true); // 바비핀 활성화
                canUseBobbyPin = true; // 바비핀 사용 가능 상태로 설정
            }
            else UnuseLockpick(); // 바비핀이 없으면 잠금 해제 종료
        }

        // 잠금 해제 성공 처리 코루틴
        IEnumerator OnUnlock()
        {
            lockpick.Unlock(); // 잠금 해제 처리 실행

            if (Unlock.audioClip != null) // 잠금 해제 사운드가 있다면
                AudioSource.PlayOneShotSoundClip(Unlock); // 사운드 재생

            yield return new WaitForSeconds(1f); // 1초 대기
            yield return new WaitUntil(() => !AudioSource.isPlaying); // 사운드 재생이 끝날 때까지 대기

            UnuseLockpick(); // 잠금 해제 종료 처리
        }
    }
}

