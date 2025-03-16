using System;
using UnityEngine;

public enum GameMode
{
    SideScroller3D,
    OpenWorld3d
}

public struct GameModeChagedEvent : IEvent
{
    public GameMode mode;

    public static implicit operator GameMode(GameModeChagedEvent evt)
    {
        return evt.mode;
    }
}

public class GameModeChangeNotifier : MonoBehaviour
{
    [SerializeField] private GameMode m_GameMode;
    private void Start()
    {
        EventBus<GameModeChagedEvent>.Raise(new GameModeChagedEvent { mode = m_GameMode });
    }
}
