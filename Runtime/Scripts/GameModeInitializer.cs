// using SAS.SceneManagement;
// using UnityEngine;
//
//
//
// public struct GameModeChangedEvent : IEvent
// {
//     public GameMode mode;
//
//     public static implicit operator GameMode(GameModeChangedEvent evt)
//     {
//         return evt.mode;
//     }
// }
//
// public class GameModeInitializer : MonoBehaviour
// {
//     [SerializeField] private GameMode m_GameMode;
//     public static GameMode CurrentGameMode { get; private set; }
//     private EventBinding<SceneGroupLoadedEvent> _sceneGroupLoadedEventBinding;
//
//     private void Awake()
//     {
//         CurrentGameMode = m_GameMode;
//         _sceneGroupLoadedEventBinding = new EventBinding<SceneGroupLoadedEvent>(OnSceneGroupLoaded);
//         EventBus<SceneGroupLoadedEvent>.Register(_sceneGroupLoadedEventBinding);
//     }
//
//     private void OnDestroy()
//     {
//         EventBus<SceneGroupLoadedEvent>.Deregister(_sceneGroupLoadedEventBinding);
//
//     }
//
//     private void OnSceneGroupLoaded(SceneGroupLoadedEvent SceneGroupLoadedEventData)
//     {
//         CurrentGameMode = m_GameMode;
//         EventBus<GameModeChangedEvent>.Raise(new GameModeChangedEvent
//         {
//             mode = m_GameMode
//         });
//     }
// }