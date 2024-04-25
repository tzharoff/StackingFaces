using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{

    /*
        Merges
        1 -> 2
        3 -> 4 -> 5
        7 -> 9 -> 8
        11 -> 10 -> -> 13
        14 -> 12
        6 -> 15
    */

    public static Action<FaceType, Vector2> onMergeProcessed;
    private Face lastSender;

    private void Awake() {
        Face.onFaceCollisionCallback += OnFaceCollision;
    }

    private void OnDestroy()
    {
        Face.onFaceCollisionCallback -= OnFaceCollision;
    }

    private void OnFaceCollision(Face sender, Face receiver){
        if(lastSender != null)
            return;

        lastSender = sender;

        ProcessMerge(sender, receiver);
    }

    private void ProcessMerge(Face sender, Face receiver){
        Vector2 faceSpawnPos = (sender.transform.position + receiver.transform.position) / 2;
        Destroy(sender.gameObject);
        Destroy(receiver.gameObject);
        
        StartCoroutine(ResetLastSender());

        onMergeProcessed?.Invoke(getNextFaceType(sender.MyFaceType), faceSpawnPos);

    }

    IEnumerator ResetLastSender(){
        yield return new WaitForEndOfFrame();
        lastSender = null;
    }

    private FaceType getNextFaceType(FaceType currentFaceType){
        switch(currentFaceType){
            case FaceType.Smile:
                return FaceType.OpenSmile;
            case FaceType.OpenSmile:
                return FaceType.Despawn;
            case FaceType.Flat:
                return FaceType.Frown;
            case FaceType.Frown:
                return FaceType.SadAwe;
            case FaceType.SadAwe:
                return FaceType.Despawn;
            case FaceType.Disappoinment:
                return FaceType.Despawn;
            case FaceType.Disbelief:
                return FaceType.DisappoinmentFrown;
            case FaceType.ConfusedDisgust:
                return FaceType.Shocked;
            case FaceType.ConfusedAwe:
                return FaceType.Despawn;
            case FaceType.Shocked:
                return FaceType.ConfusedAwe;
            case FaceType.Hmmm:
                return FaceType.SmileContent;
            case FaceType.Wink:
                return FaceType.Hmmm;
            case FaceType.XEyes:
                return FaceType.Despawn;
            case FaceType.SmileContent:
                return FaceType.Despawn;
            case FaceType.DisappoinmentFrown:
                return FaceType.XEyes;
        }
        return FaceType.Despawn;
    }
}
