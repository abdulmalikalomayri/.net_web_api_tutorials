using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace simpleapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        // here I create a attraibute and set a setter method 
        private readonly DataContext _context;

        // configuration for the JWT Token 
        private IConfiguration _configuration;

        // from This we can call any table using _context
        public UserController(DataContext context, IConfiguration configuration)
        {

            _context = context;
            _configuration = configuration;
        }

        /**_________________________________________________*/

        /***
         * Register a 
         * @method POST
         **/
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {

            // check if the user 
            if (_context.Users.Any(u => u.Username == request.Username))
            {
                return BadRequest("User already exists.");
            }

            CreatePasswordHash(request.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = CreateRandomToken()
            };

            // save the user in database 
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Send email for verifiction 

            return Ok(user);
        }


        /**
         * Login a user
         * @method POST
         * UserLoginRequest used for entring the input in the swagger
         **/
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            // Check if the user with that email exsist  
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Check if the user password is correct 
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect.");
            }

            /*
            // Check if the user verified or not
            if (user.VerifiedAt == null)
            {
                return BadRequest("Not verified!");
            }
            */

            // create a token using a private method "CreateToken"
            string token = CreateToken(user);
            

            return Ok(token);
        }

        
        /**
         * verifiy a user
         * Take a bear token and verified it in DB if it's true
         * @method POST
         */
        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if(user == null)
            {
                return BadRequest("Invalid token");
            }

            // insert current date in DB if it's true 
            // if VerifiedAt column not null than the user is verified!
            user.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok("User verified! :)");
        }

        /**
         * enable a user to reset their password using hash 
         * @method POST
         * 
         */
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            // check if user exsist 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
            {
                return BadRequest("User not found");
            }

            user.PasswordResetToken = CreateRandomToken();
            // expre in 1 day
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            // database save changes
            await _context.SaveChangesAsync();

            return Ok("You can reset your password now!");
        }

        /**
         * Update the user password
         * @method POST
         **/
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResettPassword(ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password successfully reset.");
        }

        /**____________________( Helper Function )________________________*/

        /**
         * Create a JWT token for login 
         * 
         **/
        private string CreateToken(User user)
        {
            

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            // create JWT Symmetric key = public key which is only one key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            // selecting the security algorithms 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            // createing jwt token from the above information
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        /**
         * Password Hashing 
         * Use in Register & Reset Password 
         * */
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
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
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
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
