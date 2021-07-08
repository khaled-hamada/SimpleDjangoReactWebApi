using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleDotNetWebApp.data;
using SimpleDotNetWebApp.Dtos;
using SimpleDotNetWebApp.models;
using BCrypt;
using SimpleDotNetWebApp.helpers;
using Microsoft.AspNetCore.Http;

namespace SimpleDotNetWebApp.Controllers
{
     [Route("api")]
     [ApiController]
    public class AuthController : Controller
    {
        /* get user interface repos. as a property inside the controller so it will be easily accessible**/
        private readonly IUserRepository _repo;
        private readonly JwtService _jwtService; 
        // inject both user repo interface and Jwt token generator
        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repo = repository;
            _jwtService = jwtService;
        }


        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                name = dto.name,
                email = dto.email,
                /* encrypt the user password before adding it to the database */
                password =BCrypt.Net.BCrypt.HashPassword(dto.password),

            };

            // pass the user to repository to create it 
           user =  _repo.Create(user);
             
            return Created("success", user);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto lgdto)
        {
           var user = _repo.getByEmail(lgdto.email);

            
            // wrong user
            if(user == null)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            // in case of a correct one , check password 
            // if wrong password, return error
            if(! BCrypt.Net.BCrypt.Verify(lgdto.password , user.password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            // get jwt user token 
            var jwtString = _jwtService.Generate(user.id);
            // make an http only cookie so its accesible only in the backend
            Response.Cookies.Append("jwt", jwtString, new CookieOptions
            {
                HttpOnly = true
            }) ;

            return Ok(new
            {
                   message = "success",
				   user
            });
        }


        [HttpGet("user")]
        public IActionResult user()
        {
            // token verivication may result in errors so we will wrap it in a try catch block 
            try
            {
                var jwtCookie = Request.Cookies["jwt"];
                var validationToken = _jwtService.Verify(jwtCookie);
                var userId = int.Parse(validationToken.Issuer);

                var user = _repo.getById(userId);

                return Ok(user);
            }
            catch(Exception _)
            {
                return Unauthorized();
            }


        } 
        
        [HttpPost("logout")]
        public IActionResult logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok( new
            {
                message = "successfully logout !"
            }
                );
        }
        
        [HttpPost("add")]
        public IActionResult add(NumbersDto dto)
        {
            float result = dto.numOne + dto.numTwo;

            return Ok( new
            {
                message = "successfully logout !",
                result

            }
                );
        }
    }
}
