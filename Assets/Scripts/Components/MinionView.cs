using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;
using UnityEngine.UI;

public class MinionView : BattlefieldCardView {
	
	public Sprite inactiveTaunt;
	public Sprite activeTaunt;

	public Minion minion { get; private set; }
	public override Card card { get { return minion; } }

	//void OnAttackSystemUpdate(object sender,object args)
	//{
	//	var container = sender as Container;
	//	isActive = container.GetAspect<AttackSystem>().validAttackers.Contains(minion);
	//	Refresh();
	//}

	public void Display(Minion minion)
	{
		this.minion = minion;
		Refresh();
	}

	protected override void Refresh()
	{
		if (minion == null)
			return;
		avatar.sprite = isActive ? active : inactive;
		attack.text = minion.attack.ToString();
		health.text = minion.hitPoints.ToString();
	}
}