using ASM1670.Models;

namespace ASM1670.Models.VM;

public class ShoppingCartVM
{
    public IEnumerable<Cart> ListCarts { get; set; }
    public Orders Order { get; set; }
}