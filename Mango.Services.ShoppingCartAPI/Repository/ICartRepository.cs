using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        public Task<CartDto> GetCartByUserId(string userId);

        public Task<CartDto> CreateUpdateCart(CartDto cart);

        public Task<bool> RemoveFromCart(int cartDetailsId);

        public Task<bool> ApplyCoupon(string userId, string couponCode);

        public Task<bool> RemoveCoupon(string userId);

        public Task<bool> ClearCart(string userId);
    }
}
