using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace api_aspnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly string _connectionString = "Server=172.17.0.2;Database=sciconnect;User=root;Password=99076641;";

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetAllUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new Usuario
                            {
                                Id = reader.GetInt32("id"),
                                NomeInteiro = reader.GetString("nome_inteiro"),
                                NomeUsuario = reader.GetString("nome_usuario"),
                                Email = reader.GetString("email"),
                                Senha = reader.GetString("senha"),
                                TipoUsuario = reader.GetString("tipo_usuario")
                            });
                        }
                    }
                }
            }

            return Ok(usuarios);
        }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string NomeInteiro { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string  TipoUsuario { get; set; }
    }
}

// DELIMITER //

// CREATE PROCEDURE GetAllUsers()
// BEGIN
//     SELECT 
//         id, 
//         nome_inteiro, 
//         nome_usuario, 
//         email, 
//         senha, 
//         tipo AS tipo_usuario
//     FROM usuario;
// END //

// DELIMITER ;
