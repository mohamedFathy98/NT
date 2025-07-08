using OrderTask.Models;

namespace OrderTask.Services.IServices
{
    public interface ICartService
    {
        Cart GetCart();
        Task AddToCart(int productId, int quantity = 1);
        void RemoveFromCart(int productId);
        void UpdateQuantity(int productId, int quantity);
        void ClearCart();
        int GetCartItemCount();
    }
}
