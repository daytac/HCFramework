using UnityEngine;

public class CrowdUnit : MonoBehaviour
{
    // Hangi orduya bağlıyım?
    private CrowdSystem _myCrowd;
    public void Initialize(CrowdSystem crowdSystem)
    {
        _myCrowd = crowdSystem;
        // Animasyon başlat (Koşma)
        var anim = GetComponent<Animator>();
        if (anim) anim.SetBool("IsRunning", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Engele çarparsa
        if (other.CompareTag("Obstacle"))
        {
            // Kendini yok etmesi için yöneticiye haber ver
            if (_myCrowd != null)
            {
                _myCrowd.RemoveUnit(this);
                // Burada bir "Puff" efekti veya kan efekti oluşturulabilir.
            }
        }
    }
}
/*Ana Konteyner (Kaptan):
Sahneye boş bir GameObject koyun, adını PlayerContainer yapın.
Buna SwerveMovement (veya Joystick) scriptini atın.
Buna TapMove scriptini atın (İleri gitmesi için).
ÖNEMLİ: Bu objede Rigidbody veya Collider OLMASIN. Bu sadece hayalet bir sürücüdür.

Units Parent (Askerlerin Evi):
PlayerContainer'ın içine boş bir GameObject daha açın. Adını UnitsParent yapın.
Konumu (0,0,0) olsun.

Yöneticiyi Bağla:
PlayerContainer objesine CrowdSystem scriptini atın.
Units Parent kutusuna, az önce oluşturduğunuz UnitsParent objesini sürükleyin.
Radius: 0.5 veya 0.7 yapın (Karakterlerin şişmanlığına göre).

Asker (Prefab) Hazırlığı:
Sahneye bir Cube veya Capsule koyun (İleride Mixamo karakteri olacak).
Boyutunu küçültün (Scale: 0.5, 0.5, 0.5).
Üzerine CrowdUnit scriptini atın.
Box Collider'ı olsun ve Is Trigger kutucuğu İŞARETLİ olsun (Duvara çarpınca ölsün diye).
Bu objeyi "Project" penceresine sürükleyip Prefab yapın ve sahneden silin.

Son Dokunuş:
CrowdSystem scriptindeki Unit Prefab kutusuna bu yeni prefabı sürükleyin.
Oyun başlarken en az 1 karakter olsun istiyorsanız, prefabtan bir tanesini 
UnitsParent'ın içine sürükleyip bırakın.*/