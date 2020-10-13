using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Products;

namespace Web.Service
{
    public class GenerateData
    {
        public GenerateData()
        {
            PopulateProductsTable();
        }
        public List<Product> ProductList = new List<Product>();

        private static string path = Environment.CurrentDirectory;
        private static string newPath = Path.GetFullPath(Path.Combine(path, @"wwwroot/Commerce.csv"));
        private static string[] myFile = File.ReadAllLines(newPath);
        public void PopulateProductsTable()
        {
            foreach (string line in myFile)
            {
                string[] column = line.Split(",");
                Product product = new Product()
                {
                    Name = column[1],
                    Manufacturer = column[2],
                    Price = Convert.ToDecimal(column[3]),
                    Details = column[4]
                };
                ProductList.Add(product);
            }
        }
    }
}
