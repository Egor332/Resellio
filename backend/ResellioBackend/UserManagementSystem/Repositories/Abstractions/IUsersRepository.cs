﻿using ResellioBackend.UserManagementSystem.Models.Base;

namespace ResellioBackend.UserManagementSystem.Repositories.Abstractions
{
    public interface IUsersRepository<T> where T : UserBase
    {
        public Task<T?> GetByIdAsync(int id);
        public Task<T?> GetByEmailAsync(string email);
        public Task UpdateAsync(T entity);
        public Task AddAsync(T entity);
        public Task DeleteAsync(T entity);

    }
}
