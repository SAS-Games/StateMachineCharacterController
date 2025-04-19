using UnityEngine;

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

public class GameModeInitializer : MonoBehaviour
{
    [SerializeField] private GameMode m_GameMode;

    public static GameMode CurrentGameMode { get; private set; }
    private EventBinding<GameModeChangedEvent> _gameModeChangedEventBinding;

    private void Awake()
    {
        // Set the current game mode so other scripts can access it immediately
        CurrentGameMode = m_GameMode;
        _gameModeChangedEventBinding = new EventBinding<GameModeChangedEvent>(evt => OnGameModeChanged(evt.mode));
    }

    private void Start()
    {
        // Raise the event to notify systems listening for game mode changes
        EventBus<GameModeChangedEvent>.Raise(new GameModeChangedEvent
        {
            mode = m_GameMode
        });
    }

    private void OnEnable()
    {
        EventBus<GameModeChangedEvent>.Register(_gameModeChangedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<GameModeChangedEvent>.Deregister(_gameModeChangedEventBinding);
    }

    private void OnGameModeChanged(GameMode gameMode)
    {
        CurrentGameMode = gameMode;
    }
}