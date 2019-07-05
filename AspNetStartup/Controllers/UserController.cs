using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Everest.Identity.Filters;
using System.Collections.Generic;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Core.Binding;
using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Filters;
using Everest.AspNetStartup.Core;
using Everest.AspNetStartup.Models;

namespace Everest.AspNetStartup.Controllers
{
    /// <summary>
    /// Controleur pour gérer les comptes utilisateurs dans les applications.
    /// <see cref="User"/>
    /// </summary>
    [Route("api/users")]
    public class UserController:Controller
    {
        public IRepository<User, string> userRepository { get; set; }
        private IPasswordHasher<User> passwordHasher;

        public UserController(IRepository<User, string> userRepository, IPasswordHasher<User> passwordHasher)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Recherche un compte par un ID
        /// </summary>
        /// <param name="user">Le compte à chercher</param>
        /// <returns>Un Compte correspondant à l'ID spécifié</returns>
        [HttpGet("{userId}")]
        [LoadUser]
        public User Find(User user) => user;

        [HttpGet("find")]
        public User Find([FromQuery]string username, [FromQuery]string email, [FromQuery]string phoneNumber)
        {
            if(username != null)
            {
                return userRepository.First(a => a.Username == username);
            }

            if (email != null)
            {
                return userRepository.First(a => a.Email == email);
            }

            if (phoneNumber != null)
            {
                return userRepository.First(a => a.PhoneNumber == phoneNumber);
            }

            return null;
        }


        [HttpGet]
        public IList<User> List() => userRepository.List();
            
        
        

        /// <summary>
        /// Permet de créer un compte utilisateur.
        ///
        /// </summary>
        /// <param name="model">Contient les informations sur l'utilisateur et dont sur le futur compte.</param>
        /// <remarks>L'email renseigné pour le compte ne doit pas être utilisé par un autre compte.</remarks>
        /// <returns>Le compte nouvellement crée</returns>
        ///
        [ValidModel]
        [HttpPost]
        public User Create([FromBody] AdduserModel model)
        {

            if(userRepository.Exists(a => a.Email == model.Email))
            {
                ModelState.ThrowModelError("email", "Cet adresse électronique est déjà utilisée");
            }

            User user = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email
            };

            string username = model.Name + model.Surname.Substring(0, 1).ToUpper() + model.Surname.Substring(1);
            long usernameUsage = userRepository.Count(a => a.Name == model.Name && a.Surname == model.Surname);

            if (usernameUsage == 0)
            {
                user.Username = username;
            }
            else
            {
                user.Username = username + usernameUsage;
            }
            
            string password = passwordHasher.HashPassword(user, model.Password);

            user.Password = password;

            user = userRepository.Save(user);

            return user;
        }


        /// <summary>
        /// Permet de modifier l'email de connexion d'un compte.
        /// </summary>
        /// <param name="user">Le compte dont on souhaite modifier l'email.</param>
        /// <param name="email">Le nouvel email du compte.</param>
        /// <returns>Un <see>StatusCodeResult</see> de code 201 indiquant que l'email a été modifié.</returns>
        /// <response code="201">Si l'email est mis à jour.</response>
        /// <exception cref="InvalidValueException">Si l'email renseigné est déjà utilisé par un autre compte.</exception>

        [Authorize]
        [LoadUser]
        [RequireuserOwner]
        [HttpPut("{userId}/email")]
        public StatusCodeResult UpdateEmail(User user, [FromQuery] string email)
        {
            if (userRepository.Exists(a => a.Email == email))
            {
                throw new InvalidValueException("Cet adresse électronique est déjà utilisée");
            }

            user.Email = email;
            userRepository.Update(user);

            return StatusCode(201);
        }

        /// <summary>
        /// Permet de modifier le numéro de téléphone d'un compte.
        /// </summary>
        /// <param name="user">Le compte dont on souhaite modifier le numéro de téléphone.</param>
        /// <param name="phoneNumber">Le nouveau numéro de téléphone du compte.</param>
        /// <returns>Un <see>StatusCodeResult</see> de code 201 indiquant que le numéro de 
        /// téléphone a été modifié.</returns>
        /// <response code="400"></response>
        /// <exception cref="InvalidValueException">Si le numéro de téléphone renseigné 
        /// est déjà utilisé par un autre compte.</exception>

        [Authorize]
        [RequireuserOwner]
        [HttpPut("{userId}/phoneNumber")]
        public StatusCodeResult UpdatePhoneNumber(User user, [FromQuery] string phoneNumber)
        {
            if (userRepository.Exists(a => a.PhoneNumber == phoneNumber))
            {
                throw new InvalidValueException("Ce numéro de téléphone est déjà utilisée");
            }

            user.PhoneNumber = phoneNumber;
            userRepository.Update(user);

            return StatusCode(201);
        }

        /// <summary>
        /// Permet de modifier le nom d'utilisateur d'un compte.
        /// </summary>
        /// <param name="user">Le compte dont on souhaite modifier le nom d'utilisateur.</param>
        /// <param name="username">Le nouveau nom d'utilisateur du compte.</param>
        /// <returns>Un <see>StatusCodeResult</see> de code 201 indiquant que le nom d'utilisateur
        /// a été modifié.</returns>
        /// <response code="400"></response>
        /// <exception cref="InvalidValueException">Si le nom d'utilisateur renseigné 
        /// est déjà utilisé par un autre compte.</exception>

        [Authorize]
        [RequireuserOwner]
        [HttpPut("{userId}/username")]
        public StatusCodeResult UpdateUsername(User user, [FromQuery] string username)
        {
            if(userRepository.Exists(a => a.Username == username))
            {
                throw new InvalidValueException("Ce nom d'utilisateur est déjà utilisée");
            }

            user.Username = username;
            userRepository.Update(user);

            return StatusCode(202);
        }

        /// <summary>
        /// Permet de modifier les informations d'état civil d'un compte.
        /// </summary>
        /// <param name="user">Le compte à modifier.</param>
        /// <param name="info">Les nouvelles informations du compte.</param>
        /// <returns>Le compte avec ses nouvelles informations.</returns>
        [Authorize]
        [RequireuserOwner]
        [HttpPut("{userId}/info")]
        public User UpdateInfo(User user, [FromBody] UserInfo info)
        {
            user.Name = info.Name;
            user.Surname = info.Surname;
            user.BirthDate = info.BirthDate;
            user.NationalId = info.NationalId;
            user.Gender = info.Gender;

            userRepository.Update(user);

            return user;
        }


        /// <summary>
        /// Permet de modifier l'adresse d'un compte.
        /// </summary>
        /// <param name="user">Le compte à modifier.</param>
        /// <param name="address">Les informations sur na nouvelle adresse du compte.</param>
        /// <returns>Le compte avec ses nouvelles informations d'adresse.</returns>
        [Authorize]
        [RequireuserOwner]
        [HttpPut("{userId}/address")]
        public User UpdateAddress(User user, [FromBody] Address address)
        {
            user.Country = address.Country;
            user.State = address.State;
            user.City = address.City;
            user.Street = address.Street;
            user.PostalCode = address.PostalCode;

            userRepository.Update(user);

            return user;
        }

        /// <summary>
        /// Permet de modifier le mot de passe d'un compte.
        /// </summary>
        /// <param name="user">Le compte à modifier.</param>
        /// <param name="model">Les informations sur na nouvelle adresse du compte.</param>
        /// <returns>Un <see>StatusCodeResult</see> de code 201 indiquant que le mot de passe 
        /// a été modifié.</returns>
        [Authorize]
        [RequireuserOwner]
        [HttpPut("{userId}/password")]
        public StatusCodeResult ChangePassword(User user, UpdatePasswordModel model)
        {
            if(PasswordVerificationResult.Success !=
                passwordHasher.VerifyHashedPassword(user, user.Password, model.CurrentPassword))
            {
                throw new InvalidValueException("Votre mot de passe actuel est incorrect");
            }
            string password = passwordHasher.HashPassword(user, model.NewPassword);

            user.Password = password;

            userRepository.Update(user);

            return StatusCode(202);
        }

        /// <summary>
        /// Pour réinitialiser le mot de passe d'un compte.
        /// Celà peut arriver si le propriétaire du compte perd son mot de passe.
        /// </summary>
        /// <param name="user">Le compte à modifier.</param>
        /// <param name="model">Contient le code de réinitialisation et le nouveau mot de passe.</param>
        /// <exception cref="InvalidValueException">Si le code de réinitialisation renseigné
        /// n'est pas celui assigné au compte.</exception>
        /// 
        /// <exception cref="InvalidOperationException"> Si le code de réinitialisation est expiré soit 
        /// 10 minutes après sa création.</exception>
        /// 
        /// <returns>Un <see>StatusCodeResult</see> de code 201 indiquant que le mot de passe 
        /// a été modifié.</returns>
        /// 
        [HttpPut("{userId}/password/reset")]
        public StatusCodeResult ResetPassword(User user, ResetPasswordModel model)
        {
            if(model.Code != user.ResetPasswordCode)
            {
                throw new InvalidValueException("Le code de réinitialisation de mot de passe est incorrect");
            }

            if(DateTime.Now.Subtract(user.ResetPasswordCodeCreateTime).TotalMinutes > 9.99)
            {
                throw new InvalidOperationException("Le code de réinitialisation de mot de passe est expirée");
            }


            string password = passwordHasher.HashPassword(user, model.NewPassword);

            user.Password = password;

            userRepository.Update(user);

            return StatusCode(202);
        }

        /// <summary>
        ///     Permet de vérifier que le mot de passe fourni est celui qui compte fourni.
        /// </summary>
        /// <param name="user">Le compte à vérifier.</param>
        /// <param name="password">Le mot de passe à vérifier.</param>
        /// <returns>
        ///     <code>true</code> Si le mot de passe est bien celui du compte.
        /// </returns>
        /// 
        
        [LoadUser]
        [HttpPut("{userId}/password/check")]
        public bool CheckPassword(User user, [FromForm] string password)
        {
            return PasswordVerificationResult.Success ==
                passwordHasher.VerifyHashedPassword(user, user.Password, password);
        }

        /// <summary>
        /// Permet de télécharger l'image d'un compte.
        /// </summary>
        /// <param name="user">Le compte dont on souhaite obtenir l'image.</param>
        /// <returns>Le fichier qui est l'image du compte.</returns>
        [HttpGet("{userId}/image")]
        public async Task<IActionResult> DownloadImage(User user)
        {
            return null;
        }


        /// <summary>
        /// Permet de changer l'image d'un compte.
        /// </summary>
        /// <param name="user">Le compte à modifier.</param>
        /// <param name="image">Fichier image venant d'un formulaire et qui est la nouvelle image.</param>
        ///
        /// <returns>
        ///     Un <see>StatusCodeResult</see> de code 201 indiquant que l'image a été modifié.
        /// </returns>
        [HttpPut("{userId}/image")]
        public async Task<StatusCodeResult> ChangeImage(User user, IFormFile image)
        {
            string fileName = $"{user.Id.ToString()}.{image.Name.Split('.').Last()}";
            string path = Path.Combine(Constant.USER_IMAGE_FOLDER, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            user.ImageName = fileName;

            userRepository.Update(user);
            return Ok();
        }
    }

    
}
