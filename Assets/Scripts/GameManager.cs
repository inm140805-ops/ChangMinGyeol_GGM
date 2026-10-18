using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private Transform[] S1;
    public Transform[] deadPoints;
    public Transform[] linePoint;
    public bool[] line;
    public bool fullLine;
    void Start()
    {
        StartCoroutine(SpawnEnemies(spawnTime));
    }

    private void Update()
    {
        CheakFull();
    }

    IEnumerator SpawnEnemies(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            SpawnNPC();
        }
    }

    private void SpawnNPC()
    {
        int spawnIndex = Random.Range(0, S1.Length);
        GameObject NPC = Instantiate(npcPrefab, S1[spawnIndex].position, S1[spawnIndex].rotation);
        NPCController npcController = NPC.GetComponent<NPCController>();

        npcController.spawnBool[spawnIndex] = true;
        npcController.deadPoints = deadPoints;
    }

    private void CheakFull() 
    {
        if (line[0] && line[1] && line[2])
            fullLine = true;
    }
}
