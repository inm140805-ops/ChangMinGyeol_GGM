using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class CoreLoopManager : MonoBehaviour
{
    [SerializeField] private Transform openOBJ;
    [SerializeField] private float openOBJsizeX;
    [SerializeField] private float openOBJsizeY;
    [SerializeField] private float openOBJsizeZ;
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private GameObject timerOBJ;
    [SerializeField] private GameObject wearingText;
    [SerializeField] public GameObject reportFrame;
    [SerializeField] private TextMeshProUGUI completeOrders;
    [SerializeField] private TextMeshProUGUI fallOrders;
    [SerializeField] private TextMeshProUGUI TotalRevenue;
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private GameObject winButtons;
    [SerializeField] private GameObject fallButtons;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private GameObject dayOBJ;

    private bool timerEnd;
    private Player player;
    private Animator openAnimator;
    private Animator closeAnimator;
    public bool isOpen;
    private float dayCount;
    private float totalTime = 600f;
    private float coundtFloat;
    public bool timerStart;
    public float needMoney = 50000f;

    private void Awake()
    {
        openAnimator = openButton.GetComponent<Animator>();
        closeAnimator = closeButton.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        openFounction();

        if (isOpen && !timerStart)
        {
            StartCoroutine(TimerCoroutine());
            timerStart = true;
        }

        if (timerEnd)
        {
            gameEndFuntion(false);
        }

        dayText.text = "Day " + dayCount;

        
    }

    private void NeedMoneyCount(float nowMoney)
    {
        float nextDayMoney = nowMoney * 2 / 1.5f;

        needMoney = nextDayMoney;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            openOBJ.position,
            new Vector3(openOBJsizeX * 2, openOBJsizeY * 2, openOBJsizeZ * 2)
        );
    }

    

    IEnumerator TimerCoroutine()
    {
        while (totalTime > 0)
        {
            
            float minutes = Mathf.FloorToInt(totalTime / 60);
            float seconds = Mathf.FloorToInt(totalTime % 60);

            timer.text = $"{minutes:00}:{seconds:00}";
            yield return new WaitForSeconds(1.0f);

            totalTime -= 1f;
            yield return null;
        }

        timer.text = "00:00";
        timerEnd = true;

    }


    private void openFounction()
    {
        Collider[] UIHideOverlapBox = Physics.OverlapBox(
               openOBJ.position,
               new Vector3(openOBJsizeX, openOBJsizeY, openOBJsizeZ),
               Quaternion.identity,
               LayerMask.GetMask("PlayerBody"));

        if (UIHideOverlapBox.Length > 0 && Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen && needMoney <= player.money)
            {
                gameEndFuntion(true);
            }
            else if (!isOpen)
            {
                isOpen = true;
                timerOBJ.SetActive(true);

                dayOBJ.SetActive(true);
            }
            else if (needMoney > player.money)
            {
                wearingText.SetActive(true);
                Invoke("HideWearingText", 2f);
            }
        }

        if (UIHideOverlapBox.Length > 0 && !isOpen && !reportFrame.activeSelf)
        {
            openButton.SetActive(true);
            closeButton.SetActive(false);
            openAnimator.SetBool("bHoverLoop", true);
        }
        else if (UIHideOverlapBox.Length == 0 && !isOpen && !reportFrame.activeSelf)
        {
            openButton.SetActive(false);
            openAnimator.SetBool("bHoverLoop", false);
        }

        if (UIHideOverlapBox.Length > 0 && isOpen)
        {
            closeButton.SetActive(true);
            openButton.SetActive(false);
            closeAnimator.SetBool("bHoverLoop", true);
        }
        else if (UIHideOverlapBox.Length == 0 && isOpen)
        {
            closeButton.SetActive(false);
            closeAnimator.SetBool("bHoverLoop", false);
        }
    }

    private void gameEndFuntion(bool resultbool) 
    {
        if (resultbool)
        {
            isOpen = false;
            timerStart = false;
            reportFrame.SetActive(true);
            completeOrders.text = $"완료한 주문 수: {player.completeOrder}";
            fallOrders.text = $"실패한 주문 수: {player.fallOrder}";
            result.text = $"성공";
            TotalRevenue.text = $"총 수익: {player.money}";
            fallButtons.SetActive(false);
            winButtons.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            StopAllCoroutines();
            NeedMoneyCount(player.money);
        }
        else if (!resultbool)
        {
            if (needMoney <= player.money)
            {
                gameEndFuntion(true);
            }
            else
            {
                isOpen = false;
                timerStart = false;
                reportFrame.SetActive(true);
                completeOrders.text = $"완료한 주문 수: {player.completeOrder}";
                fallOrders.text = $"실패한 주문 수: {player.fallOrder}";
                result.text = $"실패";
                TotalRevenue.text = $"총 수익: {player.money}";
                fallButtons.SetActive(true);
                winButtons.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
                StopAllCoroutines();
            }
            
        }
    }

    public void AgainButton()
    {
        
        player.money = 0;
        dayCount += 1;
        player.fallOrder = 0;
        player.completeOrder = 0;
        timerEnd = false;

        reportFrame.SetActive(false);
        fallButtons.SetActive(false);
        winButtons.SetActive(false);

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        totalTime = 600;
        timerStart = false;
        timerEnd = false;
        timer.text = "00:00";

        
    }

    private void HideWearingText()
    {
        wearingText.SetActive(false);
    }

}
