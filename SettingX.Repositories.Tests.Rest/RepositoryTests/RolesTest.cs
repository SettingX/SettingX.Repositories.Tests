using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;
using Xunit.Priority;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    [DefaultPriority(0)]
    public class RolesTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public RolesTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Write_Update_Read()
        {
            var role = new Role
            {
                Name = Guid.NewGuid().ToString(),
                RoleId = Guid.NewGuid().ToString(),
                KeyValues = new[] {new RoleKeyValue {HasFullAccess = false, RowKey = Guid.NewGuid().ToString()}}
            };

            await _client.RolesApi.SaveAsync(role);

            role.Name = Guid.NewGuid().ToString();
            role.KeyValues[0] = new RoleKeyValue {HasFullAccess = true, RowKey = Guid.NewGuid().ToString()};
            
            await _client.RolesApi.SaveAsync(role);

            var roleFromDbById = await _client.RolesApi.GetAsync(role.RoleId);
            var roleFromDbByName = await _client.RolesApi.GetByNameAsync(role.Name);
            
            Assert.True(Equal(role, roleFromDbById));
            Assert.True(Equal(role, roleFromDbByName));
        }
        
        [Fact]
        public async Task Write_Remove_Read()
        {
            var role = new Role
            {
                Name = Guid.NewGuid().ToString(),
                RoleId = Guid.NewGuid().ToString(),
                KeyValues = new[] {new RoleKeyValue {HasFullAccess = false, RowKey = Guid.NewGuid().ToString()}}
            };

            await _client.RolesApi.SaveAsync(role);
            
            await _client.RolesApi.RemoveAsync(role.RoleId);
            await _client.RolesApi.RemoveAsync(role.RoleId);
            
            var roleFromDbByName = await _client.RolesApi.GetByNameAsync(role.Name);
            var roleFromDbById = await _client.RolesApi.GetAsync(role.RoleId);
            
            Assert.Null(roleFromDbByName);
            Assert.Null(roleFromDbById);
        }
        
        [Fact]
        public async Task Write_Find()
        {
            var role1 = new Role
            {
                Name = Guid.NewGuid().ToString(),
                RoleId = Guid.NewGuid().ToString(),
                KeyValues = new[] {new RoleKeyValue {HasFullAccess = false, RowKey = Guid.NewGuid().ToString()}}
            };
            
            var role2 = new Role
            {
                Name = Guid.NewGuid().ToString(),
                RoleId = Guid.NewGuid().ToString(),
                KeyValues = new[] {new RoleKeyValue {HasFullAccess = false, RowKey = Guid.NewGuid().ToString()}}
            };
            
            var role3 = new Role
            {
                Name = Guid.NewGuid().ToString(),
                RoleId = Guid.NewGuid().ToString(),
                KeyValues = new[] {new RoleKeyValue {HasFullAccess = false, RowKey = Guid.NewGuid().ToString()}}
            };

            await _client.RolesApi.SaveAsync(role1);
            await _client.RolesApi.SaveAsync(role2);
            await _client.RolesApi.SaveAsync(role3);

            var roleFromDb = await _client.RolesApi.FindAsync(new List<string> {role1.RoleId, Guid.NewGuid().ToString()});
            
            Assert.Single(roleFromDb);
            Assert.True(Equal(role1, roleFromDb.Single()));;
        }
        
        [FactConditional, Priority(1)]
        public async Task Read_All()
        {
            var all = await _client.RolesApi.GetAllAsync();
            
            Assert.Equal(4, all.Count);
        }
        
        private static bool Equal(Role role1, Role role2)
        {
            return role1.RoleId == role2.RoleId &&
                   role1.Name == role2.Name &&
                   role1.KeyValues.All(x =>
                       role2.KeyValues.Any(y => y.HasFullAccess == x.HasFullAccess && y.RowKey == x.RowKey)) &&
                   role2.KeyValues.All(x =>
                       role1.KeyValues.Any(y => y.HasFullAccess == x.HasFullAccess && y.RowKey == x.RowKey));
        }
    }
}