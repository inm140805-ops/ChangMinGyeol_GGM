using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCUI : MonoBehaviour
{
    [SerializeField] private GameObject chatImage;
    [SerializeField] private TextMeshProUGUI chatTMP;
    [SerializeField] private GameObject body;
    [SerializeField] private float sizeX;
    [SerializeField] private float sizeY;
    [SerializeField] private float sizeZ;
    [SerializeField] private Animator chatAnimator;
    [SerializeField] private Animator interactionAnim;
    [SerializeField] private GameObject moodFrame;
    [SerializeField] private GameObject buttonFrame;
    [SerializeField] private GameObject chatFrame;
    [SerializeField] private GameObject giveButton;
    [SerializeField] private Animator giveButtonAnim;
    private NPCQueue npcQueue;
    private NPCMovement movement;
    private NPCAppearance npcAppearance;
    [SerializeField] private Sprite[] moodSprite;
    [SerializeField] private Image moodImage;
    [SerializeField] private float moodTimer;
    [SerializeField] private BoxData[] boxDatas;
    private float maxTime = 150f;
    public bool goOut;
    [SerializeField] private Image moodBar;
    private NPCController controller;
    public OrderData[] orderDatas;
    [SerializeField] private bool cheakColl;
    private int orderDataID;
    private BoxManager boxManager;
    private Grab grab;
    public bool orderComplete;
    private NPCExit npcExit;
    private Player player;

    private void Awake()
    {
        movement = GetComponentInParent<NPCMovement>();
        npcQueue = GetComponentInParent<NPCQueue>();
        controller = GetComponentInParent<NPCController>();
        boxManager = GetComponentInParent<BoxManager>();
        grab = GameObject.Find("Player").GetComponent<Grab>();
        npcExit = GetComponentInParent<NPCExit>();
        player = GameObject.Find("Player").GetComponent<Player>();
        npcAppearance = GetComponentInParent<NPCAppearance>();
    }
    

    private void Update()
    {
        bool openedChatThisFrame = UIAnim();
        Patience();
        if (cheakColl && !openedChatThisFrame)
        {
            GiveButton();
        }

        if (orderComplete == true)
        {
            UIActive(false);
            chatFrame.SetActive(false);
        }
    }

    private void Start()
    {
        OrderService();
    }

    private void GiveButton()
    {

        if (cheakColl)
        {
            Collider[] UIHideOverlapBox = Physics.OverlapBox(body.transform.position,
           new Vector3(sizeX, sizeY, sizeZ),
           Quaternion.identity,
           LayerMask.GetMask("Player"));

            if (UIHideOverlapBox.Length > 0 && cheakColl)
            {
                giveButton.SetActive(true);
                giveButtonAnim.SetBool("bHoverLoop", true);

                if (Input.GetKeyDown(KeyCode.E) && cheakColl)
                {
                    if (grab.itemID < 0 || grab.itemID >= boxDatas.Length)
                        return;

                    for (int i = 0; i < orderDatas[orderDataID].CanID.Length; i++)
                    {
                        if (boxDatas[grab.itemID].Id == orderDatas[orderDataID].CanID[i])
                        {
                            npcAppearance.clothesID = boxDatas[grab.itemID].Id;
                            StartCoroutine(npcExit.ExitCoroutine(2f));
                            orderComplete = true;
                            player.grabClothes = false;
                            grab.SetID(-1);
                            player.completeOrder += 1f;
                            player.money += 5000f;
                            break;
                        }
                    }
                }
            }
            else if (UIHideOverlapBox.Length <= 0 && cheakColl)
            {
                giveButton.SetActive(false);
                giveButtonAnim.SetBool("bHoverLoop", false);
            }
        }
    }

    private bool UIAnim()
    {
        Collider[] UIHideOverlapBox = Physics.OverlapBox(body.transform.position,
            new Vector3(sizeX, sizeY, sizeZ),
            Quaternion.identity,
            LayerMask.GetMask("Player"));

        if (Input.GetKeyDown(KeyCode.E) && !cheakColl && controller.enterLine && UIHideOverlapBox.Length > 0)
        {
            buttonFrame.SetActive(false);
            chatFrame.SetActive(true);
            cheakColl = true;

            chatAnimator.SetBool("bChat", true);
            return true;
        }

        if (controller.enterLine)
        {

            if (UIHideOverlapBox.Length > 0 && npcQueue.isQueuing)
            {
                UIActive(true);
                interactionAnim.SetBool("bHoverLoop", true);

            }
            else
            {
                UIActive(false);
                interactionAnim.SetBool("bHoverLoop", false);
            }
        }

        return false;
    }

    private void OrderService()
    {
        int dataIndex = Random.Range(0, orderDatas.Length);
        controller.orderDataID = orderDatas[dataIndex].Id;
        chatTMP.text = orderDatas[dataIndex].Chat;

        orderDataID = dataIndex;
    }

    private void UIActive(bool active)
    {

        moodFrame.SetActive(active);
        buttonFrame.SetActive(active && !cheakColl);
    }

    private void Patience()
    {
        if (!movement.inLine)
            return;

        moodTimer += Time.deltaTime;
        moodBar.fillAmount = Mathf.Clamp01(moodTimer / maxTime);

        if (moodTimer >= 100f)
        {
            moodImage.sprite = moodSprite[2];
        }
        else if (moodTimer >= 50f)
        {
            moodImage.sprite = moodSprite[1];
        }
        else
        {
            moodImage.sprite = moodSprite[0];
        }

        if (moodTimer >= maxTime)
        {
            goOut = true;
            player.fallOrder += 1f;
        }

    }
}


