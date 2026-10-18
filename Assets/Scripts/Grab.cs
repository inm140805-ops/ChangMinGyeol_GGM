using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private GameObject handR;
    [SerializeField] private GameObject handL;
    [SerializeField] private Transform handRPos;
    [SerializeField] private Transform handLPos;
    [SerializeField] private Transform orHandRPos;
    [SerializeField] private Transform orHandLPos;

    private GameObject mainItem;
    public int itemID = -1;
    [SerializeField] private GameObject item;

    public void SetID(int id)
    {
        itemID = id;
    }

    private void Update()
    {
        if (itemID == -1)
        {
            item.SetActive(false);

        }
        else
        {
            item.SetActive(true);
        }

        if (itemID != -1)
        {
            handR.transform.position = handRPos.position;
            handL.transform.position = handLPos.position;
        }else if (itemID == -1)
        {
            handR.transform.position = orHandRPos.position;
            handL.transform.position = orHandLPos.position;
        }
    }

}
