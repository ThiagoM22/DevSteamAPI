using Microsoft.EntityFrameworkCore;

namespace DevSteamAPI.Data
{
    public class DevSteamAPIContext : DbContext
    {
        public DevSteamAPIContext(DbContextOptions<DevSteamAPIContext> options) : base(options) { }


    }
}
