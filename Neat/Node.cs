using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    public class Node
    {
        private float bias;
        public NodeType Type;
        public int ID;
        private Network network;

        public Node(NodeGemone g,Network network)
        {
            this.network = network;
            bias = g.b;
            this.Type = g.Type;
            ID = g.ID;
        }


        public float Activation(float input, Node Outn,bool isOut = false)
        {

            if (isOut)
            {
                return input + bias;
            }

            float w = 0;
            for (int i = 0; i < network.NodeNum; i++)
            {
                if (network.NodeLinkConfig[ID, Outn.ID].isActivation)
                    w = network.NodeLinkConfig[ID, Outn.ID].linkW;
            }
            return input * w;


        }

        public float Activation(float[] inputs,Node out_n)
        {
            if(Type != NodeType.Input)
            {
                Node[] n_s = this.QueryLinkNodes();
                float[] in_s = new float[n_s.Length];
                for (int i = 0; i < n_s.Length; i++)
                {
                    in_s[i] = Activation(n_s[i].Activation(inputs, this), out_n, Type == NodeType.Output);
                }
                return in_s.Sum() + bias;
            }
            else
            {
                for (int i = 0; i < network.InNodes.Count; i++)
                {
                    //Console.WriteLine(network.InNodes[i].ID+"\t"+ ID);
                    if (network.InNodes[i].ID == ID)
                        return Activation(inputs[i], out_n);
                }
                
                return 0;
            }
        }

        public Node[] QueryLinkNodes()
        {
            List<Node> l = new List<Node>();
            for (int i = 0; i < network.nodes.Length; i++)
            {
                if (network.NodeLinkConfig[i, ID].isActivation)
                {
                    l.Add(network.nodes[i]);
                }
            }
            return l.ToArray();
        }
        
    }

}
