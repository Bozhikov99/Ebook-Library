using Core.Queries.User;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers.UserHandlers
{
    public class GetUserIdByUsernameHandler : IRequestHandler<GetUserIdByUsernameQuery, string>
    {
        private readonly IRepository repository;

        public GetUserIdByUsernameHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Handle(GetUserIdByUsernameQuery request, CancellationToken cancellationToken)
        {
            string username = request.Username;

            string userId = await repository.AllReadonly<User>(u => u.UserName == username)
                .Select(u => u.Id)
                .FirstAsync();

            return userId;
        }
    }
}
