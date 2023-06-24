﻿// Licence file C:\Users\asgha\Documents\ReversePOCO.txt not found.
// Please obtain your licence file at www.ReversePOCO.co.uk, and place it in your documents folder shown above.
// Defaulting to Trial version.
// <auto-generated>
// ReSharper disable All

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Database
{
    #region Database context interface

    // ****************************************************************************************************
    // This is not a commercial licence, therefore only a few tables/views/stored procedures are generated.
    // ****************************************************************************************************

    public interface IMyDbContext : IDisposable
    {
        DbSet<EventData> EventDatas { get; set; } // EventData
        DbSet<User> Users { get; set; } // Users

        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
        DatabaseFacade Database { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        string ToString();

        EntityEntry Add(object entity);
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        Task AddRangeAsync(params object[] entities);
        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
        void AddRange(IEnumerable<object> entities);
        void AddRange(params object[] entities);

        EntityEntry Attach(object entity);
        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
        void AttachRange(IEnumerable<object> entities);
        void AttachRange(params object[] entities);

        EntityEntry Entry(object entity);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;
        ValueTask<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class;
        ValueTask<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;
        ValueTask<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken);
        ValueTask<object> FindAsync(Type entityType, params object[] keyValues);
        object Find(Type entityType, params object[] keyValues);

        EntityEntry Remove(object entity);
        EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
        void RemoveRange(IEnumerable<object> entities);
        void RemoveRange(params object[] entities);

        EntityEntry Update(object entity);
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        void UpdateRange(IEnumerable<object> entities);
        void UpdateRange(params object[] entities);

        IQueryable<TResult> FromExpression<TResult> (Expression<Func<IQueryable<TResult>>> expression);

        // Stored Procedures
        List<SpGetUserEventsReturnModel> SpGetUserEvents(string pUserId);
        List<SpGetUserEventsReturnModel> SpGetUserEvents(string pUserId, out int procResult);
        Task<List<SpGetUserEventsReturnModel>> SpGetUserEventsAsync(string pUserId);

        int SpSaveUserEvent(string pUserId, string pTitle, string pInfo, string pStart, string pEnd, string pColour);
        // SpSaveUserEventAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

    }

    #endregion

    #region Database context

    // ****************************************************************************************************
    // This is not a commercial licence, therefore only a few tables/views/stored procedures are generated.
    // ****************************************************************************************************

    public class MyDbContext : DbContext, IMyDbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<EventData> EventDatas { get; set; } // EventData
        public DbSet<User> Users { get; set; } // Users

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(local);Initial Catalog=ScheduleMaster;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=false;TrustServerCertificate=true");
            }
        }

        public bool IsSqlParameterNull(SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as INullable;
            if (nullableValue != null)
                return nullableValue.IsNull;
            return (sqlValue == null || sqlValue == DBNull.Value);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EventDataConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<SpGetUserEventsReturnModel>().HasNoKey();
        }


        // Stored Procedures
        public List<SpGetUserEventsReturnModel> SpGetUserEvents(string pUserId)
        {
            int procResult;
            return SpGetUserEvents(pUserId, out procResult);
        }

        public List<SpGetUserEventsReturnModel> SpGetUserEvents(string pUserId, out int procResult)
        {
            var pUserIdParam = new SqlParameter { ParameterName = "@p_UserId", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = pUserId, Size = 256 };
            if (pUserIdParam.Value == null)
                pUserIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            const string sqlCommand = "EXEC @procResult = [dbo].[spGetUserEvents] @p_UserId";
            var procResultData = Set<SpGetUserEventsReturnModel>()
                .FromSqlRaw(sqlCommand, pUserIdParam, procResultParam)
                .ToList();

            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public async Task<List<SpGetUserEventsReturnModel>> SpGetUserEventsAsync(string pUserId)
        {
            var pUserIdParam = new SqlParameter { ParameterName = "@p_UserId", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = pUserId, Size = 256 };
            if (pUserIdParam.Value == null)
                pUserIdParam.Value = DBNull.Value;

            const string sqlCommand = "EXEC [dbo].[spGetUserEvents] @p_UserId";
            var procResultData = await Set<SpGetUserEventsReturnModel>()
                .FromSqlRaw(sqlCommand, pUserIdParam)
                .ToListAsync();

            return procResultData;
        }

        public int SpSaveUserEvent(string pUserId, string pTitle, string pInfo, string pStart, string pEnd, string pColour)
        {
            var pUserIdParam = new SqlParameter { ParameterName = "@p_UserId", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = pUserId, Size = 256 };
            if (pUserIdParam.Value == null)
                pUserIdParam.Value = DBNull.Value;

            var pTitleParam = new SqlParameter { ParameterName = "@p_Title", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = pTitle, Size = 256 };
            if (pTitleParam.Value == null)
                pTitleParam.Value = DBNull.Value;

            var pInfoParam = new SqlParameter { ParameterName = "@p_Info", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = pInfo, Size = 256 };
            if (pInfoParam.Value == null)
                pInfoParam.Value = DBNull.Value;

            var pStartParam = new SqlParameter { ParameterName = "@p_Start", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = pStart, Size = 256 };
            if (pStartParam.Value == null)
                pStartParam.Value = DBNull.Value;

            var pEndParam = new SqlParameter { ParameterName = "@p_End", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = pEnd, Size = 256 };
            if (pEndParam.Value == null)
                pEndParam.Value = DBNull.Value;

            var pColourParam = new SqlParameter { ParameterName = "@p_Colour", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = pColour, Size = 256 };
            if (pColourParam.Value == null)
                pColourParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            Database.ExecuteSqlRaw("EXEC @procResult = [dbo].[spSaveUserEvent] @p_UserId, @p_Title, @p_Info, @p_Start, @p_End, @p_Colour", pUserIdParam, pTitleParam, pInfoParam, pStartParam, pEndParam, pColourParam, procResultParam);

            return (int)procResultParam.Value;
        }

        // SpSaveUserEventAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

    }

    #endregion

    #region Database context factory

    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            return new MyDbContext();
        }
    }

    #endregion

    #region Fake Database context

    // ****************************************************************************************************
    // This is not a commercial licence, therefore only a few tables/views/stored procedures are generated.
    // ****************************************************************************************************

    public class FakeMyDbContext : IMyDbContext
    {
        public DbSet<EventData> EventDatas { get; set; } // EventData
        public DbSet<User> Users { get; set; } // Users

        public FakeMyDbContext()
        {
            _database = new FakeDatabaseFacade(new MyDbContext());

            EventDatas = new FakeDbSet<EventData>("EventDataId", "UserId", "Title", "Start", "End", "Colour");
            Users = new FakeDbSet<User>("UserId");

        }

        public int SaveChangesCount { get; private set; }
        public virtual int SaveChanges()
        {
            ++SaveChangesCount;
            return 1;
        }

        public virtual int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ++SaveChangesCount;
            return Task<int>.Factory.StartNew(() => 1, cancellationToken);
        }
        public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            ++SaveChangesCount;
            return Task<int>.Factory.StartNew(x => 1, acceptAllChangesOnSuccess, cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private DatabaseFacade _database;
        public DatabaseFacade Database { get { return _database; } }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry Add(object entity)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual Task AddRangeAsync(params object[] entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public virtual async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public virtual async ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public virtual void AddRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        public virtual void AddRange(params object[] entities)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry Attach(object entity)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual void AttachRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        public virtual void AttachRange(params object[] entities)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry Entry(object entity)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Find<TEntity>(params object[] keyValues) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual ValueTask<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual ValueTask<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual ValueTask<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual ValueTask<object> FindAsync(Type entityType, params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public virtual object Find(Type entityType, params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry Remove(object entity)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveRange(params object[] entities)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry Update(object entity)
        {
            throw new NotImplementedException();
        }

        public virtual EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateRange(params object[] entities)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TResult> FromExpression<TResult> (Expression<Func<IQueryable<TResult>>> expression)
        {
            throw new NotImplementedException();
        }


        // Stored Procedures

        public DbSet<SpGetUserEventsReturnModel> SpGetUserEventsReturnModel { get; set; }
        public List<SpGetUserEventsReturnModel> SpGetUserEvents(string pUserId)
        {
            int procResult;
            return SpGetUserEvents(pUserId, out procResult);
        }

        public List<SpGetUserEventsReturnModel> SpGetUserEvents(string pUserId, out int procResult)
        {
            procResult = 0;
            return new List<SpGetUserEventsReturnModel>();
        }

        public Task<List<SpGetUserEventsReturnModel>> SpGetUserEventsAsync(string pUserId)
        {
            int procResult;
            return Task.FromResult(SpGetUserEvents(pUserId, out procResult));
        }

        public int SpSaveUserEvent(string pUserId, string pTitle, string pInfo, string pStart, string pEnd, string pColour)
        {
            return 0;
        }

        // SpSaveUserEventAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)
    }

    #endregion

    #region Fake DbSet

    // ************************************************************************
    // Fake DbSet
    // Implementing Find:
    //      The Find method is difficult to implement in a generic fashion. If
    //      you need to test code that makes use of the Find method it is
    //      easiest to create a test DbSet for each of the entity types that
    //      need to support find. You can then write logic to find that
    //      particular type of entity, as shown below:
    //      public class FakeBlogDbSet : FakeDbSet<Blog>
    //      {
    //          public override Blog Find(params object[] keyValues)
    //          {
    //              var id = (int) keyValues.Single();
    //              return this.SingleOrDefault(b => b.BlogId == id);
    //          }
    //      }
    //      Read more about it here: https://msdn.microsoft.com/en-us/data/dn314431.aspx
    public class FakeDbSet<TEntity> :
        DbSet<TEntity>,
        IQueryable<TEntity>,
        IAsyncEnumerable<TEntity>,
        IListSource,
        IResettableService
        where TEntity : class
    {
        private readonly PropertyInfo[] _primaryKeys;
        private ObservableCollection<TEntity> _data;
        private IQueryable _query;
        public override IEntityType EntityType { get; }

        public FakeDbSet()
        {
            _primaryKeys = null;
            _data        = new ObservableCollection<TEntity>();
            _query       = _data.AsQueryable();
        }

        public FakeDbSet(params string[] primaryKeys)
        {
            _primaryKeys = typeof(TEntity).GetProperties().Where(x => primaryKeys.Contains(x.Name)).ToArray();
            _data        = new ObservableCollection<TEntity>();
            _query       = _data.AsQueryable();
        }

        public override TEntity Find(params object[] keyValues)
        {
            if (_primaryKeys == null)
                throw new ArgumentException("No primary keys defined");
            if (keyValues.Length != _primaryKeys.Length)
                throw new ArgumentException("Incorrect number of keys passed to Find method");

            var keyQuery = this.AsQueryable();
            keyQuery = keyValues
                .Select((t, i) => i)
                .Aggregate(keyQuery,
                    (current, x) =>
                        current.Where(entity => _primaryKeys[x].GetValue(entity, null).Equals(keyValues[x])));

            return keyQuery.SingleOrDefault();
        }

        public override ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            return new ValueTask<TEntity>(Task<TEntity>.Factory.StartNew(() => Find(keyValues), cancellationToken));
        }

        public override ValueTask<TEntity> FindAsync(params object[] keyValues)
        {
            return new ValueTask<TEntity>(Task<TEntity>.Factory.StartNew(() => Find(keyValues)));
        }

        public override EntityEntry<TEntity> Add(TEntity entity)
        {
            _data.Add(entity);
            return null;
        }

        public override ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return new ValueTask<EntityEntry<TEntity>>(Task<EntityEntry<TEntity>>.Factory.StartNew(() => Add(entity), cancellationToken));
        }

        public override void AddRange(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            foreach (var entity in entities)
                _data.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            foreach (var entity in entities)
                _data.Add(entity);
        }

        public override Task AddRangeAsync(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            return Task.Factory.StartNew(() => AddRange(entities));
        }

        public override Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            return Task.Factory.StartNew(() => AddRange(entities), cancellationToken);
        }

        public override EntityEntry<TEntity> Attach(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            return Add(entity);
        }

        public override void AttachRange(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            AddRange(entities);
        }

        public override void AttachRange(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            AddRange(entities);
        }

        public override EntityEntry<TEntity> Remove(TEntity entity)
        {
            _data.Remove(entity);
            return null;
        }

        public override void RemoveRange(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            foreach (var entity in entities.ToList())
                _data.Remove(entity);
        }

        public override void RemoveRange(IEnumerable<TEntity> entities)
        {
            RemoveRange(entities.ToArray());
        }

        public override EntityEntry<TEntity> Update(TEntity entity)
        {
            _data.Remove(entity);
            _data.Add(entity);
            return null;
        }

        public override void UpdateRange(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            RemoveRange(entities);
            AddRange(entities);
        }

        public override void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            var array = entities.ToArray();        RemoveRange(array);
            AddRange(array);
        }

        bool IListSource.ContainsListCollection => true;

        public IList GetList()
        {
            return _data;
        }

        IList IListSource.GetList()
        {
            return _data;
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new FakeDbAsyncQueryProvider<TEntity>(_data); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new FakeDbAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());
        }

        public void ResetState()
        {
            _data  = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        public Task ResetStateAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Factory.StartNew(() => ResetState());
        }
    }

    public class FakeDbAsyncQueryProvider<TEntity> : FakeQueryProvider<TEntity>, IAsyncEnumerable<TEntity>, IAsyncQueryProvider
    {
        public FakeDbAsyncQueryProvider(Expression expression) : base(expression)
        {
        }

        public FakeDbAsyncQueryProvider(IEnumerable<TEntity> enumerable) : base(enumerable)
        {
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                .GetMethods()
                .First(method => method.Name == nameof(IQueryProvider.Execute) && method.IsGenericMethod)
                .MakeGenericMethod(expectedResultType)
                .Invoke(this, new object[] { expression });

            return (TResult) typeof(Task).GetMethod(nameof(Task.FromResult))
                ?.MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult });
        }

        public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new FakeDbAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());
        }
    }

    public class FakeDbAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public FakeDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
        }

        public FakeDbAsyncEnumerable(Expression expression)
            : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new FakeDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return GetAsyncEnumerator(cancellationToken);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.AsEnumerable().GetEnumerator();
        }
    }

    public class FakeDbAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public FakeDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask(Task.CompletedTask);
        }
    }

    public abstract class FakeQueryProvider<T> : IOrderedQueryable<T>, IQueryProvider
    {
        private IEnumerable<T> _enumerable;

        protected FakeQueryProvider(Expression expression)
        {
            Expression = expression;
        }

        protected FakeQueryProvider(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
            Expression = enumerable.AsQueryable().Expression;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            if (expression is MethodCallExpression m)
            {
                var resultType = m.Method.ReturnType; // it should be IQueryable<T>
                var tElement = resultType.GetGenericArguments().First();
                return (IQueryable) CreateInstance(tElement, expression);
            }

            return CreateQuery<T>(expression);
        }

        public IQueryable<TEntity> CreateQuery<TEntity>(Expression expression)
        {
            return (IQueryable<TEntity>) CreateInstance(typeof(TEntity), expression);
        }

        private object CreateInstance(Type tElement, Expression expression)
        {
            var queryType = GetType().GetGenericTypeDefinition().MakeGenericType(tElement);
            return Activator.CreateInstance(queryType, expression);
        }

        public object Execute(Expression expression)
        {
            return CompileExpressionItem<object>(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return CompileExpressionItem<TResult>(expression);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (_enumerable == null) _enumerable = CompileExpressionItem<IEnumerable<T>>(Expression);
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_enumerable == null) _enumerable = CompileExpressionItem<IEnumerable<T>>(Expression);
            return _enumerable.GetEnumerator();
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider => this;

        private static TResult CompileExpressionItem<TResult>(Expression expression)
        {
            var visitor = new FakeExpressionVisitor();
            var body = visitor.Visit(expression);
            var f = Expression.Lambda<Func<TResult>>(body ?? throw new InvalidOperationException(string.Format("{0} is null", nameof(body))), (IEnumerable<ParameterExpression>) null);
            return f.Compile()();
        }
    }

    public class FakeExpressionVisitor : ExpressionVisitor
    {
    }

    public class FakeDatabaseFacade : DatabaseFacade
    {
        public FakeDatabaseFacade(DbContext context) : base(context)
        {
        }

        public override bool EnsureCreated()
        {
            return true;
        }

        public override Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(EnsureCreated());
        }

        public override bool EnsureDeleted()
        {
            return true;
        }

        public override Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(EnsureDeleted());
        }

        public override bool CanConnect()
        {
            return true;
        }

        public override Task<bool> CanConnectAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(CanConnect());
        }

        public override IDbContextTransaction BeginTransaction()
        {
            return new FakeDbContextTransaction();
        }

        public override Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(BeginTransaction());
        }

        public override void CommitTransaction()
        {
        }

        public override Task CommitTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public override void RollbackTransaction()
        {
        }

        public override Task RollbackTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public override IExecutionStrategy CreateExecutionStrategy()
        {
            return null;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    public class FakeDbContextTransaction : IDbContextTransaction
    {
        public Guid TransactionId => Guid.NewGuid();
        public void Commit() { }
        public void Rollback() { }
        public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask;
        public Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask;
        public void Dispose() { }
        public ValueTask DisposeAsync() => default;
    }

    #endregion

    #region POCO classes

    // ****************************************************************************************************
    // This is not a commercial licence, therefore only a few tables/views/stored procedures are generated.
    // ****************************************************************************************************

    // EventData
    public class EventData
    {
        public string EventDataId { get; set; } // EventDataId (Primary key) (length: 256)
        public string UserId { get; set; } // UserId (Primary key) (length: 256)
        public string Title { get; set; } // Title (Primary key) (length: 256)
        public string Info { get; set; } // Info (length: 256)
        public string Start { get; set; } // Start (Primary key) (length: 256)
        public string End { get; set; } // End (Primary key) (length: 256)
        public string Colour { get; set; } // Colour (Primary key) (length: 256)

        // Foreign keys

        /// <summary>
        /// Parent User pointed by [EventData].([UserId]) (FK_EventData_Users)
        /// </summary>
        public User User { get; set; } // FK_EventData_Users
    }

    // Users
    public class User
    {
        public string UserId { get; set; } // UserId (Primary key) (length: 256)
        public string Username { get; set; } // Username (length: 256)
        public string Password { get; set; } // Password (length: 256)

        // Reverse navigation

        /// <summary>
        /// Child EventDatas where [EventData].[UserId] point to this entity (FK_EventData_Users)
        /// </summary>
        public ICollection<EventData> EventDatas { get; set; } // EventData.FK_EventData_Users

        public User()
        {
            EventDatas = new List<EventData>();
        }
    }


    #endregion

    #region POCO Configuration

    // ****************************************************************************************************
    // This is not a commercial licence, therefore only a few tables/views/stored procedures are generated.
    // ****************************************************************************************************

    // EventData
    public class EventDataConfiguration : IEntityTypeConfiguration<EventData>
    {
        public void Configure(EntityTypeBuilder<EventData> builder)
        {
            builder.ToTable("EventData", "dbo");
            builder.HasKey(x => new { x.EventDataId, x.UserId, x.Title, x.Start, x.End, x.Colour });

            builder.Property(x => x.EventDataId).HasColumnName(@"EventDataId").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256).ValueGeneratedNever();
            builder.Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256).ValueGeneratedNever();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256).ValueGeneratedNever();
            builder.Property(x => x.Info).HasColumnName(@"Info").HasColumnType("nvarchar(256)").IsRequired(false).HasMaxLength(256);
            builder.Property(x => x.Start).HasColumnName(@"Start").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256).ValueGeneratedNever();
            builder.Property(x => x.End).HasColumnName(@"End").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256).ValueGeneratedNever();
            builder.Property(x => x.Colour).HasColumnName(@"Colour").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256).ValueGeneratedNever();

            // Foreign keys
            builder.HasOne(a => a.User).WithMany(b => b.EventDatas).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EventData_Users");
        }
    }

    // Users
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "dbo");
            builder.HasKey(x => x.UserId).HasName("PK_Users").IsClustered();

            builder.Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256).ValueGeneratedNever();
            builder.Property(x => x.Username).HasColumnName(@"Username").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256);
            builder.Property(x => x.Password).HasColumnName(@"Password").HasColumnType("nvarchar(256)").IsRequired().HasMaxLength(256);
        }
    }


    #endregion

    #region Stored procedure return models

    // ****************************************************************************************************
    // This is not a commercial licence, therefore only a few tables/views/stored procedures are generated.
    // ****************************************************************************************************

    public class SpGetUserEventsReturnModel
    {
        public string EventDataId { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Colour { get; set; }
    }


    #endregion

}
// </auto-generated>