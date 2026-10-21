using UnityEngine;
/*Bilardo veya ok atma mantığıdır. Ekranın herhangi bir yerine basarsınız,
geri çekersiniz ve bıraktığınızda karakter (veya top) fırlar.*/

[RequireComponent(typeof(Rigidbody))]
public class DragMove : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private float powerMultiplier = 10f; // Fırlatma gücü çarpanı
    [SerializeField] private float maxPower = 20f; // Maksimum fırlatma gücü (çok uçmasın)

    private Vector3 _startMousePos;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        // 1. Dokunma Başladı (Nişan alma başlıyor)
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePos = Input.mousePosition;
            _rb.linearVelocity = Vector3.zero; // Yeni atış için durdur
            _rb.angularVelocity = Vector3.zero;
        }

        // 2. Dokunma Bitti (Fırlat!)
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 currentMousePos = Input.mousePosition;
            
            // Çekme yönünü ve gücünü hesapla (Başlangıç - Bitiş)
            Vector3 direction = _startMousePos - currentMousePos;
            
            // Gücü hesapla ve sınırla
            float power = Mathf.Min(direction.magnitude * 0.05f * powerMultiplier, maxPower);
            
            // Yönü 3D dünyaya uyarla (Ekranda Y yukarıdır, dünyada Z ileridir)
            Vector3 forceDir = new Vector3(direction.x, 0, direction.y).normalized;

            // Fizik gücünü uygula
            _rb.AddForce(forceDir * power, ForceMode.Impulse);
        }
    }
}