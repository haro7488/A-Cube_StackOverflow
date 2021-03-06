﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCtrl : ItemCtrl
{

    new SpriteRenderer renderer;

    public bool isBombing = false;
    public bool isBurning = false;
    public float burningLifeTime = 3f;
    public float burnDelay = 0.1f;
    public float bombForce = 5f;

    float delay;
    Color originColor;

    new void Awake()
    {
        base.Awake();
        itemType = eItemType.Bomb;
        renderer = GetComponent<SpriteRenderer>();
        originColor = renderer.color;
        Init();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isBurning)
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
        isBombing = false;
        delay = 0f;
    }

    public void Burn(ItemCtrl sender)
    {
        if (isBurning)
        {
            return;
        }

        if ((sender.itemType == eItemType.Wood))
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
        Bomb();
        DestroyItem();
    }

    void Bomb()
    {
        if (isBombing)
            return;

        isBombing = true;

        Collider2D[] arrHit = Physics2D.OverlapCircleAll(transform.position, bombRadius);
        for (int i = 0; i < arrHit.Length; i++)
        {
            ItemCtrl item = arrHit[i].transform.GetComponent<ItemCtrl>();
            //rigidbody2D.AddForceAtPosition(force, n.point, ForceMode2D.Impulse);
            if (item != null)
            {
                Vector2 force = ((Vector2)arrHit[i].transform.position - (Vector2)transform.position) * (bombForce + Random.Range(-1f, 1f));
                item.rigidbody2D.AddForce(force, ForceMode2D.Impulse);

                if (item.itemType == eItemType.Bomb)
                {
                    BombCtrl bomb = item as BombCtrl;
                    if (bomb != null)
                        bomb.Bomb();
                }
                
                if(item.itemType == eItemType.Stone)
                {
                    StoneCtrl stone = item as StoneCtrl;
                    if (stone != null)
                        stone.Break(this);
                }
            }
        }
    }
}
