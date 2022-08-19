using DataAccess.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AdminSite.Test.Repository
{
    class MockDatabaseRepo : IDisposable
    {
        // to avoid redundant calls;
        private bool disposedValue = false;
        public ApplicationDbContext CreateInMemoryContext()
        {
            string newDatabaseName = "BobsBookstoreDatabase_" + DateTime.Now.ToFileTimeUtc();  

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: newDatabaseName)
            .Options;
            var context = new ApplicationDbContext(options);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                
                context.Database.EnsureCreated();
            }

            return context;
        }
        

       
        

        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
