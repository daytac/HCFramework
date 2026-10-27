using UnityEngine;

/* ASMR Dilimleyici (Slicer)
   Nesneye değdikçe katman katman tıraşlar, havaya fiziksel talaş/sabun kırıntıları
   fırlatır ve parçaları toplanabilir hale getirir. */
public class SimpleSlicer : MonoBehaviour
{
    [Header("Dilimleme Ayarları")]
    [SerializeField] private float sliceAmount = 0.25f; // Her temasta ne kadar incelsin?
    [SerializeField] private float sliceCooldown = 0.08f; // Katman kesme sıklığı (ASMR akıcılığı)

    [Header("ASMR Kırıntı / Talaş Ayarları")]
    [SerializeField] private int debrisPerSlice = 5; // Her dilimde kaç parça fırlasın?
    [SerializeField] private float debrisForce = 3.5f; // Fırlama gücü
    [SerializeField] private GameObject debrisPrefab; // Özel kırıntı prefabı (Boş bırakılırsa otomatik küp üretir)
    [SerializeField] private GameObject sliceEffectPrefab; // Varsa partikül efekti

    private float _lastSliceTime;
    private Vector3 _originalBladeLocalPos;

    private void Start()
    {
        _originalBladeLocalPos = transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        TrySlice(other);
    }

    private void OnTriggerStay(Collider other)
    {
        // Bıçak bloğun içinde kaldıkça tıraşlamaya devam etsin (Gerçek ASMR hissi)
        TrySlice(other);
    }

    private void TrySlice(Collider other)
    {
        if (Time.time - _lastSliceTime < sliceCooldown) return;

        if (other.CompareTag("Slicable"))
        {
            _lastSliceTime = Time.time;
            SliceObject(other.transform);
        }
    }

    private void SliceObject(Transform targetObj)
    {
        // 1. Havaya renkli kırıntılar / sabun talaşları fırlat
        SpawnDebris(targetObj);

        // 2. Bloğu üstten kademeli olarak incelt
        Vector3 newScale = targetObj.localScale;
        newScale.y -= sliceAmount;

        // Bıçağa minik bir kesme sarsıntısı ver
        transform.localPosition = _originalBladeLocalPos + Vector3.forward * 0.05f;
        CancelInvoke(nameof(ResetBladePosition));
        Invoke(nameof(ResetBladePosition), 0.04f);

        // Eğer blok tamamen tükendiyse yok et
        if (newScale.y <= 0.15f)
        {
            Destroy(targetObj.gameObject);
        }
        else
        {
            targetObj.localScale = newScale;

            // Bloğun tabanını zeminde (Y=0) sabit tut (havada asılı kalmasın)
            targetObj.position = new Vector3(targetObj.position.x, newScale.y * 0.5f, targetObj.position.z);

            if (sliceEffectPrefab)
            {
                Instantiate(sliceEffectPrefab, targetObj.position + Vector3.up * (newScale.y * 0.5f), Quaternion.identity);
            }
        }
    }

    private void SpawnDebris(Transform targetObj)
    {
        // Kesilen sabunun/bloğun rengini al
        Color blockColor = Color.cyan;
        Renderer rend = targetObj.GetComponent<Renderer>();
        if (rend != null && rend.material != null)
        {
            blockColor = rend.material.color;
        }

        float topY = targetObj.position.y + (targetObj.localScale.y * 0.5f);

        for (int i = 0; i < debrisPerSlice; i++)
        {
            GameObject piece;

            if (debrisPrefab != null)
            {
                piece = Instantiate(debrisPrefab);
            }
            else
            {
                // Prefab yoksa otomatik küçük fiziksel talaş küpü üret
                piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
                float randomScale = Random.Range(0.12f, 0.22f);
                piece.transform.localScale = new Vector3(randomScale, randomScale * 0.5f, randomScale);

                // Rengini sabun rengiyle eşle
                var pieceRend = piece.GetComponent<Renderer>();
                if (pieceRend != null)
                {
                    pieceRend.material.color = blockColor;
                }

                // Fizik bileşeni
                var rb = piece.AddComponent<Rigidbody>();
                rb.mass = 0.1f;

                // Yerden toplanabilmesi için Collectable ekle
                piece.AddComponent<Collectable>();

                // Sahneyi temiz tutmak için 4 saniye sonra yok et
                Destroy(piece, 4f);
            }

            // Kırıntıyı bloğun tam kesilen üst yüzeyinde doğur
            piece.transform.position = new Vector3(
                targetObj.position.x + Random.Range(-0.35f, 0.35f),
                topY + 0.1f,
                targetObj.position.z + Random.Range(-0.35f, 0.35f)
            );

            // Bıçağın ters yönüne (arkaya ve yukarıya) doğru fırlat (Talaş dökülme hissi)
            Rigidbody pieceRb = piece.GetComponent<Rigidbody>();
            if (pieceRb != null)
            {
                Vector3 launchDir = -transform.forward * 0.8f + Vector3.up * 1.4f + Random.insideUnitSphere * 0.4f;
                pieceRb.AddForce(launchDir.normalized * debrisForce, ForceMode.Impulse);
                pieceRb.AddTorque(Random.insideUnitSphere * 15f, ForceMode.Impulse);
            }
        }
    }

    private void ResetBladePosition()
    {
        transform.localPosition = _originalBladeLocalPos;
    }
}
