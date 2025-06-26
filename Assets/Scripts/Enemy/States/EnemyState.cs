using UnityEngine;

public class EnemyState 
{
    protected EnemyAI enemyAI;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine)
    {
        this.enemyAI = enemyAI;
        this.enemyStateMachine = enemyStateMachine; 
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
}
