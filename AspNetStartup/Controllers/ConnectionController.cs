using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System;
using Everest.AspNetStartup.Core.ExceptionTransformers;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Filters;
using Everest.AspNetStartup.Core.Binding;
using Everest.AspNetStartup.Core;
using Everest.AspNetStartup.Models;
using System.Security.Claims;
using Everest.Core.Identity;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Everest.AspNetStartup.Controllers
{
    [ExceptionTransformer]
    [Route("api/connections")]
    public class ConnectionController:Controller
    {
        private IRepository<Connection, long> connectionRepository;
        private IRepository<User, string> userRepository;
        private IPasswordHasher<User> passwordHasher;
        private IConfiguration configuration;

        public ConnectionController(IRepository<Connection, long> connectionRepository, IRepository<User, string> userRepository, 
            IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            this.connectionRepository = connectionRepository;
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.configuration = configuration;
        }

        [HttpGet("{connectionId}")]
        [LoadConnection]
        public Connection Find(Connection connection) => connection;

        /// <summary>
        /// Pour obtenir toutes les connexions d'un compte.
        /// </summary>
        /// <param name="user">Le compte dont on souhaite obtenir les connexions.</param>
        /// <returns>Une liste contenant les connexions du compte.</returns>
        [HttpGet]
        [LoadUser]
        public IList<Connection> List(User user)
        {
            return connectionRepository.List(c => c.User.Equals(user));
        }


        /// <summary>
        /// Pour créer une connexion. Autrement dit, pour d'authentifier.
        /// Obtenir une authentification n'est pas suffisante pour accéder 
        /// aux ressources protégées de l'application. Il faut encore 
        /// obtenir une authorization pour l'application que le compte utilise
        /// pour accéder à l'application.
        /// </summary>
        /// <param name="model">Contient les informations sur la connection à créer.</param>
        /// <exception cref="EntityNotFoundException">
        ///     Si l'email renseigné pour la connexion n'est utilisé par aucun compte.
        /// </exception>
        /// <exception cref="InvalidValueException">
        ///     Si le mot de passe renseigné n'est pas celui du compte
        /// </exception>
        /// <returns>La connexion nouvellement crée.</returns>
        [HttpPost]
        [ValidModel]
        public Connection Login([FromBody] LoginModel model)
        {
            User user = userRepository.First(a => a.Email.Equals(model.Email));

            if(user == null)
            {
                throw new EntityNotFoundException($"Il n'existe aucun compte ayant pour email {model.Email}.");
            }

            if (PasswordVerificationResult.Success !=
                passwordHasher.VerifyHashedPassword(user, user.Password, model.Password))
            {
                throw new InvalidValueException("Mot de passe incorrect");
            }

            Connection connection = new Connection {
                User = user,
                Browser = model.Browser,
                RemoteAddress = model.RemoteAddress,
                OS = model.OS,
                IsPersistent = model.IsPersisted,
                BeginDate = DateTime.Now,
            };

            connectionRepository.Save(connection);

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(EverestClaims.Username, user.Username),
                new Claim(ClaimTypes.Email, user.Email),

                new Claim(EverestClaims.ConnectionId, connection.Id.ToString()),
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user)),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["authorization:secretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha384);

            var tokenOptions = new JwtSecurityToken(
                issuer: configuration["authorization:validIssuer"],
                audience: configuration["authorization:validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(360),
                signingCredentials: signinCredentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            connection.AccessToken = accessToken;

            connectionRepository.Update(connection);

            return connection;
        }


        /// <summary>
        /// Pour fermer une connexion. Autrement dit, déconnecter un compte.
        /// Fermer une connexion entrainement de facto l'invalidation de toutes
        /// les authorisations liées à la connexion.
        /// </summary>
        /// <param name="connection">La connexion à fermer.</param>
        /// <exception cref="InvalidOperationException">
        ///     Si on essaye de fermer une connexion déjà fermée.
        /// </exception>
        /// <returns>
        ///     Un <see>StatusCodeResult</see> de code 204 si la connexion est fermée.
        /// </returns>
        [HttpPut("{connectionId}/logout")]
        [LoadConnection]
        public StatusCodeResult Logout(Connection connection)
        {
            if (connection.IsClosed)
            {
                throw new InvalidOperationException("Cette connexion a déjà été fermée");
            }
            connection.EndDate = DateTime.Now;
            return NoContent();
        }
    }
}
