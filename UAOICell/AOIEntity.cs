/// <summary>
///********************************************
/// ClassName    ：  AOIEntity
/// Author       ：  LCG
/// CreateTime   ：  2022/7/18 星期一
/// Description  ：  AOI实体
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;

namespace UAOICell
{
    public enum EntityDriverEnum
    {
        None,
        Client,
        Server
    }

    public class AOIEntity
    {
        public uint entityID;
        public AOIMgr aoiMgr;
        public int xIndex = 0;
        public int xLastIndex = 0;
        public int zIndex = 0;
        public int zLastIndex = 0;
        public string cellKey = "";
        public string cellLastKey = "";
        private float posX;
        public float PosX { get => posX; }
        private float posZ;
        public float PosZ { get => posZ; }
        private EntityOpEnum opEnum;
        public EntityOpEnum OpEnum { get => opEnum; }
        private CrossDirEnum dirEnum;
        public CrossDirEnum DirEnum { get => dirEnum; }
        private EntityDriverEnum driverEnum;
        public EntityDriverEnum DriverEnum { get => driverEnum; }
        AOICell[] aroundAddCell = null;
        List<AOICell> singleAddCellLst = new List<AOICell>(5);
        List<AOICell> singleRmvCellLst = new List<AOICell>(5);
        private UpdateItem entityUpdateItem;
        public AOIEntity(uint entityID, AOIMgr aoiMgr, EntityDriverEnum driverEnum)
        {
            this.entityID = entityID;
            this.aoiMgr = aoiMgr;
            this.driverEnum = driverEnum;
            entityUpdateItem = new UpdateItem(aoiMgr.Cfg.UpdateEnter, aoiMgr.Cfg.UpdateMove, aoiMgr.Cfg.UpdateExit);
        }

        public void UpdatePos(float x, float z, EntityOpEnum op = EntityOpEnum.None)
        {
            posX = x;
            posZ = z;
            opEnum = op;

            int _xIndex = (int)(Math.Floor(posX / aoiMgr.CellSize));
            int _zIndex = (int)(Math.Floor(posZ / aoiMgr.CellSize));
            string _cellkey = $"{_xIndex},{_zIndex}";
            if (_cellkey != cellKey)
            {
                xLastIndex = xIndex;
                zLastIndex = zIndex;
                cellLastKey = cellKey;

                if (cellKey != "")
                {
                    aoiMgr.MarkExitEntityCell(this); ;
                }

                xIndex = _xIndex;
                zIndex = _zIndex;
                cellKey = _cellkey;

                if (opEnum != EntityOpEnum.TransferEnter && opEnum != EntityOpEnum.TransferOut)
                {
                    opEnum = EntityOpEnum.MoveCross;
                    if (xIndex < xLastIndex)
                    {
                        if (zIndex == zLastIndex)
                        {
                            dirEnum = CrossDirEnum.Left;
                        }
                        else if (zIndex < zLastIndex)
                        {
                            dirEnum = CrossDirEnum.LeftDown;
                        }
                        else
                        {
                            dirEnum = CrossDirEnum.LeftUp;
                        }
                    }
                    else if (xIndex > xLastIndex)
                    {
                        if (zIndex == zLastIndex)
                        {
                            dirEnum = CrossDirEnum.Right;
                        }
                        else if (zIndex < zLastIndex)
                        {
                            dirEnum = CrossDirEnum.RightDown;
                        }
                        else
                        {
                            dirEnum = CrossDirEnum.RightUp;
                        }
                    }
                    else
                    {
                        if (zIndex > zLastIndex)
                        {
                            dirEnum = CrossDirEnum.Up;
                        }
                        else
                        {
                            dirEnum = CrossDirEnum.Down;
                        }
                    }
                }


                //进入新的宫格
                //this.Log($"move cross:{cellKey}");
                aoiMgr.MoveCrossCell(this);
            }
            else
            {
                opEnum = EntityOpEnum.MoveInside;
                dirEnum = CrossDirEnum.None;
                //this.Log($"move inside:{cellKey}");
                aoiMgr.MoveInsideCell(this);
            }
        }
        public void AddAroundCellView(AOICell[] aroundArr)
        {
            if (DriverEnum == EntityDriverEnum.Client)
            {
                aroundAddCell = aroundArr;
            }
        }
        public void AddCellView(AOICell cell)
        {
            if (DriverEnum == EntityDriverEnum.Client)
            {
                singleAddCellLst.Add(cell);
            }
        }
        public void RmvCellView(AOICell cell)
        {
            if (DriverEnum == EntityDriverEnum.Client)
            {
                singleRmvCellLst.Add(cell);
            }
        }

        public void CalcEntityCellViewChange()
        {
            AOICell cell = aoiMgr.GetOrCreateCell(this);
            if (cell.clientEntityConcernCount > 0 && DriverEnum == EntityDriverEnum.Client)
            {
                if (aroundAddCell != null)
                {
                    for (int i = 0; i < aroundAddCell.Length; i++)
                    {
                        HashSet<AOIEntity> set = aroundAddCell[i].holdSet;
                        foreach (var e in set)
                        {
                            entityUpdateItem.enterLst.Add(new EnterItem { id = e.entityID, x = e.PosX, z = e.PosZ });
                        }
                    }
                }
                for (int i = 0; i < singleAddCellLst.Count; i++)
                {
                    HashSet<AOIEntity> set = singleAddCellLst[i].holdSet;
                    foreach (var e in set)
                    {
                        entityUpdateItem.enterLst.Add(new EnterItem { id = e.entityID, x = e.PosX, z = e.PosZ });
                    }
                }
                for (int i = 0; i < singleRmvCellLst.Count; i++)
                {
                    HashSet<AOIEntity> set = singleRmvCellLst[i].holdSet;
                    foreach (var e in set)
                    {
                        entityUpdateItem.exitLst.Add(new ExitItem { id = e.entityID });
                    }
                }

                if (!entityUpdateItem.IsEmpty)
                {
                    aoiMgr.OnEntityCellViewChange?.Invoke(this, entityUpdateItem);
                    entityUpdateItem.Reset();
                }
            }

            aroundAddCell = null;
            singleAddCellLst.Clear();
            singleRmvCellLst.Clear();
        }
    }

    public enum EntityOpEnum
    {
        None,
        TransferEnter,
        TransferOut,
        MoveCross,
        MoveInside
    }

    public enum CrossDirEnum
    {
        None,
        Up,
        Down,
        Left,
        Right,
        LeftUp,
        RightUp,
        RightDown,
        LeftDown,
    }
}
