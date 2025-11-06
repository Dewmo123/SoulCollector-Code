using System;
using System.Collections.Generic;
using System.Data;

namespace Scripts.Network
{
    public enum Role
    {
        None = 1,
        User,
        Admin
    }
    public class LoginUserDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<Role> Roles { get; set; }
    }
    public class LoginDTO
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
    public class CreateUserDTO
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
