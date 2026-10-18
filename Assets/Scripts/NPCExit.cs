using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System.Collections;


public class NPCExit : MonoBehaviour
{
    private NavMeshAgent agent;
    public bool isExiting;
    private NPCController controller;
    private Transform[] npcDeadPoint;
    private GameManager gameManager;
    [SerializeField] private bool cheakLine;
    private NPCRoute npcRoute;
    private Transform exitTarget;
    private NPCMovement movement;
    private NPCQueue npcQueue;
    private NPCUI UI;
    private Animator anim;
    private CoreLoopManager coreLoopManager;

    void Start()
    {
        if (npcDeadPoint == null)
        {
            npcDeadPoint = new Transform[4];
        }
    }

    void Awake()
    {
        coreLoopManager = GameObject.Find("CoreLoopManager").GetComponent<CoreLoopManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<NPCController>();
        npcDeadPoint = gameManager.deadPoints;
        npcRoute = GetComponent<NPCRoute>();
        movement = GetComponent<NPCMovement>();
        npcQueue = GetComponent<NPCQueue>();
        UI = GetComponent<NPCUI>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        DestroyPrefab();
        CheakWaypoint();

        if (!coreLoopManager.timerStart)
        {
            OrderCompleteExit();
            
        }
    }


    public void DestroyPrefab()
    {
        bool[] NPCspawnBool = controller.spawnBool;

        if (movement.inLine && UI.goOut && !isExiting)
        {
            int patienceIndex = Random.Range(0, npcDeadPoint.Length);

            ReleaseLine();

            agent.isStopped = false;
            agent.updateRotation = true;
            exitTarget = npcDeadPoint[patienceIndex];
            agent.SetDestination(exitTarget.position);
            anim.SetBool("bWalk", true);
            isExiting = true;
        }

        if (!isExiting && controller.extraNPC)
        {
            if (NPCspawnBool[2])
            {
                ReleaseLine();
                agent.isStopped = false;
                exitTarget = npcDeadPoint[2];
                agent.SetDestination(exitTarget.position);
                anim.SetBool("bWalk", true);
                isExiting = true;
            }else if (NPCspawnBool[3])
            {
                ReleaseLine();
                agent.isStopped = false;
                exitTarget = npcDeadPoint[3];
                agent.SetDestination(exitTarget.position);
                anim.SetBool("bWalk", true);
                isExiting = true;
            }
        }

        if (isExiting && exitTarget == null)
        {
            if (NPCspawnBool[0] || NPCspawnBool[2])
                exitTarget = npcDeadPoint[0];
            else if (NPCspawnBool[1] || NPCspawnBool[3])
                exitTarget = npcDeadPoint[1];
        }

        if (isExiting && exitTarget != null && Vector3.Distance(transform.position, exitTarget.position) <= 0.3f)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator ExitCoroutine(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        OrderCompleteExit();
    }



    public void OrderCompleteExit()
    {
        if (isExiting)
            return;

        bool[] NPCspawnBool = controller.spawnBool;

        if (NPCspawnBool[0] || NPCspawnBool[2])
            exitTarget = npcDeadPoint[0];
        else if (NPCspawnBool[1] || NPCspawnBool[3])
            exitTarget = npcDeadPoint[1];
        else
            return;

        ReleaseLine();

        agent.isStopped = false;
        agent.updateRotation = true;
        agent.SetDestination(exitTarget.position);
        anim.SetBool("bWalk", true);
        isExiting = true;
        
    }

    public void ReleaseLine()
    {
        if (npcQueue.lineTarget >= 0 && npcQueue.lineTarget < gameManager.line.Length)
            gameManager.line[npcQueue.lineTarget] = false;

        npcQueue.lineTarget = -1;
        npcQueue.isQueuing = false;
        movement.inLine = false;
    }

    private void CheakWaypoint() 
    {
        if (npcRoute.waypoints == null)
            return;

        for (int i = 0; i < npcRoute.waypoints.Length; i ++)
        {
            if (npcRoute.waypoints[i] == null)
                continue;

            if (npcRoute.waypoints[i].name == "line0" || npcRoute.waypoints[i].name == "line1" || npcRoute.waypoints[i].name == "line2") 
            {
                cheakLine = true;
            } 
        }
    }

}
