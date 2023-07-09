using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private AudioClip pressed;
    [SerializeField] private AudioClip released;
    private bool m_activated = false;
    private SpriteRenderer m_Renderer;
    private AudioSource m_AudioSource;
    
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [Header("Events")] 
    [Space] 
    public BoolEvent onStateChangeEvent;

    void Start()
    {
        if (onStateChangeEvent == null)
            onStateChangeEvent = new BoolEvent();
        m_Renderer = GetComponent<SpriteRenderer>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlateState(bool state)
    {
        if (state)
        {
            m_Renderer.sprite = pressedSprite;
            m_AudioSource.PlayOneShot(pressed);
        }
        else
        {
            m_Renderer.sprite = idleSprite;
            m_AudioSource.PlayOneShot(released);
        }
        if (state != m_activated)
        {
            onStateChangeEvent.Invoke(state);
        }
        m_activated = state;
    }
}
