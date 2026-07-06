using System.Collections.Generic;
using SAS.StateMachineCharacterController;
using UnityEngine.InputSystem;

public abstract class ChainedInputCommand : IInputCommand
{
    protected InputAction _inputAction;
    private readonly Dictionary<InputActionPhase, List<HandlerEntry>> _handlersByPhase = new();
    protected abstract string InputActionName { get; }

    public void AddHandler(InputActionPhase phase, IConditionalInputHandler handler, int priority = 0)
    {
        if (!_handlersByPhase.TryGetValue(phase, out var list))
            _handlersByPhase[phase] = list = new();

        list.Add(new HandlerEntry(handler, priority));
        list.Sort((a, b) => b.Priority.CompareTo(a.Priority));
    }

    public void RemoveHandler(InputActionPhase phase, IConditionalInputHandler handler)
    {
        if (_handlersByPhase.TryGetValue(phase, out var list))
        {
            list.RemoveAll(entry => entry.Handler == handler);
        }
    }

    public void SetActive(InputConfig inputConfig, bool active)
    {
        if (!string.IsNullOrEmpty(InputActionName))
        {
            if (active)
            {
                if (_inputAction != null)
                    _inputAction.Disable();

                _inputAction = inputConfig.GetInputAction(InputActionName);
                if (_inputAction != null)
                {
                    _inputAction.started += OnInputStarted;
                    _inputAction.performed += OnInputPerformed;
                    _inputAction.canceled += OnInputCanceled;
                    _inputAction.Enable();
                }
            }
            else if (_inputAction != null)
            {
                _inputAction.started -= OnInputStarted;
                _inputAction.performed -= OnInputPerformed;
                _inputAction.canceled -= OnInputCanceled;
                _inputAction.Disable();
                _inputAction = null;
            }
        }
    }

    private void HandlePhase(InputActionPhase phase, InputAction.CallbackContext context)
    {
        if (_handlersByPhase.TryGetValue(phase, out var list))
        {
            foreach (var handlerEntry in list)
            {
                if (handlerEntry.Handler.CanExecute())
                {
                    handlerEntry.Handler.Execute(context);
#if ENABLE_DEBUG
                    double processTime = Time.realtimeSinceStartupAsDouble;
                    double inputTime = context.time;
                    double inputToProcessMs = (processTime - inputTime) * 1000.0;

                    int processFrame = Time.frameCount;
                    double frameDuration = Time.unscaledDeltaTime;
                    int inputFrameEstimate =
 processFrame - Mathf.RoundToInt((float)((processTime - inputTime) / frameDuration));
                    int frameDelay = processFrame - inputFrameEstimate;

                    var actionName = context.action?.name;
                    var controlName = context.control?.displayName;

                    Debug.Log($"[Input → Process] {inputToProcessMs:F2} ms | " + $"Frames: {frameDelay} | " +
                              $"InputFrame~: {inputFrameEstimate} → ProcessFrame: {processFrame} | " +
                              $"Action: {actionName} | Phase: {phase} | Control: {controlName}"
                    );
#endif
                    break;
                }
            }
        }
    }

    private void OnInputStarted(InputAction.CallbackContext context) => HandlePhase(InputActionPhase.Started, context);

    private void OnInputPerformed(InputAction.CallbackContext context) =>
        HandlePhase(InputActionPhase.Performed, context);

    private void OnInputCanceled(InputAction.CallbackContext context) =>
        HandlePhase(InputActionPhase.Canceled, context);
}