using System.Collections;

public abstract class AIBaseState
{
    protected bool canUpdate;

    public abstract IEnumerator Enter(AIController ai);

    public abstract void Update(AIController ai);
}