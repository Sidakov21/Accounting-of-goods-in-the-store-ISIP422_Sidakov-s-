using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

// Библиотека CommunityToolkit.Mvvm для удобных команд
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfApp5
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<string> Categories { get; set; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                // Обновляем состояние команд, которые зависят от выбранного товара
                (DeleteProductCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (SupplyProductCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (SellProductCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterProducts();
            }
        }

        private string _selectedSearchCategory;
        public string SelectedSearchCategory
        {
            get => _selectedSearchCategory;
            set
            {
                _selectedSearchCategory = value;
                OnPropertyChanged(nameof(SelectedSearchCategory));
                FilterProducts();
            }
        }

        private ObservableCollection<Product> _filteredProducts;

        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));
            }
        }

        // Команды
        public ICommand AddProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand SupplyProductCommand { get; }
        public ICommand SellProductCommand { get; }

        public MainViewModel()
        {
            // Инициализация категорий
            Categories = new ObservableCollection<string> { "Электроника", "Одежда", "Продукты", "Книги", "Все" };
            SelectedSearchCategory = "Все";

            // Инициализация тестовых данных
            Products = new ObservableCollection<Product>
        {
            new Product("Смартфон", 29999.99m, 10, "Электроника"),
            new Product("Футболка", 1499.50m, 25, "Одежда"),
            new Product("Яблоки", 89.90m, 100, "Продукты"),
            new Product("Война и мир", 599m, 5, "Книги"),
            new Product("Наушники", 4999m, 0, "Электроника") // Товар с нулевым остатком
        };
            FilteredProducts = new ObservableCollection<Product>(Products);

            // Инициализация команд
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanModifyProduct);
            SupplyProductCommand = new RelayCommand(SupplyProduct, CanModifyProduct);
            SellProductCommand = new RelayCommand(SellProduct, CanSellProduct);
        }

        private void AddProduct()
        {
            var newProduct = new Product();
            Products.Add(newProduct);
            FilterProducts(); // Обновляем отфильтрованный список
            SelectedProduct = newProduct; // Выделяем новый товар
        }

        private void DeleteProduct()
        {
            if (SelectedProduct != null)
            {
                Products.Remove(SelectedProduct);
                FilterProducts(); // Обновляем отфильтрованный список
            }
        }

        private void SupplyProduct()
        {
            // Просто увеличиваем количество на 10. Можно сделать окно для ввода количества.
            if (SelectedProduct != null)
            {
                SelectedProduct.Quantity += 10;
                FilterProducts(); // Обновляем отфильтрованный список, если фильтр по наличию
            }
        }

        private void SellProduct()
        {
            if (SelectedProduct != null && SelectedProduct.Quantity > 0)
            {
                SelectedProduct.Quantity -= 1;
                FilterProducts(); // Обновляем отфильтрованный список, если товар закончился
            }
        }

        private bool CanModifyProduct() => SelectedProduct != null;
        private bool CanSellProduct() => SelectedProduct != null && SelectedProduct.Quantity > 0;

        private void FilterProducts()
        {
            var query = Products.AsEnumerable();

            // Фильтр по тексту (название или код)
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p => p.Name.ToLower().Contains(SearchText.ToLower()) || p.Code.Contains(SearchText));
            }

            // Фильтр по категории
            if (SelectedSearchCategory != "Все")
            {
                query = query.Where(p => p.Category == SelectedSearchCategory);
            }

            // ВАЖНО: Проверка на null перед созданием коллекции
            FilteredProducts = new ObservableCollection<Product>(query ?? Enumerable.Empty<Product>());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
