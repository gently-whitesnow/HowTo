using System;

namespace HowTo.Entities;

public class User
{
    public User(Guid id, string name, UserRole userRole)
    {
        Id = id;
        Name = name;
        UserRole = userRole;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public UserRole UserRole { get; set; }
}