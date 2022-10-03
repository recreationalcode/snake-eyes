using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource startMusic;
    public AudioSource levelMusic;
    public AudioSource finalLevelMusic;

    public Roll rollManager;
    public CanvasManager canvasManager;

    // void Awake()
    // {
    //     DontDestroyOnLoad(startMusic);
    //     DontDestroyOnLoad(levelMusic);
    //     DontDestroyOnLoad(finalLevelMusic);
    // }

    void OnLevelWasLoaded()
    {
        rollManager = FindObjectsOfType<Roll>()[0];
        canvasManager = FindObjectsOfType<CanvasManager>()[0];

        canvasManager.musicManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!rollManager.GetCanRoll())
        {
            PlayStartMusic();
        }
        else {
            PlayLevelMusic();
        }
    }

    public void PlayStartMusic()
    {
        if (startMusic.isPlaying) return;

        startMusic.Play();
        levelMusic.Stop();
        finalLevelMusic.Stop();
    }

    public void PlayLevelMusic()
    {
        if (SceneManager.GetActiveScene().name == "Level 4")
        {
            _PlayFinalLevelMusic();
        } else {
            _PlayLevelMusic();
        }
    }

    private void _PlayLevelMusic()
    {
        if (levelMusic.isPlaying) return;

        startMusic.Stop();
        levelMusic.Play();
        finalLevelMusic.Stop();
    }

    private void _PlayFinalLevelMusic()
    {
        if (finalLevelMusic.isPlaying) return;

        startMusic.Stop();
        levelMusic.Stop();
        finalLevelMusic.Play();
    }
}
