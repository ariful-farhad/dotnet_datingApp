using System;

namespace API.Extentions;

public static class DateTimeExtensions
{
  // The key is the first parameter: this DateOnly dob. The this keyword before the type DateOnly 
  // signifies that CalculateAge is an extension method for the DateOnly struct.
  // This means you can call CalculateAge() on any variable of type DateOnly
  public static int CalculateAge(this DateOnly dob)
  {
    var today = DateOnly.FromDateTime(DateTime.Now);
    var age = today.Year - dob.Year;

    if (dob > today.AddYears(-age)) age--;
    return age;
  }
}
