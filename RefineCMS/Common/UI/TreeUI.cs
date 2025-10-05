using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RefineCMS.Helpers;
using RefineCMS.Models;
using RefineCMS.Providers;

namespace RefineCMS.Common.UI;

public class TreeAction
{
    public virtual string Type { get; set; } = "primary";
    public virtual string Name { get; set; } = string.Empty;
    public virtual string Label { get; set; } = string.Empty;
    public virtual string? Link { get; set; }
    public virtual Func<TreeUI, object?>? OnAction { get; set; } = _ => null;
    public virtual Func<TreeAction, string>? CustomRender { get; set; }
    public virtual bool Hidden { get; set; } = false;

    public virtual string Render()
    {
        if (Hidden) return "";
        return CustomRender?.Invoke(this) ?? $@"<a-button type=""{Type}"" html-type=""submit"" name=""SubmitAction"" value=""{Name}"">{Label}</a-button>";
    }
}

public class TreeItem
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public List<TreeItem> Children { get; set; } = [];

    public object GetData()
    {
        var data = new Dictionary<string, object>
        {
            ["Id"] = Id,
            ["Name"] = Name,
            ["Children"] = Children.Select(x => x.GetData()).ToList()
        };
        return data;
    }

    public void SaveData<T>(ModelProvider<T> provider, int parent = 0, int order = 0) where T : BaseModel
    {
        var entity = provider.Query().FirstOrDefault(e => EF.Property<object>(e, "Id")!.Equals(Id));
        if (entity != null)
        {
            var _parentId = typeof(T).GetProperty("ParentId");
            if (_parentId != null && _parentId.CanWrite)
            {
                _parentId.SetValue(entity, parent);
            }

            var _order = typeof(T).GetProperty("Order");
            if (_order != null && _order.CanWrite)
            {
                _order.SetValue(entity, order);
            }

            provider.Update(entity);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].SaveData(provider, Id, i);
            }
        }
    }
}

public class TreeUI
{
    public string Name { get; set; } = "tree";
    public ButtonUI? AddNew { get; set; }
    public List<TreeAction> Actions { get; set; } = [];
    public string? EditLink { get; set; } = string.Empty;
    public List<TreeItem> Items { get; set; } = [];

    public List<object> GetData()
    {
        return Items.Select(x => x.GetData()).ToList();
    }

    public void LoadModelData<TQuery, TMeta>(List<TQuery> list, ModelProvider<TMeta>? metaProvider)
        where TQuery : class
        where TMeta : BaseModel
    {
        Items = BuildIndentedList(list);
    }

    public void SaveModelData<T>(ModelProvider<T> provider) where T : BaseModel
    {
        for (var i = 0; i < Items.Count; i++)
        {
            Items[i].SaveData(provider, 0, i);
        }
    }

    public List<TreeItem> BuildIndentedList<T>(List<T> items, int parentId = 0) where T : class
    {
        var result = new List<TreeItem>();

        var children = items.Where(x => parentId.Equals(PropertyGetter.GetValue<int>(x, "ParentId")));

        foreach (var child in children)
        {
            var id = PropertyGetter.GetValue<int>(child, "Id");

            result.Add(new TreeItem
            {
                Id = id,
                Name = (PropertyGetter.GetValue<string>(child, "Name") ?? "") + (PropertyGetter.GetValue<int>(child, "Status") == 0 ? " - <i>(Disable)</i>" : ""),
                Children = BuildIndentedList(items, id)
            });
        }

        return result;
    }

    public void SetData(string json)
    {
        JArray data = JArray.Parse(json);
        Items = CreateTreeData(data);
    }

    protected List<TreeItem> CreateTreeData(JArray items)
    {
        return items.Select(x => new TreeItem
        {
            Id = x["Id"]!.Value<int>(),
            Name = x["Name"]!.Value<string>()!,
            Children = CreateTreeData(x["Children"]!.Value<JArray>()!)
        }).ToList();
    }

    public object? DoAction(string action)
    {
        return Actions.FirstOrDefault(btn => !btn.Hidden && btn.Name == action)?.OnAction?.Invoke(this);
    }
}
