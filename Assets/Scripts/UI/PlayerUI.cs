﻿// Usage: this script is meant to be placed on a UI object.
// The UI object must be a child of the Player.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour {

    private bool isPaused;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject settingsMenu = null;
    [SerializeField] private TakesInput[] disableWhilePaused;

    private bool isInVictoryScreen;
    [SerializeField] private GameObject victoryScreen = null;
    [SerializeField] private TextMeshProUGUI timeText;
	[SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TakesInput[] disableWhileInVictoryScreen;


    void Awake()
    {
        if (this.pauseMenu == null)
            Debug.LogError(GetType() + ": No pause menu object assigned");

        if (this.settingsMenu == null)
            Debug.LogError(GetType() + ": No settings menu object assigned");
        
        DisableScreens();
        HideCursor();
    }

    void Update ()
    {

        if (!this.isInVictoryScreen)
        {
            // FIX LATER: change menu keycode to Esc only on the actual build
            // Unity has weird hotkeys in the editor, so we're using "T" in the meantime.
            if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (!this.isPaused)
                {
                    PauseScreen();
                }
                else
                    UnpauseScreen();
            }
        }
    }

    public void PauseScreen()
    {
        // Undo other screens
        DisableScreens();

        // Unlock and show cursor
        ShowCursor();

        // Disable Player's controls
        DisableInputs(this.disableWhilePaused);

        // Enable pause menu
        this.pauseMenu.SetActive(true);

        this.isPaused = true;
    }

    public void UnpauseScreen()
    {
        if (!this.isPaused)
            return;

        // Lock and hide cursor
        HideCursor();

        // Enable Player's controls
        EnableInputs(this.disableWhilePaused);

        // Disable pause menu and settings menu
        this.pauseMenu.SetActive(false);
        this.settingsMenu.SetActive(false);

        this.isPaused = false;
    }

    public void VictoryScreen(float time, float bestTime)
    {
        // Undo other screens
        DisableScreens();

        // Unlock and show cursor
        ShowCursor();

        // Disable Player's controls
        DisableInputs(this.disableWhileInVictoryScreen);

        // Change victory text
		this.timeText.SetText("YOUR TIME: " + Util.GetTimeString(time));
		this.bestTimeText.SetText("BEST TIME: " + Util.GetTimeString(bestTime));

        // Enable victory screen
        this.victoryScreen.SetActive(true);

        this.isInVictoryScreen = true;
    }

    public void UnVictoryScreen()
    {
        if (!this.isInVictoryScreen)
            return;

        // Lock and hide cursor
        HideCursor();

        // Enable Player's controls
        EnableInputs(this.disableWhileInVictoryScreen);

        // Disable victory screen
        this.victoryScreen.SetActive(false);

        this.isInVictoryScreen = false;
    }

    private void DisableInputs(TakesInput[] tis)
    {
        for (int i = 0; i < tis.Length; i++)
            tis[i].DisableInput();
    }

    private void EnableInputs(TakesInput[] tis)
    {
        for (int i = 0; i < tis.Length; i++)
            tis[i].EnableInput();
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DisableScreens()
    {
        UnpauseScreen();
        UnVictoryScreen();
    }
}
