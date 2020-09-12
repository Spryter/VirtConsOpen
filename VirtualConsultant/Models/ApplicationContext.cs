using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace VirtualConsultant.Models
{
	public class ApplicationContext : DbContext
	{
		public DbSet<Клиент> Клиент { get; set; }

		public ApplicationContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=localhost;Database=VirtualConsultant;Trusted_Connection=True;");
		}
	}
}
