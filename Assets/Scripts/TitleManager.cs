using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public const string TitleScreenName = "TitleScreen";
    public const string CreditsScreenName = "CreditsScreen";
    public const string BattleGroundScreenName = "BattleGround";

    public bool isOnCreditsScene;

    private void Update()
    {
        if (isOnCreditsScene)
        {
            if (Input.GetKeyDown(KeyCode.Return)
                || Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.Escape))
            {
                EnterTitleScreen();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnterBattleGround();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            EnterCreditsScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    public void EnterBattleGround()
    {
        SceneManager.LoadScene(BattleGroundScreenName);
    }

    public void EnterCreditsScreen()
    {
        SceneManager.LoadScene(CreditsScreenName);
    }

    public void EnterTitleScreen()
    {
        SceneManager.LoadScene(TitleScreenName);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game has been quit");
    }
}