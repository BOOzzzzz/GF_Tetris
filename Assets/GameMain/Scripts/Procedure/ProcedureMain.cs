using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using GameMain.Scripts.Event;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BOO.Procedure
{
    public class ProcedureMain : ProcedureBase
    {
        public int width = 10;
        public int height = 20;
        public Transform[,] grid;

        public Vector2 originPos;
        public Vector2 nextBlockPos;
        public Vector2 pivot;
        public List<Vector2> originBlockPos;
        public List<Vector2> nextSingleBlockPos;
        public Color color;
        public Color nextColor;
        public Sprite previewBlockSprite;

        private bool gameOver = false;
        private EntityBlock currentEntity;
        private EntityBlock previewEntity;
        private EntityBlock nextEntity;
        private int currentRow = 0;
        private int nextRow = 0;

        private int scoreIndex = 0;
        private readonly int scoreWeight = 10;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            grid = new Transform[width, height];
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormMain"), "Main");
            GameEntry.Resource.LoadAsset(AssetUtility.GetSpriteAsset("Block-Shadow@3x"), typeof(Sprite),
                new LoadAssetCallbacks(LoadAssetSuccess));
            GameEntry.Event.Subscribe(SpawnBlockEventArgs.EventId, SpawnBlock);
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId, GameOverEvent);
            GameEntry.Event.Subscribe(UpdatePreviewBlockEventArgs.EventId, UpdatePreviewBlockInfo);
            AwaitableExtensions.SubscribeEvent();
            GameEntry.Event.Fire(this, SpawnBlockEventArgs.Create());
            
            scoreIndex = 0;
            GameEntry.Event.Fire(this, UpdateScoreEventArgs.Create(scoreIndex * scoreWeight));
        }

        private void LoadAssetSuccess(string assetname, object asset, float duration, object userdata)
        {
            previewBlockSprite = asset as Sprite;
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds,
            float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (gameOver)
            {
                gameOver = false;
                ChangeState<ProcedureGameOver>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(SpawnBlockEventArgs.EventId, SpawnBlock);
            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId, GameOverEvent);
            GameEntry.Event.Unsubscribe(UpdatePreviewBlockEventArgs.EventId, UpdatePreviewBlockInfo);
            AwaitableExtensions.UnsubscribeEvent();
            ClearGrid();
        }

        private void UpdatePreviewBlockInfo(object sender, GameEventArgs e)
        {
            var offset = currentEntity.CalculatePositionOffset();
            Vector3 position = currentEntity.CachedTransform.position + offset;
            Quaternion rotation = currentEntity.CachedTransform.rotation;
            previewEntity.SetPreviewBlockInfo(position, rotation);
        }

        private void GameOverEvent(object sender, GameEventArgs e)
        {
            gameOver = true;
        }

        private async void SpawnBlock(object sender, GameEventArgs e)
        {
            if (previewEntity != null)
            {
                if (previewEntity.isActiveAndEnabled)
                    GameEntry.Entity.HideEntity(previewEntity.Entity);
            }
            if (nextEntity != null)
            {
                if (nextEntity.isActiveAndEnabled)
                    GameEntry.Entity.HideEntity(nextEntity.Entity);
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            currentRow = currentRow == 0 ? Random.Range(1, 8) : nextRow;
            nextRow = Random.Range(1, 8);
            DREntity drCurEntity = dtEntity.GetDataRow(currentRow);
            DREntity drNextEntity = dtEntity.GetDataRow(nextRow);
            originBlockPos = drCurEntity.SingleBlockPosition;
            originPos = drCurEntity.OriginPosition;
            pivot = drCurEntity.Pivot;
            color = drCurEntity.Color;
            nextColor = drNextEntity.Color;
            nextSingleBlockPos = drNextEntity.SingleBlockPosition;
            nextBlockPos = drNextEntity.NextBlockPosition;
            var current = await GameEntry.Entity.ShowEntityAsync(GameEntry.Entity.GenerateSerialID(),
                typeof(EntityBlock),
                AssetUtility.GetEntityAsset(drCurEntity.AssetName), drCurEntity.AssetGroup, userData: BlockStatus.Normal);
            var preview = await GameEntry.Entity.ShowEntityAsync(GameEntry.Entity.GenerateSerialID(),
                typeof(EntityBlock),
                AssetUtility.GetEntityAsset(drCurEntity.AssetName), drCurEntity.AssetGroup, userData: BlockStatus.Preview);
            currentEntity = current.Logic as EntityBlock;
            previewEntity = preview.Logic as EntityBlock;
            GameEntry.Event.Fire(this, UpdatePreviewBlockEventArgs.Create());
            
            var next = await GameEntry.Entity.ShowEntityAsync(GameEntry.Entity.GenerateSerialID(),
                typeof(EntityBlock),
                AssetUtility.GetEntityAsset(drNextEntity.AssetName), drNextEntity.AssetGroup, userData: BlockStatus.Next);
            nextEntity = next.Logic as EntityBlock;

            if (!currentEntity.IsBlockInArea())
            {
                GameEntry.Event.Fire(this, GameOverEventArgs.Create());
                GameEntry.Entity.HideEntity(currentEntity.Entity);
                GameEntry.Entity.HideEntity(previewEntity.Entity);
                GameEntry.Entity.HideEntity(nextEntity.Entity);
                currentEntity.isLocked = true;
            }
        }

        public void ClearTheRows(int min, int max)
        {
            for (int i = max; i >= min; i--)
            {
                if (HasLine(i))
                {
                    DeleteLine(i);
                    DropLine(i);
                }
            }
        }

        private bool HasLine(int line)
        {
            for (int i = 0; i < width; i++)
            {
                if (grid[i, line] == null)
                    return false;
            }

            return true;
        }

        private void DeleteLine(int line)
        {
            scoreIndex++;
            GameEntry.Event.Fire(this,UpdateScoreEventArgs.Create(scoreIndex * scoreWeight));
            for (int i = 0; i < width; i++)
            {
                grid[i, line].gameObject.SetActive(false);
                grid[i, line] = null;
            }
        }

        private void DropLine(int line)
        {
            for (int j = line; j < height - 1; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (grid[i, j + 1] != null)
                    {
                        grid[i, j] = grid[i, j + 1];
                        grid[i, j + 1] = null;
                        grid[i, j].transform.position += Vector3.down;
                    }
                }
            }
        }

        private void ClearGrid()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = null;
                }
            }
        }
    }
}