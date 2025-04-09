using System;
using API.DTOs;
using API.Entities;
using API.Extentions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<AppUser, MemberDto>()
      .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
      .ForMember(destination => destination.PhotoUrl, option =>
          option.MapFrom(source => source.Photos.FirstOrDefault(row => row.IsMain == true)!.Url));
    // ! -> means that if this is null, it will immediately return null, won't check null.Url

    CreateMap<Photo, PhotoDto>();
  }
}


// for the first formember
// Why it didn't need the explicit .ForMember before (In-Memory Mapping):
// Fetching Full Objects: You fetched the complete AppUser objects from the database into your application's memory (.ToList() or similar).
// Executing C# Code: When you called mapper.Map(appUserObject, memberDtoObject), AutoMapper executed in your C# application.
// Convention over Configuration: It looked at MemberDto.Age. It didn't find AppUser.Age. By convention, it then looked for a method AppUser.GetAge().
// Method Execution: It found your public int GetAge() method (which internally calls DateOfBirth.CalculateAge()). It executed this C# method directly on the AppUser object living in memory.
// Result: The integer returned by GetAge() was assigned to MemberDto.Age.
// Why it needs the explicit .ForMember now (Database Projection with ProjectTo):
// Building a SQL Query: ProjectTo does not fetch full AppUser objects first. It aims to build an efficient SQL SELECT statement.
// Analyzing Expressions: It takes your AutoMapper configuration, specifically the expressions like d => d.Age and s => s.DateOfBirth.CalculateAge().
// Translation, Not Execution: It does not execute the C# CalculateAge() method at this stage. Instead, it passes the expression s => s.DateOfBirth.CalculateAge() to Entity Framework Core.
// EF Core's Job: EF Core tries to translate that C# expression into an equivalent SQL expression. Can it translate DateOfBirth? Yes (it's a column). Can it translate the logic inside CalculateAge? Only if that logic uses operations and functions that EF Core knows how to convert to SQL (like accessing .Year, .Month, .Day, using standard date functions like DATEDIFF if available, basic arithmetic).
// Explicit Instruction Needed: ProjectTo cannot rely on the convention of finding a GetAge() method because it's operating at the database query level, not with C# objects in memory. It needs the explicit .ForMember(...) mapping to know which expression to try and translate for the Age property.
// In essence:
// The old way executed the GetAge() C# method in memory.
// The ProjectTo way translates the s => s.DateOfBirth.CalculateAge() C# expression into SQL if possible.