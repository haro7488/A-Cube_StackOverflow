﻿using System.Collections;
using UnityEngine;

public class ParticleAutoDeactiveCtrl : MonoBehaviour {

    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Use this for initialization
    void OnEnable () {
        StartCoroutine(CoDeactive());
	}

    IEnumerator CoDeactive()
    {
        yield return new WaitForSeconds(2f);
        //yield return new WaitUntil(() => !particle.isPlaying);
        Debug.Log("IsPlaying Done");
        transform.position = Vector3.one * 500f;
        ObjectPool.Release(this.gameObject);
    }
}
