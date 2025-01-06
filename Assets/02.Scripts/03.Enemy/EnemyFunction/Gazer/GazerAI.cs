using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazerAI : EnemyAI
{
    public Camera playerSight;
    private Animator animator;
    //박스콜라이더도 되지만 렌더러를 쓰는 이유는 화면에 보이는지에 대해 더 적합. 박스콜라이더는 물리적 충돌 쪽에 더 적합
    [SerializeField]private Renderer GazerRenderer;

    protected override void Awake()
    {
        base.Awake();
        enemy.SoundTime = 40f;
        //playerSight = MainGameManager.Instance.Player.GetComponentInChildren<Camera>();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckTarget()
    {
        base.CheckTarget();
    }

    protected override void CheckMissTime(bool isTarget)
    {
        base.CheckMissTime(isTarget);
    }

    public override void GetAggroGage(float amount)
    {
        base.GetAggroGage(amount);
    }

    public override void FeelThePlayer()
    {
        base.FeelThePlayer();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    protected override bool IsInAttackRange()
    {
        return base.IsInAttackRange();
    }

    private bool IsGazed(Camera playerSight) 
    {
        if (GazerRenderer == null) return false;

        //카메라의 뷰 프러스텀
        // 뷰프러스텀 : 3D 그래픽스에서 카메라가 볼수 있는 영역.아래 코드는 카메라 뷰의 프러스텀 평면을 계산중
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerSight);

        // 렌더러의 바운딩 박스와 뷰 프러스텀 교차 체크
        return GeometryUtility.TestPlanesAABB(planes, GazerRenderer.bounds);
    }

    private void BeGazedInitialize() 
    {
        IsAttacking = false;
        checkMissTime = 0;
        EnemyAistate = AIState.Idle;
    }

    public override int UpdateState()
    {
        if (IsGazed(playerSight))
        {
            animator.speed = 0;
            BeGazedInitialize();
            return (int)EnemyAistate;
        }
        else
        {
            animator.speed = 1;
        }
        return base.UpdateState();
    }
    protected override void PlaySoundBasedOnState()
    {
        base.PlaySoundBasedOnState();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
