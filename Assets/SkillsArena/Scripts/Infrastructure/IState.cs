namespace SkillsArena
{
    public interface IState
    {
        void Exit();
    }

    public interface IPayloadedState<PayloadValue, PayloadValue2> : IState
    {
        void Enter(PayloadValue value, PayloadValue2 value2);
    }

    public interface IDefaultState : IState
    {
        void Enter();
    }
}