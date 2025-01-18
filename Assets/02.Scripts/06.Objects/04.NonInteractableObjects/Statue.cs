using UnityEngine;
using System.Collections;

public class Statue : MonoBehaviour
{
    private bool isRotating = false;

    private void OnEnable()
    {
        // MainGameManager에서 OnObjectDestroyed 이벤트를 구독
        OnDestroyPaper.OnObjectDestroyed += HandleObjectDestroyed;
    }

    private void OnDisable()
    {
        // 구독 해제
        OnDestroyPaper.OnObjectDestroyed -= HandleObjectDestroyed;
    }

    // 오브젝트가 파괴되었을 때 실행
    private void HandleObjectDestroyed(GameObject destroyedObject)
    {
            StartCoroutine(SetRandomRotationWithDelay());
    }

    // 2초 지연 후 랜덤 회전을 설정하는 코루틴
    private IEnumerator SetRandomRotationWithDelay()
    {

        if (isRotating)
        yield break;

        isRotating = true;
        yield return new WaitForSeconds(2f);

        if (this != null)
        {
            SoundManger.Instance.MakeEnviornmentSound("Statue_Moving");

            // 90, 180, 270 중 하나를 랜덤으로 선택
            int[] rotationValues = { 90, 180, 270 };
            int randomIndex = Random.Range(0, rotationValues.Length);
            float randomYRotation = rotationValues[randomIndex];

            // targetObject의 회전값 설정
            Vector3 currentRotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(currentRotation.x, randomYRotation, currentRotation.z);
        }

        isRotating = false;
    }
}
