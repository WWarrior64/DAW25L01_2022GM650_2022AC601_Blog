using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace L01_2022GM650_2022AC601.Models
{
	public class blogDBContext : DbContext
	{
		public blogDBContext(DbContextOptions<blogDBContext> options) : base(options)
		{

		}
		public DbSet<roles> roles { get; set; }
		public DbSet<usuarios> usuarios { get; set; }
		public DbSet<publicaciones> publicaciones { get; set; }
		public DbSet<comentarios> comentarios { get; set; }
		public DbSet<calificaciones> calificaciones { get; set; }

	}
}
