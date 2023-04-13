namespace IndoAgri.IndoAgriData.Model
{
    using IndoAgri.IndoAgriData.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Core.Mapping;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class Entities : DbContext
    {
        public Entities(String connString)
            : base(connString)
        {
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }

            return 0;
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }

            return Task.FromResult(0);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }

            return Task.FromResult(0);
        }

        protected virtual void ThrowEnhancedValidationException(DbEntityValidationException e)
        {
            var errorMessages = e.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("; ", errorMessages);
            var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);
            throw new DbEntityValidationException(exceptionMessage, e.EntityValidationErrors);
        }

        public async Task BulkInsertAllAsync<T>(IEnumerable<T> entities)
        {
            using (var conn = new SqlConnection(Database.Connection.ConnectionString))
            {
                conn.Open();

                Type t = typeof(T);

                var bulkCopy = new SqlBulkCopy(conn)
                {
                    DestinationTableName = GetTableName(t)
                };

                var table = new DataTable();

                var properties = t.GetProperties().Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    Type propertyType = property.PropertyType;
                    if (propertyType.IsGenericType &&
                        propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    }

                    table.Columns.Add(new DataColumn(property.Name, propertyType));
                }

                foreach (var entity in entities)
                {
                    table.Rows.Add(
                        properties.Select(property => property.GetValue(entity, null) ?? DBNull.Value).ToArray());
                }

                bulkCopy.BulkCopyTimeout = 0;
                await bulkCopy.WriteToServerAsync(table);
            }
        }

        public void BulkInsertAll<T>(IEnumerable<T> entities)
        {
            using (var conn = new SqlConnection(Database.Connection.ConnectionString))
            {
                conn.Open();

                Type t = typeof(T);

                var bulkCopy = new SqlBulkCopy(conn)
                {
                    DestinationTableName = GetTableName(t)
                };

                var table = new DataTable();

                var properties = t.GetProperties().Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    Type propertyType = property.PropertyType;
                    if (propertyType.IsGenericType &&
                        propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    }

                    table.Columns.Add(new DataColumn(property.Name, propertyType));
                }

                foreach (var entity in entities)
                {
                    table.Rows.Add(
                        properties.Select(property => property.GetValue(entity, null) ?? DBNull.Value).ToArray());
                }

                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(table);
            }
        }



        public string GetTableName(Type type)
        {
            var metadata = ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace;
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);

            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);

            var table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            return (string)table.MetadataProperties["Table"].Value ?? table.Name;
        }
        public async Task SaveDataInTables(DataTable dataTable, string tablename)
        {
            if (dataTable.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(Database.Connection.ConnectionString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = tablename;
                        con.Open();
                        sqlBulkCopy.WriteToServer(dataTable);
                        con.Close();
                    }
                }
            }
        }

        public async Task DeleteAsyncBP(String ESTATE, String FromDate , String ToDate)
        {
        
            using (SqlConnection conn = new SqlConnection(Database.Connection.ConnectionString))
            {
                try
                {
                    conn.Open();

                    //using (SqlCommand cmd = new SqlCommand("Delete from ZTUAGRI_BP_RPT_Temp where LOADDATE BETWEEN convert(varchar(10),convert(date,@LOADDATEFROM),104) AND convert(varchar(10),convert(date,@LOADDATETO),104) AND ESTATE = @Estate;", conn))
                    using (SqlCommand cmd = new SqlCommand("Delete from ZTUAGRI_BP_RPT_Temp where BPDATE BETWEEN @LOADDATEFROM AND @LOADDATETO AND ESTATE = @Estate;", conn))
                    {
                        cmd.Parameters.Add("@Estate", System.Data.SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@LOADDATEFROM", System.Data.SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@LOADDATETO", System.Data.SqlDbType.VarChar, 10);
                        cmd.Parameters["@Estate"].Value = ESTATE;
                        cmd.Parameters["@LOADDATEFROM"].Value = FromDate;
                        cmd.Parameters["@LOADDATETO"].Value = ToDate;
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    //_logger.LogMessage(eLogLevel.ERROR, DateTime.Now, e.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public async Task DeleteAsyncSPBS(String ESTATE, String FromDate, String ToDate)
        {

            using (SqlConnection conn = new SqlConnection(Database.Connection.ConnectionString))
            {
                try
                {
                    conn.Open();

                    //using (SqlCommand cmd = new SqlCommand("Delete from ZTUAGRI_BP_RPT_Temp where LOADDATE BETWEEN convert(varchar(10),convert(date,@LOADDATEFROM),104) AND convert(varchar(10),convert(date,@LOADDATETO),104) AND ESTATE = @Estate;", conn))
                    using (SqlCommand cmd = new SqlCommand("Delete from ZTUAGRI_BP_RPT_Temp where LOADDATE BETWEEN @LOADDATEFROM AND @LOADDATETO AND ESTATE = @Estate;", conn))
                    {
                        cmd.Parameters.Add("@Estate", System.Data.SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@LOADDATEFROM", System.Data.SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@LOADDATETO", System.Data.SqlDbType.VarChar, 10);
                        cmd.Parameters["@Estate"].Value = ESTATE;
                        cmd.Parameters["@LOADDATEFROM"].Value = FromDate;
                        cmd.Parameters["@LOADDATETO"].Value = ToDate;
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    //_logger.LogMessage(eLogLevel.ERROR, DateTime.Now, e.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}