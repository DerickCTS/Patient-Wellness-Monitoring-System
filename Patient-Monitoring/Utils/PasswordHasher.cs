using System.Security.Cryptography;

namespace Patient_Monitoring.Utils
{
    public class PasswordHasher
    {
        public static string Hash(string password)

        {

            var salt = RandomNumberGenerator.GetBytes(16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);

            var hash = pbkdf2.GetBytes(32);

            var result = new byte[1 + salt.Length + hash.Length];

            result[0] = 1;

            Buffer.BlockCopy(salt, 0, result, 1, salt.Length);

            Buffer.BlockCopy(hash, 0, result, 1 + salt.Length, hash.Length);

            return Convert.ToBase64String(result);

        }

        public static bool Verify(string password, string storedBase64)

        {

            try

            {

                var data = Convert.FromBase64String(storedBase64);

                if (data.Length != 1 + 16 + 32) return false;

                var salt = new byte[16];

                Buffer.BlockCopy(data, 1, salt, 0, 16);

                var storedHash = new byte[32];

                Buffer.BlockCopy(data, 1 + 16, storedHash, 0, 32);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);

                var hash = pbkdf2.GetBytes(32);

                return CryptographicOperations.FixedTimeEquals(hash, storedHash);

            }

            catch

            {

                return false;

            }

        }

    }
}


            