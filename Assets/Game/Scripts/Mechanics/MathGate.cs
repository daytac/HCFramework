using UnityEngine;
using TMPro; // TextMeshPro kullanacaksan (Önerilir) yoksa TextMesh

public class MathGate : MonoBehaviour
{
    public enum GateType { Add, Multiply, Subtract, Divide }

    [Header("Kapı Ayarları")]
    public GateType operation; // İşlem türü
    public int value = 5; // Değer (Örn: 5 ekle, 2 ile çarp)
    
    [Header("Görsel (Opsiyonel)")]
    public TextMeshPro gateText; // Kapının üzerindeki yazı

    private void Start()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        // Kapının üzerindeki yazıyı güncelle (Örn: "x2" veya "+5")
        string symbol = "";
        switch (operation)
        {
            case GateType.Add: symbol = "+"; break;
            case GateType.Multiply: symbol = "x"; break;
            case GateType.Subtract: symbol = "-"; break;
            case GateType.Divide: symbol = "÷"; break;
        }
        
        if(gateText != null) gateText.text = symbol + value;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sadece Player çarparsa çalışsın
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Kapıdan geçildi! İşlem: {operation} {value}");

            // --- BURASI ÖNEMLİ ---
            // Önceki adımlarda yaptığımız sistemlere bağlanıyoruz.
            
            // 1. Eğer CrowdSystem (Count Masters) kullanıyorsan:
            var crowd = other.GetComponentInParent<CrowdSystem>(); // Parent'ta olabilir
            if (crowd != null)
            {
                if (operation == GateType.Add) crowd.AddUnits(value);
                if (operation == GateType.Multiply) crowd.AddUnits(crowd.GetUnitCount() * (value - 1)); // Basit çarpma mantığı
            }

            // 2. Eğer StackSystem kullanıyorsan:
            // (StackManager'a bir Add fonksiyonu eklediğini varsayıyoruz)
            var stack = other.GetComponent<StackManager>();
            if (stack != null)
            {
                // Test için sadece konsola yazdıralım, stack mantığı karmaşıktır.
                Debug.Log("Stack System bulundu, eleman eklenmeli.");
            }

            // 3. Hiçbiri yoksa basit test için Player'ı büyütelim (Görsel Kanıt)
            if (crowd == null && stack == null)
            {
                if (operation == GateType.Add || operation == GateType.Multiply)
                    other.transform.localScale *= 1.2f; // Büyüt
                else
                    other.transform.localScale *= 0.8f; // Küçült
            }

            // Kapıyı kapat (Tekrar tekrar çarpmasın)
            GetComponent<Collider>().enabled = false; 
            // İstersen kapıyı yok et: Destroy(gameObject);
        }
    }
}