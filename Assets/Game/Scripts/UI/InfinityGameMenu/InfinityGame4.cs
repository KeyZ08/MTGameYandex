using Game;
using UnityEngine;

public class InfinityGame4 : InfinityGameMode
{
    protected override void Awake()
    {
        base.Awake();
        IsValid = true;
    }

    public override void Click()
    {
        base.Click();
        infinityGameMenu.StateUpdate(this);
    }

    public override void StateUpdate(InfinityUIButton button)
    {
        infinityGameMenu.StateUpdate(this);
    }

    public LevelGame4 GetSettings()
    {
        var lastLevel = LevelManager.LastLevel;
        return new LevelGame4(Map.Levels[lastLevel].Training, new WinSettings(25), int.MaxValue);
    }
}