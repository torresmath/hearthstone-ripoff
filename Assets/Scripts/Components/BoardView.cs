﻿using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.Pooling;
using UnityEngine;

public class BoardView : MonoBehaviour {
	public GameObject damageMarkPrefab;
	public List<PlayerView> playerViews;
	public SetPooler cardPooler;

	private void Start()
	{
		var match = GetComponentInParent<GameViewSystem>().container.GetMatch();
		for (int i = 0; i < match.players.Count; ++i)
		{
			playerViews[i].SetPlayer(match.players[i]);
		}
	}
}