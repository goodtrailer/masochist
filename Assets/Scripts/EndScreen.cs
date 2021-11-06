// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    private TMP_Text text;

    public void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void Show(string text)
    {
        this.text.text = text;
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Show", true);
    }
}
