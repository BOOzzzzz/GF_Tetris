using System;
using System.Collections.Generic;
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

    public bool isLocked;

    private Vector3 originPosition;
    private Vector3 pivot;
    private bool isPreviewBlock;
    private Color color;
    private Sprite originalSprite;
    private string originalName;
    private List<Vector2> blockPos;

    private ProcedureMain procedureMain;

    //Property
    public bool IsLocked => isLocked;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        originalSprite = GetComponentInChildren<SpriteRenderer>().sprite;
        originalName = name.Substring(0, 5);
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        name = originalName + " - " + EntityExtension.serialID;
        isPreviewBlock = (bool)userData;

        procedureMain = GameEntry.Procedure.CurrentProcedure as ProcedureMain;

        if (isPreviewBlock)
        {
            isLocked = true;
        }
        else
        {
            timer = 0;
            fallTime = 1;

            CachedTransform.position = procedureMain.originPos;
            CachedTransform.rotation = Quaternion.identity;
            pivot = procedureMain.pivot;
            color = procedureMain.color;

            isLocked = false;
        }

        blockPos = procedureMain.originBlockPos;
        int index = 0;
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sprite =
                isPreviewBlock ? procedureMain.previewBlockSprite : originalSprite;
            child.GetComponent<SpriteRenderer>().color = isPreviewBlock ? Color.white : color;
            child.gameObject.SetActive(true);
            child.localPosition = blockPos[index];
            index++;
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

            GameEntry.Event.Fire(this, UpdatePreviewBlockEventArgs.Create());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            CachedTransform.position += Vector3.right;
            if (!IsBlockInArea())
            {
                CachedTransform.position -= Vector3.right;
            }

            GameEntry.Event.Fire(this, UpdatePreviewBlockEventArgs.Create());
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
                }), BlockRange().Item1, BlockRange().Item2);
            }

            timer = 0;
        }

        fallTime = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ? 0.02f : 1f;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CachedTransform.RotateAround(CachedTransform.TransformPoint(pivot), Vector3.forward, 90);
            if (!IsBlockInArea())
            {
                CachedTransform.RotateAround(CachedTransform.TransformPoint(pivot), Vector3.forward, -90);
            }

            GameEntry.Event.Fire(this, UpdatePreviewBlockEventArgs.Create());
        }
    }

    #region MovableBlock

    public Vector3 CalculatePositionOffset()
    {
        int index = 0;
        while (IsBlockInArea(index - 1))
        {
            index--;
        }

        return new Vector3(0, index, 0);
    }

    public bool IsBlockInArea()
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

    public bool IsBlockInArea(int addY)
    {
        foreach (Transform child in transform)
        {
            var x = Mathf.RoundToInt(child.position.x);
            var y = Mathf.RoundToInt(child.position.y + addY);
            if (x < 0 || y < 0 || x > procedureMain.width - 1 || y > procedureMain.height - 1 ||
                procedureMain.grid[x, y] != null)
            {
                return false;
            }
        }

        return true;
    }

    private void AddBlockToGrid(Action<int, int> onComplete, int min, int max)
    {
        foreach (Transform child in transform)
        {
            var x = Mathf.RoundToInt(child.position.x);
            var y = Mathf.RoundToInt(child.position.y);
            procedureMain.grid[x, y] = child;
        }

        onComplete?.Invoke(min, max);
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

    #endregion

    #region PreviewBlock

    public void SetPreviewBlockInfo(Vector3 position, Quaternion rotation)
    {
        CachedTransform.position = position;
        CachedTransform.rotation = rotation;
    }

    #endregion
}