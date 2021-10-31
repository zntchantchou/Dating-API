using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using System;

namespace API.Data
{
  public class Seed
  {
    public static async Task SeedUsers(DataContext context)
    {
      if (await context.Users.AnyAsync()) return;
      var userData = await System.IO.File.ReadAllTextAsync("Data/userSeedData.json");
      var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
      foreach (var user in users)
      {
        using var hmac = new HMACSHA512();
        Console.WriteLine("Seeding user", user);
        user.Username = user.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("pa$$w0rd"));
        user.PasswordSalt = hmac.Key;
        context.Add(user);
      }
      await context.SaveChangesAsync();
    }
    public static async Task SeedUserCampaigns(DataContext context)
    {
      Console.WriteLine("Seeding Campaigns...");
      if (await context.Campaigns.AnyAsync()) return;

      var users = await context.Users.ToListAsync();
      foreach (var user in users)
      {
        for (var i = 0; i < 3; i++)
        {
          var title = $"{user.Username}_campaign_{i}";
          var campaign = new Campaign
          {
            Title = title,
            Description = $"Description for {user.Email} 's number {i} campaign ",
            AdminId = user.AppUserId,
          };
          Console.WriteLine($"Saved campaigns for user {user.Email}");
          context.Campaigns.Add(campaign);
          await context.SaveChangesAsync();
          var newCampaign = await context.Campaigns.FirstOrDefaultAsync(c => c.Title == title);
          var userCampaign = new UserCampaign
          {
            UserId = user.AppUserId,
            CampaignId = newCampaign.CampaignId
          };
          context.UserCampaigns.Add(userCampaign);
          await context.SaveChangesAsync();
        }
      }
    }
  }
}