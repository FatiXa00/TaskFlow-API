using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            CreatedAt = u.CreatedAt
        });

        return Ok(userDtos);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };

        return Ok(userDto);
    }

    // GET: api/users/email/{email}
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };

        return Ok(userDto);
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Vérifier si l'email existe déjà
        var existingUserByEmail = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUserByEmail != null)
            return BadRequest(new { message = "Email already exists" });

        // Vérifier si le username existe déjà
        var existingUserByUsername = await _userRepository.GetByUsernameAsync(registerDto.Username);
        if (existingUserByUsername != null)
            return BadRequest(new { message = "Username already exists" });

        // Pour simplifier, on hash le mot de passe de façon basique
        // En production, utilise BCrypt ou Identity
        var passwordHash = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(registerDto.Password));

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);

        var userDto = new UserDto
        {
            Id = createdUser.Id,
            Username = createdUser.Username,
            Email = createdUser.Email,
            CreatedAt = createdUser.CreatedAt
        };

        return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
    }

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password" });

        // Vérifier le mot de passe (version simplifiée)
        var passwordHash = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(loginDto.Password));

        if (user.PasswordHash != passwordHash)
            return Unauthorized(new { message = "Invalid email or password" });

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };

        return Ok(userDto);
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found" });

        await _userRepository.DeleteAsync(id);

        return NoContent();
    }
}