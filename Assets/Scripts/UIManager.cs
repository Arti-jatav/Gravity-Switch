using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    public Button replayButton;
    [SerializeField] private GravityCameraRig cameraRig;
 

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f; 

    }

    private void Start()
    {
        replayButton.onClick.RemoveAllListeners();
        replayButton.onClick.AddListener(Replay);

        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Cubes: " + score;
    }

    public void ShowGameOver()
    {

        gameOverPanel.SetActive(true);
        cameraRig.SetCursorState(true);
        Time.timeScale = 0f;
    }
    public void ShowWin()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Replay()
    {
       
        cameraRig.SetCursorState(true);
        Time.timeScale = 1f;
     
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}