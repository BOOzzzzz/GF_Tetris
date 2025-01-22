using UnityEngine;
using UnityGameFramework.Runtime;

public class EntityBlock : EntityLogic
{
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        transform.position = (Vector2)userData;
    }
}
