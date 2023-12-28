namespace LINQ_TO_SQL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq.Mapping;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table(Name = "Countries")]
    public class Country
    {
        public Country(string name_country, string name_capital, int number, float area, string part)
        {
            this.NameCountry = name_country;
            this.NameCapital = name_capital;
            this.Number = number;
            this.Area = area;
            this.Part = part;
        }

        [Column(IsPrimaryKey = true, Name = "id")]
        public int Id { get; set; }

        [Column(Name = "name_country")]
        public string NameCountry { get; set; }

        [Column(Name = "name_capital")]
        public string NameCapital { get; set; }

        [Column(Name = "number")]
        public int Number { get; set; }

        [Column(Name = "area")]
        public float Area { get; set; }

        [Column(Name = "part")]
        public string Part { get; set; }
    }
}
