using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Linq.Mapping;
using 
namespace Model
{
    [Table(Name = "Countries")]
    public class Country
    {
        [Column(IsPrimaryKey = true, Name = "id")]
        public int Id { get; set; }

        [Column(Name = "name_country")]
        public string name_country { get; set; }

        [Column(Name = "name_capital")]
        public string name_capital { get; set; }

        [Column(Name = "number")]
        public int number { get; set; }

        [Column(Name = "area")]
        public float area { get; set; }

        [Column(Name = "part")]
        public string part { get; set; }

        public Country(string name_country, string name_capital, int number, float area, string part)
        {
            this.name_country = name_country;
            this.name_capital = name_capital;
            this.number = number;
            this.area = area;
            this.part = part;
        }
    }
}
