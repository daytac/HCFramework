using UnityEngine;
using UnityEngine.SceneManagement; // Sahne resetlemek için şart
using UnityEngine.UI; // Text ve Button için

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public enum PanelType { Intro, Game, Pause, Win, Fail }

    [Header("Paneller")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;

    [Header("HUD Elemanları")]
    [SerializeField] private Text levelText;
    [SerializeField] private Text scoreText; // İleride skor tutarsan

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // PANELLERİ AÇIP KAPATAN FONKSİYON
    public void ShowPanel(PanelType panel)
    {
        // Önce hepsini kapat
        introPanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        failPanel.SetActive(false);

        // İstenileni aç
        switch (panel)
        {
            case PanelType.Intro: introPanel.SetActive(true); break;
            case PanelType.Game: gamePanel.SetActive(true); break;
            case PanelType.Pause: pausePanel.SetActive(true); break;
            case PanelType.Win: winPanel.SetActive(true); break;
            case PanelType.Fail: failPanel.SetActive(true); break;
        }
    }

    // --- BUTON FONKSİYONLARI (Editörden Bağlanacak) ---

    // 1. Intro Ekranındaki "Tap to Play" butonu
    public void Button_TapToPlay()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
    }

    // 2. Oyun içindeki "Pause" butonu
    public void Button_Pause()
    {
        GameManager.Instance.PauseGame();
    }

    // 3. Pause ekranındaki "Resume" butonu
    public void Button_Resume()
    {
        GameManager.Instance.ResumeGame();
    }

    // 4. Fail/Pause ekranındaki "Restart" butonu
    public void Button_Restart()
    {
        GameManager.IsRetrying = true;

        // Sahneyi yenile
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 5. Win ekranındaki "Next Level" butonu
    public void Button_NextLevel()
    {
        // Level yönetimi henüz yoksa başa sarar
        // İleride: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Button_Restart(); 
    }
}