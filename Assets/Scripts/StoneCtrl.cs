﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCtrl : ItemCtrl {

    public float radius = 0.8f;
    public bool isSparking = false;
    public bool isBreak = false;
    Color originColor;
    public Color breakColor;

    new private void Awake()
    {
        base.Awake();
        itemType = eItemType.Stone;
        originColor = renderer.color;
    }
	
    protected override void Init()
    {
        base.Init();
        isSparking = false;
        isBreak = false;
        renderer.color = originColor;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Item"))
        {
            ItemCtrl item = collision.gameObject.GetComponent<ItemCtrl>();
            if (item.itemType == eItemType.Stone)
            {
                StoneCtrl stone = item as StoneCtrl;
                if (stone != null)
                    stone.Spark(this);
            }
        }
    }

    public void Spark(ItemCtrl sender)
    {
        if (isSparking)
        {
            return;
        }
        Debug.Log("Sparking");

        if (sender.itemType == eItemType.Stone)
        {
            StoneCtrl stone = sender as StoneCtrl;
            isSparking = true;
            stone.isSparking = true;
        }

        Vector2 dist = sender.transform.position - transform.position;
        Vector2 pos = transform.position;
        dist = pos + (dist * 0.5f);
        Collider2D[] arrColl = Physics2D.OverlapCircleAll(dist, radius);
        foreach (var n in arrColl)
        {
            ItemCtrl item = n.GetComponent<ItemCtrl>();
            if(item != null)
            {
                if (item.itemType == eItemType.Wood)
                {
                    WoodCtrl wood = item as WoodCtrl;
                    if (wood != null)
                    {
                        wood.Burn(this);
                    }
                }
            }
        }   
    }

    public void Break(ItemCtrl sender)
    {
        if(isBreak)
        {
            DestroyItem();
        }
        renderer.color = breakColor;
        isBreak = true;
    }
}
