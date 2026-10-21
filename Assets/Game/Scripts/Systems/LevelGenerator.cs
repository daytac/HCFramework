using System.Collections.Generic;
using UnityEngine;
/*2. Manager Kurulumu:
Managers objesine LevelGenerator scriptini atın.
Road Prefabs: Listeyi açın, hazırladığınız Road_Straight prefabını ekleyin.
Player Transform: Küp (Karakter) objenizi sürükleyin.*/
public class LevelGenerator : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private RoadBlock[] roadPrefabs; // Rastgele seçilecek yol parçaları
    [SerializeField] private Transform playerTransform; // Oyuncu kim?
    [SerializeField] private int initialRoadCount = 5; // Başlangıçta kaç parça olsun?
    [SerializeField] private float roadDestroyDistance = 30f; // Oyuncu ne kadar uzaklaşınca yol silinsin?

    private List<RoadBlock> _activeRoads = new List<RoadBlock>();
    private Vector3 _nextSpawnPoint; // Bir sonraki yolun doğacağı koordinat

    private void Start()
    {
        // Başlangıç noktasını belirle (Sıfır noktası)
        _nextSpawnPoint = Vector3.zero;

        // İlk yolu ve devamını oluştur
        for (int i = 0; i < initialRoadCount; i++)
        {
            SpawnRoad();
        }
    }

    private void Update()
    {
        // Oyuncu yoksa hata vermesin
        if (playerTransform == null) return;

        // MANTIK: Eğer oyuncu, son yol parçasına yaklaştıysa yeni yol üret
        // Son yolun pozisyonu ile oyuncu arasındaki mesafe kontrolü
        if (_activeRoads.Count > 0)
        {
            // Son yol parçasının merkezi oyuncuya 50 birimden daha yakınsa yeni yol ekle
            RoadBlock lastRoad = _activeRoads[_activeRoads.Count - 1];
            
            if (Vector3.Distance(playerTransform.position, lastRoad.transform.position) < 50f)
            {
                SpawnRoad();
            }
        }

        // ESKİ YOLLARI SİLME
        // Listenin başındaki (en arkadaki) yol oyuncudan çok uzaklaştı mı?
        if (_activeRoads.Count > 0)
        {
            RoadBlock firstRoad = _activeRoads[0];
            
            // Yolun oyuncunun arkasında kaldığından emin ol (Z ekseni farkı)
            if (playerTransform.position.z - firstRoad.transform.position.z > roadDestroyDistance)
            {
                _activeRoads.RemoveAt(0); // Listeden çıkar
                firstRoad.DestroyOldRoad(); // Sahneden sil
            }
        }
    }

    private void SpawnRoad()
    {
        // 1. Rastgele bir prefab seç
        int randomIndex = Random.Range(0, roadPrefabs.Length);
        RoadBlock newRoadPrefab = roadPrefabs[randomIndex];

        // 2. Yeni yolu, belirlenen noktada (_nextSpawnPoint) yarat
        RoadBlock newRoad = Instantiate(newRoadPrefab, _nextSpawnPoint, Quaternion.identity);

        // 3. Bu yolu "Managers" klasörü altında toplama (Hiyerarşi temiz kalsın diye isteğe bağlı)
        newRoad.transform.SetParent(transform);

        // 4. BİR SONRAKİ yolun doğacağı yeri güncelle:
        // Yeni doğan yolun "ConnectPoint"inin olduğu yer, bir sonrakinin başlangıcıdır.
        if (newRoad.ConnectPoint != null)
        {
            _nextSpawnPoint = newRoad.ConnectPoint.position;
        }
        else
        {
            // Eğer ConnectPoint koymayı unuttuysanız düz mantık 10 metre ileri atar
            _nextSpawnPoint += new Vector3(0, 0, 10f); 
        }

        // 5. Listeye ekle
        _activeRoads.Add(newRoad);
    }
}