using UnityEngine;
/*Şimdi bu veriyi kullanarak karakteri hareket ettiren modülü yazalım. 
Bu bir modül olduğu için karakterin üzerine atılacak.*/

public class SwerveMovement : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private float forwardSpeed = 5f; // İleri koşma hızı
    [SerializeField] private float swerveSpeed = 10f; // Sağa sola gitme hızı
    [SerializeField] private float maxX = 4f; // Yolun sınırları (Platformdan düşmemesi için)

    private void Update()
    {
        // 1. Oyun başlamadıysa hareket etme (GameManager kontrolü)
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        // 2. İleri Hareket (Otomatik)
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 3. Swerve (Sağa Sola) Hareket
        if (InputManager.Instance.IsTouching)
        {
            // InputManager'dan gelen veriyi alıyoruz
            float moveX = InputManager.Instance.MoveFactorX * swerveSpeed;

            // Yeni pozisyonu hesapla
            float newX = transform.position.x + moveX;

            // Sınırları (Clamp) uygula: -4 ile +4 arasında tut
            newX = Mathf.Clamp(newX, -maxX, maxX);

            // Pozisyonu güncelle
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}