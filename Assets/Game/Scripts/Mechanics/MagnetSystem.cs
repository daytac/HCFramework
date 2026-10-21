using UnityEngine;
/*Bu script karakterin üzerindedir. Etraftaki Collectable nesnelerini kendine çeker.*/
public class MagnetSystem : MonoBehaviour
{
    [Header("Mıknatıs Ayarları")]
    [SerializeField] private float range = 5f; // Çekim alanı
    [SerializeField] private float pullSpeed = 10f; // Çekme hızı

    private void Update()
    {
        // Her karede etraftaki nesneleri tara (Performans için Timer konulabilir ama şimdilik Update yeterli)
        // LayerMask ile sadece "Collectable" layerını taramak en iyisidir ama basitleştiriyoruz.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (var hitCollider in hitColliders)
        {
            // Nesnenin üzerinde "Collectable" scripti var mı?
            Collectable item = hitCollider.GetComponent<Collectable>();

            if (item != null && !item.IsCollected)
            {
                // Nesneyi oyuncuya doğru hareket ettir
                hitCollider.transform.position = Vector3.MoveTowards(
                    hitCollider.transform.position, 
                    transform.position, 
                    pullSpeed * Time.deltaTime
                );
            }
        }
    }

    // Editörde mıknatısın alanını görmek için (Gizmos)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}