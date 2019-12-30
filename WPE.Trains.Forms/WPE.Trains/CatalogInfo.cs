using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public class CatalogInfo : IComparable<CatalogInfo>
    {
        public string CatalogList { get; set; }
        public string Identifier { get; set; }
        public string Manufacturer { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Language { get; set; }

        public int CompareTo(CatalogInfo other)
        {
            int yearCompare = this.GetStartYear().CompareTo(other.GetStartYear());
            if (yearCompare == 0)
            {
                return this.Year.Length.CompareTo(other.Year.Length);
            }
            return yearCompare;
        }

        public int GetStartYear()
        {
            var prefix = string.Concat(Year.TakeWhile(c => !char.IsDigit(c)));
            string yearString = Year.Substring(prefix.Length);
            yearString = string.Concat(yearString.TakeWhile(c => char.IsDigit(c)));
            return int.Parse(yearString);
        }

        public override string ToString()
        {
            return $"{Description} ({Year})";
        }
    }
}
