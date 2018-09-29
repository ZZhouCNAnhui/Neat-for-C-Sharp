using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    /// <summary>
    /// 神经网络基因
    /// </summary>
    public class Genome
    {
        /// <summary>
        /// 神经网络连接参数表
        /// </summary>
        public bool[,] LinkGenones;
        /// <summary>
        /// 神经网络权重参数表
        /// </summary>
        public float[,] LinkWs;
        /// <summary>
        /// 节点基因
        /// </summary>
        public NodeGemone[] nodeGemones;
        private int baoluizhi;
        public int Length { get { return nodeGemones.Length; } }
        /// <summary>
        /// 生成基因
        /// </summary>
        /// <param name="inputL">基因神经网络输入数量</param>
        /// <param name="outputL">基因神经网络输出数量</param>
        public Genome(int inputL,int outputL)
        {
            baoluizhi = inputL + outputL;
            nodeGemones = new NodeGemone[baoluizhi];
            for (int i = 0; i < inputL; i++)
            {
                nodeGemones[i] = new NodeGemone();
                nodeGemones[i].b = 0;
                nodeGemones[i].ID = i;
                nodeGemones[i].Type = NodeType.Input;
            }
            for (int i = inputL; i < outputL + inputL; i++)
            {
                nodeGemones[i] = new NodeGemone();
                nodeGemones[i].b = 0;
                nodeGemones[i].ID = i;
                nodeGemones[i].Type = NodeType.Output;
            }

            LinkGenones = new bool[baoluizhi, baoluizhi];
            LinkWs = new float[baoluizhi, baoluizhi];
            for (int inp = 0; inp < baoluizhi; inp++)
            {
                for (int outp = 0; outp < baoluizhi; outp++)
                {
                    if (nodeGemones[inp].Type == NodeType.Input &&
                        nodeGemones[outp].Type == NodeType.Output)
                        LinkGenones[inp, outp] = true;
                }
            }

            Random r = new Random();

            LinkWs = new float[baoluizhi, baoluizhi];
            for (int i = 0; i < baoluizhi; i++)
            {
                for (int j = 0; j < baoluizhi; j++)
                {
                    LinkWs[i, j] = (float)(r.NextDouble() * 2);
                }
            }
        }

        /// <summary>
        /// 与另一个基因交叉配对
        /// </summary>
        /// <param name="g">另一个基因</param>
        /// <param name="Qingdd">基因保留度</param>        
        /// /// <returns></returns>
        public Genome Combination(Genome g,float Qingdd=0.5f)
        {
            Genome child = (Genome)MemberwiseClone();
            Random r = new Random();

            for (int i = 0; i < g.nodeGemones.Length; i++)
            {
                if(r.NextDouble() < Qingdd)
                {
                    child.nodeGemones[i] = g.nodeGemones[i];
                }
            }
            for (int inp = 0; inp < g.nodeGemones.Length; inp++)
            {
                for (int oup = 0; oup < g.nodeGemones.Length; oup++)
                {
                    bool g1 = g.LinkGenones[inp, oup];
                    bool g2 = child.LinkGenones[inp, oup];

                    if (g1)
                    {
                        if (!g2)
                        {
                            child.LinkGenones[inp, oup] = g1;
                            child.LinkWs[inp, oup] = g.LinkWs[inp, oup];
                        }
                        else if(r.NextDouble() < Qingdd)
                        {
                            child.LinkGenones[inp, oup] = g1;
                            child.LinkWs[inp, oup] = g.LinkWs[inp, oup];
                        }
                    }
                }
            }
            
            return child;
        }
            
        /// <summary>
        /// 变异
        /// </summary>
        /// <param name="w">变异率</param>
        public void Variation(float w=0.1f)
        {
            int num = Length;
            Random r = new Random();
            for (int inp = 0; inp < num; inp++)
            {
                for (int oup = 0; oup < num; oup++)
                {
                    if (r.NextDouble() < w)
                    {
                        if (r.NextDouble() < 0.5f)
                        {
                            //节点变异
                            bool[,] newConfig = new bool[Length + 1, Length + 1];
                            newConfig.LoadArray(LinkGenones);
                            int from;
                            do
                                from = r.Next(Length);
                            while (nodeGemones[from].Type != NodeType.Output);

                            List<int> indexs = new List<int>();
                            for (int i = 0; i < Length; i++)
                                if (LinkGenones[from, i])
                                    indexs.Add(i);
                            int index = indexs.ToArray().RandomChoice();

                            var l = nodeGemones.ToList();
                            l.Add(new NodeGemone());
                            nodeGemones = l.ToArray();

                            newConfig[from, index] = false;
                            newConfig[from, Length] = true;
                            newConfig[Length, index] = true;
                            LinkGenones = newConfig;
                        }
                        else
                        {
                            //连接变异
                            bool[,] newConfig = (bool[,])LinkGenones.Clone();
                            int to;
                            int testnum = 0;
                            do
                            {
                                testnum++;
                                int from = r.Next(Length);
                                to = r.Next(Length);
                                newConfig[from, to] = true;

                            } while (newConfig.CheakLoop(to, to) && testnum < 10);
                            LinkGenones = newConfig;
                        }

                        float mean_w = LinkWs.Mean();
                        int input = r.Next(Length);
                        List<int> inds = new List<int>();
                        for (int i = 0; i < Length; i++)
                            if (LinkGenones[input, i])
                                inds.Add(i);
                        int ind = inds.ToArray().RandomChoice();
                        LinkWs[input, ind] = ClassExtend.NormalDistribution(mean_w, 1);
                        nodeGemones[input].b = ClassExtend.NormalDistribution(mean_w, 1);

                    }
                }
            }

        }
        
        private bool isExistenceNode(int id)
        {
            foreach (var g in nodeGemones)
            {
                if (g.ID == id)
                    return true;
            }
            return false;
        }
    }


    /// <summary>
    /// 节点基因结构
    /// </summary>
    public struct NodeGemone
    {
        /// <summary>
        /// 偏置
        /// </summary>
        public float b;
        /// <summary>
        /// 节点类型
        /// </summary>
        public NodeType Type;
        /// <summary>
        /// 节点ID
        /// </summary>
        public int ID;
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// 输入节点
        /// </summary>
        Input,
        /// <summary>
        /// 隐藏节点
        /// </summary>
        Hide,
        /// <summary>
        /// 输出节点
        /// </summary>
        Output
    }


}
