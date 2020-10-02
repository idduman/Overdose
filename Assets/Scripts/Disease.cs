using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverdoseTheGame
{
    /// <summary>
    /// Oyunun üstündeki hastalık barların her birini ifade eden sınıftır.
    /// </summary>
    public class Disease
    {
        /// <summary>
        /// Hastalık tipidir. Karaciğer hastalığı gibi..
        /// </summary>
        public DiseaseType Type { get; set; }

        /// <summary>
        /// Hastalığın güncel değeridir.
        /// </summary>
        private double _point;
        public double Point
        {
            get
            {
                return _point;
            }
            set
            {
                if (Point == value) return;
                _point = value;
                if (Point <= HealthyThreshold)
                {
                    Status = DiseaseStatus.Healty;
                }
                else if (Point >= 100)
                {
                    Status = DiseaseStatus.Overdose;
                }
                else
                {
                    Status = DiseaseStatus.Normal;
                }
            }
        }

        /// <summary>
        /// hastalığın olabileceği aralık değerini tutan sınıftır. Max ve Min olmak üzere iki değer tutar
        /// </summary>
        public PointRange Range { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double HealthyThreshold { get; set; }

        /// <summary>
        /// Barın/hastalığın durumudur. Normal, healty ve overdose olabilir.
        /// </summary>
        public DiseaseStatus Status { get; set; }
    }
}
