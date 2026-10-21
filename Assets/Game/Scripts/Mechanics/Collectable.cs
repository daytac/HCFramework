using UnityEngine;
/*Önce yerdeki nesnelerin "Ben toplanabilirim!" diye bağırmasını 
sağlayan basit bir etiket scripti yazalım.*/
public class Collectable : MonoBehaviour
{
    public bool IsCollected { get; private set; } // Zaten toplandı mı?

    public void Collect()
    {
        IsCollected = true;
        
        // Toplanınca collider'ı kapat ki tekrar çarpmasın
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        // İstersen burada partikül patlatabilirsin (ileride Event ile yapacağız)
    }
}