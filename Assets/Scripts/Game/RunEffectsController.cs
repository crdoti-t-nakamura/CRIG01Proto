/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「走る際のエフェクトを制御する」スクリプト
 * 
 * 作成情報： DATE:2019/06/17 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEffectsController : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        particle.Stop();
    }

    public void PlayParticle()
    {
        if (!particle.isEmitting)
        {
            particle.Play();
        }
    }

    public void StopParticle()
    {
        particle.Stop();
    }
}
