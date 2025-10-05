using Microsoft.EntityFrameworkCore;
using RefineCMS.Common;
using RefineCMS.Models;
using System.Linq.Expressions;

namespace RefineCMS.Providers;

public class ModelProvider<T>(DB _db) where T : BaseModel
{
    public T Add(T model)
    {
        if (model.CreatedAt == null)
        {
            model.CreatedAt = DateTime.Now;
        }

        model.UpdatedAt = DateTime.Now;
        _db.Set<T>().Add(model);
        _db.SaveChanges();
        return model;
    }

    public T Update(T model)
    {
        model.UpdatedAt = DateTime.Now;
        _db.Entry(model).State = EntityState.Modified;
        _db.SaveChanges();
        return model;
    }

    public void Delete(T model)
    {
        _db.Set<T>().Remove(model);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        _db.Set<T>().Where(e => EF.Property<object>(e, "Id")!.Equals(id)).ExecuteDelete();
    }

    public void Delete(Expression<Func<T, bool>> predicate)
    {
        _db.Set<T>().Where(predicate).ExecuteDelete();
    }

    public T? FindById(params object[] keyValues)
    {
        return _db.Set<T>().Find(keyValues);
    }

    public bool Exists(int id)
    {
        return _db.Set<T>().Any(e => EF.Property<object>(e, "Id")!.Equals(id));
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return _db.Set<T>().Any(predicate);
    }

    public T? FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return _db.Set<T>().FirstOrDefault(predicate);
    }

    public IQueryable<T> Query()
    {
        return _db.Set<T>();
    }

    public void UpdateMeta(int objectId, Dictionary<string, string> meta)
    {
        var dbSet = _db.Set<T>();

        // 1. Get existing meta records for this object
        var existing = dbSet
            .Where(e => EF.Property<int>(e, "ObjectId") == objectId)
            .ToList();

        var existingKeys = new HashSet<string>(
            existing.Select(e => (string)typeof(T).GetProperty("MetaKey")!.GetValue(e)!)
        );

        // 2. Update existing values or delete if missing
        foreach (var entity in existing)
        {
            var key = (string)typeof(T).GetProperty("MetaKey")!.GetValue(entity)!;

            if (meta.TryGetValue(key, out var newValue))
            {
                var currentValue = (string)typeof(T).GetProperty("MetaValue")!.GetValue(entity)!;
                if (currentValue != newValue)
                {
                    typeof(T).GetProperty("MetaValue")!.SetValue(entity, newValue);
                    _db.Entry(entity).State = EntityState.Modified;
                }
            }
            else
            {
                dbSet.Remove(entity);
            }
        }

        // 3. Insert new keys
        foreach (var key in meta.Keys.Except(existingKeys))
        {
            var entity = Activator.CreateInstance<T>()!;
            typeof(T).GetProperty("ObjectId")!.SetValue(entity, objectId);
            typeof(T).GetProperty("MetaKey")!.SetValue(entity, key);
            typeof(T).GetProperty("MetaValue")!.SetValue(entity, meta[key]);
            dbSet.Add(entity);
        }

        // 4. Save all changes in one transaction
        _db.SaveChanges();
    }

    public Dictionary<string, string> GetMeta(int objectId)
    {
        var meta = _db.Set<T>().Where(e => EF.Property<int>(e, "ObjectId") == objectId).ToList();

        return meta.ToDictionary(
            e => (string?)e.GetType().GetProperty("MetaKey")?.GetValue(e) ?? "",
            e => (string?)e.GetType().GetProperty("MetaValue")?.GetValue(e) ?? ""
        );
    }

    public void DeleteMeta(int objectId)
    {
        _db.Set<T>().Where(e => EF.Property<int>(e, "ObjectId") == objectId).ExecuteDelete();
    }

    public void UpdateOption(string optionType, Dictionary<string, string> meta)
    {
        var dbSet = _db.Set<T>();

        // 1. Get existing meta records for this object
        var existing = dbSet
            .Where(e => EF.Property<string>(e, "OptionType").Equals(optionType))
            .ToList();

        var existingKeys = new HashSet<string>(
            existing.Select(e => (string)typeof(T).GetProperty("OptionName")!.GetValue(e)!)
        );

        // 2. Update existing values or delete if missing
        foreach (var entity in existing)
        {
            var key = (string)typeof(T).GetProperty("OptionName")!.GetValue(entity)!;

            if (meta.TryGetValue(key, out var newValue))
            {
                var currentValue = (string)typeof(T).GetProperty("OptionValue")!.GetValue(entity)!;
                if (currentValue != newValue)
                {
                    typeof(T).GetProperty("OptionValue")!.SetValue(entity, newValue);
                    _db.Entry(entity).State = EntityState.Modified;
                }
            }
            else
            {
                dbSet.Remove(entity);
            }
        }

        // 3. Insert new keys
        foreach (var key in meta.Keys.Except(existingKeys))
        {
            var entity = Activator.CreateInstance<T>()!;
            typeof(T).GetProperty("OptionType")!.SetValue(entity, optionType);
            typeof(T).GetProperty("OptionName")!.SetValue(entity, key);
            typeof(T).GetProperty("OptionValue")!.SetValue(entity, meta[key]);
            dbSet.Add(entity);
        }

        // 4. Save all changes in one transaction
        _db.SaveChanges();
    }

    public Dictionary<string, string> GetOption(string optionType)
    {
        var meta = _db.Set<T>()
            .Where(e => EF.Property<string>(e, "OptionType").Equals(optionType))
            .ToList();

        return meta.ToDictionary(
            e => (string?)e.GetType().GetProperty("OptionName")?.GetValue(e) ?? "",
            e => (string?)e.GetType().GetProperty("OptionValue")?.GetValue(e) ?? ""
        );
    }

    public void DeleteOption(string optionType)
    {
        _db.Set<T>().Where(e => EF.Property<string>(e, "OptionType").Equals(optionType)).ExecuteDelete();
    }
}