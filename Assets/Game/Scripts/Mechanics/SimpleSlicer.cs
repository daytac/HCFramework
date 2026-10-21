using UnityEngine;

public class SimpleSlicer : MonoBehaviour
{
    [Header("Kesici Ayarları")]
    [SerializeField] private GameObject sliceEffectPrefab; // Kesince çıkacak partikül (Opsiyonel)

    private void OnTriggerEnter(Collider other)
    {
        // "Slicable" (Kesilebilir) etiketli bir şeye çarptık mı?
        if (other.CompareTag("Slicable"))
        {
            SliceObject(other.transform);
        }
    }

    private void SliceObject(Transform targetObj)
    {
        // ASMR Sabun Kesme Hilesi:
        // Nesnenin Y (Boy) eksenini küçültüyoruz. Sanki üstü kesilmiş gibi duruyor.
        
        Vector3 newScale = targetObj.localScale;
        newScale.y *= 0.5f; // Boyunu yarıya indir

        // Eğer çok küçüldüyse yok et
        if (newScale.y < 0.1f)
        {
            Destroy(targetObj.gameObject);
        }
        else
        {
            targetObj.localScale = newScale;
            
            // Efekt varsa patlat
            if(sliceEffectPrefab) 
                Instantiate(sliceEffectPrefab, targetObj.position, Quaternion.identity);
        }
    }
}