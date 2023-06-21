using ImageUploaderWebMVC.Models;
using ImageUploaderWebMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageUploaderWebMVC.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ILogger _logger;
        private readonly IImageService _imageService;

        public ImagesController(ILogger<ImagesController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var images = await _imageService.GetImagesAsync();
            return View(images);
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var image = await _imageService.GetImageByIdAsync(id);
            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ImageFile")] Image image)
        {
            if (ModelState.IsValid)
            {
                await _imageService.AddImageAsync(image);
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var image = await _imageService.GetImageByIdAsync(id);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Url,BlobName")] Image image)
        {
            if (ModelState.IsValid)
            {
                await _imageService.UpdateImageAsync(id, image);
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _imageService.GetImageByIdAsync(id);
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _imageService.DeleteImageAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
