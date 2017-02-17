using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class SettingsRepository : BaseRepository
    {
        public SettingsRepository(AppDBContext context) : base(context)
        {

        }

        public string GetSettingValue(string Key)
        {
            return context.Settings.Where(s => s.Key == Key).FirstOrDefault()?.Value;
        }
    }
}
