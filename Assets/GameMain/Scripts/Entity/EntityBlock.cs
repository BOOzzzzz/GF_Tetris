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
    public bool isLocked;

    private Vector3 originPosition;
    private Vector3 pivot;
    public BlockStatus status;
    private Color color;
    private Sprite originalSprite;
    private string originalName;
    private List<Vector2> blockPos;
    private List<Vector2> nextBlockPos;

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
        status = (BlockStatus)userData;

        procedureMain = GameEntry.Procedure.CurrentProcedure as ProcedureMain;

        switch (status)
        {
            case BlockStatus.Normal:

                CachedTransform.position = procedureMain.originPos;
                CachedTransform.rotation = Quaternion.identity;
                pivot = procedureMain.pivot;
                color = procedureMain.color;

                isLocked = false;
                break;
            case BlockStatus.Preview:
                isLocked = true;
                break;
            case BlockStatus.Next:
                isLocked = true;
                nextBlockPos = procedureMain.nextSingleBlockPos;
                int i = 0;
                foreach (Transform child in transform)
                {
                    child.GetComponent<SpriteRenderer>().sprite = originalSprite;
                    child.GetComponent<SpriteRenderer>().color = procedureMain.nextColor;
                    child.gameObject.SetActive(true);
                    child.localPosition = nextBlockPos[i];
                    i++;
                }

                CachedTransform.position = procedureMain.nextBlockPos;
                CachedTransform.rotation = Quaternion.identity;
                break;
        }

        if (status != BlockStatus.Next)
        {
            blockPos = procedureMain.originBlockPos;
            int index = 0;
            foreach (Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().sprite =
                    status==BlockStatus.Preview ? procedureMain.previewBlockSprite : originalSprite;
                child.GetComponent<SpriteRenderer>().color = status==BlockStatus.Preview ? Color.white : color;
                child.gameObject.SetActive(true);
                child.localPosition = blockPos[index];
                index++;
            }
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

    public void AddBlockToGrid(Action<int, int> onComplete, int min, int max)
    {
        foreach (Transform child in transform)
        {
            var x = Mathf.RoundToInt(child.position.x);
            var y = Mathf.RoundToInt(child.position.y);
            procedureMain.grid[x, y] = child;
        }

        onComplete?.Invoke(min, max);
    }

    public Tuple<int, int> BlockRange()
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

public enum BlockStatus
{
    Normal,
    Preview,
    Next
}