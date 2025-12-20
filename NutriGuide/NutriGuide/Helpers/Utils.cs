using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NutriGuide.Helpers
{
    public static class Utils
    {
        public static string generatePasswordHash(string password)
        {
            //generate rundom key
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Generate hash
            using (var pdkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pdkdf2.GetBytes(32);

                //combine salt +hash 
                byte[] hashBytes = new byte[48];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 32);

                return Convert.ToBase64String(hashBytes);
            }


        }
        //----------------------------------------
        public static string generateToken(string role, string id, string name)
        {
            #region generate claims

            var userdata = new List<Claim>();
            userdata.Add(new Claim(ClaimTypes.Role, role));
            userdata.Add(new Claim(ClaimTypes.Sid, id));
            userdata.Add(new Claim(ClaimTypes.Name, name));

            #endregion

            #region SigningCredentials + secret key

            var key = "ITI - Team5 Graduation Project Track Full Stack web Developer using .NET Minya Branch";
            var secretkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            var signingcer = new SigningCredentials
                (
                secretkey,
                SecurityAlgorithms.HmacSha256
                );

            #endregion

            var tokenObject = new JwtSecurityToken
               (
                   claims: userdata,
                   expires: DateTime.Now.AddDays(1),
                   signingCredentials: signingcer
               );

            // convert token => string
            var token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
        //----------------------------------------
        public static string GenerateCode(int length = 6)
        {
            const string digits = "0123456789";
            var randomNumber = new byte[length];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = digits[randomNumber[i] % digits.Length];
            }

            return new string(result);
        }
        //----------------------------------------
        public static bool verifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            byte[] storedPasswordHash = new byte[32];
            Array.Copy(hashBytes, 16, storedPasswordHash, 0, 32);

            using (var pdkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] newHash = pdkdf2.GetBytes(32);
                for (int i = 0; i < 32; i++)
                {
                    if (newHash[i] != storedPasswordHash[i])
                        return false;
                }
            }
            return true;

        }
        
        //----------------------------------------    
        

    }
}
