using UnityEngine;

// Singleton yapını koruyoruz
public class GameManager : Singleton<GameManager>
{
    // Oyunun durumları
    public enum GameState { Intro, Playing, Win, Fail }
    public GameState CurrentState;
    public static bool IsRetrying = false;

   
    private void Start()
    {
        
        // Eğer oyuncu "Yeniden Dene" diyerek geldiyse direkt oyunu başlat
        if (IsRetrying)
        {
            IsRetrying = false; // Değişkeni sıfırla ki bir dahaki sefere normal açılsın
            ChangeState(GameState.Playing);
            Time.timeScale = 1f;
        }
        // Eğer oyun ilk defa açılıyorsa Intro göster
        else
        {
            ChangeState(GameState.Intro);
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        
        // Durum değiştiğinde zamanı her zaman normale döndür (Garanti olsun)
        Time.timeScale = 1f;

        // --- GÜNCELLEME: UIManager BAĞLANTILARI ---
        switch (CurrentState)
        {
            case GameState.Intro:
                Debug.Log("Oyun Hazırlanıyor... Tap to Play bekleniyor.");
                UIManager.Instance.ShowPanel(UIManager.PanelType.Intro); // Intro Paneli Aç
                break;
                
            case GameState.Playing:
                Debug.Log("Oyun Başladı!");
                UIManager.Instance.ShowPanel(UIManager.PanelType.Game); // Oyun içi HUD Aç
                break;
                
            case GameState.Win:
                Debug.Log("Kazandın! Konfetiler patlasın.");
                UIManager.Instance.ShowPanel(UIManager.PanelType.Win); // Win Ekranı Aç
                break;
                
            case GameState.Fail:
                Debug.Log("Kaybettin! Yeniden dene ekranı.");
                UIManager.Instance.ShowPanel(UIManager.PanelType.Fail); // Fail Ekranı Aç
                break;
        }
    }
    
    // --- YENİ EKLENEN: DURDURMA VE DEVAM ETME ---

    public void PauseGame()
    {
        Time.timeScale = 0f; // Zamanı dondur (Fizik ve Update durur)
        UIManager.Instance.ShowPanel(UIManager.PanelType.Pause); // Pause Menüsü Aç
        Debug.Log("Oyun Duraklatıldı.");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Zamanı tekrar akıt
        UIManager.Instance.ShowPanel(UIManager.PanelType.Game); // HUD'a geri dön
        Debug.Log("Oyun Devam Ediyor.");
    }

    // Tap To Play butonu bu fonksiyonu çağıracak
    public void StartGame()
    {
        if(CurrentState == GameState.Intro)
        {
            ChangeState(GameState.Playing);
        }
    }
}