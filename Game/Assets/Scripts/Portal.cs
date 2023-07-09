using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int totalPlateAmount;
    private int m_PlateAmount;
    private Animator m_Animator;

    void Start()
    {
        m_PlateAmount = 0;
        m_Animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        m_Animator.SetBool("Activated", m_PlateAmount == totalPlateAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("t");
        if (other.CompareTag("Player"))
        {
            if (m_Animator.GetBool("Activated"))
                GameManager.Instance.SwitchScene(GameManager.GameState.LEVEL);
        }
    }

    public void ChangePlateAmount(bool shouldAdd)
    {
        m_PlateAmount += shouldAdd ? 1 : -1;
    }
}
