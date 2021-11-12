using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Cursor : MonoBehaviour
{

    void Awake()
    {
        transform.DOShakeScale(1f, 0.5f, 0, 0f).SetLoops(-1, LoopType.Restart);
    }
}
