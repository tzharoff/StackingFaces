using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody2D))]
public class Face : MonoBehaviour
{
    public static Action<Face, Face> onFaceCollisionCallback;

    [SerializeField] private FaceType myFaceType;
    [SerializeField] private Sprite faceSprite;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetBodyToKinematic();
    }

    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.TryGetComponent(out Face face)){
            if(myFaceType != face.MyFaceType)
                return;
                
            onFaceCollisionCallback?.Invoke(this, face);
        }
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

    public FaceType MyFaceType{
        get { return myFaceType; }
    }

    public Sprite FaceSprite
    {
        get
        {
            return faceSprite;
        }
    }

    [Button]
    private void GetSprite()
    {
        faceSprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

}
