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
    public GameObject HTP;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        //TODO add options
        set_main.SetActive(true);
        set_options.SetActive(false);
        HTP.SetActive(false);
    }

    public void ButtonPressedStart()
    {
        GameManager.Instance.SwitchScene(GameManager.GameState.LEVEL);
    }

    public void ButtonPressedExit()
    {
        Debug.Log("bye");
        Application.Quit();
    }

    public void ButtonPressedDefault()
    {
        //resets options
    }

    public void ButtonPressedHTP()
    {
        if (HTP.active == true)
        {
            HTP.SetActive(false);
        } else
        {
            HTP.SetActive(true);
        }
        
    }

    public void Update()
    {
        if (set_options.active == true)
        {
            UpdateSliderText();
        }

    }

    public void UpdateSliderText()
    {
        //example:
        //rolls = GameObject.Find("sliderRolls").GetComponent<Slider>().value;
        //GameObject.Find("rolls").GetComponent<TextMeshProUGUI>().text = rolls.ToString();
    }

}


