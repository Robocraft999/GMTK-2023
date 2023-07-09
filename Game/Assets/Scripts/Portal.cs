using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int totalPlateAmount;
    public AudioClip activation;
    public AudioClip levelComplete;
    private int m_PlateAmount;
    private Animator m_Animator;
    private AudioSource m_AudioSource;

    void Start()
    {
        m_PlateAmount = 0;
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    
    void FixedUpdate()
    {
        var shouldBeActivated = m_PlateAmount == totalPlateAmount;
        if (m_Animator.GetBool("Activated") != shouldBeActivated && shouldBeActivated)
            m_AudioSource.PlayOneShot(activation);
        m_Animator.SetBool("Activated", shouldBeActivated);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_Animator.GetBool("Activated"))
            {
                m_AudioSource.PlayOneShot(levelComplete);
                GameManager.Instance.SwitchScene(GameManager.GameState.LEVEL);
            }
        }
    }

    public void ChangePlateAmount(bool shouldAdd)
    {
        m_PlateAmount += shouldAdd ? 1 : -1;
    }
}
