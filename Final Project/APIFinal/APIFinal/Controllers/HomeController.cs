using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIFinal.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        List<PlayerData> details = new List<PlayerData>();
        public HomeController()
        {
            details.Add(new PlayerData
            {
                PlayerId = 1,
                PlayerName = "Lionel Messi",
                Nationality = "Argentina",
                FootballClub = "FC Barcelona",
                ClubNationality = "Spain",
                Role = "Forward",
                PerceivedRank = 1,
            });

            details.Add(new PlayerData
            {
                PlayerId = 2,
                PlayerName = "Luis Suarez",
                Nationality = "Uruguay",
                FootballClub = "FC Barcelona",
                ClubNationality = "Spain",
                Role = "Forward",
                PerceivedRank = 5,
            });

            details.Add(new PlayerData
            {
                PlayerId = 3,
                PlayerName = "Neymar",
                Nationality = "Brazil",
                FootballClub = "Paris Saint German",
                ClubNationality = "France",
                Role = "Forward",
                PerceivedRank = 3,
            });

            details.Add(new PlayerData
            {
                PlayerId = 4,
                PlayerName = "Cristiano Ronaldo",
                Nationality = "Portugal",
                FootballClub = "Juventus FC",
                ClubNationality = "Italy",
                Role = "Forward",
                PerceivedRank = 2,
            });

            details.Add(new PlayerData
            {
                PlayerId = 5,
                PlayerName = "Kylian Mbappe",
                Nationality = "France",
                FootballClub = "Paris Saint German",
                ClubNationality = "France",
                Role = "Forward",
                PerceivedRank = 4,
            });
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("Secret")]
        public IActionResult Secret()
        {
            return View();
        }

        [Route("Authenticate")]
        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "random_id"),
                new Claim("grandma", "cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {access_token = tokenJson});
        }

        [Authorize]
        [Route("GetPlayerDetailsbyText")]
        public IActionResult GetPlayerDetailsbyText(int id, string p_name)
        {
            var player_data = details.Where(a => a.PlayerId == id).FirstOrDefault();
            var player_data2 = details.Where(a => a.PlayerName == p_name).FirstOrDefault();

            if (p_name == null && player_data != null)
            {
                return Ok("Player Name is " + player_data.PlayerName + ".\n" +
                    "His role of play in football is " + player_data.Role + ".\n" +
                    "He is from " + player_data.Nationality + ".\n" +
                    "He plays for " + player_data.FootballClub + " club in " + player_data.ClubNationality);
            }
            else if(player_data2 != null)
            {
                return Ok("Player Name is " + player_data2.PlayerName + ".\n" +
                    "His role of play in football is " + player_data2.Role + ".\n" +
                    "He is from " + player_data2.Nationality + ".\n" +
                    "He plays for " + player_data2.FootballClub + " club in " + player_data2.ClubNationality);
            }

            return NotFound("Player Not found by given details");
        }

        [Authorize]
        [Route("GetAllPlayerDetails")]
        public IActionResult GetAllPlayerDetails()
        {
            var output_info = new {data = details};
            return Ok(output_info);
        }

        [Authorize]
        [Route("GetSpecificPlayerDetails")]
        public IActionResult GetSpecificPlayerDetails(int id)
        {
            var output_info = new { data = details.Where(a => a.PlayerId == id).FirstOrDefault() };
            return Ok(output_info);
        }
    }
}
