using Restaurant_Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Restaurant_Application.DB_Layer;
using Restaurant_Application.ViewModel;

namespace Restaurant_Application.Page_Screens
{
    /// <summary>
    /// Interaction logic for AddFoodItems.xaml
    /// </summary>
    public partial class AddFoodItem : Page
    {
        private RestaurantViewModel _rVmObj;
        public AddFoodItem()
        {
            InitializeComponent();
            this.WindowHeight = 450;
            this.WindowWidth = 600;
            DataContext = new RestaurantViewModel();
        }

        public void Clear_Click(object sender, RoutedEventArgs e)
        {
            foodnametxt.Clear();
            Descriptiontxt.Clear();
            pricetxt.Clear();
        }

        public void AddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (foodnametxt.Text == "" || Descriptiontxt.Text == "" || pricetxt.Text == "")
                {
                    status.Foreground = Brushes.Red;
                    status.Content = "Усі поля обов'язкові.";
                }
                else
                {
                    _rVmObj = new RestaurantViewModel();
                    FoodItems fooditem = new FoodItems();
                    fooditem.FoodName = foodnametxt.Text;
                    fooditem.Description = Descriptiontxt.Text;
                    fooditem.fPrice = Convert.ToInt32(pricetxt.Text.ToString());
                    _rVmObj.AddFoodItem(fooditem);
                    DataContext = new RestaurantViewModel();
                    status.Foreground = Brushes.Green;
                    status.Content = "Страва успішно додана.";
                }
            }
            catch (Exception exp)
            {
                status.Foreground = Brushes.Red;
                status.Content = "Введіть значення.";
            }

        }

    }
}
