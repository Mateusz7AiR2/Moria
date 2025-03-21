﻿using Microsoft.EntityFrameworkCore;
using MoriaModels.Attributes;
using MoriaModels.Models.Base;

namespace MoriaModels.Models.EntityPersonel;

[Index(nameof(Username), IsUnique = true)]
public class Employee: BaseModel
{
    //public int Id { get; set; }

    [Searchable]
    public string FirstName { get; set; }

    [Searchable]
    public string LastName { get; set; }

    [Searchable]
    public string Username { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }

    //public int PositionId { get; set; }
    [Searchable]
    public Position? Position { get; set; }
    public bool Admin { get; set; }

    //public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
