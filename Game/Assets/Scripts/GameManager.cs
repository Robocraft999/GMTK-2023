using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameState m_state;
    private int m_level = 0;
    public GameState State { 
        get { 
            return m_state; 
        } 
        private set { 
            if (m_state == value) return;
            OnStateChange?.Invoke(m_state, value);
            m_state = value;
        }
    }
    public delegate void GameStateDelegate(GameState oldState, GameState newState);
    public event GameStateDelegate OnStateChange;

    public System.Random random;

    public List<PlayerController> Players;

    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        random = new System.Random();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        m_state = GameState.MENU;
        OnStateChange += StateChangeHandler;
    }

    void StateChangeHandler(GameState oldState, GameState newState)
    {
        
    }


    public void SwitchScene(GameState newState)
    {
        switch (newState)
        {
            case GameState.MENU:
                SceneManager.LoadScene("Menu");
                break;
            case GameState.LEVEL:
                SceneManager.LoadScene("Level" + m_level);
                break;
        }
        State = newState;

    }

    public enum GameState
    {
        MENU, LEVEL
    }
}

