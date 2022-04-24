using JuanShoesStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JuanShoesStore.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly JuanContext _context;
        public OrderController(JuanContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Shoe).AsQueryable();
            string itemPerPageStr = _context.Setting.FirstOrDefault(x => x.Key == "ItemPerPage").Value;
            int itemPerPage = string.IsNullOrEmpty(itemPerPageStr) ? 3 : int.Parse(itemPerPageStr);

            return View(Pagination<Order>.Create(order, page, itemPerPage));
        }

        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Shoe).FirstOrDefault(x => x.Id == id);

            if (order ==null)
                return NotFound();
        
            return View(order);
        }
        [HttpPost]
        public IActionResult Edit(Order order)
        {
            
            Order existOrder = _context.Orders.FirstOrDefault(x=>x.Id == order.Id);

            if (existOrder == null) { return BadRequest(); }

            existOrder.OrderStatus = order.OrderStatus;
            _context.SaveChanges();

            return RedirectToAction("index");
        }


    }
}
