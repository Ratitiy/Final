using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    QuestData currentQuest;
    Transform orderSpawn;

    GameObject spawnedOrderObj;
    GameObject activeNPC;

    public Transform[] npcSpawnPoints;

    public bool questActive = false;

    void Awake()
    {
        Instance = this;
    }

    public void StartQuest(QuestData data, Transform spawnPoint)
    {
        if (questActive) return;

        questActive = true;
        currentQuest = data;
        orderSpawn = spawnPoint;

        StartCoroutine(PrepareOrder());
    }

    System.Collections.IEnumerator PrepareOrder()
    {
        // รอเวลาตาม QuestData
        yield return new WaitForSeconds(currentQuest.prepareTime);

        // สร้างอาหาร
        spawnedOrderObj = Instantiate(
            currentQuest.orderPrefab,
            orderSpawn.position,
            orderSpawn.rotation
        );

        SpawnRandomNPC();
    }

    void SpawnRandomNPC()
    {
        if (activeNPC != null) Destroy(activeNPC);

        int r = Random.Range(0, npcSpawnPoints.Length);
        activeNPC = Instantiate(
            Resources.Load<GameObject>("DeliveryNPC"),
            npcSpawnPoints[r].position,
            npcSpawnPoints[r].rotation
        );
    }

    public void DeliverSuccess()
    {
        questActive = false;

        // ให้รางวัลเงิน (เชื่อมกับ UpgradeUI)
        var ui = FindAnyObjectByType<UpgradeUI>();
        ui.playerMoney += (int)currentQuest.rewardMoney;
        ui.UpdateMoneyUI();

        if (spawnedOrderObj != null)
            Destroy(spawnedOrderObj);

        if (activeNPC != null)
            Destroy(activeNPC);

        // หลังส่งเสร็จ → พร้อมสุ่มเควสใหม่ทันที
        currentQuest = null;
    }
}
