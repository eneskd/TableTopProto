using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    public Item Item { get; protected set; }

    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemCountText;


    public void InitializeItem(Item item)
    {
        Item = item;
        SetImage(item.ItemImage);
        SetImageScale(item.UIImageScaler * Vector3.one);
    }

	public void UpdateCount(int count)
	{
		_itemCountText.text = count.ToString();
	}


	public void SetImage(Sprite itemImage)
    {
        _itemImage.sprite = itemImage;
    }

    public void SetImageScale(Vector3 scale)
    {
        _itemImage.transform.localScale = scale;
    }

}
