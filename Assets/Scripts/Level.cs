using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverdoseTheGame
{
    /// <summary>
    /// Levele ait özellikleri tutan sınıftır.
    /// Levelde gösterilecek hastalıkları(barları) tutar.
    /// İlaç üretir, ilaçların etkilerini barlara uygular.
    /// </summary>
    public class Level
    {
        public PillGenerator PillGenerator { get; }

        /// <summary>
        /// Level numarası
        /// </summary>
        public int Number { get; private set; }

        public IReadOnlyDictionary<DiseaseType, Disease> Diseases { get; private set; }

        /// <summary>
        /// Levelin durumunu tutar. Overdose, Healty ya da normal
        /// </summary>
        public DiseaseStatus Status
        {
            get
            {
                bool isHealthy = true;
                foreach (var pair in Diseases)
                {
                    if (pair.Value.Status == DiseaseStatus.Overdose)
                    {
                        return DiseaseStatus.Overdose;
                    }
                    else if (pair.Value.Status == DiseaseStatus.Normal)
                    {
                        isHealthy = false;
                    }
                }

                return isHealthy ? DiseaseStatus.Healty : DiseaseStatus.Normal;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return Status == DiseaseStatus.Healty;
            }
        }

        /// <summary>
        /// Oyuncuya gösterilecek ilaç sayısını tutar
        /// </summary>
        public int PillCount { get; private set; }

        /// <summary>
        /// Her termde ilaçların etkisinin maksimum ve minimum aralığını tutar. 
        /// Pill generationda kullanılıyor
        /// </summary>
        public PointRange PointRange { get; private set; }

        public Level()
        {
            PillGenerator = new PillGenerator();
        }

        /// <summary>
        /// XML dosyasını okuyarak level oluşturur
        /// </summary
        public static Level Load(LevelConfiguration levelConf)
        {
            var level = new Level()
            {
                Number = levelConf.LevelNumber,
                PointRange = new PointRange(levelConf.Range[0], levelConf.Range[1]),
                PillCount = levelConf.PillCount
            };

            level.Diseases = new ReadOnlyDictionary<DiseaseType, Disease>
                (levelConf.Diseases.Select(x => new Disease()
                {
                    Point = x.InitialPoint,
                    Range = new PointRange(x.Range[0], x.Range[1]),
                    Type = (DiseaseType)Enum.Parse(typeof(DiseaseType), x.Type)
                }).ToDictionary(x => x.Type, x => x));

            return level;
        }

        /// <summary>
        /// İlaçları üreten sınıftır
        /// </summary>
        public IEnumerable<Pill> GeneratePills()
        {
            return PillGenerator.Generate(Diseases.Keys, PointRange, PillCount);
        }

        /// <summary>
        /// Seçilen ilacın barlara etkisini sağlar.
        /// <returns></returns>
        public DiseaseStatus ApplyEffects(Pill selectedPill)
        {
            foreach (var effect in selectedPill.Effects)
            {
                Disease disease;
                if (Diseases.TryGetValue(effect.Item1, out disease))
                {
                    disease.Point += effect.Item2;
                    if (disease.Point < 0)
                    {
                        disease.Point = 0;
                    }
                }
            }

            return Status;
        }
    }
}
