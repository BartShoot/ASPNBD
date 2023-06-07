using ASPNBD.Models;
using ASPNBD.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPNBD.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IDataService _dataService;

		public HomeController(ILogger<HomeController> logger, IDataService dataService)
		{
			_logger = logger;
			_dataService = dataService;
		}

        public async Task<IActionResult> Index(ComputerFilter filter)
		{
			var computers = await _dataService.GetComputersAsync(filter.Year, filter.ComputerName);
			var model = new ComputerList
			{
				Computers = computers,
				Filter = filter
			};
			return View(model);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Computers computer)
		{
			if(ModelState.IsValid)
			{
				await _dataService.Create(computer);
				return RedirectToAction("Index");
			}
			return View();
		}

		public async Task<IActionResult> Edit(string id)
		{
            var computer = await _dataService.GetComputerAsync(id);
            if(computer == null)
			{
                return NotFound();
            }
            return View(computer);
        }

		[HttpPost]
		public async Task<IActionResult> Edit(Computers computer)
		{
            if(ModelState.IsValid)
			{
                await _dataService.Update(computer);
                return RedirectToAction("Index");
            }
            return View(computer);
        }

		public async Task<IActionResult> Delete(string id)
		{
			await _dataService.Delete(id);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> AttachImage(string id)
		{
			var computer = await _dataService.GetComputerAsync(id);
			var imageData = await _dataService.GetImage(computer.ImageId);
			var base64Image = Convert.ToBase64String(imageData);

			ViewBag.Base64Image = base64Image;
			if(computer == null)
			{
                return NotFound();
            }
            return View(computer);
		}

		[HttpPost]
		public async Task<IActionResult> AttachImage(string id, IFormFile image)
		{
            if(image != null)
			{
                await _dataService.StoreImage(id, image.OpenReadStream(), image.FileName);
            }
            return RedirectToAction("Index");
        }

		public async Task<IActionResult> GetImage(string id)
		{
			var image = await _dataService.GetImage(id);
			if(image == null)
			{
                return NotFound();
            }
            else
			{
                return File(image, "image/png");
            }
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel
			{
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
		}
	}
}
