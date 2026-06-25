using UnityEngine;

[DefaultExecutionOrder(-2000)]
public class GameModeInitializer : MonoBehaviour
{
    [SerializeField] private GameMode m_GameMode;
    public static GameMode CurrentGameMode { get; private set; }

    private void Awake()
    {
        CurrentGameMode = m_GameMode;
    }
}