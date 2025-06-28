using UnityEngine;

// this calss is the base class for all the enemy states
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

    // this executes when entering each state
    public virtual void EnterState() { }

    // this executes when leaving this state
    public virtual void ExitState() { }

    //this executes every frame while in this state (like normal Update)
    public virtual void Update() { }
}
