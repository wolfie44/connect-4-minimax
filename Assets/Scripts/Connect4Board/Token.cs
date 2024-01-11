 using UnityEngine;
 using System;

public class Token : ICloneable
{
    public GameObject VisualToken { get; set; }
    public int IdOwner { get; set; }

    public Token(GameObject gameObject, int owner)
    {
        VisualToken = gameObject;
        IdOwner = owner;
    }

    public object Clone()
    {
        return new Token(null, this.IdOwner);
    }
}
