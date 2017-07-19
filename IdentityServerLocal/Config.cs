using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using IdentityServer4.Models;
using Newtonsoft.Json;

namespace IdentityServerLocal
{   
    public class Config
    {
        private static List<ApiResource> ApiResources { get; }
        private static List<Client> Clients { get; }
        private static bool DataLoadError { get; set; }

        static Config()
        {
            try
            {
                ApiResources = new List<ApiResource>();
                Clients = new List<Client>();
                DataLoadError = false;

                var content = File.ReadAllText("auth-data.json");
                var authData = JsonConvert.DeserializeObject<AuthData>(content);

                foreach (var apiResource in authData.ApiResources)
                {
                    ApiResources.Add(new ApiResource(apiResource.Name, apiResource.Value));
                }

                foreach (var clientData in authData.Clients)
                {
                    var client = new Client
                    {
                        ClientId = clientData.ClientId,
                        AllowedGrantTypes = getGrantTypes(clientData.AllowedGrantType),
                        AllowedScopes = clientData.AllowedScopes,
                    };

                    foreach (var clientDataClientSecret in clientData.ClientSecrets)
                    {
                        client.ClientSecrets.Add(new Secret(clientDataClientSecret.Sha256()));
                    }

                    foreach (var clientDataClaim in clientData.Claims)
                    {
                        client.Claims.Add(new Claim(clientDataClaim.Name, clientDataClaim.Value));
                    }

                    Clients.Add(client);
                }
            }
            catch (Exception e)
            {
                DataLoadError = true;
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            if (DataLoadError)
            {
                return new List<ApiResource>
                {
                    new ApiResource("Service.Read", "Service Read access"),
                    new ApiResource("Service.Write", "Service Write access")
                };
            }

            return ApiResources;
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            if (DataLoadError)
            {
                return new List<Client>
                {
                    new Client
                    {
                        ClientId = "client",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,

                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = {"Service.Read","Service.Write" },
                        Claims = new List<Claim>()
                        {
                            new Claim("instanceid","0")
                        }
                    }
                };
            }

            return Clients;
        }

        private static IEnumerable<string> getGrantTypes(string typeName)
        {
            switch (typeName)
            {
                case "ClientCredentials":
                    return GrantTypes.ClientCredentials;
                    
                case "Code":
                    return GrantTypes.Code;

                case "CodeAndClientCredentials":
                    return GrantTypes.CodeAndClientCredentials;

                case "Hybrid":
                    return GrantTypes.Hybrid;

                case "HybridAndClientCredentials":
                    return GrantTypes.HybridAndClientCredentials;

                case "Implicit":
                    return GrantTypes.Implicit;

                case "ImplicitAndClientCredentials":
                    return GrantTypes.ImplicitAndClientCredentials;

                case "ResourceOwnerPassword":
                    return GrantTypes.ResourceOwnerPassword;

                case "ResourceOwnerPasswordAndClientCredentials":
                    return GrantTypes.ResourceOwnerPasswordAndClientCredentials;

                default:
                    return new List<string>();
            }
        }
    }
}
