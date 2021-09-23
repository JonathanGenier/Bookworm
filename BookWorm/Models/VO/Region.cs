using System;

namespace BookWorm.Models.VO
{
    public class Region
    {
        private int _id;
        private string _name;
        private double _tax;

        public Region() { }

        public Region(int id, string name, double tax)
        {
            Id = id;
            Name = name;
            Tax = tax;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public double Tax { get => _tax; set => _tax = value; }

        public override string ToString()
        {
            return String.Format("ID: {0}, Name: {1}, Tax: {2}", Id, Name, (Tax*100) + "%");
        }
    }
}
