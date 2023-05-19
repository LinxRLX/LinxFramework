using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player sInstance { get; private set; }

    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float rotateSpeed = 50f;

    public void SetMoveSpeed(float newValue)
    {
        moveSpeed = newValue;
    }
    public void SetRotateSpeed(float newValue)
    {
        rotateSpeed = newValue;
    }
    
    private bool mIsWalking = false;

    public bool isWalking
    {
        get => mIsWalking;
    }

    private Vector3 mLastInteractDir;

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = API.sGameInput.GetMovementVectorNormalized();
        // 从摇杆数据转为角色移动方向
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            mLastInteractDir = moveDir;
        }
    }


    private void HandleMovement()
    {
        Vector2 inputVector = API.sGameInput.GetMovementVectorNormalized();
        // 从摇杆数据转为角色移动方向
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        // 碰撞检测
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            // 无法按照摇杆方向移动
            // 尝试只移动X方向
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // 只移动X方向可行
                moveDir = moveDirX;
            }
            else
            {
                // 只移动X方向不可行
                // 尝试只移动Z方向
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                    playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // 只移动Z方向可行
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            // 角色移动
            transform.position += moveDir * moveDistance;
        }

        mIsWalking = moveDir != Vector3.zero;
        // 设置角色转向
        if (transform.forward != moveDir)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }
}