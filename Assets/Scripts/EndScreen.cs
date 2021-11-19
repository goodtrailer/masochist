// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using System;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    private EntityManager entityManager => World.DefaultGameObjectInjectionWorld.EntityManager;
    private Animator animator;
    private TMP_Text tmp;

    private bool prevInProgress = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        tmp = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        using (EntityQuery query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<RunData>()))
        {
            RunData r;
            try
            {
                r = query.GetSingleton<RunData>();
            }
            catch (InvalidOperationException)
            {
                Debug.Log("No RunData singleton!");
                return;
            }

            if (r.InProgress != prevInProgress)
            {
                int time = (int)(Time.time - r.StartTime);
                tmp.text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
                animator.SetBool("Show", !r.InProgress);
            }

            prevInProgress = r.InProgress;
        }
    }
}
