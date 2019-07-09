using Everest.AspNetStartup.Core.Extensions;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Infrastruture;
using Everest.Identity.Infrastruture;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everest.AspNetStartup.Filters
{
    public class AccessTokenMiddleware
    {
        private RequestDelegate nextDelegate;
        private IRepository<Connection, long> connectionRepository;
        private AccessTokenValidator accessTokenValidator;

        public AccessTokenMiddleware(RequestDelegate nextDelegate, 
            IRepository<Connection, long> connectionRepository, AccessTokenValidator accessTokenValidator)
        {
            this.nextDelegate = nextDelegate;
            this.connectionRepository = connectionRepository;
            this.accessTokenValidator = accessTokenValidator;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            AuthorizationResult result = new AuthorizationResult();
            httpContext.Items["Authorization"] = result;
            try
            {
                string accessToken = httpContext.GetBearerToken();

                accessTokenValidator.Validate(accessToken);

                Connection connection = connectionRepository.First(a => a.AccessToken == accessToken);

                httpContext.Items["Authorization.Connection"] = connection;
                httpContext.Items["Authorization.User"] = connection.User;

                result.Successed = true;
                result.Connection = connection;


            }
            catch (Exception e)
            {
                result.Successed = false;
                result.Exception = e;
                while (result.Exception.InnerException != null)
                {
                    result.Exception = result.Exception.InnerException;
                }
                Console.Error.WriteLine("Access token is absent or is not valid");
            }

            await nextDelegate.Invoke(httpContext);
        }
    }


    public static class AccessTokenMiddlewareExtension
    {
        public static void UseAccessTokenAuthorization(this IApplicationBuilder app)
        {
            app.UseMiddleware<AccessTokenMiddleware>();
        }
    }

}
