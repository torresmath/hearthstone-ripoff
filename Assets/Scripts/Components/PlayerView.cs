using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.Animation;
using TheLiquidFire.Notifications;
using UnityEngine;

public class PlayerView : MonoBehaviour {
	public DeckView deck;
	public HandView hand;
	public TableView table;
	public HeroView hero;
	public GameObject cardPrefab;

	public Player player { get; private set; }

	public void SetPlayer(Player player)
	{
		this.player = player;
		var heroCard = player.hero[0] as Hero;
		hero.SetHero(heroCard);
	}

	private void OnEnable()
	{
		this.AddObserver(OnPerformDamageAction, "HeroView.OnPerformDamageAction");
	}

	private void OnDisable()
	{
		this.RemoveObserver(OnPerformDamageAction, "HeroView.OnPerformDamageAction");
	}

	void OnPerformDamageAction(object sender, object args)
	{
		var action = args as DamageAction;

		if (action.targets.Contains(hero.hero))
		{
			StartCoroutine(DamageHero(action));
		}
	}

	public GameObject GetMatch(Card card)
	{
		switch (card.zone)
		{
			case Zones.Battlefield:
				return table.GetMatch(card);
			case Zones.Hero:
				return hero.gameObject;
			default:
				return null;
		}
	}

	IEnumerator DamageHero(GameAction action)
	{
		var scaleToDamage = 1.5f;
		var scaleTo = new Vector3(scaleToDamage, scaleToDamage, 1f);
		var healthRt = hero.health.rectTransform as RectTransform;

		int fontSize = hero.health.fontSize;
		int maxFontSize = hero.health.fontSize + 20;

		var damageColor = (byte) 255;

		Tweener tweener = healthRt.ScaleTo(scaleTo, 0.25f, EasingEquations.EaseOutBack);
		while (tweener.IsPlaying) {
			damageColor -= 5;
			hero.health.color = new Color32(255, damageColor, damageColor, 255);
			yield return null; 
		}

		tweener = healthRt.ScaleTo(Vector3.one, 0.25f, EasingEquations.EaseInBack);
		while (tweener.IsPlaying) {
			damageColor += 5;
			hero.health.color = new Color32(255, damageColor, damageColor, 255);
			yield return null; 
		}

		hero.health.color = new Color32(255, 255, 255, 255);
	}
}