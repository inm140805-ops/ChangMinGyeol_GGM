using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.Runtime.CompilerServices;

public class BoxManager : MonoBehaviour
{
    [SerializeField] private Transform[] boxs;
    [SerializeField] private TextMeshProUGUI[] nameTMP;
    [SerializeField] private GameObject[] uiButtonsGet;
    [SerializeField] private GameObject[] uiButtonsPut;
    [SerializeField] public BoxData[] boxDatas;
    [SerializeField] private UnityEngine.UI.Image[] iconImage;
    [SerializeField] private float sizeX;
    [SerializeField] private float sizeY;
    [SerializeField] private float sizeZ;

    private Grab grab;
    private Player player;
    private Animator animatorGet;
    private Animator animatorPut;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        grab = GameObject.Find("Player").GetComponent<Grab>();
    }

    

    private void Update()
    {
        OnTriggerInteraction();
        ConfigSetting();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < boxs.Length; i++)
        {
            Transform box = boxs[i];

            Gizmos.DrawWireCube(
                box.position,
                new Vector3(sizeX * 2, sizeY * 2, sizeZ * 2)
            );
        }
    }

    private void ConfigSetting()
    {
        for (int i = 0; i < boxs.Length; i++)
        {
            TextMeshProUGUI nameText = nameTMP[i];
            UnityEngine.UI.Image icon = iconImage[i];

            nameText.text = boxDatas[i].BoxName;
            icon.sprite = boxDatas[i].Icon;
        }
    }

    private void OnTriggerInteraction()
    {
        bool grabClothesPlayer = player.grabClothes;


        for (int i = 0; i < boxs.Length; i++)
        {
            Collider[] UIHideOverlapBox = Physics.OverlapBox(
               boxs[i].position,
               new Vector3(sizeX, sizeY, sizeZ),
               Quaternion.identity,
               LayerMask.GetMask("PlayerBody"));

            animatorGet = uiButtonsGet[i].GetComponent<Animator>();
            animatorPut = uiButtonsPut[i].GetComponent<Animator>();

            if (player.grabClothes == true)
            {
                uiButtonsGet[i].SetActive(false);
                if (uiButtonsGet[i].activeSelf == true)
                {
                    animatorGet.SetBool("bHoverLoop", false);
                }
            }
            else if (player.grabClothes == false)
            {
                uiButtonsPut[i].SetActive(false);
                if (uiButtonsPut[i].activeSelf == true)
                {
                    animatorPut.SetBool("bHoverLoop", false);
                }
            }

            if (UIHideOverlapBox.Length > 0 && player.grabClothes == false)
            {
                print($"KIA: {UIHideOverlapBox.Length}");
                print($"여기에 닿았습니다 {boxs[i].name}");
                

                uiButtonsGet[i].SetActive(true);
                animatorGet.SetBool("bHoverLoop", true);

                if (Input.GetKeyDown(KeyCode.E) && player.grabClothes == false)
                {
                    player.grabClothes = true;
                    grab.SetID(i);


                }
            }
            else if (UIHideOverlapBox.Length > 0 && player.grabClothes == true && grab.itemID == i)
            {
                uiButtonsPut[i].SetActive(true);
                animatorPut.SetBool("bHoverLoop", true);

                if (Input.GetKeyDown(KeyCode.E) && player.grabClothes == true)
                {
                    player.grabClothes = false;
                    grab.SetID(-1);
                    return;
                }
            }
            else
            {
                uiButtonsGet[i].SetActive(false);
                uiButtonsPut[i].SetActive(false);
                if (uiButtonsGet[i].activeSelf == true || uiButtonsPut[i].activeSelf == true)
                {
                    animatorGet.SetBool("bHoverLoop", false);
                    animatorPut.SetBool("bHoverLoop", false);
                }
            }
        }
    }
}
