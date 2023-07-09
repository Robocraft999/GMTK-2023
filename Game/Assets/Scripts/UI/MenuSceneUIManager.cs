using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;


public class MenuSceneUIManager : MonoBehaviour
{
    public static MenuSceneUIManager Instance { get; private set; }

    public GameObject set_main;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        //TODO add options
        set_main.SetActive(true);
    }

    public void ButtonPressedStart()
    {
        GameManager.Instance.SwitchScene(GameManager.GameState.LEVEL);
    }

    public void ButtonPressedExit()
    {
        GameManager.Instance.QuitApplication();
    }

    public void ButtonPressedDefault()
    {
        //resets options
    }
    

    public void Update()
    {

    }

}


