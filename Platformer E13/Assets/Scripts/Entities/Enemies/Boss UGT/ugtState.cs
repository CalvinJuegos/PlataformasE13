using UnityEngine;

public abstract class ugtState
{
    protected bossUGTbehaviour boss;
    protected GameObject player;
    protected Animator animator;

    public ugtState(bossUGTbehaviour boss, GameObject player, Animator animator)
    {
        this.boss = boss;
        this.player = player;
        this.animator = animator;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
