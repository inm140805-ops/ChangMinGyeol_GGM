using UnityEngine;

public enum SpawnArea
{
    Area1,
    Area2,
    Area3,
    Area4
}

public class NPCRoute : MonoBehaviour
{
    public Transform[] waypoints;
    private NPCController controller;

    private void Awake()
    {
        controller = GetComponent<NPCController>();
    }

    void Start()
    {
        SetRoute();
    }

    public Transform[] GetWaypoints()
    {
        return waypoints;
    }

    private void SetRoute()
    {
        Routes routes = FindAnyObjectByType<Routes>();

        if (controller.spawnBool[0] == true)
        {
            print("구역1");
            waypoints = routes.Route1;
        }
        else if (controller.spawnBool[1] == true)
        {
            print("구역2");
            waypoints = routes.Route2;
        }
        else if (controller.spawnBool[2] == true)
        {
            print("구역3");
            waypoints = routes.Route3;
        }
        else if (controller.spawnBool[3] == true)
        {
            print("구역4");
            waypoints = routes.Route4;
        }
    }
}
