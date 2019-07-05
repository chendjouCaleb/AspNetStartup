using Everest.AspNetStartup.Core.Binding;
using Everest.AspNetStartup.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Everest.AspNetStartup.Entities
{
    [ModelBinder(BinderType = typeof(ItemValueModelBinder))]
    public class User:Entity<string>
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Username { get; set; }

        public string Name { get; set; }

        public string  Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string NationalId { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ImageName { get; set; }

        public string ImageURL { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public string APIURL { get; set; }

        public string WebURL { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string ResetPasswordCode { get; set; }

        [JsonIgnore]
        public DateTime ResetPasswordCodeCreateTime { get; set; }

        public string AboutMe { get; set; }

        public string Website { get; set; }

        [JsonIgnore]
        public virtual IList<Connection> Connections { get; set; }

        [JsonIgnore]
        public virtual List<UserRole> UserRoles { get; set; }
    }
}
