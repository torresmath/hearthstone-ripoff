using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.Animation;
using TheLiquidFire.Notifications;
using UnityEngine;
using UnityEngine.UI;

public class ManaView : MonoBehaviour {
	public List<Image> slots;
	public Sprite available;
	public Sprite unavailable;
	public Sprite locked;
	public Sprite slot;

	private void OnEnable()
	{
		this.AddObserver(OnManaValueChangedNotification, ManaSystem.ValueChangedNotification);
	}

	private void OnDisable()
	{
		this.RemoveObserver(OnManaValueChangedNotification, ManaSystem.ValueChangedNotification);
	}

	void OnManaValueChangedNotification(object sender, object args)
	{
		var currentPlayer = args as Player;
		if (currentPlayer.index != 0)
			return;

		var mana = currentPlayer.mana;
		for (int i = 0; i < mana.Available; ++i)
			SetSpriteForImageSlot(available, i);

		for (int i = mana.Available; i < mana.Unlocked; ++i)
		{
			SetSpriteForImageSlot(unavailable, i);
		}

		for (int i = mana.Unlocked - mana.overloaded; i < mana.Unlocked; ++i)
			SetSpriteForImageSlot(locked, i);

		for (int i = mana.Unlocked; i < Mana.MaxSlots; ++i)
			SetSpriteForImageSlot(slot, i);
	}

	void SetSpriteForImageSlot(Sprite sprite, int slotIndex)
	{
		if (slotIndex >= 0 && slotIndex < slots.Count)
		{			
			StartCoroutine(HideMana(slots[slotIndex].gameObject.GetComponent<RectTransform>(), sprite, slotIndex));
		}
		
	}

	IEnumerator HideMana(RectTransform rectTransform, Sprite sprite, int slotIndex)
	{
		Tweener tweener = null;
		var currentSprite = slots[slotIndex].sprite;
		if (currentSprite == available && sprite != available)
		{
			tweener = rectTransform.ScaleTo(Vector3.zero, 0.5f, EasingEquations.EaseInOutBack);
		}
		while (tweener != null) yield return null;
		yield return ShowMana(rectTransform, sprite, currentSprite, slotIndex);
	}

	IEnumerator ShowMana(RectTransform rectTransform, Sprite sprite, Sprite currentSprite, int slotIndex)
	{
		var incrementedSpeed= (float)slotIndex / 10f;
		var scaleUp = new Vector3(1.25f, 1.25f);
		Tweener tweener = null;
		if (currentSprite != available && sprite == available)
		{
			tweener = rectTransform.ScaleTo(scaleUp, 0.25f + incrementedSpeed, EasingEquations.EaseInOutBack);
			while (tweener != null) yield return null;
		}

		slots[slotIndex].sprite = sprite;

		tweener = rectTransform.ScaleTo(Vector3.one, 0.15f + incrementedSpeed, EasingEquations.EaseInOutBack);
		while (tweener != null) yield return null;
	}
}