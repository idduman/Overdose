using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverdoseTheGame
{
    /// <summary>
    /// XML dosyasını okumak için oluşturulan sınıftır
    /// </summary>
    public class LevelConfiguration
    {
        public int LevelNumber { get; set; }

        public int PillCount { get; set; }

        public int[] Range { get; set; }

        public int HealthyThreshold { get; set; }

        public List<DiseaseConfiguration> Diseases { get; set; }
    }

    public class DiseaseConfiguration
    {
        public string Type { get; set; }

        public int[] Range { get; set; }

        public int InitialPoint { get; set; }
    }
}
