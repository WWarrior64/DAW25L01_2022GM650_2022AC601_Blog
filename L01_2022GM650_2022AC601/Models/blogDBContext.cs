using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace L01_2022GM650_2022AC601.Models
{
	public class blogDBContext : DbContext
	{
		public blogDBContext(DbContextOptions<blogDBContext> options) : base(options)
		{

		}

	}
}
