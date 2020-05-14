using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TheLiquidFire.Pooling;
using TheLiquidFire.Notifications;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Extensions;
using TheLiquidFire.Animation;

public class TableView : MonoBehaviour {
	public List<MinionView> minions = new List<MinionView> ();
	SetPooler minionPooler;

	private void Awake()
	{
		var board = GetComponentInParent<BoardView>();
		minionPooler = board.minionPooler;
	}

	private void OnEnable()
	{
		this.AddObserver(OnPrepareSummon, Global.PrepareNotification<SummonMinionAction>());
	}

	private void OnDisable()
	{
		this.RemoveObserver(OnPrepareSummon, Global.PrepareNotification<SummonMinionAction>());
	}

	void OnPrepareSummon(object sender, object args)
	{
		var action = args as SummonMinionAction;
		if (GetComponentInParent<PlayerView>().player.index == action.minion.ownerIndex)
			action.perform.viewer = SummonMinion;
	}

	public GameObject GetMatch(Card card)
	{
		for (int i = minions.Count - 1; i >= 0; --i)
		{
			if (minions[i].minion == card)
				return minions[i].gameObject;
		}

		return null;
	}

	public IEnumerator SummonMinion(IContainer game, GameAction action)
	{
		var summon = action as SummonMinionAction;
		var playerView = GetComponentInParent<PlayerView>();
		var cardView = playerView.hand.GetView(summon.minion);
		playerView.hand.Dismiss(cardView);
		StartCoroutine(playerView.hand.LayoutCards(true));

		var minionView = minionPooler.Dequeue().GetComponent<MinionView>();
		minionView.transform.ResetParent(transform);
		minions.Add(minionView);
		minionView.gameObject.SetActive(true);

		minionView.Display(summon.minion);
		var pos = GetComponentInParent<PlayerView>().hand.activeHandle.position;
		minionView.transform.position = pos;

		var tweener = LayoutMinions();
		tweener.duration = 0.5f;
		tweener.equation = EasingEquations.EaseOutBounce;
		while (tweener != null)
			yield return null;
	}

	Tweener LayoutMinions()
	{
		var xPos = (minions.Count / -2f) + 0.5f;
		Tweener tweener = null;
		for (int i = 0; i < minions.Count; ++i)
		{
			tweener = minions[i].transform.MoveToLocal(new Vector3(xPos, 0, 0), 0.25f);
			xPos += 1;
		}

		return tweener;
	}
}