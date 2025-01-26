using System;
using BOO;
using BOO.Procedure;
using GameMain.Scripts.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = BOO.GameEntry;

public class EntityBlock : EntityLogic
{
    private float fallTime = 1.0f;
    private float timer = 0.0f;
    private bool isLocked;
    private Vector3 pivot;

    private ProcedureMain procedureMain;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        procedureMain = (ProcedureMain)userData;
        CachedTransform.position = procedureMain.originPosition;
        pivot = procedureMain.pivot;
        
        timer = 0;
        fallTime = 1;

        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().color = procedureMain.color;
        }

        if (!IsBlockInArea())
        {
            GameEntry.Event.Fire(this, GameOverEventArgs.Create());
            GameEntry.Entity.HideEntity(Entity);
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (isLocked) return;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            CachedTransform.position += Vector3.left;
            if (!IsBlockInArea())
            {
                CachedTransform.position -= Vector3.left;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            CachedTransform.position += Vector3.right;
            if (!IsBlockInArea())
            {
                CachedTransform.position -= Vector3.right;
            }
        }

        timer += elapseSeconds;
        if (timer > fallTime)
        {
            CachedTransform.position += Vector3.down;
            if (!IsBlockInArea())
            {
                CachedTransform.position -= Vector3.down;
                AddBlockToGrid(((min, max) =>
                {
                    procedureMain.ClearTheRows(min, max);
                    GameEntry.Event.Fire(this, SpawnBlockEventArgs.Create());
                    isLocked = true;
                }),BlockRange().Item1, BlockRange().Item2);
            }

            timer = 0;
        }

        fallTime = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ? 0.02f : 1f;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(pivot),Vector3.forward, 90);
            if (!IsBlockInArea())
            {
                transform.RotateAround(transform.TransformPoint(pivot), Vector3.forward, -90);
            }
        }
    }

    private bool IsBlockInArea()
    {
        foreach (Transform child in transform)
        {
            var x = Mathf.RoundToInt(child.position.x);
            var y = Mathf.RoundToInt(child.position.y);
            if (x < 0 || y < 0 || x > procedureMain.width - 1 || y > procedureMain.height - 1 ||
                procedureMain.grid[x, y] != null)
            {
                return false;
            }
        }

        return true;
    }

    private void AddBlockToGrid(Action<int,int> onComplete,int min,int max)
    {
        foreach (Transform child in transform)
        {
            var x = Mathf.RoundToInt(child.position.x);
            var y = Mathf.RoundToInt(child.position.y);
            procedureMain.grid[x, y] = child;
        }
        onComplete?.Invoke(min,max);
    }
    
    private Tuple<int, int> BlockRange()
    {
        int yMax = Int32.MinValue;
        int yMin = Int32.MaxValue;
        foreach (Transform child in transform)
        {
            var x = Mathf.RoundToInt(child.position.x);
            var y = Mathf.RoundToInt(child.position.y);
            if (y > yMax)
            {
                yMax = y;
            }

            if (y < yMin)
            {
                yMin = y;
            }
            procedureMain.grid[x, y] = child;
        }
        
        return Tuple.Create(yMin, yMax); 
    }
}