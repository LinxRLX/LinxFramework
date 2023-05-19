using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string BLEND = "Blend";

    [SerializeField] private Player mPlayer;
    private Animator mAnimator;
    private static readonly int s_Blend = Animator.StringToHash(BLEND);
    private float m_blendValue = 0;
    public float blendSpeed = 10f;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (mPlayer.isWalking)
        {
            m_blendValue += blendSpeed * Time.deltaTime;
            var maxValue = 0.6f;
            if (m_blendValue > maxValue)
            {
                m_blendValue = maxValue;
            }

            
        }
        else
        {
            m_blendValue -= blendSpeed * Time.deltaTime;
            var minValue = 0f;
            if (m_blendValue < minValue)
            {
                m_blendValue = minValue;
            }
        }
        mAnimator.SetFloat(s_Blend, m_blendValue);
    }
}