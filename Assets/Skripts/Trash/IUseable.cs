using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IUseable
{
    Sprite MyIcon
    {
        get;
    }

    void Use();
}
