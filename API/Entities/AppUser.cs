﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
  public class AppUser
  {
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Email { get; set; }

    [Required]
    public bool Active { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int AvatarId { get; set; }
    public virtual Avatar Avatar { get; set; }
    public virtual ICollection<Campaign> Campaigns { get; set; } = new HashSet<Campaign>(); 
  }
}
