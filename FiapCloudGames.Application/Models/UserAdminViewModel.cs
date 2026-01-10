using FiapCloudGames.Domain.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapCloudGames.Application.Models
{
    public record UserAdminViewModel(
        Guid Id,
        string Name,
        string Email,
        string Role,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt
    )
    {
        public static UserAdminViewModel FromEntity(User user)
            => new(
                user.Id,
                user.Name,
                user.Email.Address,
                user.Role.Value,
                user.IsActive,
                user.CreatedAt,
                user.UpdatedAt
            );
    }
}
