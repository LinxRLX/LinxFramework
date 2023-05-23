using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour
{
    public enum FieldType
    {
        Circular,
        Sector
    }

    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float rotateSpeed = 50f;
    [SerializeField] private float targetRotateSpeed = 20f;
    [SerializeField] private float fieldOfView = 10f;
    [SerializeField] private float fieldAngle = 30f;
    [SerializeField] private FieldType fieldType = FieldType.Circular;
    private Transform m_target = null;
    private float m_targetFindDt = 0;

    public bool IsWalking { get; private set; } = false;

    private void Awake()
    {
        Debug.Assert(API.Player == null, "Player Instance Awake Again");
        API.Player = this;
        DrawAttackArea(transform, fieldAngle, fieldOfView);
    }

    private void Update()
    {
        // 移动和旋转
        HandleMovement();
        // 查找目标
        HandleTargetFinder();
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
        if (m_target != null)
        {
            var targetDir = m_target.transform.position - transform.position;
            targetDir.y = 0;
            var targetRotation = Quaternion.LookRotation(targetDir);
            if (m_targetFindDt > 0.3f)
            {
                transform.rotation = targetRotation;
            }
            else
            {
                m_targetFindDt += Time.deltaTime;
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * targetRotateSpeed);
            }
        }
        else if (IsWalking && transform.forward != moveDir)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    private void HandleTargetFinder()
    {
        switch (fieldType)
        {
            case FieldType.Circular:
                HandleTargetFinder_Circular();
                break;
            case FieldType.Sector:
                HandleTargetFinder_Sector();
                break;
            default:
                HandleTargetFinder_Circular();
                break;
        }
    }

    private void HandleTargetFinder_Sector()
    {
        // 先找出圆形范围内的敌人
        var colliders = Physics.OverlapSphere(transform.position, fieldOfView, LayerMask.GetMask("Enemy"));
        if (colliders.Length <= 0)
        {
            m_targetFindDt = 0;
            m_target = null;
            return;
        }

        float nearest = 0;
        m_target = null;


        foreach (var inDistanceCollider in colliders)
        {
            // 先判断距离
            var distance = Vector3.Distance(transform.position, inDistanceCollider.transform.position);
            if (distance >= nearest && m_target != null)
            {
                continue;
            }

            // 判断在扇形范围内
            var angle = Vector3.Angle(transform.forward, inDistanceCollider.transform.position - transform.position);
            if (angle <= fieldAngle / 2)
            {
                m_target = inDistanceCollider.transform;
                nearest = Vector3.Distance(m_target.position, transform.position);
            }
        }
    }

    private void HandleTargetFinder_Circular()
    {
        var colliders = Physics.OverlapSphere(transform.position, fieldOfView, LayerMask.GetMask("Enemy"));
        if (colliders.Length <= 0)
        {
            m_targetFindDt = 0;
            m_target = null;
            return;
        }

        var nearest = m_target != null ? Vector3.Distance(m_target.position, transform.position) : 0;
        foreach (var inRangeCollider in colliders)
        {
            if (m_target == null)
            {
                m_target = inRangeCollider.transform;
                nearest = Vector3.Distance(m_target.position, transform.position);
                continue;
            }

            var distance = Vector3.Distance(transform.position, inRangeCollider.transform.position);
            if (distance < nearest)
            {
                m_target = inRangeCollider.transform;
                nearest = Vector3.Distance(m_target.position, transform.position);
            }
        }
    }

    /// <summary>
    /// 绘制攻击区域
    /// </summary>
    public void DrawAttackArea(Transform t, float angle, float radius)
    {
        var segments = 20;
        var deltaAngle = angle / segments;
        var forward = t.forward;

        var vertices = new Vector3[segments + 2];
        vertices[0] = t.position;
        for (var i = 1; i < vertices.Length; i++)
        {
            var pos = Quaternion.Euler(0f, -angle / 2 + deltaAngle * (i - 1), 0f) * forward * radius + t.position;
            vertices[i] = pos;
        }

        var trianglesAmount = segments;
        var triangles = new int[segments * 3];
        for (var i = 0; i < trianglesAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;
        }

        var go = new GameObject("AttackArea");
        go.transform.position = new Vector3(0, 0.1f, 0);
        go.transform.SetParent(transform);
        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();
        var mesh = new Mesh();
        mr.material.shader = Shader.Find("Unlit/Color");
        mr.material.color = Color.blue;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mf.mesh = mesh;
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