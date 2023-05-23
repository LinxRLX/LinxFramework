using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string BLEND = "Blend";

    [SerializeField] private Player mPlayer;
    private Animator m_animator;
    private static readonly int s_Blend = Animator.StringToHash(BLEND);
    private float m_blendValue = 0;
    public float blendSpeed = 10f;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (mPlayer.IsWalking)
        {
            m_blendValue += blendSpeed * Time.deltaTime;
            const float maxValue = 0.6f;
            if (m_blendValue > maxValue)
            {
                m_blendValue = maxValue;
            }

            
        }
        else
        {
            m_blendValue -= blendSpeed * Time.deltaTime;
            const float minValue = 0f;
            if (m_blendValue < minValue)
            {
                m_blendValue = minValue;
            }
        }
        m_animator.SetFloat(s_Blend, m_blendValue);
    }
}