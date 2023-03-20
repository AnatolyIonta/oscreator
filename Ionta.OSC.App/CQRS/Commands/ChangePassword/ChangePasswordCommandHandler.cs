using Ionta.OSC.App.Services;
using Ionta.OSC.App.Services.HashingPassword;
using MediatR;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IOscStorage _stroe;
        private readonly IUserProvider _userProvider;
        public ChangePasswordCommandHandler(IOscStorage store, IUserProvider userProvider) 
        { 
            _stroe = store;
            _userProvider = userProvider;
        }
        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var id = _userProvider.GetCurrentId();
            var user = _stroe.Users.FirstOrDefault(e => e.Id == id);
            if(user == null) { throw new Exception("Юзер не найден!"); }
            var oldPassword = HashingPasswordService.Hash(request.OldPassword);
            if (oldPassword == user.Password)
            {
                var password = HashingPasswordService.Hash(request.Password);
                user.Password = password;
                await _stroe.SaveChangesAsync();
                return true;
            }
            throw new Exception("Пароли разные!");
        }
    }
}
 