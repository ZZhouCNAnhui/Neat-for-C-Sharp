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
    }
}
