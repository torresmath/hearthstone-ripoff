﻿using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class GlobalGameState : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnBeginSequence, ActionSystem.beginSequenceNotification);
        this.AddObserver(OnCompleteAllActions, ActionSystem.completeNotification);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnBeginSequence, ActionSystem.beginSequenceNotification);
        this.RemoveObserver(OnCompleteAllActions, ActionSystem.completeNotification);
    }

    void OnBeginSequence(object sender, object args)
    {
        container.ChangeState<SequenceState>();
    }

    void OnCompleteAllActions(object sender, object args)
    {
        container.ChangeState<PlayerIdleState>();
    }
}
