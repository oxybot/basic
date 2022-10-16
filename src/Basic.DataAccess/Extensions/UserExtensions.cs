// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Security.Cryptography;
using System.Text;

namespace Basic.Model
{
    /// <summary>
    /// Extension methods for the <see cref="User"/> class.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Changes the password of a user, updating its salt at the same time.
        /// </summary>
        /// <param name="user">The reference user.</param>
        /// <param name="newPassword">The new password, if any.</param>
        /// <remarks>
        /// If <paramref name="newPassword"/> is <c>null</c> or empty, the password of the user
        /// will be removed so that the user can't connect anymore via a direct account.
        /// </remarks>
        public static void ChangePassword(this User user, string newPassword)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                // User is set to not be able to connect any more
                user.Salt = null;
                user.Password = null;
                return;
            }

            using (var random = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[32];
                random.GetNonZeroBytes(data);
                user.Salt = Convert.ToBase64String(data);
            }

            user.Password = user.HashPassword(newPassword);
        }

        /// <summary>
        /// Computes the hash of a password for a user based on its currently defined salt.
        /// </summary>
        /// <param name="user">The reference user.</param>
        /// <param name="password">The password for which the salt has to be computed.</param>
        /// <returns>The hashed password for this user.</returns>
        /// <exception cref="ArgumentNullException">One of the arguments is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The user doesn't have a salt defined.</exception>
        public static string HashPassword(this User user, string password)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (user.Salt == null)
            {
                throw new ArgumentException("no salt defined for this user", nameof(user));
            }

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(user.Salt + password);
                var result = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(result);
            }
        }
    }
}
