using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace LuzanTest.Tables
{
    public partial class People
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }
        public override string ToString()
        {
            return string.Join(" ", new List<string> { Name, Surname, Patronymic, BirthDate.ToString("dd.MM.yyyy"), Gender, (DateTime.Now.Year - BirthDate.Year).ToString() });
        }
        public override bool Equals(object obj)
        {
            if(obj.GetType() != typeof(People)) return false;
            People o = (People)obj;
            if(
                o.Name == Name &&
                o.Surname == Surname &&
                o.Patronymic== Patronymic &&
                o.BirthDate == BirthDate &&
                o.Gender == Gender
                ) return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = -348523623;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Surname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Patronymic);
            hashCode = hashCode * -1521134295 + BirthDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Gender);
            return hashCode;
        }
    }
}
