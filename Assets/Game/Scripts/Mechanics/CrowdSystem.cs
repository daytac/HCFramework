using System.Collections.Generic;
using UnityEngine;

public class CrowdSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private CrowdUnit unitPrefab; // Çoğaltılacak asker modeli
    [SerializeField] private Transform unitsParent; // Askerler hiyerarşide nerede dursun?
    [SerializeField] private float radius = 0.5f; // Askerler ne kadar sıkışık dursun?
    [SerializeField] private float moveSmoothness = 5f; // Dizilime kayma hızı

    // Altın açı (Doğadaki en verimli paketleme açısı: ~137.5 derece)
    private readonly float _goldenAngle = 137.508f; 

    private List<CrowdUnit> _units = new List<CrowdUnit>();

    private void Start()
    {
        // Başlangıçta sahnede (Container içinde) zaten asker varsa onları listeye al
        foreach (Transform child in unitsParent)
        {
            CrowdUnit unit = child.GetComponent<CrowdUnit>();
            if (unit != null)
            {
                _units.Add(unit);
                unit.Initialize(this);
            }
        }
        
        // İlk dizilimi yap
        FormatCrowd();
    }

    private void Update()
    {
        // Her karede askerleri olması gereken pozisyona doğru yumuşakça kaydır
        // Bu sayede biri ölünce diğerleri boşluğu "akarak" doldurur.
        for (int i = 0; i < _units.Count; i++)
        {
            Vector3 targetPos = GetFormationPosition(i);
            _units[i].transform.localPosition = Vector3.Lerp(_units[i].transform.localPosition, targetPos, moveSmoothness * Time.deltaTime);
        }

        // Eğer ordu bittiyse oyunu kaybet
        if (_units.Count == 0 && GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Fail);
        }
        //space e basınca 5 ekleme yapcak
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddUnits(5);
            Debug.Log("bastın");
        }
    }

    // Matematik Kapısından (+50) geçince bu çağrılacak
    public void AddUnits(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CrowdUnit newUnit = Instantiate(unitPrefab, unitsParent);
            newUnit.transform.localPosition = Vector3.zero; // Ortadan doğsun
            newUnit.Initialize(this);
            _units.Add(newUnit);
        }
    }

    // Asker ölünce bu çağrılacak
    public void RemoveUnit(CrowdUnit unit)
    {
        if (_units.Contains(unit))
        {
            _units.Remove(unit);
            Destroy(unit.gameObject);
        }
    }

    // Sadece tetiklemek için var, asıl iş Update'de
    private void FormatCrowd()
    {
        // Gerekirse buraya özel efektler eklenebilir
    }

    // FERMAT SPİRALİ (MATEMATİKSEL DİZİLİM)
    private Vector3 GetFormationPosition(int index)
    {
        // 0. asker (Kaptan) her zaman merkezde (0,0,0)
        if (index == 0) return Vector3.zero;

        // Formül: Mesafe = Karekök(index) * yarıçap
        float distance = Mathf.Sqrt(index) * radius;
        
        // Açı = index * Altın Açı
        float angle = index * _goldenAngle * Mathf.Deg2Rad; // Radyana çevir

        // Kutupsal koordinattan Kartezyen (X,Z) koordinata geçiş
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        return new Vector3(x, 0, z);
    }

    public int GetUnitCount()
    {
        return _units.Count;
    }
}