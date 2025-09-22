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

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public class product
    {
        private static int _lastProductId = 1;
        private string _name;
        private decimal _price;
        private int _quantity;
        private bool _isAvailable;
        private string _category;

        // Статический список категорий
        public static string[] Categories = new string[]
        {
            "Электроника",
            "Одежда",
            "Продукты",
            "Книги",
            "Спорттовары"
        };

        
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Изменения Git
        }
    }
}
