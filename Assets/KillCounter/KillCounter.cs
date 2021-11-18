using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
	[SerializeField] int killCount = 0;
	[SerializeField] int totalKillCount;

	ObjectPool objectPool;


	private void Awake()
	{
		objectPool = FindObjectOfType<ObjectPool>();

		totalKillCount = killCount;
	}

	public void IncreaseCount()
	{
		killCount++;
		if (killCount == 10)
		{
			objectPool.AddToPool();
			killCount = 0;
		}
	}
}
