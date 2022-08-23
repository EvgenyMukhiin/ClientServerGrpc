using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcServer.Models
{
    public class Rectangle
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Route { get; set; }
    }
    public class RandomPoint : Rectangle
    {
        public RandomPoint()
        {
            Random rnd = new Random();
            int value1 = rnd.Next(0, 550);
            int value2 = rnd.Next(0, 380);
            X = value1;
            Y = value2;
            Route = "+";
        }
    }
    public class CollectionPoint : IEnumerable
    {
        Random rnd = new Random();
        public List<RandomPoint> items = new List<RandomPoint>();
        public async Task CollectionPointStart(int sizeMin, int sizeMax)
        {
            await Task.Run(() =>
            {
                int size = rnd.Next(sizeMin, sizeMax);
                for (int i = sizeMin; i < size; i++)
                {
                    RandomPoint point = new RandomPoint() { Id = i };
                    items.Add(point);
                }
            });
        }
        public void UpdatePoint()
        {
            //шаг
            int step = rnd.Next(1,4);
            //рамки поля
            const int w = 550;
            const int h = 380;
            //стартовые значения
            foreach (var item in items)
            {
                int startX = item.X;
                int startY = item.Y;
                try
                {
                    if (item.Route == "+")
                    {
                        if (startX >= w)
                        {
                            item.X = startX - step;
                            item.Route = "-";
                        }
                        else
                            item.X = startX + step;
                    }
                    if (item.Route == "-")
                    {
                        if (startX <= 0)
                        {
                            item.X = startX + step;
                            item.Route = "+";
                        }
                        else
                            item.X = startX - step;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        public IEnumerator GetEnumerator() => items.GetEnumerator();
    }
}
