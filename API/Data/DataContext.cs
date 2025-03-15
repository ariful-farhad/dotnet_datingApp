using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
  // we need to tell our class about Entities
  // Users going to be the table in our database
  public DbSet<AppUser> Users { get; set; }
}
