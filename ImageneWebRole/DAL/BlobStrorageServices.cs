using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageneWebRole.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ImageneWebRole.DAL
{
    public class BlobStrorageServices : IImagesService
    {
        private ImageneWebRoleContext db = new ImageneWebRoleContext();

        public Task<List<Image>> GetImages()
        {
            return (db.Images.ToListAsync());
        }

        public void DeleteImage(int id)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
            db.SaveChangesAsync();
        }

        public Task<int> AddNewImage(Image image)
        {
            db.Images.Add(image);
            db.SaveChangesAsync();
            return Task.FromResult(image.Id);
        }

        public Task<Image> GetImage(int? id)
        {
            return (db.Images.FindAsync(id));
        }

        public Task<int> Edit(Image image)
        {
            db.Entry(image).State = EntityState.Modified;
            db.SaveChangesAsync();
            return Task.FromResult(image.Id);
        }
    }
}