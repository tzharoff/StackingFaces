using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Face : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetBodyToKinematic();
    }

    public void SetBodyToKinematic()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public void SetBodyToDynamic()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }



}
