using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] Items items;

    [SerializeField] GameObject handInv;
    [SerializeField] GameObject bagInv;
    [SerializeField] public GameObject cursorSlot;

    private Item[] hand;
    private int[] handAmount;
    private Item[] bag = new Item[10];
    private int[] bagAmount = new int[10];

    private Item cursorItem;
    private int cursorItemAmount;

    public int currentHandSlotIndex = 0;
    public Item CurrenItem => hand[currentHandSlotIndex];
    public int CurrentItemAmount => handAmount[currentHandSlotIndex];

    SlotController[] handSlots;
    Image[] handSlotImages;
    TMP_Text[] handSlotCounts;
    Image[] handSlotColors;

    SlotController[] bagSlots;
    Image[] bagSlotImages;
    TMP_Text[] bagSlotCounts;

    Image cursorImage;
    TMP_Text cursorText;

    private int beforeNum;

    private void Awake()
    {
        Instance = this;
        Init();
    }
    public void Init()
    {
        hand = new Item[5];
        handAmount = new int[5];
        handSlots = handInv.GetComponentsInChildren<SlotController>();
        handSlotImages = new Image[handSlots.Length];
        handSlotCounts = new TMP_Text[handSlots.Length];
        handSlotColors = new Image[handSlots.Length];
        bagSlots = bagInv.GetComponentsInChildren<SlotController>();
        bagSlotImages = new Image[bagSlots.Length];
        bagSlotCounts = new TMP_Text[bagSlots.Length];

        cursorImage = cursorSlot.GetComponentsInChildren<Image>()[1];
        cursorText = cursorSlot.GetComponentInChildren<TMP_Text>();


        for (int i = 0;  i < handSlots.Length; i++)
        {
            handSlotColors[i] = handSlots[i].gameObject.GetComponent<Image>();
            handSlotImages[i] = handSlots[i].gameObject.GetComponentsInChildren<Image>()[1];
            handSlotCounts[i] = handSlotImages[i].gameObject.GetComponentInChildren<TMP_Text>();
        }
        for (int i = 0; i < bagSlots.Length; i++)
        {
            bagSlotImages[i] = bagSlots[i].gameObject.GetComponentsInChildren<Image>()[1];
            bagSlotCounts[i] = bagSlotImages[i].gameObject.GetComponentInChildren<TMP_Text>();
        }

        cursorSlot.SetActive(false);
        InventoryUpdate();
        ToggleBagInventory();
        GiveFirstItems();

    }

    public void Update()
    {
        if (cursorSlot.activeSelf)
        {
            cursorSlot.transform.position = Input.mousePosition;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentHandSlotIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { currentHandSlotIndex = 1; }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { currentHandSlotIndex = 2; }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { currentHandSlotIndex = 3; }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { currentHandSlotIndex = 4; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleBagInventory();
        }
        SwitchCurrentSlot();

    }

    public void GiveFirstItems()
    {
        AddItem(items.pickaxe);
    }

    public void SwitchCurrentSlot()
    {
        for (int i = 0; i < handSlots.Length; i++)
        {
            handSlotColors[i].color = new Color(0.66f,0.66f,0.66f);
        }
        handSlotColors[currentHandSlotIndex].color = new Color(0.7f,0,0);
    }

    public void InventoryUpdate()
    {
        // 아이템을 사용했을 때, 먹었을 때, 옮겼을 때 등 업데이트
        // 가진 아이템의 정보를 이미지와 텍스트로 변환한다.

        for (int i = 0; i < hand.Length;i++)
        {
            if (hand[i] != null)
            {
                if (!handSlotImages[i].enabled)
                {
                    handSlotImages[i].enabled = true;
                }
                if (!handSlotCounts[i].enabled)
                {
                    handSlotCounts[i].enabled = true;
                }
                handSlotImages[i].sprite = hand[i].Sprite;
                handSlotCounts[i].text = handAmount[i].ToString();
            }
            else
            {
                handSlotImages[i].enabled = false;
                handSlotCounts[i].enabled = false;
            }
        }
        for (int i = 0; i < bag.Length; i++)
        {
            if (bag[i] != null)
            {
                if (!bagSlotImages[i].enabled)
                {
                    bagSlotImages[i].enabled = true;
                }
                if (!bagSlotCounts[i].enabled)
                {
                    bagSlotCounts[i].enabled = true;
                }
                bagSlotImages[i].sprite = bag[i].Sprite;
                bagSlotCounts[i].text = bagAmount[i].ToString();
            }
            else
            {
                bagSlotImages[i].enabled = false;
                bagSlotCounts[i].enabled = false;
            }
        }
        if (cursorSlot.activeSelf)
        {
            cursorImage.sprite = cursorItem.Sprite;
            cursorText.text = cursorItemAmount.ToString();
        }
    }

    public void ToggleBagInventory()
    {
        if (bagInv.activeSelf)
        {
            bagInv.SetActive(false);
        }
        else
        {
            bagInv.SetActive(true);
            InventoryUpdate();
        }
    }

    public bool AddItem(Item item)
    {
        int nullindex = -1;
        for (int i = 0; i < hand.Length; i++)
        {
            if (hand[i] == item && handAmount[i] < item.MaxAmount)
            {
                handAmount[i]++;
                InventoryUpdate();
                return true;
            }
            if (hand[i] == null && nullindex == -1)
            {
                nullindex = i;
            }
        }
        if (nullindex != -1)
        {
            hand[nullindex] = item;
            handAmount[nullindex]++;
            InventoryUpdate();
            return true;
        }
        for (int i = 0; i < bag.Length; i++)
        {
            if (bag[i] == item && bagAmount[i] < item.MaxAmount)
            {
                bagAmount[i]++;
                InventoryUpdate();
                return true;
            }
            if (bag[i] == null && nullindex == -1)
            {
                nullindex = i;
            }
        }
        if (nullindex != -1)
        {
            bag[nullindex] = item;
            bagAmount[nullindex]++;
            InventoryUpdate();
            return true;
        }
        return false;
    }
    public void SelectItem(int num)
    {
        if (!bagInv.activeSelf)
        {
            return;
        }
        beforeNum = num;
        cursorSlot.SetActive(true);
        if (num < 50)
        {
            int i = num - 1;
            cursorItem = hand[i];
            cursorItemAmount = handAmount[i];
            hand[i] = null;
            handAmount[i] = 0;

        }
        else
        {
            int i = num - 51;
            cursorItem = bag[i];
            cursorItemAmount = bagAmount[i];
            bag[i] = null;
            bagAmount[i] = 0;
        }
        InventoryUpdate();
    }
    public void ReplaceItem(int num)
    {
        if (num < 50)
        {
            int i = num - 1;
            if (beforeNum < 50)
            {
                int j = beforeNum - 1;
                hand[j] = hand[i];
                handAmount[j] = handAmount[i];
            }
            else
            {
                int j = beforeNum - 51;
                bag[j] = hand[i];
                bagAmount[j] = handAmount[i];
            }
            hand[i] = cursorItem;
            handAmount[i] = cursorItemAmount;
            cursorItem = null;
            cursorItemAmount = 0;

        }
        else
        {
            int i = num - 51;
            if (beforeNum < 50)
            {
                int j = beforeNum - 1;
                hand[j] = bag[i];
                handAmount[j] = bagAmount[i];
            }
            else
            {
                int j = beforeNum - 51;
                bag[j] = bag[i];
                bagAmount[j] = bagAmount[i];
            }
            bag[i] = cursorItem;
            bagAmount[i] = cursorItemAmount;
            cursorItem = null;
            cursorItemAmount = 0;
        }
        cursorSlot.SetActive(false);
        InventoryUpdate();
        
    }

    public void Consume()
    {
        handAmount[currentHandSlotIndex]--;
        if (handAmount[currentHandSlotIndex] <= 0)
        {
            handAmount[currentHandSlotIndex] = 0;
            hand[currentHandSlotIndex] = null;
        }
        InventoryUpdate();
    }


}
