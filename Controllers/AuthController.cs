using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using simpleapi.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Cryptography;

namespace simpleapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public static User user = new User();
        // for JWT Token
        private static IConfiguration _configuration;
        // private final attirbute 
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        /*
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

        

            return Ok(user);
        }
         
         * return Task<IActionResult>
         **/
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            // validate request 

            // hashing password 
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // store request + password in user object
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = CreateRandomToken()
            };

            // save object in to the Database 
            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            Console.WriteLine(request.Username);

             

            // Check if the user password is 
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            // create a jwt token 
            string token = CreateToken(user);

            // var refreshToken = GenerateRefreshToken();
            // SetRefreshToken(refreshToken);

            return Ok(token);
        }


        /**____________________( Helper Function )________________________*/

        /**
         * Password Hashing 
         * Use in Register & Reset Password 
         * */
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /**
         * @reting string 
         * get payload and it will return a jwt token
         * 
         **/
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
                // new Claim(ClaimTypes.Name, user.Username),
                // new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        /*
         * Check the Hashed password if it's correct or not 
         * Helper Method that Check the Hash when user login 
         * Return True if the Hash is correct
         * 
         **/
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        /**
         * Create a Hash Token 
         * Hash token use for authenticate the user request in verify email and in request reset password
         * */
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
