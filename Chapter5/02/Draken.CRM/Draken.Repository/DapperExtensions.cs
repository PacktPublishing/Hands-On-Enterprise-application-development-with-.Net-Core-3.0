using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Draken.Repository
{
    public static class DapperExtensions
    {
        public static T Insert<T>(this IDbConnection connection, T model)
        {
            IEnumerable<T> result = SqlMapper.Query<T>(connection, GetInsertQuery<T>(model), model);
            return result.First();
        }

        public static T Update<T>(this IDbConnection connection, T model)
        {
            IEnumerable<T> result = SqlMapper.Query<T>(connection, GetUpdateQuery<T>(model), model);
            return result.First();
        }

        private static string GetInsertQuery<T>(T model)
        {
            PropertyInfo[] props = model.GetType().GetProperties();
            string[] columns = props.Select(p => p.Name).Where(s => s != "Id").ToArray();

            return string.Format("INSERT INTO {0} ({1}) OUTPUT inserted.Id VALUES (@{2})",
                                 model.GetType().Name,
                                 string.Join(",", columns),
                                 string.Join(",@", columns));
        }

        private static string GetUpdateQuery<T>(T model)
        {
            PropertyInfo[] props = model.GetType().GetProperties();
            string[] columns = props.Select(p => p.Name).ToArray();

            var parameters = columns.Select(name => name + "=@" + name).ToList();

            return string.Format("UPDATE {0} SET {1} WHERE Id=@Id", model.GetType().Name, string.Join(",", parameters));
        }
    }
}
