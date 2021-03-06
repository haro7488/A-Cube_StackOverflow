﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	//singleton
	static SpawnManager[] instance = null;
	public static SpawnManager[] Instance { get { return instance; } }
	[SerializeField]
	int singletonIndex;

	//spawn position
	Transform spawnPosition;

	//target items
	public GameObject[] itemObjects;
	ObjectPool[] itemPools;

	//selected items per turn
	ObjectPool[] selectedItems = new ObjectPool[5];

	//selected item index
	int selectedIndex = 0;
	int selectedIndex_ { get { return selectedIndex; } set { selectedIndex = value; } }

	public float spawnDelayPerSec = 3f;
	public WaitForSeconds waitForSpawn;

	void Awake() {
		InitializeSingleton();
		spawnPosition = transform;
		itemPools = new ObjectPool[itemObjects.Length];
		for (int i = 0; i < itemObjects.Length; ++i) {
			itemPools[i] = ObjectPool.CreateFor(itemObjects[i]);
		}
		waitForSpawn = new WaitForSeconds(spawnDelayPerSec);
	}

	void InitializeSingleton() {
		if (instance == null) {
			instance = new SpawnManager[2];
		}
		instance[singletonIndex] = this;
	}

	void SelecteItemsPerTurn() {
		for(int i = 0; i < selectedItems.Length; ++i) {
			selectedItems[i] = itemPools[Random.Range(0, itemPools.Length)];
		}
	}

	public void Run() {
		StartCoroutine(CoSpawn());
	}

	public void Stop() {
		StopAllCoroutines();
	}

	IEnumerator CoSpawn() {
		while (true) {
			SelecteItemsPerTurn();
			yield return waitForSpawn;
			GameObject dropItem = selectedItems[selectedIndex_].Retain(spawnPosition.position);
			GiveControlFocus(dropItem);
		}
	}

	void GiveControlFocus(GameObject dropItem) {
		InputManager inputManager = InputManager.Instance;
		inputManager.arrTargetObject[singletonIndex] = dropItem.gameObject;
		inputManager.arrTargetRigidbody[singletonIndex] = dropItem.GetComponent<Rigidbody2D>();
	}

	public void SetSelectedIndex(int selectedIndex) {
		selectedIndex_ = selectedIndex;
	}
}