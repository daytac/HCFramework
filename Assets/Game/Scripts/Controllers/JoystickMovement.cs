using UnityEngine;

public class JoystickMovement : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float maxJoystickRange = 100f; // Joystick ne kadar uzayabilsin (Piksel)

    private Vector3 _firstTouchPos;   // İlk dokunduğumuz yer (Merkez)
    private Vector3 _currentTouchPos; // Şu an parmağın olduğu yer
    private bool _hasStarted = false; // Kontrol başladı mı?

    private void Update()
    {
        // GameManager kontrolü
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        // 1. Dokunma Başladı (Merkezi belirle)
        if (Input.GetMouseButtonDown(0))
        {
            _firstTouchPos = Input.mousePosition;
            _hasStarted = true;
        }

        // 2. Dokunma Bitti (Resetle)
        if (Input.GetMouseButtonUp(0))
        {
            _hasStarted = false;
        }

        // 3. Parmağını Sürüklüyor (Hareket Et)
        if (_hasStarted)
        {
            _currentTouchPos = Input.mousePosition;

            // Fark vektörü (Yön ve Şiddet)
            Vector2 difference = _currentTouchPos - _firstTouchPos;

            // Eğer çok az çektiyse (Deadzone) hareket etme (Titremeyi önler)
            if (difference.magnitude < 10f) return;

            // Vektörü sınırla (Çok uzağa çekse bile hız sabit kalsın)
            if (difference.magnitude > maxJoystickRange)
            {
                difference = difference.normalized * maxJoystickRange;
            }

            // Yönü hesapla (0 ile 1 arasında bir değer)
            Vector3 direction = new Vector3(difference.x, 0, difference.y).normalized;
            
            // --- DEBUG İÇİN (Konsola yazdır) ---
            // Eğer karakter gitmiyorsa Console'da bu yazıyı görüp görmediğine bak
            //Debug.Log("Joystick Yönü: " + direction); 

            // Hareketi Uygula
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // Dönüşü Uygula
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}