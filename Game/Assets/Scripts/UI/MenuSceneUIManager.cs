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
    public GameObject set_options;

    public bool UseSafePortals { get; set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        set_main.SetActive(true);
        set_options.SetActive(false);
        UseSafePortals = true;
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
        UseSafePortals = true;
        GameObject.Find("safePortals").GetComponent<Toggle>().isOn = true;
    }
    

    public void Update()
    {
        if (set_options.activeSelf)
        {
            
        }
    }

}


