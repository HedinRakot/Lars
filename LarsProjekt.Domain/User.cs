﻿namespace LarsProjekt.Domain;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public string Password { get; set; }
    public int Number { get; set; }
}