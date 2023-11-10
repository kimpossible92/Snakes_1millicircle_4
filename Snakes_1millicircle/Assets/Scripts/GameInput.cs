using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    //private PlayerInputActions playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnAlternateInteractAction;

    private void Awake()
    {
        //playerInputActions = new PlayerInputActions();
        //playerInputActions.Player.Enable();
        //playerInputActions.Player.Interact.performed += Interact_performed;
        //playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }
    
    //private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    //{
    //    OnAlternateInteractAction?.Invoke(this, EventArgs.Empty);
    //}

    //private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    //{
    //    OnInteractAction?.Invoke(this, EventArgs.Empty);
    //}

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = Vector2.zero;
        
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
