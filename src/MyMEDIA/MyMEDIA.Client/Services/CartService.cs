using MyMEDIA.Shared.Entities;

namespace MyMEDIA.Client.Services;

public class CartService
{
    public List<OrderItem> Items { get; private set; } = new();
    public event Action? OnChange;

    public void AddToCart(Product product)
    {
        var existing = Items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = 1,
                UnitPrice = product.FinalPrice
            });
        }
        NotifyStateChanged();
    }

    public decimal Total() => Items.Sum(i => i.UnitPrice * i.Quantity);

    public void Clear()
    {
        Items.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
