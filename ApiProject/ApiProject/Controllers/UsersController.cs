using Microsoft.AspNetCore.Mvc;
using ApiProject.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiProject.Controllers
{
    [Route("api/Users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User { Id = 1, Name = "Mykyta Tkachuk", Email = "mykyta@example.com" },
            new User { Id = 2, Name = "Anastasia Vorobei", Email = "nastya@example.com" }
        };

        [HttpGet]
        public IActionResult GetUsers() => Ok(users);

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            newUser.Id = users.Count + 1;
            users.Add(newUser);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name ?? user.Name;
            user.Email = updatedUser.Email ?? user.Email;
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]  
        public IActionResult DeleteUser(int id)
        {
            var userRole = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            Console.WriteLine($"User role: {userRole}");

            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            users.Remove(user);
            return NoContent();
        }
    }
}
