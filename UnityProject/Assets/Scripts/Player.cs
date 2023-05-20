using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float rotateSpeed = 50f;

    public bool IsWalking { get; private set; } = false;

    private void Awake()
    {
        Debug.Assert(API.Player == null, "Player Instance Awake Again");
        API.Player = this;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var inputVector = API.GameInput.GetMovementVectorNormalized();
        // 从摇杆数据转为角色移动方向
        var moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        // 碰撞检测
        var moveDistance = moveSpeed * Time.deltaTime;
        const float playerRadius = .7f;
        const float playerHeight = 2f;
        var playerPosition = transform.position;

        var canMove = !Physics.CapsuleCast(playerPosition, playerPosition + Vector3.up * playerHeight,
            playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            // 无法按照摇杆方向移动
            // 尝试只移动X方向
            var moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, playerPosition + Vector3.up * playerHeight,
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
                var moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, playerPosition + Vector3.up * playerHeight,
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

        IsWalking = moveDir != Vector3.zero;
        // 设置角色转向
        if (transform.forward != moveDir)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    public void SetMoveSpeed(float newValue)
    {
        moveSpeed = newValue;
    }

    public void SetRotateSpeed(float newValue)
    {
        rotateSpeed = newValue;
    }
}