
using System;

public abstract class InfinityGameMode : InfinityUIButton
{
    public string scene;
    public bool IsValid
    {
        get; protected set;
    }

    protected InfinityGameMenu infinityGameMenu;

    protected override void Awake()
    {
        gameMode = this;
        base.Awake();
        infinityGameMenu = GetComponentInParent<InfinityGameMenu>();
    }

    public abstract void StateUpdate(InfinityUIButton button);
}


