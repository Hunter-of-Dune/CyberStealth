using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public delegate void LevelChanged();
    public static LevelChanged LEVEL_START_EVENT;
    public static LevelChanged LEVEL_END_EVENT;

    public static string GAME_LEVEL_NAME = "Sneak to the end…";
    public static int GAME_LEVEL = 1;

    public List<string> SceneNames = new List<string>();

    AsyncOperation async;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError(name + ": Tried to create a second instance of GameManager singleton.");
        }

        DontDestroyOnLoad(transform.root);
    }

    void Start()
    {
        LevelAdvancePanel.FadeInToEndLevel(LoadLevel);
    }

    void StartLevel()
    {
        if (LEVEL_START_EVENT != null)
        {
            LEVEL_START_EVENT();
        } 
    }

    void LoadLevel()
    {
        async = SceneManager.LoadSceneAsync("Level " + GAME_LEVEL);
        async.completed += Async_completed;
        
    }

    public static void RestartLevel()
    {
        if (LEVEL_END_EVENT != null)
        {
            LEVEL_END_EVENT();
        }
        LevelAdvancePanel.FadeInToEndLevel(Instance.LoadLevel);
    }

    private void Async_completed(AsyncOperation obj)
    {
        AlertModeManager.SwitchToAlertMode(false);
        AudioFootstepCollider.MUTE_NEXT_STEPS();
        LevelAdvancePanel.FadeOutToBeginLevel(StartLevel);
        Transform t = GameObject.Find("_PlayerStart").transform;
        InteractingPlayer.SetPosition(t.position, t.rotation);
        InteractingPlayer.caughtByEnemy = false;
    }

    void Update()
    {

    }
}
