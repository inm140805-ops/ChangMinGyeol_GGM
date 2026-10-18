using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public bool maxPatience;
    public Transform[] deadPoints;
    private NPCMovement movement;
    private NavMeshAgent agent;
    public bool enterLine;
    public int orderDataID;

    private NPCUI npcUI;


    public bool extraNPC { get; private set; }
    public bool[] spawnBool;

    private void Update()
    {
        EnterLine();
    }

    private void Start()
    {
        print("ID:" + orderDataID);
        SetExtra();
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<NPCMovement>();
        npcUI = GetComponent<NPCUI>();
    }

    private void SetExtra()
    {
        int extraIndex = Random.Range(0, 2);
        if ( extraIndex == 1 )
            extraNPC = true;
    }

    private void EnterLine()
    {
        if (movement.inLine && !agent.pathPending && agent.remainingDistance < 0.3f)
            enterLine = true;
    }
}
