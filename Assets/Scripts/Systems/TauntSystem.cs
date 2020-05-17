﻿using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;
using System.Linq;

public class TauntSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
    }

    void OnFilterAttackTargets(object sender, object args)
    {
        var candidates = args as List<Card>;
        if (!TargetsContainTaunt(candidates))
            return;

        for (int i = candidates.Count - 1; i >= 0; --i)
        {
            if (candidates[i].GetAspect<Taunt>() == null)
                candidates.RemoveAt(i);
        }
    }

    bool TargetsContainTaunt(List<Card> cards)
    {
        return cards.Any(card => card.GetAspect<Taunt>() != null);
    }
}
