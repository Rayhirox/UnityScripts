/*
 * Rayhirox
 * 2022-08-07
 * 2022-08-08
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Buffers;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Controls;

public class YSceneManager : MonoBehaviour
{
    public enum Position { Top, Bottom };
    public Position DisplayPosition = Position.Top;
    

    public bool AdaptiveWidth = true;
    
    public float xInterval = 3f;
    public float yInterval = 3f;
    public float buttonWidth = 100f;
    public float buttonHeight = 30f;

    private bool isDisplayed = false;
    private Keyboard keyboard;

    private void Update()
    {
        keyboard = Keyboard.current;
        if (keyboard == null)
        {
            Debug.LogError("There is no available keyboard.");
        }
        if (keyboard.backquoteKey.wasPressedThisFrame)
        {
            isDisplayed = !isDisplayed;
        }
    }

    private void OnGUI()
    {
        if (!isDisplayed) { return; }

        float x = xInterval;
        float y = yInterval;
        if(DisplayPosition == Position.Bottom) { y = Screen.height - yInterval - buttonHeight; }
        
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string sceneName = SceneUtility.GetScenePathByBuildIndex(i);
            var startPos = sceneName.LastIndexOf("/") + 1;
            var endPos = sceneName.LastIndexOf(".");
            sceneName = sceneName.Substring(startPos, endPos - startPos);

            if (AdaptiveWidth)
            {
                buttonWidth = sceneName.Length * 6f + 20f;
            }

            if (sceneName != null)
            {
                if (x + buttonWidth + xInterval >= Screen.width)
                {
                    if(DisplayPosition == Position.Bottom) y -= buttonHeight + yInterval;
                    else y += buttonHeight + yInterval;
                    x = xInterval;
                }

                if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), sceneName))
                {
                    AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(sceneName);
                }

                x += buttonWidth + xInterval;
            }
        }
    }
}

