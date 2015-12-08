using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImageneWebRole.Models;
using ImageneWebRole.DAL;

namespace ImageneWebRole.Controllers
{
    public class ImagesController : Controller
    {
        private BlobStrorageServices StorageService = new BlobStrorageServices();

        // GET: Images
        public async Task<ActionResult> Index()
        {
            return View(await StorageService.GetImages());
        }

        // GET: Images/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = await StorageService.GetImage(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,ImagePath")] Image image, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                await StorageService.AddNewImage(image, imageFile);
                return RedirectToAction("Index");
            }

            return View(image);
        }

        // GET: Images/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = await StorageService.GetImage(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,ImagePath")] Image image, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                await StorageService.Edit(image, imageFile);
                return RedirectToAction("Index");
            }
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = await StorageService.GetImage(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StorageService.DeleteImage(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            ImageneWebRoleContext db = new ImageneWebRoleContext();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
