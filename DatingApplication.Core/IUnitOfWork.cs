using DatingApplication.Core.IRepository;
using DatingApplication.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core
{
    public interface IUnitOfWork
    {
        IBaseRepository<UserLike> UserLike {  get; }
        IBaseRepository<ApplicationUser> ApplicationUser { get; }

        int Complete();

    }
}
