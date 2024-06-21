using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using BusinessObjects.Entities;
namespace DataAccessObjects;

public class BaseDao<T> where T : class
{
    public static void Add(T entity)
    {
        var context = new BcbpContext();
        var dbSet = context.Set<T>();
        dbSet.Add(entity);
        context.SaveChanges();
        context.Entry(entity).State = EntityState.Detached;
    }

    public static IQueryable<T> GetAll()
    {
        var context = new BcbpContext();
        var dbSet = context.Set<T>();
        return dbSet.AsQueryable().AsNoTracking(); ;
    }

    public static void Delete(T entity)
    {
        var context = new BcbpContext();
        var dbSet = context.Set<T>();
        dbSet.Remove(entity);
        context.SaveChanges();
    }

    public static void Update(T entity)
    {
        var context = new BcbpContext();
        var tracker = context.Attach(entity);
        tracker.State = EntityState.Modified;
        context.SaveChanges();
        context.Entry(entity).State = EntityState.Detached;
    }

    public static IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        var context = new BcbpContext();
        var dbSet = context.Set<T>();
        return dbSet.Where(expression).AsQueryable().AsNoTracking();
    }

    public static void AddRange(IEnumerable<T> entities)
    {
        var context = new BcbpContext();
        context.AddRange(entities);
        context.SaveChanges();
    }

    public static void RemoveRange(IEnumerable<T> entities)
    {
        var context = new BcbpContext();
        var dbSet = context.Set<T>();
        dbSet.RemoveRange(entities);
        context.SaveChanges();
    }
}