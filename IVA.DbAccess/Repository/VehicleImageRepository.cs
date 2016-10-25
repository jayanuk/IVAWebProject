using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class VehicleImageRepository : BaseRepository
    {
        public VehicleImageRepository(AppDBContext context) : base(context)
        {
        }

        public long Add(IVehicleImage Image)
        {
            VehicleImage image = new VehicleImage();
            image.SRId = Image.SRId;
            image.ImageBytes = Image.ImageBytes;
            image.Date = Image.Date;
            image.Order = Image.Order;

            context.VehicleImages.Add(image);
            context.SaveChanges();
            return image.Id;
        }

        public void AddRange(List<VehicleImage> Images)
        {
            List<VehicleImage> images = new List<VehicleImage>();
            foreach(var Image in Images)
            {
                VehicleImage image = new VehicleImage();
                image.SRId = Image.SRId;
                image.ImageBytes = Image.ImageBytes;
                image.Date = Image.Date;
                image.Order = Image.Order;

                images.Add(image);
            }
            context.VehicleImages.AddRange(images);
            context.SaveChanges();
        }

        public List<IVehicleImage> GetByServicerequestId(long SRId)
        {
            return context.VehicleImages.Where(i => i.SRId == SRId).ToList<IVehicleImage>();
        }

        public void DeleteByServiceRequest(long SRId)
        {
            var images = context.VehicleImages.Where(i => i.SRId == SRId);
            context.VehicleImages.RemoveRange(images);
            context.SaveChanges();
        }

        public void Update(long SRId, List<VehicleImage> Images)
        {
            if(Images != null)
            {
                DeleteByServiceRequest(SRId);
                AddRange(Images);
            }
        }
    }
}
