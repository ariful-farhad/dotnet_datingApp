using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("Photos")]
public class Photo
{
  public int Id { get; set; }
  public required string Url { get; set; } // this will be stored in cloud
  public bool IsMain { get; set; }
  public string? PublicId { get; set; } // we will get a publicId from cloud

  // Navigation properties
  // Navigation Property: A property on an entity class that allows you to "navigate" from that entity to related entities. 
  // They don't represent data stored directly in the main table for that entity; instead, they represent the relationship itself.
  public int AppUserId { get; set; }
  public AppUser AppUser { get; set; } = null!; //It essentially tells the compiler: "Even though I'm not initializing this AppUser property here, trust me, it won't be null when it's actually used". This is a specific type of navigation property that points to a single related entity. 
  // It typically represents the "one" side of a relationship or the principal end from the dependent's perspective.
  // It lets you access all the Photos associated with a AppUser via Photos.AppUser. there won't be any column in the database table


}

// https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many#required-one-to-many


