using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    [Header("Bileşenler")]
    [SerializeField] private Animator animator; // Karakterin animatörü (Varsa)
    [SerializeField] private Rigidbody mainRigidbody; // Ana hareket fiziği
    [SerializeField] private Collider mainCollider; // Ana kapsül collider

    [Header("Ayarlar")]
    [SerializeField] private float pushForce = 5f; // Çarpınca geriye savrulma gücü

    // Vücudun parçalarındaki tüm fizik bileşenleri
    private List<Rigidbody> _ragdollRigidbodies = new List<Rigidbody>();
    private List<Collider> _ragdollColliders = new List<Collider>();

    private void Awake()
    {
        // 1. Karakterin altındaki (Kemiklerindeki) tüm fizik bileşenlerini bul
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        Collider[] cols = GetComponentsInChildren<Collider>();

        // 2. Ana karakterin kendi fiziğini (Main) listeye ekleme, onları ayır
        foreach (var rb in rbs)
        {
            if (rb != mainRigidbody) _ragdollRigidbodies.Add(rb);
        }

        foreach (var col in cols)
        {
            if (col != mainCollider) _ragdollColliders.Add(col);
        }

        // Başlangıçta Ragdoll KAPALI olsun (Karakter dimdik dursun)
        ToggleRagdoll(false);
    }

    // Ragdoll'u Aç/Kapa yapan ana fonksiyon
    public void ToggleRagdoll(bool state)
    {
        // state = true ise RAGDOLL AKTİF (Ölü)
        // state = false ise RAGDOLL PASİF (Canlı)

        // 1. Animasyon durumu (Ölünce kapanmalı)
        if (animator != null) animator.enabled = !state;

        // 2. Ana hareket fiziği (Ölünce bu devre dışı kalmalı ki kemikler düşsün)
        if (mainRigidbody != null) mainRigidbody.isKinematic = state; 
        if (mainCollider != null) mainCollider.enabled = !state;

        // 3. Kemiklerin durumu
        foreach (var rb in _ragdollRigidbodies)
        {
            rb.isKinematic = !state; // Canlıyken kinematic (donuk), ölünce fiziksel
        }

        foreach (var col in _ragdollColliders)
        {
            col.enabled = state; // Canlıyken çarpışmasın, ölünce çarpsın
        }
    }

    // Dışarıdan veya çarpışma anında çağrılacak fonksiyon
    public void ActivateRagdoll()
    {
        // Zaten ölüyseniz tekrar ölmeyin
        if (GameManager.Instance.CurrentState == GameManager.GameState.Fail) return;

        // Ragdoll'u aç
        ToggleRagdoll(true);

        // Hareket scriptlerini durdur (Swerve, Joystick vs.)
        DisableMovements();

        // Hafif bir savrulma efekti (Opsiyonel: Geriye doğru itme)
        // Rastgele bir kemiğe güç uygula (Genelde Kalça/Hips [0] olur)
        if (_ragdollRigidbodies.Count > 0)
        {
            _ragdollRigidbodies[0].AddForce(-transform.forward * pushForce, ForceMode.Impulse);
        }

        Debug.Log("Karakter Öldü ve Yığıldı!");
        
        // Oyunu bitir
        GameManager.Instance.ChangeState(GameManager.GameState.Fail);
    }

    private void DisableMovements()
    {
        // Karakterin üzerindeki olası hareket kodlarını bul ve kapat
        var swerve = GetComponent<SwerveMovement>();
        if (swerve) swerve.enabled = false;

        var joystick = GetComponent<JoystickMovement>();
        if (joystick) joystick.enabled = false;
        
        var forward = GetComponent<TapMove>();
        if (forward) forward.enabled = false;
    }

    // Basit test için: "Obstacle" etiketli bir şeye çarparsa öl
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ActivateRagdoll();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bir şeye temas ettim: " + other.name);

        // Çarptığım şeyin etiketi 'Obstacle' mı?
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Engele çarptım! Ölüyorum...");
            ActivateRagdoll();
        }
    }
}