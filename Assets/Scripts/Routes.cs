using UnityEngine;

public class Routes : MonoBehaviour
{
    [SerializeField] private Transform[] route1;
    [SerializeField] private Transform[] route2;
    [SerializeField] private Transform[] route3;
    [SerializeField] private Transform[] route4;

    public Transform[] Route1 => route1;
    public Transform[] Route2 => route2;
    public Transform[] Route3 => route3;
    public Transform[] Route4 => route4;
}
