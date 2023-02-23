using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject DeathMenu;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject WinMenu;
    public static bool GamePause;
    static bool RestartLevel;
    static bool Death;
    static bool Win;
    void Awake()
    {
        if (RestartLevel)
            UnPause();
        else if (Death)
            _Death();
        else if (Win)
            _Win();
        else
            Main();
    }
    public void Main()
    {
        Time.timeScale = 0; GamePause = true;
        MainMenu.SetActive(true);
        DeathMenu.SetActive(false);
        PauseMenu.SetActive(false);
        WinMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Pause(); }
    }
    void Pause()
    {
        if(!PauseMenu.activeSelf)
        {
            Time.timeScale = 0; GamePause = true;
            PauseMenu.SetActive(true);
        }
    }
    public void UnPause()
    {
        RestartLevel = false;
        StartCoroutine(Wait(
            delegate ()
            {
                MainMenu.SetActive(false);
                PauseMenu.SetActive(false);
                DeathMenu.SetActive(false);
                WinMenu.SetActive(false);
            },
            delegate ()
            {
                Time.timeScale = 1; GamePause = false;
            }
        ));
    }
    public void Restart()
    {
        RestartLevel = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator Wait(Action action1, Action action2)
    {
        action1.Invoke();
        yield return true;
        yield return new WaitForEndOfFrame();
        action2.Invoke();
    }
    public static void PlayerDeath()
    {
        Death = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void _Death()
    {
        Death = false;
        Time.timeScale = 0; GamePause = true;
        MainMenu.SetActive(false);
        DeathMenu.SetActive(true);
        PauseMenu.SetActive(false);
        WinMenu.SetActive(false);
    }
    public static void PlayerWin()
    {
        Win = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void _Win()
    {
        Win = false;
        Time.timeScale = 0; GamePause = true;
        MainMenu.SetActive(false);
        DeathMenu.SetActive(false);
        PauseMenu.SetActive(false);
        WinMenu.SetActive(true);
    }
}
