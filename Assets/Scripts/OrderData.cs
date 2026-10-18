using UnityEngine;

public enum ShirtList
{
    FormalShirt = 0,
    CasualShirt = 1,
    TShirt = 2,
    Sweater = 4,
    Hoodie = 5,
    RainJacket = 6,
    Suit = 7
}

[CreateAssetMenu(menuName = "GameSettings/Order Data")]
public class OrderData : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string chat;
    [SerializeField] private ShirtList[] shirt;
    [SerializeField] private int[] canID;


    public int Id => id;
    public string Chat => chat;
    public ShirtList[] ShirtList => shirt;
    public int[] CanID => canID;
}
