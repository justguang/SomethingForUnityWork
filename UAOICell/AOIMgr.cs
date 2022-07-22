/// <summary>
///********************************************
/// ClassName    ：  AOIMgr
/// Author       ：  LCG
/// CreateTime   ：  2022/7/18 星期一
/// Description  ：  AOI管理器
///********************************************/
/// </summary>
using System;
using ULogs;
using System.Collections.Generic;

namespace UAOICell
{
    public class AOIConfigs
    {
        public string MapName = "";
        public int CellSize = 20;
        public int InitCount = 200;

        public int UpdateEnter = 10;
        public int UpdateMove = 50;
        public int UpdateExit = 10;
    }

    public class AOIMgr
    {
        public string mgrName;
        private AOIConfigs cfg;
        public AOIConfigs Cfg { get => cfg; private set => cfg = value; }
        private int cellSize;
        public int CellSize { get => cellSize; private set => cellSize = value; }
        private List<AOIEntity> entityLst;
        private Dictionary<string, AOICell> cellDic;
        public Action<AOIEntity, UpdateItem> OnEntityCellViewChange;
        public Action<AOICell, UpdateItem> OnCellEntityOpCombine;
        public Action<int, int> OnCreateNewCell;
        public AOIMgr(AOIConfigs cfg)
        {
            Cfg = cfg;
            mgrName = cfg.MapName;
            CellSize = cfg.CellSize;
            cellDic = new Dictionary<string, AOICell>(cfg.InitCount);
            entityLst = new List<AOIEntity>();
        }

        public AOIEntity EnterCell(uint entityID, float x, float z, EntityDriverEnum driverEnum)
        {
            AOIEntity entity = new AOIEntity(entityID, this, driverEnum);
            entity.UpdatePos(x, z, EntityOpEnum.TransferEnter);
            entityLst.Add(entity);
            return entity;
        }
        public void UpdatePos(AOIEntity entity, float x, float z)
        {
            entity.UpdatePos(x, z);
        }
        public void ExitCell(AOIEntity entity)
        {
            if (cellDic.TryGetValue(entity.cellKey, out AOICell cell))
            {
                cell.ExitCell(entity);
            }
            else
            {
                this.Log($"cellDic can not find:{entity.cellKey}");
            }
            if (!entityLst.Remove(entity))
            {
                this.Log($"entityLst can not find:{entity.cellKey}");
            }
        }
        public void CalcAOIUpdate()
        {
            for (int i = 0; i < entityLst.Count; i++)
            {
                entityLst[i].CalcEntityCellViewChange();
            }

            foreach (var item in cellDic)
            {
                AOICell cell = item.Value;

                if (cell.exitSet.Count > 0)
                {
                    cell.holdSet.ExceptWith(cell.exitSet);
                    cell.exitSet.Clear();
                }

                if (cell.enterSet.Count > 0)
                {
                    cell.holdSet.UnionWith(cell.enterSet);
                    cell.enterSet.Clear();
                }

                cell.CalcCellOpCombine();
            }
        }
        public void MoveCrossCell(AOIEntity entity)
        {
            AOICell cell = GetOrCreateCell(entity);
            if (!cell.IsCalcBoundary)
            {
                CalcCellBoundary(cell);
            }
            cell.EnterCell(entity);
        }
        public void MoveInsideCell(AOIEntity entity)
        {
            if (cellDic.TryGetValue(entity.cellKey, out AOICell cell))
            {
                cell.MoveCell(entity);
            }
            else
            {
                this.Log($"cellDic can not find:{entity.cellKey}");
            }
        }
        public void MarkExitEntityCell(AOIEntity entity)
        {
            if (cellDic.TryGetValue(entity.cellKey, out AOICell cell))
            {
                cell.exitSet.Add(entity);
            }
            else
            {
                this.Log($"cellDic can not find:{entity.cellKey}");
            }
        }
        public AOICell GetOrCreateCell(AOIEntity entity)
        {
            AOICell cell;
            if (!cellDic.TryGetValue(entity.cellKey, out cell))
            {
                cell = new AOICell(entity.xIndex, entity.zIndex, this);
                cellDic.Add(entity.cellKey, cell);

                OnCreateNewCell?.Invoke(cell.xIndex, cell.zIndex);
            }

            return cell;
        }

        void CalcCellBoundary(AOICell cell)
        {
            int xIndex = cell.xIndex;
            int zIndex = cell.zIndex;

            cell.AroundArr = new AOICell[9];
            int index = 0;
            for (int i = xIndex - 2; i <= xIndex + 2; i++)
            {
                for (int j = zIndex - 2; j <= zIndex + 2; j++)
                {
                    string key = $"{i},{j}";
                    if (!cellDic.TryGetValue(key, out AOICell ac))
                    {
                        ac = new AOICell(i, j, this);
                        cellDic.Add(key, ac);
                        OnCreateNewCell?.Invoke(i, j);
                    }

                    if (i > xIndex - 2
                    && i < xIndex + 2
                    && j > zIndex - 2
                    && j < zIndex + 2)
                    {
                        cell.AroundArr[index] = ac;
                        ++index;
                    }
                }
            }

            //up 3:exit,3:enter,6:move
            {
                cell.UpArr = new AOICell[12];
                cell.UpArr[0] = cellDic[$"{xIndex - 1},{zIndex - 2}"];
                cell.UpArr[1] = cellDic[$"{xIndex},{zIndex - 2}"];
                cell.UpArr[2] = cellDic[$"{xIndex + 1},{zIndex - 2}"];

                cell.UpArr[3] = cellDic[$"{xIndex - 1},{zIndex + 1}"];
                cell.UpArr[4] = cellDic[$"{xIndex},{zIndex + 1}"];
                cell.UpArr[5] = cellDic[$"{xIndex + 1},{zIndex + 1}"];

                cell.UpArr[6] = cellDic[$"{xIndex - 1},{zIndex}"];
                cell.UpArr[7] = cellDic[$"{xIndex},{zIndex}"];
                cell.UpArr[8] = cellDic[$"{xIndex + 1},{zIndex}"];
                cell.UpArr[9] = cellDic[$"{xIndex - 1},{zIndex - 1}"];
                cell.UpArr[10] = cellDic[$"{xIndex },{zIndex - 1}"];
                cell.UpArr[11] = cellDic[$"{xIndex + 1},{zIndex - 1}"];
            }

            //下移操作：3:exit，3:enter，6:move
            {
                cell.DownArr = new AOICell[12];
                cell.DownArr[0] = cellDic[$"{xIndex - 1},{zIndex + 2}"];
                cell.DownArr[1] = cellDic[$"{xIndex},{zIndex + 2}"];
                cell.DownArr[2] = cellDic[$"{xIndex + 1},{zIndex + 2}"];

                cell.DownArr[3] = cellDic[$"{xIndex - 1},{zIndex - 1}"];
                cell.DownArr[4] = cellDic[$"{xIndex},{zIndex - 1}"];
                cell.DownArr[5] = cellDic[$"{xIndex + 1},{zIndex - 1}"];

                cell.DownArr[6] = cellDic[$"{xIndex - 1},{zIndex + 1}"];
                cell.DownArr[7] = cellDic[$"{xIndex},{zIndex + 1}"];
                cell.DownArr[8] = cellDic[$"{xIndex + 1},{zIndex + 1}"];
                cell.DownArr[9] = cellDic[$"{xIndex - 1},{zIndex}"];
                cell.DownArr[10] = cellDic[$"{xIndex},{zIndex}"];
                cell.DownArr[11] = cellDic[$"{xIndex + 1},{zIndex}"];
            }

            //左移操作：3:exit，3:enter，6:move
            {
                cell.LeftArr = new AOICell[12];
                cell.LeftArr[0] = cellDic[$"{xIndex + 2},{zIndex + 1}"];
                cell.LeftArr[1] = cellDic[$"{xIndex + 2},{zIndex}"];
                cell.LeftArr[2] = cellDic[$"{xIndex + 2},{zIndex - 1}"];

                cell.LeftArr[3] = cellDic[$"{xIndex - 1},{zIndex + 1}"];
                cell.LeftArr[4] = cellDic[$"{xIndex - 1},{zIndex}"];
                cell.LeftArr[5] = cellDic[$"{xIndex - 1},{zIndex - 1}"];

                cell.LeftArr[6] = cellDic[$"{xIndex},{zIndex + 1}"];
                cell.LeftArr[7] = cellDic[$"{xIndex},{zIndex}"];
                cell.LeftArr[8] = cellDic[$"{xIndex},{zIndex - 1}"];
                cell.LeftArr[9] = cellDic[$"{xIndex + 1},{zIndex + 1}"];
                cell.LeftArr[10] = cellDic[$"{xIndex + 1},{zIndex}"];
                cell.LeftArr[11] = cellDic[$"{xIndex + 1},{zIndex - 1}"];
            }

            //右移操作：3:exit，3:enter，6:move
            {
                cell.RightArr = new AOICell[12];
                cell.RightArr[0] = cellDic[$"{xIndex - 2},{zIndex + 1}"];
                cell.RightArr[1] = cellDic[$"{xIndex - 2},{zIndex}"];
                cell.RightArr[2] = cellDic[$"{xIndex - 2},{zIndex - 1}"];

                cell.RightArr[3] = cellDic[$"{xIndex + 1},{zIndex + 1}"];
                cell.RightArr[4] = cellDic[$"{xIndex + 1},{zIndex}"];
                cell.RightArr[5] = cellDic[$"{xIndex + 1},{zIndex - 1}"];

                cell.RightArr[6] = cellDic[$"{xIndex - 1},{zIndex + 1}"];
                cell.RightArr[7] = cellDic[$"{xIndex - 1},{zIndex}"];
                cell.RightArr[8] = cellDic[$"{xIndex - 1},{zIndex - 1}"];
                cell.RightArr[9] = cellDic[$"{xIndex},{zIndex + 1}"];
                cell.RightArr[10] = cellDic[$"{xIndex},{zIndex}"];
                cell.RightArr[11] = cellDic[$"{xIndex},{zIndex - 1}"];
            }

            //左上操作：5:exit, 5:enter, 4:move
            {
                cell.LeftUpArr = new AOICell[14];
                cell.LeftUpArr[0] = cellDic[$"{xIndex},{zIndex - 2}"];
                cell.LeftUpArr[1] = cellDic[$"{xIndex + 1},{zIndex - 2}"];
                cell.LeftUpArr[2] = cellDic[$"{xIndex + 2},{zIndex - 2}"];
                cell.LeftUpArr[3] = cellDic[$"{xIndex + 2},{zIndex - 1}"];
                cell.LeftUpArr[4] = cellDic[$"{xIndex + 2},{zIndex}"];

                cell.LeftUpArr[5] = cellDic[$"{xIndex - 1},{zIndex - 1}"];
                cell.LeftUpArr[6] = cellDic[$"{xIndex - 1},{zIndex}"];
                cell.LeftUpArr[7] = cellDic[$"{xIndex - 1},{zIndex + 1}"];
                cell.LeftUpArr[8] = cellDic[$"{xIndex},{zIndex + 1}"];
                cell.LeftUpArr[9] = cellDic[$"{xIndex + 1},{zIndex + 1}"];

                cell.LeftUpArr[10] = cellDic[$"{xIndex},{zIndex}"];
                cell.LeftUpArr[11] = cellDic[$"{xIndex + 1},{zIndex}"];
                cell.LeftUpArr[12] = cellDic[$"{xIndex},{zIndex - 1}"];
                cell.LeftUpArr[13] = cellDic[$"{xIndex + 1},{zIndex - 1}"];
            }

            //右上操作：5:exit, 5:enter, 4:move
            {
                cell.RightUpArr = new AOICell[14];
                cell.RightUpArr[0] = cellDic[$"{xIndex - 2},{zIndex}"];
                cell.RightUpArr[1] = cellDic[$"{xIndex - 2},{zIndex - 1}"];
                cell.RightUpArr[2] = cellDic[$"{xIndex - 2},{zIndex - 2}"];
                cell.RightUpArr[3] = cellDic[$"{xIndex - 1},{zIndex - 2}"];
                cell.RightUpArr[4] = cellDic[$"{xIndex},{zIndex - 2}"];

                cell.RightUpArr[5] = cellDic[$"{xIndex - 1},{zIndex + 1}"];
                cell.RightUpArr[6] = cellDic[$"{xIndex},{zIndex + 1}"];
                cell.RightUpArr[7] = cellDic[$"{xIndex + 1},{zIndex + 1}"];
                cell.RightUpArr[8] = cellDic[$"{xIndex + 1},{zIndex}"];
                cell.RightUpArr[9] = cellDic[$"{xIndex + 1},{zIndex - 1}"];

                cell.RightUpArr[10] = cellDic[$"{xIndex - 1},{zIndex}"];
                cell.RightUpArr[11] = cellDic[$"{xIndex},{zIndex}"];
                cell.RightUpArr[12] = cellDic[$"{xIndex - 1},{zIndex - 1}"];
                cell.RightUpArr[13] = cellDic[$"{xIndex},{zIndex - 1}"];
            }

            //左下操作：5:exit, 5:enter, 4:move
            {
                cell.LeftDownArr = new AOICell[14];
                cell.LeftDownArr[0] = cellDic[$"{xIndex},{zIndex + 2}"];
                cell.LeftDownArr[1] = cellDic[$"{xIndex + 1},{zIndex + 2}"];
                cell.LeftDownArr[2] = cellDic[$"{xIndex + 2},{zIndex + 2}"];
                cell.LeftDownArr[3] = cellDic[$"{xIndex + 2},{zIndex + 1}"];
                cell.LeftDownArr[4] = cellDic[$"{xIndex + 2},{zIndex}"];

                cell.LeftDownArr[5] = cellDic[$"{xIndex - 1},{zIndex + 1}"];
                cell.LeftDownArr[6] = cellDic[$"{xIndex - 1},{zIndex}"];
                cell.LeftDownArr[7] = cellDic[$"{xIndex - 1},{zIndex - 1}"];
                cell.LeftDownArr[8] = cellDic[$"{xIndex},{zIndex - 1}"];
                cell.LeftDownArr[9] = cellDic[$"{xIndex + 1},{zIndex - 1}"];

                cell.LeftDownArr[10] = cellDic[$"{xIndex},{zIndex + 1}"];
                cell.LeftDownArr[11] = cellDic[$"{xIndex + 1},{zIndex + 1}"];
                cell.LeftDownArr[12] = cellDic[$"{xIndex},{zIndex}"];
                cell.LeftDownArr[13] = cellDic[$"{xIndex + 1},{zIndex}"];
            }

            //右下操作：5:exit, 5:enter, 4:move
            {
                cell.RightDownArr = new AOICell[14];
                cell.RightDownArr[0] = cellDic[$"{xIndex - 2},{zIndex + 2}"];
                cell.RightDownArr[1] = cellDic[$"{xIndex - 2},{zIndex + 1}"];
                cell.RightDownArr[2] = cellDic[$"{xIndex - 2},{zIndex}"];
                cell.RightDownArr[3] = cellDic[$"{xIndex - 1},{zIndex + 2}"];
                cell.RightDownArr[4] = cellDic[$"{xIndex},{zIndex + 2}"];

                cell.RightDownArr[5] = cellDic[$"{xIndex - 1},{zIndex - 1}"];
                cell.RightDownArr[6] = cellDic[$"{xIndex},{zIndex - 1}"];
                cell.RightDownArr[7] = cellDic[$"{xIndex + 1},{zIndex - 1}"];
                cell.RightDownArr[8] = cellDic[$"{xIndex + 1},{zIndex}"];
                cell.RightDownArr[9] = cellDic[$"{xIndex + 1},{zIndex + 1}"];

                cell.RightDownArr[10] = cellDic[$"{xIndex - 1},{zIndex + 1}"];
                cell.RightDownArr[11] = cellDic[$"{xIndex},{zIndex + 1}"];
                cell.RightDownArr[12] = cellDic[$"{xIndex - 1},{zIndex}"];
                cell.RightDownArr[13] = cellDic[$"{xIndex},{zIndex}"];
            }
            cell.IsCalcBoundary = true;
        }

        public Dictionary<string, AOICell> GetExistCellDic()
        {
            return cellDic;
        }
    }
}
