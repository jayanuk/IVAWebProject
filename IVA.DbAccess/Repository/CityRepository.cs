using IVA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class CityRepository : BaseRepository
    {
        public CityRepository(AppDBContext context) : base(context)
        {
        }

        public List<City> GetCities(string Text)
        {
            var result = new List<City>();
            if(String.IsNullOrEmpty(Text))
                result = context.Cities.OrderBy(c => c.Name).ToList();
            else
                result = context.Cities.Where(c => c.Name.Contains(Text)).OrderBy(c => c.Name).ToList();
            return result;
        }

    }
}
