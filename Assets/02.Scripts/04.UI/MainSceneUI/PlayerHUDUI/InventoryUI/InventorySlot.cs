using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private InventoryData CurrentData = null;
    [SerializeField] private Image CurrentItemImage;
    [SerializeField] private TextMeshProUGUI CurrentItemAmount;
    [SerializeField] private InventoryController InventoryController;

    public void ChangeData(InventoryData itemData)
    {
        if (itemData == null) { ResetSlot(); return; }
        if (itemData.ItemData == null) { ResetSlot(); return; }

        CurrentData = itemData;
        ChangeUI();
    }

    public void ResetSlot()
    {
        CurrentItemImage.sprite = null;
        CurrentItemAmount.text = "";
    }

    public void OnClick()
    {
        if (CurrentData == null || CurrentData.ItemData == null)
        {
            SoundManger.Instance.MakeEnviornmentSound("InventoryError");
            InventoryController.ChangeData(null);
            return;
        }
        SoundManger.Instance.MakeEnviornmentSound("Click4");
        InventoryController.ChangeData(CurrentData);
    }

    public void ChangeUI()
    {
        CurrentItemImage.sprite = CurrentData.ItemData.itemSO.ItemImage;
        CurrentItemAmount.text = (CurrentData.Amount).ToString();
    }

    public void ClearData() // �߰���: ClearData �޼��� ����
    {
        CurrentData = null; // ���� �����͸� �ʱ�ȭ
        CurrentItemImage.sprite = null; // ������ �̹����� ���ϴ�.
        CurrentItemAmount.text = ""; // ���� ǥ�ø� �ʱ�ȭ
    }
}
