using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Restaurant_Application.Model;
using System.Data;
using System.Collections.ObjectModel;

namespace Restaurant_Application.DB_Layer
{
    public class DataAccessLayer
    {
        private readonly RestaurantDB _rDBContext;
        enum Orderstatus { InProgress, OrderDelivered, Closed, Cancelled };
        enum bookingstatus { Booked, Reserved, Available };

        public DataAccessLayer()
        {
            _rDBContext = new RestaurantDB();
        }

        public ICollection<FoodItems> GetFoodItems()
        {
            return _rDBContext.FoodItems.OrderBy(p => p.FoodID).ToArray();
        }

        public List<TableList> getTableList()
        {
            return _rDBContext.TableList.ToList();
        }

        public int InsertNewFoodItem(FoodItems newfoodItem)
        {
            var res = _rDBContext.FoodItems.Add(newfoodItem);
            _rDBContext.SaveChanges();
            return res.FoodID;
        }

        public void UpdateFoodDetails(FoodItems fooditem)
        {
            var entity = _rDBContext.FoodItems.Find(fooditem.FoodID);

            if (entity == null)
            {
                throw new NotImplementedException("Handle appropriately for your API design.");
            }

            _rDBContext.Entry(fooditem).State = System.Data.Entity.EntityState.Modified;
            _rDBContext.SaveChanges();
        }

        public void UpdateOrderDetails(ViewOrderItems fooditem)
        {
            var returnedOrder = _rDBContext.FoodOrders.First(x => x.OrderID == fooditem.OrderID);
            var foodprice = returnedOrder.Price / returnedOrder.Quantity;
            returnedOrder.Quantity = fooditem.Quantity;
            returnedOrder.Price = fooditem.Quantity * foodprice;
            var res = _rDBContext.SaveChanges();
        }

        public void DeleteFoodDetails(FoodItems foodItem)
        {
            _rDBContext.FoodItems.Remove(foodItem);
            _rDBContext.SaveChanges();
        }

        public List<TableList> getTableListToPlaceOrder()
        {
            return _rDBContext.TableList.ToList().Where(p => p.BookingStatus != "Booked").ToList();
        }

        public List<FoodOrders> getOrderItems(Orders orderObj)
        {
            Orders order = new Orders();
            var data = from o in _rDBContext.Orders
                       join t in _rDBContext.TableList on o.TableID equals t.TableID
                       where t.TableID == orderObj.TableID && t.BookingStatus == "Booked"
                       select new { o.OrderID };
            foreach (var a in data)
            {
                order.OrderID = a.OrderID;
            }

            return _rDBContext.FoodOrders.ToList().Where(p => p.OrderID == order.OrderID).ToList();
        }

        public int InsertOrder(List<ViewOrderItems> obj)
        {
            foreach (var items in obj)
            {
                _rDBContext.FoodOrders.Add(new FoodOrders
                {
                    OrderID = items.OrderID,
                    Price = items.Price,
                    Quantity = items.Quantity,
                    FoodID = items.FoodID,
                });
            }

            return _rDBContext.SaveChanges();
        }

        public FoodItems getFoodDetails(int FoodID)
        {
            return _rDBContext.FoodItems.FirstOrDefault(p => p.FoodID == FoodID);
        }

        public void DeleteOrderItem(ViewOrderItems order)
        {
            _rDBContext.Orders.Remove(new Orders { OrderID = order.OrderID });
            _rDBContext.SaveChanges();
        }

        public ViewOrderItems PlaceOrder(ViewOrderItems obj)
        {
            var createdOrder = _rDBContext.Orders.Add(new Orders
            {
                TableID = obj.TableID,
                CreatedDate = DateTime.UtcNow,
                OrderStatus = Orderstatus.InProgress.ToString(),
                TotalPrice = obj.Price
            });

            var resultFromDb = _rDBContext.SaveChanges();
            obj.OrderID = createdOrder.OrderID;
            return obj;
        }

        public ObservableCollection<ViewOrderItems> getFoodOrderDetails(TableList selectedtable)
        {
            ObservableCollection<ViewOrderItems> orderItems = new ObservableCollection<ViewOrderItems>();

            var foodOrders = _rDBContext.FoodOrders.Where(x => x.Orders.TableID == selectedtable.TableID).ToList();

            foreach (var item in foodOrders)
            {
                orderItems.Add(new ViewOrderItems { FoodID = item.FoodID, OrderID = item.OrderID, BookingStatus = item.Orders.OrderStatus, FoodName = item.fooditems.FoodName, OrderCreatedDate = item.Orders.CreatedDate, Price = item.Price, Quantity = item.Quantity, TableID = selectedtable.TableID, TableName = selectedtable.TableName });
            }

            return orderItems;
        }

        public void UpdateTableStatus(TableList t)
        {
            var tableList = _rDBContext.TableList.First(x => x.TableID == t.TableID);
            tableList.BookingStatus = t.BookingStatus;
            _rDBContext.SaveChanges();
        }
    }
}
