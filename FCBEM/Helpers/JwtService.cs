using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Core.Commons.FCBEMConstants;
namespace FCBEM24.Helpers
{

    public static class JwtService
    {

        public static string GenerateTokenAsync(string email)
        {


            var claims = new[]
        {
            new Claim(ClaimTypes.Email, email)

        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRETKEY));
            // Tạo chữ ký (signing credentials) bằng thuật toán HMAC và khóa bí mật
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo JWT token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: signingCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            string DecodeTokenAsyncO = DecodeTokenAsync(tokenString);
            return tokenString;
        }

        public static string DecodeTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Cấu hình các tham số xác minh cho việc giải mã token
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRETKEY)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                // Giải mã JWT token và xác minh tính hợp lệ
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Trích xuất giá trị email từ các khai báo (claims) trong token
                var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

                return email;
            }
            catch
            {
                // Xảy ra lỗi trong quá trình giải mã hoặc xác minh token
                return null;
            }
        }
    }
}
