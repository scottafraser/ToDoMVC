using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models;

namespace ToDo.Controllers

{
    public class ItemsController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet("/home")]
        public IActionResult Back()
        {
            return View("Index");
        }

        [HttpGet("/list")]
        public IActionResult List()
        {
            return View();
        }

        [HttpPost("/list")]
        public IActionResult Create()
        {
            Item newItem = new Item(Request.Form["new-item"], Request.Form["date"], 0);
            newItem.Save();
            List<Item> allItems = Item.GetAll();
            return View("List", allItems);
        }

        [HttpGet("/delete")]
        public IActionResult Delete()
        {
            Item.DeleteAll();

            return View();
        }

        [HttpGet("/items/{id}")]
        public ActionResult Details(int id)
        {
            Item item = Item.Find(id);
            return View(item);
        }

        [HttpGet("/items/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Item thisItem = Item.Find(id);
            return View(thisItem);
        }

        [HttpPost("/items/{id}/update")]
        public ActionResult Update(int id)
        {
            Item thisItem = Item.Find(id);
            thisItem.Edit(Request.Form["newname"]);
            return RedirectToAction("Index");
        }
    }
}
