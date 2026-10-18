using UnityEngine;

public class NPCQueue : MonoBehaviour
{
    public bool isQueuing;
    public int lineTarget = -1;
    private GameManager gameManager;
    private NPCExit npcExit;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        npcExit = GetComponent<NPCExit>();
    }

    private void Update()
    {
        TryAssignLine();
    }

    public void TryAssignLine()
    {
        if (!isQueuing || npcExit.isExiting)
            return;

        for (int i = 0; i < gameManager.line.Length; i++)
        {
            if (!gameManager.line[i] && lineTarget == -1)
            {
                lineTarget = i;
                gameManager.line[i] = true;

                return;
            }
        }
    }
}
