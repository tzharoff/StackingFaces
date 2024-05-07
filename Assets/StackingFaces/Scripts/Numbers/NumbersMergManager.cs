using System;
using System.Collections;
using UnityEngine;

public class NumbersMergManager : MonoBehaviour
{

    public static Action<int, Vector2, float, int> onMergeProcessed;
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
        float bigNumber = sender.NumValue > receiver.NumValue ? sender.NumValue : receiver.NumValue;
        int newMergeCount = sender.MergeCount > receiver.MergeCount ? sender.MergeCount : receiver.MergeCount;
        Debug.Log($"sender.MergeCount {sender.MergeCount}, sender.MergeCount {sender.MergeCount}");
        newMergeCount++;
        Debug.Log($"newMergeCount {newMergeCount}");
        float scaleSize = (bigNumber / newValue) + (float)newMergeCount;

        Destroy(sender.gameObject);
        Destroy(receiver.gameObject);

        StartCoroutine(ResetLastSender());

        onMergeProcessed?.Invoke(newValue, numberSpawnPos, scaleSize, newMergeCount);

    }

    IEnumerator ResetLastSender()
    {
        yield return new WaitForEndOfFrame();
        lastSender = null;
    }
}
