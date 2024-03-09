 using UnityEngine;
 using System;


[Serializable]
public class Token : ICloneable
{
    public GameObject VisualToken { get; set; }
    public ulong IdOwner { get; set; }

    public Token(GameObject gameObject, ulong owner)
    {
        VisualToken = gameObject;
        IdOwner = owner;
    }

    public object Clone()
    {
        return new Token(null, this.IdOwner);
    }
}
