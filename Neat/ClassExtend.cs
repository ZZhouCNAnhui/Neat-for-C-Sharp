using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    static public class ClassExtend
    {
        static public T[] RandomChoices<T>(this T[] ts,int num)
        {
            Random random = new Random();
            int Max = ts.Count();
            T[] result = new T[num];
            for (int i = 0; i < num; i++)
            {
                result[i] = ts[random.Next(Max)];
            }
            return result;
        }
        static public T RandomChoice<T>(this T[] ts)
        {
            Random random = new Random();
            int Max = ts.Count();
            return ts[random.Next(Max)];
        }

        static public int Toint(this bool b)
        {
            if (b)
                return 1;
            else
                return 0;
        }
        static public int[] Toints(this bool[] bs)
        {
            int[] r = new int[bs.Length];
            for (int i = 0; i < r.Length; i++)
                r[i] = bs[i].Toint();
            return r;
        }
        static public float[] Toflaots(this bool[] bs)
        {
            float[] r = new float[bs.Length];
            for (int i = 0; i < r.Length; i++)
                r[i] = bs[i].Toint();
            return r;
        }
        static public bool Tobool(this float f)
        {
            float v = f - (int)Math.Truncate(f);
            int r = (int)Math.Round(v);
            if (r == 1)
                return true;
            else
                return false;
        }

        static public bool CheakLoop(this bool[,] config, int id,int bascId)
        {
            List<bool> req = new List<bool>();
            for (int i = 0; i < config.GetLength(0); i++)
            {
                if (config[i, id])
                {
                    if (i == bascId)
                        return true;
                    else
                        req.Add(CheakLoop(config, i, bascId));
                    
                }
            }

            foreach (var b in req)
                if (b)
                    return true;
            return false;
        }
        
        static public float Mean(this float[,] array)
        {
            float sun = 0;
            int num = array.GetLength(0) * array.GetLength(1);
            foreach (var item in array)
            {
                sun += item;
            }
            return sun / num;
        }
        static public int Maxid(this float[] a)
        {
            int id = 0;
            float maxv = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if(maxv < a[i])
                {
                    id = i;
                    maxv = a[i];
                }
            }
            return id;
        }
        /// <summary>
        /// 输入数组必须小于本数组（任何维度）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="a"></param>
        static public void LoadArray(this bool[,] array,bool[,] a)
        {
            for (int i = 0; i < array.GetLongLength(0); i++)
            {
                for (int j = 0; j < array.GetLongLength(1); j++)
                {
                    array[i, j] = a[i, j];
                }
            }
        }

        /// <summary>
        /// N(0,1)正态分布 
        /// </summary>
        /// <returns></returns>
        static public double[] NormalDistribution()
        {
            Random rand = new Random();
            double[] y;
            double u1, u2, v1 = 0, v2 = 0, s = 0, z1 = 0, z2 = 0;
            while (s > 1 || s == 0)
            {
                u1 = rand.NextDouble();
                u2 = rand.NextDouble();
                v1 = 2 * u1 - 1;
                v2 = 2 * u2 - 1;
                s = v1 * v1 + v2 * v2;
            }
            z1 = Math.Sqrt(-2 * Math.Log(s) / s) * v1;
            z2 = Math.Sqrt(-2 * Math.Log(s) / s) * v2;
            y = new double[] { z1, z2 };
            return y; //返回两个服从正态分布N(0,1)的随机数z0 和 z1
        }
        /// <summary>
        /// 正态分布 
        /// </summary>
        /// <param name="mean">均值</param>
        /// <param name="standardDeviation">标准差</param>
        /// <returns></returns>
        static public double NormalDistribution(double mean,double standardDeviation)
        {
            var r = NormalDistribution()[0];
            return mean + r * standardDeviation;
        }
        /// <summary>
        /// 正态分布 
        /// </summary>
        /// <param name="mean">均值</param>
        /// <param name="standardDeviation">标准差</param>
        /// <returns></returns>
        static public float NormalDistribution(float mean, float standardDeviation)
        {
            float r = (float)NormalDistribution()[0];
            return mean + r * standardDeviation;
        }

    }
}
