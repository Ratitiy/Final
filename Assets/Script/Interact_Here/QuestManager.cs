using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;



    public QuestData currentQuest;
    Transform orderSpawn;

    public GameObject[] npcPrefabs;

    GameObject spawnedOrderObj;
    GameObject activeNPC;

    public Transform[] npcSpawnPoints;

    public Maker maker;

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
        
        yield return new WaitForSeconds(currentQuest.prepareTime);

       
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

        int m = Random.Range(0, npcPrefabs.Length);
        GameObject selectedNPC = npcPrefabs[m];

        activeNPC = Instantiate(selectedNPC,
            npcSpawnPoints[r].position,
            npcSpawnPoints[r].rotation
        );
        if (maker != null)
        {
            maker.Target(activeNPC.transform);
        }
    }

    public void DeliverSuccess()
    {
        questActive = false;

        if(maker != null)
        {
            maker.Target(activeNPC.transform);
        }
        
        var ui = FindAnyObjectByType<UpgradeUI>();
        ui.playerMoney += (int)currentQuest.rewardMoney;
        ui.UpdateMoneyUI();

        if (spawnedOrderObj != null)
            Destroy(spawnedOrderObj);

        if (activeNPC != null)
            Destroy(activeNPC,5);

        
        currentQuest = null;
    }
}
