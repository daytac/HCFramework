using UnityEngine;

// Bu yapı, objenin sahne yenilenince yok olmasını sağlar.
// Böylece yeni sahnede taze referanslarla yeni bir Manager doğar.
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Sahnede bu tipten var mı diye bak
                _instance = FindObjectOfType<T>();

                // Yoksa hata verme, null dönsün (Creation yapmıyoruz, sahnede olmalı)
                if (_instance == null)
                {
                    // Debug.LogWarning("Sahnede " + typeof(T).Name + " bulunamadı!");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // Eğer zaten bir instance varsa ve bu ben değilsem, kendimi yok edeyim
        // (Çift Manager olmasın diye)
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
        }
        
        // DİKKAT: Burada "DontDestroyOnLoad(gameObject)" KULLANMIYORUZ.
        // Managerlar da sahneyle birlikte ölüp yenilenmeli.
    }
}