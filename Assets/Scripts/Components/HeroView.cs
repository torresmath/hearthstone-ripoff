using System.Collections;
using TheLiquidFire.Animation;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : BattlefieldCardView {

	public Text armor;	

	public Hero hero { get; private set; }
	public override Card card { get { return hero; } }

	public void SetHero(Hero hero)
	{
		this.hero = hero;
		Refresh();
	}

	//private void OnEnable()
	//{
	//	this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
	//}

	//private void OnDisable()
	//{
	//	this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
	//}

	public override void OnPerformDamageAction(object sender, object args)
	{
		var action = args as DamageAction;
		if (action.targets.Contains(card as IDestructable))
		{
			Refresh();
		}

		this.PostNotification("HeroView.OnPerformDamageAction", action);
	}

	protected override void Refresh()
	{
		Debug.Log("Hero refresh is active? " + isActive);
		if (hero == null)
			return;
		avatar.sprite = isActive ? active : inactive;
		attack.text = hero.attack.ToString();
		health.text = hero.hitPoints.ToString();
		armor.text = hero.armor.ToString();
	}

}