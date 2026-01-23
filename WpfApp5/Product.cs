using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    internal class Product : INotifyPropertyChanged
    {
        private static int _nextId = 1000001; // Начинаем с 1000001, чтобы код был 6-значным и начинался с 1
        private string _name;
        private decimal _price;
        private int _quantity;
        private string _category;

        public string Code { get; } // Только для чтения, задается при создании

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(IsAvailable)); // При изменении количества обновляем доступность
            }
        }

        // Вычисляемое свойство: товар в наличии, если Quantity > 0
        public bool IsAvailable => Quantity > 0;

        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(nameof(Category)); }
        }

        public Product(string name, decimal price, int quantity, string category)
        {
            Code = _nextId.ToString();
            _nextId++; // Увеличиваем ID для следующего товара

            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        // Конструктор по умолчанию для кнопки "Добавить"
        public Product() : this("Новый товар", 0, 0, "Электроника")
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
