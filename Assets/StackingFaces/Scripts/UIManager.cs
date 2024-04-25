using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region singleton
    public static UIManager Instance { get; private set;}

    private void Awake() {
        if(Instance != null)
            Destroy(Instance.gameObject);
        
        Instance = this;

        FaceManager.SetNextFaceSprite += SetNextFaceSprite;
    }
    #endregion

    private void OnDestroy() {
        FaceManager.SetNextFaceSprite -= SetNextFaceSprite;
    }

    [SerializeField] private Image nextFaceSprite;

    public void SetNextFaceSprite(Sprite newSprite){
        if(newSprite == null)
            return;

        nextFaceSprite.sprite = newSprite;
    }

}
