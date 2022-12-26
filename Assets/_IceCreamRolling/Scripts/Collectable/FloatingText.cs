using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    public void setText(int number)
    {
        _text.text = "+" + number;
    }

    private void Start()
    {
        transform.DOScale(transform.localScale.x * .3f, 1).SetDelay(.3f);
        transform.DOLocalMoveY(transform.position.y + 2, 1).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
