using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
  public async Task<MemberDto?> GetMemberAsync(string username)
  {
    return await context.Users
      .Where(x => x.UserName == username)
      .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();
  }

  public async Task<IEnumerable<MemberDto>> GetMembersAsync()
  {
    return await context.Users
      .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
      .ToListAsync();
  }

  public async Task<AppUser?> GetUserByUsernameAsync(string username)
  {
    //     Key behavior:
    // Returns the only user that matches the condition.

    // Returns null if no match.

    // Throws an error if more than one user matches.
    return await context.Users
    .Include(x => x.Photos)
    .SingleOrDefaultAsync(x => x.UserName == username);
  }
  public async Task<AppUser?> GetUserByIdAsync(int id)
  {
    return await context.Users.FindAsync(id);
  }


  public async Task<IEnumerable<AppUser>> GetUsersAsync()
  {
    return await context.Users
      .Include(x => x.Photos)
      .ToListAsync();
  }

  public async Task<bool> SaveAllAsync()
  {
    return await context.SaveChangesAsync() > 0;
    // This method saves all changes made in the current DbContext to the actual database.

    // 🧠 Why it's important:
    // When you make changes like:

    // Adding a new user

    // Updating user data

    // Deleting a user

    // These changes are made in memory first — they aren't stored in the database until you call SaveChangesAsync().

    // 🧪 Example:
    // csharp
    // Copy
    // Edit
    // var user = await context.Users.FindAsync(1);
    // user.UserName = "newName";
    // await context.SaveChangesAsync(); // This saves the updated name to the DB
    // 🔧 Return value:
    // Returns an int — the number of state entries (rows) written to the database.
  }

  public void Update(AppUser user)
  {
    context.Entry(user).State = EntityState.Modified;

    //     This manually tells Entity Framework:
    // 👉 “This user object has changed — please update it in the database when I call SaveChangesAsync().”

    // 🧠 When do you need it?
    // You only need this if:

    // You're working with a disconnected entity (like from an API or manually created object).

    // EF is not tracking the object, so it doesn't know it was changed.

    // 🧪 Example:
    // Let's say you received an updated user from the front end of your web app:

    // csharp
    // Copy
    // Edit
    // var user = new AppUser { Id = 1, UserName = "updatedName" };
    // context.Entry(user).State = EntityState.Modified;
    // await context.SaveChangesAsync();
    // Even though you didn’t load the user using FindAsync, EF will now update the record with ID = 1 to have UserName = "updatedName"
  }
}
