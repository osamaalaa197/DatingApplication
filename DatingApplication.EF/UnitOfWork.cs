using DatingApplication.Core;
using DatingApplication.Core.IRepository;
using DatingApplication.Core.Models;
using DatingApplication.EF.Data;
using DatingApplication.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public IBaseRepository<UserLike> UserLike {  get; private set; }
        public IBaseRepository<ApplicationUser> ApplicationUser { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public UnitOfWork(ApplicationContext context) 
        {
            _context=context;
            UserLike=new BaseRepository<UserLike>(_context);
            ApplicationUser=new BaseRepository<ApplicationUser>(_context);
        }
    }
}
