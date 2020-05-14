﻿using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public static class GameFactory
{
    public static Container Create()
    {
        Container game = new Container();

        // Add Systems
        game.AddAspect<ActionSystem>();
        game.AddAspect<DataSystem>();
        game.AddAspect<MatchSystem>();
        game.AddAspect<PlayerSystem>();
        game.AddAspect<DestructableSystem>();
        game.AddAspect<VictorySystem>();
        game.AddAspect<CardSystem>();
        game.AddAspect<MinionSystem>();
        game.AddAspect<ManaSystem>();
        game.AddAspect<AttackSystem>();
        game.AddAspect<CombatSystem>();

        // Add Other
        game.AddAspect<StateMachine>();
        game.AddAspect<GlobalGameState>();

        return game;
    }
}
