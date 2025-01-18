using SAS.SceneManagement;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class PositionSetter : MonoBehaviour
    {
        [SerializeField] private string m_StartPointName = "StartPoint";
        private EventBinding<SceneGroupLoadedEvent> _sceneGroupLoadedEventBinding;

        void Awake()
        {
            _sceneGroupLoadedEventBinding = new EventBinding<SceneGroupLoadedEvent>(SetPlayerPositionAtSpawnPoint);
            EventBus<SceneGroupLoadedEvent>.Register(_sceneGroupLoadedEventBinding);
        }

        void SetPlayerPositionAtSpawnPoint(SceneGroupLoadedEvent sceneGroupLoadedEvent)
        {
            var sceneGroup = sceneGroupLoadedEvent.sceneGroup;
            var startPoint = SceneUtility.FindComponentInScene<SavePoint>(sceneGroup.FindSceneNameByType(SceneType.ActiveScene), m_StartPointName);
            if (startPoint != null)
                transform.position = startPoint.transform.position;
            Debug.Log("Set Player Position at Spawn Point");
            EventBus<RespawnEvent>.Raise(new RespawnEvent { transform = this.transform });
        }

        void OnDestroy()
        {
            EventBus<SceneGroupLoadedEvent>.Deregister(_sceneGroupLoadedEventBinding);
        }
    }
}
