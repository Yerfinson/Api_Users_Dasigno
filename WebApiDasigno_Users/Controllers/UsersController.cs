using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using WebApiDasigno_Users.Context;
using WebApiDasigno_Users.Models;

namespace WebApiDasigno_Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(AppDBcontext context) : ControllerBase
    {
        private readonly AppDBcontext _context = context;

        [HttpPost("CreateUser")]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Users))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(
            [SwaggerParameter("Primer nombre del usuario"), 
            RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El primer nombre solo puede contener letras"),Required,
            MaxLength(50, ErrorMessage = "El primer nombre no puede tener más de 50 caracteres")]
            string Primer_Nombre,
            [SwaggerParameter("Segundo nombre del usuario (opcional)"),
            RegularExpression(@"^[a-zA-ZñÑ]+$", ErrorMessage = "El segundo nombre solo puede contener letras"),
            MaxLength(50, ErrorMessage = "El segundo nombre no puede tener más de 50 caracteres")] 
            string? Segundo_nombre,
            [SwaggerParameter("Primer Apellido del usuario"),
            RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El primer Apellido solo puede contener letras"),Required,
            MaxLength(50, ErrorMessage = "El primer Apellido no puede tener más de 50 caracteres")] 
            string Primer_Apellido,
            [SwaggerParameter("Segundo Apellido del usuario (opcional)"),
            RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El segundo apellido solo puede contener letras"),
            MaxLength(50, ErrorMessage = "El segundo apellido no puede tener más de 50 caracteres")]
            string? Segundo_Apellido,
            [SwaggerParameter("Fecha de nacimiento del usuario (formato: YYYY-MM-DD)"),Required]
            DateTime Fecha_de_nacimineto,
            [SwaggerParameter("Sueldo del usuario"),Range(1,int.MaxValue,ErrorMessage ="El sueldo debe ser mayor a cero"),Required] int Sueldo) {
            try
            {
                // Crear un nuevo objeto de usuario con los datos proporcionados
                var NuevoUsuario = new Users
                {
                    First_Name = Primer_Nombre,
                    Middle_Name = Segundo_nombre,
                    Last_Name = Primer_Apellido,
                    Second_Last_Name = Segundo_Apellido,
                    Date_of_birth = Fecha_de_nacimineto,
                    Salary = Sueldo,
                    Created_date = DateTime.Now,
                };

                //  Agregar el nuevo usuario al contexto de la base de datos
                _context.Users.Add(NuevoUsuario);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Devolver una respuesta 200 OK con el nuevo usuario creado
                return Ok(NuevoUsuario);
            }
            catch (Exception ex)
            {
                // En caso de error,devolver una respuesta 400 Bad Request con el mensaje de error
                return BadRequest(ex);
            }
        }
        [HttpGet("GetUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Users))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserById(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id_User == userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found", userId });
                }
                else
                {
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"An error occurred while fetching user with ID {userId}: {ex}");

                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }
        // GET: api/Users/Search
        [HttpGet("SearchUsersPagination")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Users>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Users>> SearchUsers(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validar parámetros de paginación
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be positive integers.");
                }

                IQueryable<Users> query = _context.Users;

                // Aplicar filtros de búsqueda
                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    query = query.Where(u => u.First_Name.Contains(firstName.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    query = query.Where(u => u.Last_Name.Contains(lastName.Trim()));
                }

                // Paginar resultados y ejecutar consulta
                var users = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                if (users.Count == 0)
                {
                    return NotFound("No se encontró ningún usuario que coincida con los criterios de búsqueda especificados.");
                }

                return users;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpDelete("DeleteUserById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Users))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserById([Required] int userId)
        {
            try
            {
                // Buscar el usuario por su ID
                var user = await _context.Users.FindAsync(userId);

                // Verificar si el usuario existe
                if (user != null)
                {
                    // Eliminar el usuario de la base de datos
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    return Ok(user);
                }
                else
                {
                    // Si el usuario no se encuentra, devolver un código de estado 404 Not Found
                    return NotFound(new { message = "The user was not found." });
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error durante la eliminación del usuario, devolver un código de estado 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error occurred while deleting the user.", error = ex.Message });
            }
        }
        [HttpPut("UpdateUserFromUserId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Users))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(
            [SwaggerParameter("ID del usuario"),Required]
            int userId,
            [SwaggerParameter("Primer nombre del usuario"),
            RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El primer nombre solo puede contener letras"),
            MaxLength(50, ErrorMessage = "El primer nombre no puede tener más de 50 caracteres")]
            string? Primer_Nombre,
             [SwaggerParameter("Segundo nombre del usuario"),
            RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El segundo nombre solo puede contener letras"),
            MaxLength(50, ErrorMessage = "El segundo nombre no puede tener más de 50 caracteres")]
            string? Segundo_nombre,
            [SwaggerParameter("Primer Apellido del usuario"),
            RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El primer Apellido solo puede contener letras"),
            MaxLength(50, ErrorMessage = "El primer Apellido no puede tener más de 50 caracteres")]
            string? Primer_Apellido,
            [SwaggerParameter("Segundo Apellido del usuario"),
            RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El segundo apellido solo puede contener letras"),
            MaxLength(50, ErrorMessage = "El segundo apellido no puede tener más de 50 caracteres")]
            string? Segundo_Apellido,            
            [SwaggerParameter("Fecha de nacimiento del usuario (formato: YYYY-MM-DD)")]
            DateTime? Fecha_de_nacimineto,
            [SwaggerParameter("Sueldo del usuario"), Range(1, int.MaxValue, ErrorMessage = "El sueldo debe ser mayor a cero")]
            int? Sueldo)
        {
            try
            {
                DateTime updatedate = DateTime.Now;
                // Buscar el usuario existente por su ID
                var existingUser = _context.Users.FirstOrDefault(u => u.Id_User == userId);                
                if (existingUser != null)
                {
                    existingUser.First_Name = Primer_Nombre ?? existingUser.First_Name;
                    existingUser.Middle_Name = Segundo_nombre ?? existingUser.Middle_Name;
                    existingUser.Last_Name = Primer_Apellido ?? existingUser.Last_Name;
                    existingUser.Second_Last_Name = Segundo_Apellido ?? existingUser.Second_Last_Name;
                    existingUser.Date_of_birth = Fecha_de_nacimineto ?? existingUser.Date_of_birth;
                    existingUser.Salary = Sueldo ?? existingUser.Salary;
                    existingUser.Update_date = updatedate;

                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();
                    return Ok(existingUser);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        message = "Update failed! The specified Userid record does not exist",
                    });
                }
                                                
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al actualizar el usuario", error = ex.Message });
            }
        }
    }
}
