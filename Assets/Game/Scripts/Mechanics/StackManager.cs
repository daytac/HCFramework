using System.Collections.Generic;
using UnityEngine;
//O MEŞHUR "SNAKE" HAREKETİ ve SADECE DİKEY KULE İÇİN (Alternatif Kod) var
/*Bu script karakterin üzerinde duracak ve topladığı elemanları yönetecek.*/

/*1. Fizik Kuralları Kontrolü (En Önemlisi!)
Karakterde (Cube): StackManager scripti var mı? Evet.
Rigidbody bileşeni var mı? Zorunlu. (Use Gravity'yi kapatabilirsin ama Rigidbody olmak zorunda).
Box Collider var mı? Evet.
Collider'ın üzerindeki "Is Trigger" kutucuğu İŞARETLİ Mİ? (İşaretli olmalı).
Toplanacak Nesnede (Top/Sphere):Collectable scripti var mı? Evet.
Sphere Collider (veya Box Collider) var mı? Evet.
Collider'ın üzerindeki "Is Trigger" kutucuğu İŞARETLİ Mİ? (Bu da işaretli olsun, garanti olsun).
2. Referans Hatası Kontrolü
StackManager scriptine baktığında "Stack Target" kısmı boş mu?
Oraya karakterin kendisini (Cube) sürükleyip bıraktın mı? */
public class StackManager : MonoBehaviour
{
    // Mod Seçimi
    public enum StackMode { Snake, VerticalTower, ForwardCarry }
    
    [Header("Mod Ayarları")]
    [SerializeField] private StackMode currentMode = StackMode.Snake;
    
    [Header("Genel Ayarlar")]
    [SerializeField] private Transform stackTarget; // İlk hedef (Genelde Player)
    [SerializeField] private float distanceBetween = 1.0f; // Nesneler arası mesafe
    [SerializeField] private float followDelay = 10f; // Snake takibinin yumuşaklığı (Lerp hızı)

    // Toplananların listesi
    public List<Transform> CollectedList = new List<Transform>();

    private void Start()
    {
        // Eğer hedef atanmadıysa kendini hedef al
        if (stackTarget == null) stackTarget = transform;

        // Listeyi başlat ve hedefi ekle
        if (CollectedList.Count == 0)
        {
            CollectedList.Add(stackTarget);
        }
    }

    private void Update()
    {
        // GameManager kontrolü (İsteğe bağlı açabilirsiniz)
        // if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        // Sadece Snake modunda sürekli pozisyon hesabı gerekir.
        // Tower modunda nesneler Player'ın çocuğu (Child) olduğu için otomatik gider.
        if (currentMode == StackMode.Snake)
        {
            MoveSnake();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collectable item = other.GetComponent<Collectable>();

        if (item != null && !item.IsCollected)
        {
            item.Collect();
            AddNewItem(item.transform);
        }
    }

    public void AddNewItem(Transform itemToAdd)
    {
        // Önce listeye ekle
        CollectedList.Add(itemToAdd);

        // Moduna göre yerleştir
        switch (currentMode)
        {
            case StackMode.Snake:
                // Snake modunda özel bir şey yapmaya gerek yok, Update'de takip edecek.
                // Başlangıçta saçmalamasın diye son elemanın arkasına ışınlayalım.
                itemToAdd.position = CollectedList[CollectedList.Count - 2].position;
                break;

            case StackMode.VerticalTower: // DİKEY KULE (Coffee Stack)
                // Nesneyi Player'ın içine at (Child yap)
                itemToAdd.SetParent(stackTarget); 
                
                // Pozisyonunu ayarla (Yukarı doğru artarak)
                // (Listedeki sıra sayısı * Mesafe)
                float yOffset = (CollectedList.Count - 1) * distanceBetween;
                
                // Karakterin merkezinden yukarı doğru
                itemToAdd.localPosition = new Vector3(0, yOffset, 0); 
                
                // Dönüşünü sıfırla (Dik dursun)
                itemToAdd.localRotation = Quaternion.identity;
                break;

            case StackMode.ForwardCarry: // ELİNDE TAŞIMA (Pizza)
                itemToAdd.SetParent(stackTarget);
                // Öne doğru artarak (Z ekseni)
                float zOffset = (CollectedList.Count - 1) * distanceBetween;
                itemToAdd.localPosition = new Vector3(0, 0, zOffset);
                itemToAdd.localRotation = Quaternion.identity;
                break;
        }
    }

    private void MoveSnake()
    {
    // Döngüyü tersten kuruyoruz (Son gelen, bir öncekini takip eder)
        for (int i = CollectedList.Count - 1; i > 0; i--)
        {
            Transform current = CollectedList[i];
            Transform target = CollectedList[i - 1]; // Bir önceki nesne

            // Hedeflenen pozisyon: Bir öncekinin tam arkası değil, hareket geçmişi.
            // Ama basit ve performanslı bir "Lerp" takibi yapacağız.

            Vector3 targetPos = target.position;
            
            // Eğer tam arkasında değil de "Kuyruk" gibi gelsin istiyorsan:
            current.position = Vector3.Lerp(current.position, targetPos, 10f * Time.deltaTime); // Yumuşak takip
            current.rotation = Quaternion.Lerp(current.rotation, target.rotation, 15f * Time.deltaTime);
            
            // Üst üste binmemeleri için mesafeyi koru (Basit hile)
            // Not: Tam profesyonel snake için "History Point" listesi tutulur ama bu HC için yeterlidir.
            Vector3 directionToTarget = (targetPos - current.position).normalized;
            float dist = Vector3.Distance(targetPos, current.position);
            
            if (dist < distanceBetween)
            {
                // Çok yaklaştıysa durmasın ama yavaşlasın veya pozisyonunu korusun
                 // Bu kısım oyunun türüne göre değişir (Runner için genelde burası boştur, sadece Lerp yeter)
            }
        }
    }
}