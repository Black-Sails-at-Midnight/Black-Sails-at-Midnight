using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBindingSurface
{
    public void Bind(GameObject source);
    public void Unbind();
}
