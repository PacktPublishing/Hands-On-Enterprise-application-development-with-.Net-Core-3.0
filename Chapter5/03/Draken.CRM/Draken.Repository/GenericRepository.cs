﻿using Dapper;
using Draken.Domain.Entities;
using Draken.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Draken.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        internal readonly IConfiguration configuration;
        
        //This represents the table name. Note that all our Model names are directly related to the table.
        internal readonly string entityName;

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public GenericRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.entityName = typeof(T).Name;
        }

        public void Add(T model)
        {
            using (IDbConnection connection = Connection)
            {
                model = connection.Insert<T>(model);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection connection = Connection)
            {
                connection.Execute("DELETE FROM " + entityName + " WHERE Id=@Id", new { Id = id });
            }
        }

        public T Get(int id)
        {
            T item = default;

            using (IDbConnection connection = Connection)
            {
                item = connection.Query<T>("SELECT * FROM " + entityName + " WHERE Id=@Id", new { id }).SingleOrDefault();
            }

            return item;
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> items = null;

            using (IDbConnection connection = Connection)
            {
                items = connection.Query<T>("SELECT * FROM " + entityName);
            }

            return items;
        }

        public void Update(T model)
        {
            using (IDbConnection connection = Connection)
            {
                model = connection.Update<T>(model);
            }
        }
    }
}
