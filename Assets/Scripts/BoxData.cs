
using UnityEngine;
using UnityEngine.UI;

public enum Shirt
{
    FormalShirt = 0,    // 격식 있는 자리
    CasualShirt = 1,    // 일상·데이트·외출
    TShirt = 2,         // 더운 날·편한 차림
    Sweater = 4,        // 추운 날
    Hoodie = 5,          // 편한 외출복
    RainJacket = 6,      // 비 오는 날
    Suit = 7             // 정장
}

[CreateAssetMenu(menuName = "gggg/Box Data")]
public class BoxData : ScriptableObject
{
    [SerializeField] private Shirt[] shirt;
    [SerializeField] private string boxName;
    [SerializeField] private int id;
    [SerializeField] private Sprite icon;

    public Shirt[] ShirtList => shirt;
    public string BoxName => boxName;
    public int Id => id;
    public Sprite Icon => icon;
}
