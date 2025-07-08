using Newtonsoft.Json;
using OrderTask.Models;
using OrderTask.Services.IServices;

namespace OrderTask.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;
        private const string CartSessionKey = "ShoppingCart";

        public CartService(IHttpContextAccessor httpContextAccessor, IProductService productService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        public Cart GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
            {
                return new Cart();
            }

            return JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
        }

        public async Task AddToCart(int productId, int quantity = 1)
        {
            var cart = GetCart();
            var product = await _productService.GetProductByIdAsync(productId);

            if (product != null)
            {
                cart.AddItem(product, quantity);
                SaveCart(cart);
            }
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.RemoveItem(productId);
            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            cart.UpdateQuantity(productId, quantity);
            SaveCart(cart);
        }

        public void ClearCart()
        {
            var cart = new Cart();
            SaveCart(cart);
        }

        public int GetCartItemCount()
        {
            var cart = GetCart();
            return cart.TotalItems;
        }

        private void SaveCart(Cart cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, cartJson);
        }
    }
}
