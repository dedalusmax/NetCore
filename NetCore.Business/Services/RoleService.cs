using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetCore.Business.Models;
using NetCore.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities = NetCore.Data.Entities;

namespace NetCore.Business.Services
{
    public class RoleService
    {
        private readonly IGenericRepository<Entities.Role> _roleRepository;

        public RoleService(IGenericRepository<Entities.Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        #region Public methods
        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepository
                .AsReadOnly()
                .ProjectTo<Role>()
                .ToListAsync();
        }
        #endregion

        #region Internal methods

        //internal static void RegisterMappings(Profile profile)
        //{
        //    profile.CreateMap<Entities.Role, Role>()
        //        .ForMember(_ => _.ParentRoleName, _ => _.MapFrom(__ => __.ParentRole.Name))
        //        .ForMember(_ => _.ParentRoleDisplayName, _ => _.MapFrom(__ => __.ParentRole.DisplayName));
        //}

        #endregion
    }
}
