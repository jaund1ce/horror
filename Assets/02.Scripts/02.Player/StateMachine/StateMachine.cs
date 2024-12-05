
public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();
    public void PhysicsUpdate();
}

public abstract class StateMachine
{
    protected IState currentState;//statemachine을 상속 받는 어떠한 상태 (base, air, hide, ...)

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update() //MonoBehaviour에 의해 호출되는 생명주기 함수와 다른 함수이다.
    {
        currentState?.Update(); 
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
