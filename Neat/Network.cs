using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    /// <summary>
    /// 可运行的神经网络
    /// </summary>
    public class Network
    {
        /// <summary>
        /// 节点连接参数表
        /// </summary>
        public LinkConfig[,] NodeLinkConfig;
        /// <summary>
        /// 节点列表
        /// </summary>
        public Node[] nodes;
        /// <summary>
        /// 节点数量
        /// </summary>
        public int NodeNum;
        /// <summary>
        /// 输出节点
        /// </summary>
        public List<Node> OutNodes;
        /// <summary>
        /// 输入节点
        /// </summary>
        public List<Node> InNodes;

        /// <summary>
        /// 由神经网络基因生成可运行的神经网络
        /// </summary>
        /// <param name="genome">神经网络基因</param>
        public Network(Genome genome)
        {
            NodeNum = genome.nodeGemones.Length;
            NodeLinkConfig = new LinkConfig[NodeNum, NodeNum];
            for (int y = 0; y < NodeNum; y++)
            {
                for (int x = 0; x < NodeNum; x++)
                {
                    NodeLinkConfig[x, y].isActivation = genome.LinkGenones[x, y];
                    NodeLinkConfig[x, y].linkW = genome.LinkWs[x, y];
                }
            }

            nodes = new Node[genome.nodeGemones.Length];
            OutNodes = new List<Node>();
            InNodes = new List<Node>();
            for (int i = 0; i < nodes.Length; i++)
            {
                if (genome.nodeGemones[i].Type == NodeType.Output)
                    OutNodes.Add(new Node(genome.nodeGemones[i], this));
                else if (genome.nodeGemones[i].Type == NodeType.Input)
                    InNodes.Add(new Node(genome.nodeGemones[i], this));
                nodes[i] = new Node(genome.nodeGemones[i], this);
            }

        }

        /// <summary>
        /// 运行神经网络
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>输入</returns>
        public float[] Activation(float[] input)
        {
            float[] l = new float[OutNodes.Count];
            for (int i = 0; i < OutNodes.Count; i++)
            {
                l[i] = OutNodes[i].Activation(input, null);
            }
            return l;
        }

    }
    /// <summary>
    /// 节点连接参数结构
    /// </summary>
    public struct LinkConfig
    {
        /// <summary>
        /// 连接是否被激活
        /// </summary>
        public bool isActivation;
        /// <summary>
        /// 连接权重
        /// </summary>
        public float linkW;
    }

}
