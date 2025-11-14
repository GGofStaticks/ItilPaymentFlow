using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;
using ItilPaymentFlow.Domain.Tickets;
using ItilPaymentFlow.Domain.ValueObjects;


namespace ItilPaymentFlow.Domain.Users
{
    public sealed class User : AggregateRoot<Guid>
    {
        // теперь айди это гуид, унаследовано через AggregateRoot<Guid>

        public string Email { get; private set; } = null!;
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string? MiddleName { get; private set; }

        public string PasswordHash { get; private set; } = null!;
        public string? Role { get; private set; }

        public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>();

        private User() { } // для EF

        public User(Guid id, string email, string passwordHash, string? role,
            string firstName, string lastName, string? middleName = null)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        public static User Create(string email, string passwordHash, string? role,
            string firstName, string lastName, string? middleName = null)
        {
            return new User(Guid.NewGuid(), email, passwordHash, role, firstName, lastName, middleName);
        }

        public void UpdateEmail(string email) => Email = email;
        public void UpdatePasswordHash(string passwordHash) => PasswordHash = passwordHash;
        public void SetRole(string? role) => Role = role;
    }
}