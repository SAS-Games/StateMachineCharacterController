public enum GameMode
{
    SideScroller,
    FreeRoam
}

public struct GameModeChangedEvent : IEvent
{
    public GameMode mode;

    public static implicit operator GameMode(GameModeChangedEvent evt)
    {
        return evt.mode;
    }
}