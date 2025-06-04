using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [SerializeField] Button slotButton;
    [SerializeField] Button itemButton;


    public void ItemButtonClick()
    {
        if (InventoryManager.Instance.cursorSlot.activeSelf)
        {
            GiveSlotNumToReplaceFunction();
        }
        else
        {
            GiveSlotNumToSelectFunction();
        }
    }
    public void SlotButtonClick()
    {
        if (InventoryManager.Instance.cursorSlot.activeSelf)
        {
            GiveSlotNumToReplaceFunction();
        }
        else
        {
            return;
        }
    }
    public void GiveSlotNumToSelectFunction()
    {
        int.TryParse(gameObject.name, out int slotNum);
        InventoryManager.Instance.SelectItem(slotNum);
    }
    public void GiveSlotNumToReplaceFunction()
    {
        int.TryParse(gameObject.name, out int slotNum);
        InventoryManager.Instance.ReplaceItem(slotNum);
    }
}
