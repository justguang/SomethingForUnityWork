/// <summary>
///********************************************
/// ClassName    ：  UpdateItem
/// Author       ：  LCG
/// CreateTime   ：  2022/7/19 星期二
/// Description  ：  AOI信息数据
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;

namespace UAOICell
{
    public class UpdateItem
    {
        public List<EnterItem> enterLst;
        public List<MoveItem> moveLst;
        public List<ExitItem> exitLst;

        public bool IsEmpty
        {
            get => enterLst.Count == 0 && moveLst.Count == 0 && exitLst.Count == 0;
        }
        public UpdateItem(int enter, int move, int exit)
        {
            enterLst = new List<EnterItem>(enter);
            moveLst = new List<MoveItem>(move);
            exitLst = new List<ExitItem>(exit);
        }
        public void Reset()
        {
            enterLst.Clear();
            moveLst.Clear();
            exitLst.Clear();
        }
        public override string ToString()
        {
            string content = "";
            if (enterLst != null)
            {
                for (int i = 0; i < enterLst.Count; i++)
                {
                    EnterItem em = enterLst[i];
                    content += $"Enter:{em.id} {em.x},{em.z}\n";
                }
            }
            if (moveLst != null)
            {
                for (int i = 0; i < moveLst.Count; i++)
                {
                    MoveItem mm = moveLst[i];
                    content += $"Move:{mm.id} {mm.x},{mm.z}\n";
                }
            }
            if (exitLst != null)
            {
                for (int i = 0; i < exitLst.Count; i++)
                {
                    ExitItem em = exitLst[i];
                    content += $"Exit:{em.id}\n";
                }
            }

            return content;
        }
    }

    public struct EnterItem
    {
        public uint id;
        public float x;
        public float z;
        public EnterItem(uint id, float x, float z)
        {
            this.id = id;
            this.x = x;
            this.z = z;
        }
    }
    public struct MoveItem
    {
        public uint id;
        public float x;
        public float z;
        public MoveItem(uint id, float x, float z)
        {
            this.id = id;
            this.x = x;
            this.z = z;
        }
    }
    public struct ExitItem
    {
        public uint id;
        public ExitItem(uint id)
        {
            this.id = id;
        }
    }
}
