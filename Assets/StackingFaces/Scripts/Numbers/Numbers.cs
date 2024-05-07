using System;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class Numbers : MonoBehaviour
{
    public static Action<Numbers, Numbers> onNumbersCollisionCallback;

    [Header("References")]
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D _rigidbody2D;


    [Header("Settings")]
    [SerializeField] private Color _evenColor = Color.blue;
    [SerializeField] private Color _oddColor = Color.green;
    [SerializeField] private int mergeCount = 0;

    private int _numValue;
    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (tmpText == null) GetTextMeshPro();
        if (spriteRenderer == null) GetSpriteRenderer();

        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetBodyToKinematic();
        NumValue = _numValue;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out Numbers numbers))
        {
            if (IsEven != numbers.IsEven)
                return;

            onNumbersCollisionCallback?.Invoke(this, numbers);
        }
    }

    private void SetColor()
    {
        if (IsEven)
        {
            SetEvenColor();
            return;
        }

        SetOddColor();
    }

    private void SetEvenColor()
    {
        MyColor = _evenColor;
    }

    private void SetOddColor()
    {
        MyColor = _oddColor;
    }


    public int MergeCount
    {
        get { return mergeCount; }
        set { mergeCount = value; }
    }

    public void SetScale(float scale)
    {
        transform.localScale *= scale;
    }

    public bool IsEven
    {
        get { return NumValue % 2 == 0; }
    }

    public void SetBodyToKinematic()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public void SetBodyToDynamic()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void MoveTo(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

    public int NumValue
    {
        get { return _numValue; }
        set {
            _numValue = value;
            tmpText.text = _numValue.ToString();
            SetColor();
        }
    }

    public Color MyColor
    {
        get { return spriteRenderer.color; }
        set { spriteRenderer.color = value; }
    }

    [Button]
    public void GetTextMeshPro()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    [Button]
    public void GetSpriteRenderer()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
}
