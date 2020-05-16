using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.Notifications;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattlefieldCardView : MonoBehaviour
{
    public Image avatar;
    public Text attack;
    public Text health;
    public Sprite inactive;
    public Sprite active;
    protected bool isActive;
    protected bool isTargetable;

    public abstract Card card { get; }

    private void OnEnable()
    {
        this.AddObserver(OnPlayerIdleEnter, PlayerIdleState.EnterNotification);
        this.AddObserver(OnPlayerIdleExit, PlayerIdleState.ExitNotification);
        this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnPlayerIdleEnter, PlayerIdleState.EnterNotification);
        this.RemoveObserver(OnPlayerIdleExit, PlayerIdleState.ExitNotification);
        this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
    }

    void OnPlayerIdleEnter(object sender, object args)
    {
        var container = (sender as PlayerIdleState).container;
        isActive = container.GetAspect<AttackSystem>().validAttackers.Contains(card) && card.ownerIndex == 0;
        isTargetable = container.GetAspect<AttackSystem>().validTargets.Contains(card) && card.ownerIndex == 1;
        Refresh();
    }

    void OnPlayerIdleExit(object sender, object args)
    {
        isActive = isTargetable = false;
    }

    protected abstract void Refresh();

    public virtual void OnPerformDamageAction(object sender, object args)
    {
        var action = args as DamageAction;
        if (action.targets.Contains(card as IDestructable))
        {
            Refresh();
        }
    }
}
