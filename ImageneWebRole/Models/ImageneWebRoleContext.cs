using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImageneWebRole.Models
{
    public class ImageneWebRoleContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ImageneWebRoleContext() : base("name=ImageneWebRoleContext")
        {
        }

        public System.Data.Entity.DbSet<ImageWebRole.Model.Image> Images { get; set; }
    }
}
