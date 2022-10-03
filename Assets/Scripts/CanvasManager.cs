using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public Roll rollManager;
    public MusicManager musicManager;
    public GameObject gameStartUI;
    public GameObject inGameUI;
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public bool isGameStart;
    
    // Start is called before the first frame update
    void Start()
    {
        // gameStartUI = GameObject.Find("Game Start UI");
        // inGameUI = GameObject.Find("In Game UI");
        // gameOverUI = GameObject.Find("Game Over UI");
        // gameWinUI = GameObject.Find("Game Win UI");

        gameStartUI.SetActive(isGameStart);
        inGameUI.SetActive(!isGameStart);
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        rollManager.CanRoll(true);
        musicManager.PlayLevelMusic();

        gameStartUI.SetActive(false);
        inGameUI.SetActive(true);
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    public void GameOver()
    {
        rollManager.CanRoll(false);
        musicManager.PlayStartMusic();

        gameStartUI.SetActive(false);
        inGameUI.SetActive(false);
        gameOverUI.SetActive(true);
        gameWinUI.SetActive(false);        
    }

    public void TryAgain()
    {
        // TODO Move this outta here
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameWin()
    {
        rollManager.CanRoll(false);

        gameStartUI.SetActive(false);
        inGameUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(true);        
    }
}
