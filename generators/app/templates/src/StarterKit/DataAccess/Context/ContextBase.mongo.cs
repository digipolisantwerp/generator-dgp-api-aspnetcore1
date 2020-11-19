﻿using MongoDB.Driver;
using StarterKit.DataAccess.Options;

namespace StarterKit.DataAccess.Context
{
  public class ContextMongo : IContext
  {
    private IClientSessionHandle _transactionSession;
    protected readonly MongoClient Client;
    protected readonly IMongoDatabase Database;

    public ContextMongo(DataAccessSettingsMongo options)
    {
      Client = new MongoClient(options.GetConnectionString());
      Database = Client.GetDatabase(options.DbName);
    }

    /// <summary>
    /// add IMongoCollections in derived class like so:
    ///
    /// public IMongoCollection<EntityBase> Entities => _database.GetCollection<EntityBase>("Entities");
    /// </summary>


    public void BeginTransaction()
    {
      _transactionSession = Client.StartSession();
      _transactionSession.StartTransaction();
    }

    public void Commit()
    {
      try
      {
        _transactionSession.CommitTransaction();
      }
      finally
      {
        _transactionSession.Dispose();
      }
    }

    public void Rollback()
    {
      _transactionSession.AbortTransactionAsync();
      _transactionSession.Dispose();
    }
  }
}
