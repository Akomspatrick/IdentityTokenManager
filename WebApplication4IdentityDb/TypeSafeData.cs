using System.Collections;
using System.Collections.Specialized;
using System.Security.Claims;

namespace WebApplication4IdentityDb
{
    public class TypeSafeData
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string DesignerEngineer = "Designer_Engineer";
            public const string Assembler = "Assembler";
            public const string ProductionManager = "Production_Manager";
        }

        public static class DefaultPassword
        {
            public const string Admin = "admin";
            public const string User = "user";
            //public const string Contributor = "contributor";
        }

        public static class Features //or functions
        {
            /// <summary>
            /// Generate a list of dictions with key and value
            /// </summary>
            public static ListDictionary features = new ListDictionary { { "001", "Read" }, { "002", "Write" }, { "003", "Update" }, { "004", "Delete" } };

            public static List<Claim> GetFeaturesAsClaims()
            {
                var claims = new List<Claim>();

                foreach (DictionaryEntry menu in TypeSafeData.Features.features)
                {
                    claims.Add(new Claim(menu.Key.ToString(), menu.Value.ToString()));
                }


                return claims;
            }

        }
        public static class Contoller
        {
            public const string Student = "Student";
            public const string Teacher = "Teacher";
            public const string Module = "Module";
            public const string ClassRoom = "ClassRoom";
        }

        public static class Permissions
        {
            public const int None = 0;
            public const int Read = 1;
            public const int Write = 2;
            public const int Update = 3;
            public const int Delete = 4;
        }

        public static class Policies
        {
            public const string ReadPolicy = "ReadPolicy";
            public const string ReadAndWritePolicy = "AddAndReadPolicy";
            public const string FullControlPolicy = "FullControlPolicy";

            public const string GenericPolicy = "GenericPolicy";
        }
    }
}
