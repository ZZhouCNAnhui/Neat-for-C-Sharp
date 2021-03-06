﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    public class Population
    {
        public delegate float FitnessFunction(Genome genome);
        

        public Genome[] genomes;

        /// <summary>
        /// 生成种群
        /// </summary>
        /// <param name="input">种群神经网络输入</param>
        /// <param name="Output">种群神经网络输出</param>
        /// <param name="num">种群数量</param>
        public Population(int input,int Output,int num)
        {
            genomes = new Genome[num];
            for (int i = 0; i < genomes.Length; i++)
            {
                genomes[i] = new Genome(input, Output);
            }
        }

        private Genome[] Clear(Genome[] genomes)
        {
            List<Genome> l = new List<Genome>();
            
            foreach (var item in genomes)
            {
                if (item != null)
                    l.Add(item);
            }
            return l.ToArray();
        }

        private void Reproduction()
        {
            Genome[] Rg = Clear(genomes);
            for (int i = 0; i < genomes.Length; i++)
            {
                if(genomes[i] == null)
                {
                    Genome p1 = Rg.RandomChoice();
                    Genome p2 = Rg.RandomChoice();
                    Genome Child = null;
                    if(p1.nodeGemones.Length > p2.nodeGemones.Length)
                        Child = p2.Combination(p1);
                    else
                        Child = p1.Combination(p2);
                    Child.Variation();
                    genomes[i] = Child;
                }

            }
        }

        public Genome Run(FitnessFunction function,int num = 100,float F = 0.6f)
        {
            Genome BestG = null;
            for (int j = 0; j < num; j++)
            {
                Dictionary<Genome, float> pairs = new Dictionary<Genome, float>();
                float mean = 0;
                float maxFitness = 0;
                foreach (var item in genomes)
                {
                    var fl = function(item);
                    pairs.Add(item, fl);
                    mean += fl;
                    if (maxFitness < fl)
                    {
                        maxFitness = fl;
                        BestG = item;
                    }
                }
                mean /= genomes.Length;

                List<int> RomoveList = new List<int>();
                for (int i = 0; i < genomes.Length; i++)
                    if (pairs[genomes[i]] < mean * F)
                        RomoveList.Add(i);

                foreach (var i in RomoveList)
                    genomes[i] = null;

                Reproduction();
            }

            return BestG;

        }

    }
}
