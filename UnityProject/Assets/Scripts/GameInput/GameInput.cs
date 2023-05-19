using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PlayerInputActions mPlayerInputActions;

    private void Awake()
    {
        if (API.sGameInput != null)
        {
            Destroy(API.sGameInput);
        }
        API.sGameInput = this;
        mPlayerInputActions = new PlayerInputActions();
        mPlayerInputActions.Player.Enable();

        mPlayerInputActions.Player.Interact.performed += Interact_Performed;
    }

    private void Interact_Performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = mPlayerInputActions.Player.Move.ReadValue<Vector2>();
        // πÈ“ªªØ
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
