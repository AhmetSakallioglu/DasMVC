using Microsoft.EntityFrameworkCore;

namespace dasMVC.Entities
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<UserAppointment> UserAppointments { get; set; }
	}
}
