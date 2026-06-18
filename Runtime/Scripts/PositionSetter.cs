// using System.Linq;
// using SAS.SceneManagement;
// using UnityEngine;
//
// namespace SAS.StateMachineCharacterController
// {
//     public class PositionSetter : MonoBehaviour
//     {
//         [SerializeField] private string m_StartPointName = "StartPoint";
//         private EventBinding<SceneGroupLoadedEvent> _sceneGroupLoadedEventBinding;
//
//         void Awake()
//         {
//             var points = GameObject.FindObjectsByType<SavePoint>(FindObjectsSortMode.InstanceID);
//             var startPoint = points.FirstOrDefault(point => point.transform.name == m_StartPointName)?.transform;
//             if (startPoint != null)
//                 transform.position = startPoint.transform.position;
//             _sceneGroupLoadedEventBinding = new EventBinding<SceneGroupLoadedEvent>(SetPlayerPositionAtSpawnPoint);
//             EventBus<SceneGroupLoadedEvent>.Register(_sceneGroupLoadedEventBinding);
//         }
//
//         void SetPlayerPositionAtSpawnPoint(SceneGroupLoadedEvent sceneGroupLoadedEvent)
//         {
//             var sceneGroup = sceneGroupLoadedEvent.sceneGroup;
//             SetSpawnPosition(sceneGroup);
//             Debug.Log("Set Player Position at Spawn Point");
//             EventBus<RespawnEvent>.Raise(new RespawnEvent { transform = this.transform });
//         }
//
//         private void SetSpawnPosition(SceneGroup sceneGroup)
//         {
//             var startPoint =
//                 SceneUtility.FindComponentInScene<SavePoint>(sceneGroup.FindSceneNameByType(SceneType.ActiveScene),
//                     m_StartPointName);
//             if (startPoint != null)
//                 transform.position = startPoint.transform.position;
//         }
//
//         void OnDestroy()
//         {
//             EventBus<SceneGroupLoadedEvent>.Deregister(_sceneGroupLoadedEventBinding);
//         }
//     }
// }