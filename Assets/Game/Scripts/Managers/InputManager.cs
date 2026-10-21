using UnityEngine;
/*Bu script, hem Mouse (Editörde test için) hem de Dokunmatik
 (Mobil için) girdileri tek bir dilde konuşacak.*/

/*Çözüm Adımları
Unity'de sol üstten Edit > Project Settings... menüsüne tıkla.

Açılan pencerede soldan Player sekmesini seç.

Sağ tarafta (biraz aşağılarda) Configuration başlığını bul.

Active Input Handling yazan ayarı göreceksin. Şu an muhtemelen "Input System Package (New)" seçilidir.

Onu "Both" (İkisi de) olarak değiştir.

Unity sana "Yeniden başlatmam lazım, onaylıyor musun?" diye soracak. Apply (Onayla) de.*/

// Daha önce oluşturduğumuz Singleton yapısını kullanıyoruz
public class InputManager : Singleton<InputManager>
{
    // Diğer scriptlerin okuyacağı veriler
    public bool IsTouching { get; private set; }
    public float MoveFactorX { get; private set; } // Sağa/Sola ne kadar kaydırdı?
    public Vector2 MouseDelta { get; private set; } // Toplam kaydırma vektörü

    private Vector3 _lastMousePos;

    // input hassasiyeti (Panelden ayarlayabilirsiniz)
    [SerializeField] private float sensitivity = 1f;

    private void Update()
    {
        // 1. Dokunma Başladı mı?
        if (Input.GetMouseButtonDown(0))
        {
            IsTouching = true;
            _lastMousePos = Input.mousePosition;
        }
        // 2. Dokunma Bitti mi?
        else if (Input.GetMouseButtonUp(0))
        {
            IsTouching = false;
            MoveFactorX = 0f;
            MouseDelta = Vector2.zero;
        }
        // 3. Parmağını Sürüklüyor mu?
        else if (Input.GetMouseButton(0))
        {
            // Farenin şu anki pozisyonu ile önceki pozisyonu arasındaki fark
            Vector3 currentPos = Input.mousePosition;
            Vector3 delta = currentPos - _lastMousePos;

            // Ekran çözünürlüğüne göre normalize ediyoruz (Farklı telefonlarda aynı hissi versin diye)
            MoveFactorX = (delta.x / Screen.width) * sensitivity;
            
            MouseDelta = delta;
            _lastMousePos = currentPos;
        }
    }
}