using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractActionEvent;
    private PlayerInputActions m_playerInputActions;

    private void Awake()
    {
        if (API.GameInput != null)
        {
            Destroy(API.GameInput);
        }
        API.GameInput = this;
        m_playerInputActions = new PlayerInputActions();
        m_playerInputActions.Player.Enable();

        m_playerInputActions.Player.Interact.performed += Interact_Performed;
    }

    private void Interact_Performed(InputAction.CallbackContext obj)
    {
        OnInteractActionEvent?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        var inputVector = m_playerInputActions.Player.Move.ReadValue<Vector2>();
        // πÈ“ªªØ
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
