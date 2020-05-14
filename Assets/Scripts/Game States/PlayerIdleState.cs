using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class PlayerIdleState : BaseState
{
	public const string EnterNotification = "PlayerIdleState.EnterNotification";
	public const string ExitNotification = "PlayerIdleState.ExitNotification";

	public override void Enter()
	{
		container.GetAspect<AttackSystem>().Refresh();
		Temp_AutoChangeTurnForAI();
		this.PostNotification(EnterNotification);
	}

	public override void Exit()
	{
		this.PostNotification(ExitNotification);
	}

	void Temp_AutoChangeTurnForAI()
	{
		if (container.GetMatch().CurrentPlayer.mode != ControlModes.Local)
		{
			container.GetAspect<MatchSystem>().ChangeTurn();
		}
	}
}