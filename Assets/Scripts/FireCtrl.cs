﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : ItemCtrl {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Item"))
        {
            ItemCtrl item = collision.gameObject.GetComponent<ItemCtrl>();
            if (item.itemType == eItemType.Wood)
            {
                WoodCtrl wood = item as WoodCtrl;
                if (wood != null)
                    wood.Burn(this);
            }
        }
    }
}
