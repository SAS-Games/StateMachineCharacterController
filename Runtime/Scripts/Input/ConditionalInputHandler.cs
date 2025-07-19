using System;
using SAS.StateMachineCharacterController;
using UnityEngine.InputSystem;

public class ConditionalInputHandler : IConditionalInputHandler
{
    private readonly Func<bool> _condition;
    private Action<InputAction.CallbackContext> _actions;

    public ConditionalInputHandler(Func<bool> condition, Action<InputAction.CallbackContext> action)
    {
        _condition = condition;
        AddAction(action);
    }

    public bool CanExecute() => _condition();

    public void Execute(InputAction.CallbackContext context)
    {
        _actions?.Invoke(context);
    }

    public void AddAction(Action<InputAction.CallbackContext> action) => _actions += action;
    public void RemoveAction(Action<InputAction.CallbackContext> action) => _actions -= (action);
}