﻿using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public class VictorySystem : Aspect
{
    public bool IsGameOver()
    {
        var match = container.GetMatch();
        foreach (Player p in match.players)
        {
            Hero h = p.hero[0] as Hero;
            if (h.hitPoints <= 0) { return true; }
        }

        return false;
    }
}
