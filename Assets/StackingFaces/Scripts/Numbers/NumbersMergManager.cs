using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersMergManager : MonoBehaviour
{

    public static Action<int, Vector2> onMergeProcessed;
    private Numbers lastSender;

    private void Awake()
    {
        Numbers.onNumbersCollisionCallback += OnNumbersCollision;
    }

    private void OnDestroy()
    {
        Numbers.onNumbersCollisionCallback -= OnNumbersCollision;
    }

    private void OnNumbersCollision(Numbers sender, Numbers receiver)
    {
        if (lastSender != null)
            return;

        lastSender = sender;

        ProcessMerge(sender, receiver);
    }

    private void ProcessMerge(Numbers sender, Numbers receiver)
    {
        Vector2 numberSpawnPos = (sender.transform.position + receiver.transform.position) / 2;
        int newValue = sender.NumValue + receiver.NumValue;
        Destroy(sender.gameObject);
        Destroy(receiver.gameObject);

        StartCoroutine(ResetLastSender());

        onMergeProcessed?.Invoke(newValue, numberSpawnPos);

    }

    IEnumerator ResetLastSender()
    {
        yield return new WaitForEndOfFrame();
        lastSender = null;
    }
}
