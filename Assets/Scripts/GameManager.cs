using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    enum GameStates { Playing, Paused}
    enum GoalStates { None, Win, Lose}

    public bool IsPaused => currentGameState == GameStates.Paused;
    public bool IsGameOver=>
        currentGoalState==GoalStates.Win || 
        currentGoalState==GoalStates.Lose;
    
    [SerializeField]GameStates currentGameState;
    [SerializeField] GoalStates currentGoalState;
    [SerializeField] GameObject pauseUi;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    private BaDumTsss _player;
    private AgentManager _agent;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //Time.timeScale = 1f;
        currentGameState = GameStates.Playing;
        _player = FindObjectOfType<BaDumTsss>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Resume()
    {
        pauseUi.SetActive(!pauseUi.activeSelf);
        //Time.timeScale = pauseUi.activeSelf ? 0f : 1.0f;
        currentGameState = pauseUi.activeSelf ? GameStates.Paused : GameStates.Playing;
    }
    public void Restart()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    [ContextMenu("Win")]    
    public void Win()
    {
        currentGoalState = GoalStates.Win;
        winScreen.SetActive(true);
        //Time.timeScale = 0f;
    }

    [ContextMenu("Lose")]
    public void Lose()
    {
        currentGoalState = GoalStates.Lose;
        loseScreen.SetActive(true);
        //Time.timeScale = 0f;
    }
}
