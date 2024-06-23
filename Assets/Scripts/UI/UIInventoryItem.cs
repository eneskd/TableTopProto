using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    public Item Item { get; protected set; }

    [Header("References")]
    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemCountText;

    [Header("Bump Variables")]
    [SerializeField] private AnimationCurve _bumpAnimationCurve;
	[SerializeField] private float _bumpStrength = 1.5f;
	[SerializeField] private float _bumpDuration = 0.3f;

    private Coroutine _bumpCoroutine;

	public void InitializeItem(Item item)
    {
        Item = item;
        SetImage(item.ItemImage);
        SetImageScale(item.UIImageScaler * Vector3.one);
    }

	public void UpdateCount(int count)
	{
		_itemCountText.text = count.ToString();
        _bumpCoroutine = StartCoroutine(BumpCoroutine());
	}


	public void SetImage(Sprite itemImage)
    {
        _itemImage.sprite = itemImage;
    }

    public void SetImageScale(Vector3 scale)
    {
        _itemImage.transform.localScale = scale;
    }

    private IEnumerator BumpCoroutine()
    {
        float t = 0f;
        Vector3 initialScale = transform.localScale;
		Vector3 targetScale = initialScale * _bumpStrength;

        while (t < _bumpDuration)
        {
            t += Time.deltaTime;
            float ratio = t / _bumpDuration;

            transform.localScale = Vector3.Lerp(initialScale, targetScale, _bumpAnimationCurve.Evaluate(ratio));
            yield return null;
        }

        transform.localScale = initialScale;
    }

}
