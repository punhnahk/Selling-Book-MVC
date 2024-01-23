using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASM1670.Models.VM;

public class BookVm
{
    public Book Book { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; }
}