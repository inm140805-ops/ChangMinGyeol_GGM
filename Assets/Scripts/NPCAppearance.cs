using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCAppearance : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private SkinnedMeshRenderer[] colorMeshes;
    [SerializeField] private Material[] clothesMaterial;
    private int colorIndex;
    

    private NPCUI npcUI;
    public int clothesID;
    private SkinnedMeshRenderer renderer;

    private void Awake()
    {
        renderer = transform.Find("Sphere").GetComponentInChildren<SkinnedMeshRenderer>();
        npcUI = GetComponentInParent<NPCUI>();
    }

    private void Update()
    {
        if (npcUI.orderComplete)
        {
            SetClothes();
        }
    }

    void Start()
    {
        ChangeColor();
    }

    private void SetClothes()
    {
        Material[] mats = renderer.materials;
        mats[0] = clothesMaterial[clothesID];
        renderer.materials = mats;
        mats[1].SetColor("_BaseColor", colors[colorIndex]);
    }

    private void ChangeColor()
    {
        colorIndex = Random.Range(0, colors.Length);
        Material[] mats = renderer.materials;
        mats[1].SetColor("_BaseColor", colors[colorIndex]);

        foreach (SkinnedMeshRenderer mesh in colorMeshes)
        {
            mesh.material.color = colors[colorIndex];
        }
    }

}
