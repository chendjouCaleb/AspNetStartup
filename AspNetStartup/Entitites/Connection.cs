using Everest.AspNetStartup.Core.Binding;
using Everest.AspNetStartup.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Everest.AspNetStartup.Entities
{
    [ModelBinder(BinderType = typeof(ItemValueModelBinder))]
    public class Connection:Entity<long>
    { 
        public string Browser { get; set; }

        public string RemoteAddress { get; set; }

        public string OS { get; set; }

        public bool IsPersistent { get; set; }


        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsClosed { get => EndDate != null; }


        public virtual User User { get; set; }
        public string UserId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

    }
}
