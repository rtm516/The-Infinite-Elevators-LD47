using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Singleton Pattern
    public static UIManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    #endregion

    [Header("HUD")]
    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private GameObject use;

    [SerializeField]
    private TextMeshProUGUI floorsText;

    [Header("Panels")]
    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject pauseMenu;

    [Header("Other")]
    [SerializeField]
    private TextMeshProUGUI gameOverScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCrosshairVisible(bool visbile)
	{
        crosshair.SetActive(visbile);
    }

    public void setUseVisible(bool visbile)
    {
        use.SetActive(visbile);
    }

    public void setFloorCount(int floors)
    {
        floorsText.SetText("Floors: " + floors.ToString().PadLeft(2, '0'));
    }

    public void setGameOverVisible(bool visible)
    {
        Time.timeScale = visible ? 0f : 1f;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        PlayerController.Instance.enabled = !visible;
        gameOverScore.SetText("Floors completed: " + GameManager.Instance.floors);
        gameOverMenu.SetActive(visible);
        floorsText.gameObject.SetActive(!visible);
    }

    public void setPauseVisible(bool visible)
    {
        Time.timeScale = visible ? 0f : 1f;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        PlayerController.Instance.enabled = !visible;
        pauseMenu.SetActive(visible);
        floorsText.gameObject.SetActive(!visible);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
