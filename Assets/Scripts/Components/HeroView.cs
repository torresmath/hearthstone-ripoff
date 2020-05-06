using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : MonoBehaviour {
	public Image avatar;
	public Text attack;
	public Text health;
	public Text armor;
	public Sprite active;
	public Sprite inactive;

	public Hero hero { get; private set; }

	public void SetHero(Hero hero)
	{
		this.hero = hero;
		Refresh();
	}

	private void OnEnable()
	{
		this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
	}

	private void OnDisable()
	{
		this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
	}

	void Refresh()
	{
		if (hero == null)
			return;
		avatar.sprite = inactive; // TODO: Add actiavion logic
		attack.text = hero.attack.ToString();
		health.text = hero.hitPoints.ToString();
		armor.text = hero.armor.ToString();
	}

	void OnPerformDamageAction(object sender, object args)
	{
		var action = args as DamageAction;
		if (action.targets.Contains(hero))
		{
			Refresh();
			action.perform.viewer = HeroDamageViewer;
		}
	}

	IEnumerator HeroDamageViewer(IContainer game, GameAction action)
	{
		Debug.Log("Hero Damage Viewer");
		yield return true;
		for (int i = health.fontSize; i < (health.fontSize + 50); i++)
		{
			health.fontSize++;
			yield return null;
		}
	}
}