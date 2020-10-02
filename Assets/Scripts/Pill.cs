using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverdoseTheGame
{
    /// <summary>
    /// İlacı temsil eden sınıftır.
    /// </summary>
    public class Pill
    {
        /// <summary>
        /// İlacın adı
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// İlacın açıklaması
        /// </summary>
        public string Description { get; private set; } //todo: içerik üretimi sırasında ayarlanacak

        //henüz kullanılmıyor.
        //todo: feature x term boyunca y etkisinin sağlanması
        public IEnumerable<Tuple<DiseaseType, double>> SpecialEffects { get; private set; }

        /// <summary>
        /// the effect of the pill to status bars
        /// </summary>
        public IEnumerable<Tuple<DiseaseType, double>> Effects { get; private set; }

        public Pill(string name, string description, List<Tuple<DiseaseType, double>> effect)
        {
            Name = name;
            Description = description;
            Effects = effect;
        }

        public override string ToString()
        {
            return string.Join(", ", Effects.Select(x => $"{x.Item1} = {x.Item2}"));
        }
    }
}
