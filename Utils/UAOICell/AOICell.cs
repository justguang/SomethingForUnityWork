/// <summary>
///********************************************
/// ClassName    ：  AOICell
/// Author       ：  LCG
/// CreateTime   ：  2022/7/18 星期一
/// Description  ：  AOI宫格
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;

namespace RGuang.Utils.UAOICell
{
    public class AOICell
    {
        public int xIndex;
        public int zIndex;
        public AOIMgr aoiMgr;
        public AOICell[] AroundArr = null;

        public AOICell[] UpArr = null;
        public AOICell[] DownArr = null;
        public AOICell[] LeftArr = null;
        public AOICell[] RightArr = null;
        public AOICell[] LeftUpArr = null;
        public AOICell[] RightUpArr = null;
        public AOICell[] LeftDownArr = null;
        public AOICell[] RightDownArr = null;
        public bool IsCalcBoundary;

        public int clientEntityConcernCount;
        public int serverEntityConcernCount;

        public HashSet<AOIEntity> holdSet = new HashSet<AOIEntity>();
        public HashSet<AOIEntity> enterSet = new HashSet<AOIEntity>();
        public HashSet<AOIEntity> exitSet = new HashSet<AOIEntity>();
        public UpdateItem cellUpdateItem;
        public AOICell(int xIndex, int zIndex, AOIMgr aoiMgr)
        {
            this.xIndex = xIndex;
            this.zIndex = zIndex;
            this.aoiMgr = aoiMgr;
            cellUpdateItem = new UpdateItem(aoiMgr.Cfg.UpdateEnter, aoiMgr.Cfg.UpdateMove, aoiMgr.Cfg.UpdateExit);
        }
        public void EnterCell(AOIEntity entity)
        {
            if (!enterSet.Add(entity))
            {
                this.Error($"cellSet already exist:{entity.entityID}");
                return;
            }

            if (entity.OpEnum == EntityOpEnum.TransferEnter)
            {
                entity.AddAroundCellView(AroundArr);
                for (int i = 0; i < AroundArr.Length; i++)
                {
                    AroundArr[i].AddCellOp(CellOpEnum.EntityEnter, entity);
                }
            }
            else if (entity.OpEnum == EntityOpEnum.MoveCross)
            {
                switch (entity.DirEnum)
                {
                    case CrossDirEnum.Up:
                        StraightMove(UpArr, entity);
                        break;
                    case CrossDirEnum.Down:
                        StraightMove(DownArr, entity);
                        break;
                    case CrossDirEnum.Left:
                        StraightMove(LeftArr, entity);
                        break;
                    case CrossDirEnum.Right:
                        StraightMove(RightArr, entity);
                        break;
                    case CrossDirEnum.LeftUp:
                        SkewMove(LeftUpArr, entity);
                        break;
                    case CrossDirEnum.RightUp:
                        SkewMove(RightUpArr, entity);
                        break;
                    case CrossDirEnum.RightDown:
                        SkewMove(RightDownArr, entity);
                        break;
                    case CrossDirEnum.LeftDown:
                        SkewMove(LeftDownArr, entity);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                this.Error($"{entity.entityID} OpEnum:{entity.OpEnum} error.");
            }
        }
        public void MoveCell(AOIEntity entity)
        {
            for (int i = 0; i < AroundArr.Length; i++)
            {
                AroundArr[i].AddCellOp(CellOpEnum.EntityMove, entity);
            }
        }
        public void ExitCell(AOIEntity entity)
        {
            exitSet.Add(entity);
            for (int i = 0; i < AroundArr.Length; i++)
            {
                AroundArr[i].AddCellOp(CellOpEnum.EntityExit, entity);
            }
        }

        void StraightMove(AOICell[] arr, AOIEntity entity)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (i < 3)
                {
                    entity.RmvCellView(arr[i]);
                    arr[i].AddCellOp(CellOpEnum.EntityExit, entity);
                }
                else if (i >= 3 && i < 6)
                {
                    entity.AddCellView(arr[i]);
                    arr[i].AddCellOp(CellOpEnum.EntityEnter, entity);
                }
                else
                {
                    arr[i].AddCellOp(CellOpEnum.EntityMove, entity);
                }
            }
        }
        void SkewMove(AOICell[] arr, AOIEntity entity)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (i < 5)
                {
                    entity.RmvCellView(arr[i]);
                    arr[i].AddCellOp(CellOpEnum.EntityExit, entity);
                }
                else if (i >= 5 && i < 10)
                {
                    entity.AddCellView(arr[i]);
                    arr[i].AddCellOp(CellOpEnum.EntityEnter, entity);
                }
                else
                {
                    arr[i].AddCellOp(CellOpEnum.EntityMove, entity);
                }
            }
        }
        void AddCellOp(CellOpEnum cellOp, AOIEntity entity)
        {
            switch (cellOp)
            {
                case CellOpEnum.EntityEnter:
                    if (entity.DriverEnum == EntityDriverEnum.Client)
                    {
                        ++clientEntityConcernCount;
                    }
                    else
                    {
                        ++serverEntityConcernCount;
                    }
                    cellUpdateItem.enterLst.Add(new EnterItem(entity.entityID, entity.PosX, entity.PosZ));
                    break;
                case CellOpEnum.EntityMove:
                    cellUpdateItem.moveLst.Add(new MoveItem(entity.entityID, entity.PosX, entity.PosZ));
                    break;
                case CellOpEnum.EntityExit:
                    if (entity.DriverEnum == EntityDriverEnum.Client)
                    {
                        --clientEntityConcernCount;
                    }
                    else
                    {
                        --serverEntityConcernCount;
                    }

                    cellUpdateItem.exitLst.Add(new ExitItem(entity.entityID));
                    break;
                default:
                    break;
            }
        }
        public void CalcCellOpCombine()
        {
            if (!cellUpdateItem.IsEmpty)
            {
                if (clientEntityConcernCount > 0 && holdSet.Count > 0)
                {
                    aoiMgr.OnCellEntityOpCombine?.Invoke(this, cellUpdateItem);
                }
                cellUpdateItem.Reset();
            }
        }

        public override string ToString()
        {
            return $"CellName:{xIndex},{zIndex} ExistEntity:{holdSet.Count} ClientConcernEntity:{clientEntityConcernCount} ServerConcernEntity:{serverEntityConcernCount}";
        }
    }

    enum CellOpEnum
    {
        EntityEnter,
        EntityMove,
        EntityExit,
    }
}
