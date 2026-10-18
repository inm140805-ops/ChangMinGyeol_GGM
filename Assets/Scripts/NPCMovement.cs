using UnityEngine;
using UnityEngine.AI;
public class NPCMovement : MonoBehaviour
{
    [SerializeField] public bool inLine;
    private int currentWaypoint;
    private NPCRoute route;
    private NPCQueue npcQueue;
    private NPCController controller;
    private NavMeshAgent agent;
    private GameManager gameManager;
    private Animator anim;
    private NPCExit npcExit;
    private NPCAppearance appearance;
    private CoreLoopManager coreLoopManager;
    private Vector3 moveDestination;
    private bool hasMoveDestination;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<NPCController>();
        route = GetComponent<NPCRoute>();
        npcExit = GetComponent<NPCExit>();
        npcQueue = GetComponent<NPCQueue>();
        appearance = GetComponent<NPCAppearance>();
        coreLoopManager = GameObject.Find("CoreLoopManager").GetComponent<CoreLoopManager>();
    }

    private void Update()
    {
        IsLineCheak();
        MoveWaypoint();
    }

    private void MoveWaypoint()
    {
        bool[] npcSpawnBool = controller.spawnBool;
        Transform[] npcDeadPoint = controller.deadPoints;
        Transform[] npcWaypoints = route.waypoints;

        if (npcExit.isExiting)
            return;

        if (coreLoopManager.isOpen && npcQueue.isQueuing && npcQueue.lineTarget == -1)
            npcQueue.TryAssignLine();

        if (npcQueue.isQueuing && npcQueue.lineTarget != -1)
        {
            agent.isStopped = false;
            SetMoveDestination(gameManager.linePoint[npcQueue.lineTarget].position);
            if (agent.hasPath && !agent.pathPending && agent.remainingDistance < 0.3f)
            {
                print("줄 도착");
                agent.isStopped = true;
                anim.SetBool("bWalk", false);
                agent.updateRotation = false;
                transform.rotation = Quaternion.Euler(0, 90f, 0);
                return;
            }
            return;
        }
        else if ((npcQueue.isQueuing && npcQueue.lineTarget == -1) || !coreLoopManager.isOpen)
        {
            npcExit.ReleaseLine();

            if (npcSpawnBool[0] || npcSpawnBool[2])
            {
                anim.SetBool("bWalk", true);
                agent.isStopped = false;
                agent.SetDestination(npcDeadPoint[0].position);
                npcExit.isExiting = true;
                return;
            }
            else if (npcSpawnBool[1] || npcSpawnBool[3])
            {
                anim.SetBool("bWalk", true);
                agent.isStopped = false;
                agent.SetDestination(npcDeadPoint[1].position);
                npcExit.isExiting = true;
                return;
            }
        }

        if (npcWaypoints.Length == 0)
            return;

        int lastPoint = npcWaypoints.Length - 1;
        while (lastPoint >= 0 && npcWaypoints[lastPoint] == null)
            lastPoint--;

        if (currentWaypoint >= npcWaypoints.Length)
        {
            agent.isStopped = true;
            anim.SetBool("bWalk", false);
            return;
        }

        if (npcWaypoints[currentWaypoint] == null)
        {
            agent.isStopped = true;
            anim.SetBool("bWalk", false);
            return;
        }

        Transform target = npcWaypoints[currentWaypoint];

        agent.isStopped = false;
        SetMoveDestination(target.position);

        anim.SetBool("bWalk", agent.velocity.magnitude > 0.1f);

        if (agent.hasPath && !agent.pathPending && agent.remainingDistance < 0.3f)
        {
            if (currentWaypoint >= lastPoint)
            {
                npcQueue.isQueuing = true;
                return;
            }

            agent.isStopped = true;
            anim.SetBool("bWalk", false);
            print("웨이포인트 도착");

            currentWaypoint++;
        }
    }

    private void SetMoveDestination(Vector3 destination)
    {
        if (hasMoveDestination && (moveDestination - destination).sqrMagnitude < 0.01f)
            return;

        agent.SetDestination(destination);
        moveDestination = destination;
        hasMoveDestination = true;
    }

    private void IsLineCheak()
    {
        if (npcQueue.lineTarget == 0 || npcQueue.lineTarget == 1 || npcQueue.lineTarget == 2)
        {
            inLine = true;
        }
        else
        {
            inLine = false;
        }
    }
}
