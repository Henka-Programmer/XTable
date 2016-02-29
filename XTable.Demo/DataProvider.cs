using System.Collections.Generic;
using System.Collections.ObjectModel;
using XTable.Demo.Models;

namespace XTable.Demo
{
    public static class DataProvider
    {
        public static ObservableCollection<Laptop> GetLaptops()
        {
            ObservableCollection<Laptop> laps = new ObservableCollection<Laptop>();

            laps.Add(new Laptop() { Manufacture = "DELL", Name = "Inspiron 1525", CPU = "T7250", RAM = 2048, Price = 65400 });
            laps.Add(new Laptop() { Manufacture = "DELL", Name = "Inspiron 1525", CPU = "T5750", RAM = 2048, Price = 69000 });
            laps.Add(new Laptop() { Manufacture = "DELL", Name = "Studio 1535", CPU = "T5750", RAM = 2048, Price = 73500 });
            laps.Add(new Laptop() { Manufacture = "DELL", Name = "Vostro 1510", CPU = "T5870", RAM = 2048, Price = 72400 });

            laps.Add(new Laptop() { Manufacture = "HP", Name = "530", CPU = "T5200", RAM = 1024, Price = 54500 });
            laps.Add(new Laptop() { Manufacture = "HP", Name = "6720s", CPU = "T5670", RAM = 1024, Price = 63700 });
            laps.Add(new Laptop() { Manufacture = "HP", Name = "Pavilion dv9233", CPU = "T5670", RAM = 1024, Price = 78000 });
            return laps;
        }
    }
}