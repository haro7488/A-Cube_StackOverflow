﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCtrl : ItemCtrl {

    public bool isBurning = false;
    public float burningLifeTime = 3f;
    public float burnDelay = 0.1f;

    float delay;
    Color originColor;

	new void Awake () {
        base.Awake();
        itemType = eItemType.Wood;
        originColor = renderer.color;
        Init();
	}
	
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(isBurning)
        {
            if (collision.collider.CompareTag("Item"))
            {
                ItemCtrl item = collision.gameObject.GetComponent<ItemCtrl>();
                if(item.itemType == eItemType.Wood)
                {
                    WoodCtrl wood = item as WoodCtrl;
                    if (wood != null)
                        wood.Burn(this);
                }

                if (item.itemType == eItemType.Bomb)
                {
                    BombCtrl bomb = item as BombCtrl;
                    if (bomb != null)
                        bomb.Burn(this);
                }
            }
        }
    }

    protected override void Init()
    {
        renderer.color = originColor;
        isBurning = false;
        delay = 0f;
    }

    public void Burn(ItemCtrl sender)
    {
        if (isBurning)
        {
            return;
        }

        if((sender.itemType == eItemType.Wood))
        {
            if (delay < burnDelay)
            {
                delay += Time.deltaTime;
                return;
            }
        }

        isBurning = true;
        renderer.color = Color.black;

        StartCoroutine(CoBurn());
    }

    float bombRadius = 2f;
    IEnumerator CoBurn()
    {
        //rigidbody2D.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(burningLifeTime);
        Collider2D[] arrHit = Physics2D.OverlapCircleAll(transform.position, bombRadius);
		DestroyItem();
    }
}
