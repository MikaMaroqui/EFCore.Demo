using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Companies.Demo.Entities;
using Companies.Demo.Models;
using EFCore.Demo.Db;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Demo.Services
{
    public interface IClientService
    {
        IEnumerable<Client> GetAll();

        IEnumerable<Client> GetAllWithProjects();

        Task<Client> GetAsync(Guid id);

        Task AddAsync(ClientCreateModel model);

        Task UpdateAsync(Guid id, ClientUpdateModel model);

        Task RemoveAsync(Guid id);
    }

    public class ClientService : IClientService
    {
        private readonly IDbRepository<Client> _repository;

        public ClientService(IDbRepository<Client> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Client> GetAll()
        {
            return _repository
                .GetQueryable()
                .ToList();
        }

        public IEnumerable<Client> GetAllWithProjects()
        {
            return _repository
                .GetQueryable()
                .Include(x => x.Projects)
                .ToList();
        }

        public async Task<Client> GetAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task AddAsync(ClientCreateModel model)
        {
            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = model.Name
            };

            await _repository.AddAsync(client);
        }

        public async Task UpdateAsync(Guid id, ClientUpdateModel model)
        {
            var client = await _repository.GetAsync(id);

            // TODO: Throw proper exception
            if(client is null)
                return;

            client.Name = model.Name;

            await _repository.SaveAsync(client);
        }

        public async Task RemoveAsync(Guid id)
        {
            await _repository.RemoveAsync(id);
        }
    }
}
