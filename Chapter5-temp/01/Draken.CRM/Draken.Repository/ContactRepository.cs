using Dapper;
using Draken.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Draken.Repository
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll();
        Contact Get(int id);
        void Create(Contact contact);
        void Delete(int id);
        void Update(Contact contact);
    }

    public class ContactRepository : IContactRepository
    {
        private readonly IConfiguration configuration;

        public ContactRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Create(Contact contact)
        {
            using (IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                const string query = @"INSERT INTO [Contact] (FirstName, LastName, Designation, Email, BusinessPhone, 
                                        MobilePhone, Status, LinkedIn, Street1, Street2, City, State, Country, 
                                        ZipCode, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
                                        (@FirstName, @LastName, @Designation, @Email, @BusinessPhone, 
                                        @MobilePhone, @Status, @LinkedIn, @Street1, @Street2, @City, @State, @Country, 
                                        @ZipCode, @CreatedDate, @ModifiedDate, @CreatedBy, @ModifiedBy)";

                db.Execute(query, new
                {
                    contact.FirstName,
                    contact.LastName,
                    contact.Designation,
                    contact.Email,
                    contact.BusinessPhone,
                    contact.MobilePhone,
                    contact.Status,
                    contact.LinkedIn,
                    contact.Street1,
                    contact.Street2,
                    contact.City,
                    contact.State,
                    contact.Country,
                    contact.ZipCode,
                    CreatedDate = DateTime.Now,
                    Modifieddate = DateTime.Now,
                    CreatedBy = "System",
                    ModifiedBy = "System"
                });
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                const string query = @"DELETE FROM 
                    [Contact] 
                    
                    WHERE Id = @id";

                db.Execute(query, new { id });
            }
        }

        public Contact Get(int id)
        {
            using (IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                const string query = @"SELECT *
                                            FROM [Contact] WHERE id = @id;";

                return db.QuerySingleOrDefault<Contact>(query, new { id });
            }
        }

        public IEnumerable<Contact> GetAll()
        {
            using (IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                const string query = @"SELECT *
                                            FROM [Contact];";

                return db.Query<Contact>(query);
            }
        }

        public void Update(Contact contact)
        {
            using (IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                const string query = @"UPDATE [Contact] SET FirstName = @FirstName, 
                                        LastName = @LastName, 
                                        Designation = @Designation, 
                                        Email = @Email, 
                                        BusinessPhone = @BusinessPhone, 
                                        MobilePhone = @MobilePhone, 
                                        Status = @Status, 
                                        LinkedIn = @LinkedIn, 
                                        Street1 = @Street1, 
                                        Street2 = @Street2, 
                                        City = @City, 
                                        State = @State, 
                                        Country = @Country, 
                                        ZipCode = @ZipCode, 
                                        CreatedDate = @CreatedDate, 
                                        ModifiedDate = @ModifiedDate, 
                                        CreatedBy = @CreatedBy, 
                                        ModifiedBy = @ModifiedBy
                                     WHERE Id = @Id";
                    
                db.Execute(query, new
                {
                    contact.Id,
                    contact.FirstName,
                    contact.LastName,
                    contact.Designation,
                    contact.Email,
                    contact.BusinessPhone,
                    contact.MobilePhone,
                    contact.Status,
                    contact.LinkedIn,
                    contact.Street1,
                    contact.Street2,
                    contact.City,
                    contact.State,
                    contact.Country,
                    contact.ZipCode,
                    CreatedDate = DateTime.Now,
                    Modifieddate = DateTime.Now,
                    CreatedBy = "System",
                    ModifiedBy = "System"
                });
            }
        }
    }

}
