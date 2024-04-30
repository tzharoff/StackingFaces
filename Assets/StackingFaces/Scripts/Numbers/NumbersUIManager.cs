using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NumbersUIManager : MonoBehaviour
{
    #region singleton
    public static NumbersUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;

        NumbersManager.SetNextNumberSprite += SetNextNumberSprite;
    }
    #endregion

    private void OnDestroy()
    {
        NumbersManager.SetNextNumberSprite -= SetNextNumberSprite;
    }

    [SerializeField] private Numbers nextNumber;

    public void SetNextNumberSprite(int numValue)
    {
        nextNumber.NumValue = numValue;
    }

}
