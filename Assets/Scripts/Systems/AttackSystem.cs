﻿using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class AttackSystem : Aspect, IObserve
{
    public const string FilterAttackersNotification = "AttackSystem.ValidateAttackerNotification";
    public const string FilterTargetsNotification = "AttackSystem.ValidateTargetNotification";

    public List<Card> validAttackers { get; private set; }
    public List<Card> validTargets { get; private set; }

    public void Awake()
    {
        this.AddObserver(OnValidateAttackAction, Global.ValidateNotification<AttackAction>());
        this.AddObserver(OnPerformAttackAction, Global.PerformNotification<AttackAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnValidateAttackAction, Global.ValidateNotification<AttackAction>());
        this.RemoveObserver(OnPerformAttackAction, Global.PerformNotification<AttackAction>(), container);
    }

    void OnValidateAttackAction(object sender, object args)
    {
        var action = sender as AttackAction;
        if (!validAttackers.Contains(action.attacker) ||
            !validTargets.Contains(action.target))
        {
            var validator = args as Validator;
            validator.Invalidate();
        }
    }

    void OnPerformAttackAction(object sender, object args)
    {
        var action = args as AttackAction;
        var attacker = action.attacker as ICombatant;
        attacker.remainingAttacks--;

        var target = action.target as IDestructable;
        var damageAction = new DamageAction(target, attacker.attack);
        container.AddReaction(damageAction);
    }

    public List<Card> GetActive(Player player)
    {
        List<Card> list = new List<Card>();
        list.AddRange(player[Zones.Hero]);
        
        list.AddRange(player[Zones.Battlefield]);
        return list;
    }

    List<Card> GetFiltered(Player player, string filterNotificationName)
    {
        List<Card> list = GetActive(player);
        container.PostNotification(filterNotificationName, list);
        return list;
    }

    public void Refresh()
    {
        var match = container.GetMatch();
        validAttackers = GetFiltered(match.CurrentPlayer, FilterAttackersNotification);
        Debug.Log("Valid attackers size " + validAttackers.Count);

        validTargets = GetFiltered(match.OpponentPlayer, FilterTargetsNotification);        
    }

}
