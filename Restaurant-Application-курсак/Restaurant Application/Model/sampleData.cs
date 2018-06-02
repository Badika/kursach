using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    public class sampleData : DropCreateDatabaseAlways<RestaurantDB>
    {
        enum bookingstatus { Available, Reserved, Booked }
        protected override void Seed(RestaurantDB context)
        {
            try
            {
                List<FoodItems> fooditems = new List<FoodItems>()
                {
                    new FoodItems { FoodName = "Суші", Description = "Авокадо, сир, рис, васабі, імбир", fPrice = 25 },
                    new FoodItems { FoodName = "Борщ", Description = "морква, буряк, вода", fPrice = 25 },
                    new FoodItems { FoodName = "Морозиво", Description = "молоко, цукор", fPrice = 18 },
                    new FoodItems { FoodName = "Канапки", Description = "хліб, масло, сир", fPrice = 12 },
                    new FoodItems { FoodName = "Шаурма", Description = "мясо", fPrice = 5 }
                };
                foreach(FoodItems f in fooditems)
                {
                    context.FoodItems.Add(f);
                }
                List<TableList> tablelist = new List<TableList>()
                {
                    new TableList { TableName = "Столик 1", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 2", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 3", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 4", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 5", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 6", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 7", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 8", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 9", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Столик 10", BookingStatus = bookingstatus.Available.ToString() }
                };
                foreach (TableList t in tablelist)
                {
                    context.TableList.Add(t);
                }
                context.SaveChanges();
                base.Seed(context);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
