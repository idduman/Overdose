using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverdoseTheGame
{
    /// <summary>
    /// Her term başında DiseaseType alıp belirlenen count kadar pill üretecek sınıftır.,
    /// DiseaseType, hastalıklardır.
    /// </summary>
    public class PillGenerator
    {
        public Random Random = new Random();
        
        /// <summary>
        /// ilaçları üreten metoddur
        /// </summary>
        /// <param name="diseaseTypes">hastalıklardır</param>
        /// <param name="range">ilacın etkisinin aralığıdır max ve min değer tutar</param>
        /// <param name="pillCount">kaç tane ilaç üretileceği bilgisidir</param>
        /// <returns></returns>
        public IEnumerable<Pill> Generate(IEnumerable<DiseaseType> diseaseTypes, PointRange range, int pillCount)
        {
            var pills = new Pill[pillCount];
            var pillSum = new double[pillCount];
            var diseaseCount = diseaseTypes.Count();

            for (int i = 0; i < pillCount; i++)
            {
                pillSum[i] = Random.Next((int)range.Min, (int)range.Max);
                var effects = new List<Tuple<DiseaseType, double>>(diseaseCount);

                for (int j = 0; j < diseaseCount; j++)
                {
                    var x = pillSum[i] / (diseaseCount - j);
                    var r = new PointRange(range);
                    if (x > 0)
                    {
                        r.Min += x * 2;
                    }
                    else if (x < 0)
                    {
                        r.Max += x * 2;
                    }

                    effects.Add(new Tuple<DiseaseType, double>(diseaseTypes.ElementAt(j), Random.Next((int)r.Min, (int)r.Max)));

                    pillSum[i] -= effects[j].Item2;
                }

                pills[i] = new Pill("", "", effects);
            }

            return pills;
        }
    }
}
