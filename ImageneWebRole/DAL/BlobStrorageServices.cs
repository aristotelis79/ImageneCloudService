using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageneWebRole.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace ImageneWebRole.DAL
{
    public class BlobStrorageServices : IImagesService
    {
        private ImageneWebRoleContext db = new ImageneWebRoleContext();

        //Create BlobContainer if not exist and return the reference of myimages container
        public CloudBlobContainer GetCloudBlobContainer()
        {
            try
            {
                CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("ImageStorageAccountControl"));
                CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();
                CloudBlobContainer blobContainer = blobClient.GetContainerReference("myimages");
                if (blobContainer.CreateIfNotExists())
                {
                    // Enable public access on the newly created "myimages" container
                    blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }
                return blobContainer;
            }
            catch (Exception ex)
            {
                throw new HttpException(404, ex.Message);
            }
        }

        async private Task<Image> AddNewBlob (Image image, HttpPostedFileBase imageFile)
        {
            BlobStrorageServices blobStorageService = new BlobStrorageServices();
            CloudBlobContainer blobContainer = blobStorageService.GetCloudBlobContainer();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(imageFile.FileName);
            await blob.UploadFromStreamAsync(imageFile.InputStream);
            image.ImagePath = blob.Uri.ToString();
            return image;
        }

        public Task<List<Image>> GetImages()
        {
            return (db.Images.ToListAsync());
        }

        public void DeleteImage(int id)
        {
            Image image = db.Images.Find(id);
            BlobStrorageServices blobStorageService = new BlobStrorageServices();
            CloudBlobContainer blobContainer = blobStorageService.GetCloudBlobContainer();
            CloudBlob blob = blobContainer.GetBlobReference(image.ImagePath);
            blob.DeleteIfExistsAsync();
            db.Images.Remove(image);
            db.SaveChangesAsync();
        }

        async public Task<int> AddNewImage(Image image, HttpPostedFileBase imageFile)
        {
            Image newImage = await AddNewBlob(image, imageFile);
            db.Images.Add(newImage);
            await db.SaveChangesAsync();
            return image.Id;
        }

        public Task<Image> GetImage(int? id)
        {
            return (db.Images.FindAsync(id));
        }

        async public Task<int> Edit(Image image, HttpPostedFileBase imageFile)
        {
            Image newImage = await AddNewBlob(image, imageFile);
            db.Entry(image).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return image.Id;
        }
    }
}