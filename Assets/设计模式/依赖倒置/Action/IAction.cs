public interface IAction
{
    public void Execute(BaseActor actor, params object[] args);
}

public interface IAttack : IAction { }
public interface IMove : IAction { }
public interface IJump : IAction { }
public interface IFly : IAction { }