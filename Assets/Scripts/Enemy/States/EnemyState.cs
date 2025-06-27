using UnityEngine;

public class EnemyState 
{
    protected EnemyAI enemyAI;
    protected EnemyStateMachine enemyStateMachine;
    protected EnemyAnimationController enemyAnimationController;

    public EnemyState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine, EnemyAnimationController enemyAnimationController)
    {
        this.enemyAI = enemyAI;
        this.enemyStateMachine = enemyStateMachine; 
        this.enemyAnimationController = enemyAnimationController;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
}
