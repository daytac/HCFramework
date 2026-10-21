using UnityEngine;
/*Önce her bir yol parçasına "Benden sonraki yol nereye eklensin?"
 bilgisini veren bir etiket yapıştıracağız.*/
public class RoadBlock : MonoBehaviour
{
    // Bir sonraki yolun bağlanacağı nokta (Bitiş ucu)
    public Transform ConnectPoint;

    // Sahne dışına çıkınca kendi kendini yok etme (Basit optimizasyon)
    // Not: İleride bunu Object Pool ile değiştirebiliriz.
    public void DestroyOldRoad()
    {
        Destroy(gameObject);
    }
}
/*1. Yol Parçası (Prefab) Hazırlama:
Sahneye bir Plane (Zemin) oluşturun.
Scale Z: 2 yapın (20 metre uzunluk olur).
Position: (0, 0, 10) yapın. NEDEN? Çünkü Plane'in merkezi ortasıdır.
Pivotu başa almak için onu 10 birim ileri ittik ki (0,0,0) noktası yolun başlangıcı olsun.
Bu Plane'i boş bir GameObject içine atın. Adını Road_Straight koyun.
Road_Straight objesine RoadBlock scriptini ekleyin.

Connect Point Ayarı:
Road_Straight objesinin içine boş bir GameObject daha oluşturun. Adını EndPoint koyun.
Bu EndPointi yolun tam bittiği uca (Z: 20) taşıyın.
RoadBlock scriptindeki Connect Point kutusuna bu EndPoint objesini sürükleyin.
Şimdi bu Road_Straight objesini Proje klasörüne sürükleyip Prefab yapın ve sahneden silin.*/