using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RefineCMS.Helpers;
using RefineCMS.Models;
using RefineCMS.Providers;
using System.Text.RegularExpressions;

namespace RefineCMS.Common.UI;

public class FormAction
{
    public virtual string Type { get; set; } = "primary";
    public virtual string Name { get; set; } = string.Empty;
    public virtual string Label { get; set; } = string.Empty;
    public virtual Func<FormUI, object?>? OnAction { get; set; } = _ => null;
    public virtual Func<FormAction, string>? CustomRender { get; set; }
    public virtual bool Hidden { get; set; } = false;

    public virtual string Render()
    {
        if (Hidden) return "";
        return CustomRender?.Invoke(this) ?? $@"<a-button type=""{Type}"" html-type=""submit"" name=""SubmitAction"" value=""{Name}"">{Label}</a-button>";
    }
}

public class DeleteFormAction : FormAction
{
    public override string Type { get; set; } = "link";
    public override string Name { get; set; } = "delete";

    public override string Render()
    {
        if (Hidden) return "";
        return CustomRender?.Invoke(this) ?? $@"<field-delete-action type=""{Type}"" name=""{Name}"">{Label}</field-delete-action>";
    }
}

public class ButtonFormAction : FormAction
{
    public override string Type { get; set; } = "default";
    public virtual string Link { get; set; } = string.Empty;

    public override string Render()
    {
        if (Hidden) return "";
        return CustomRender?.Invoke(this) ?? $@"<a-button type=""{Type}"" href=""{Link}"" target=""_blank"">{Label}</a-button>";
    }
}

public class FormField
{
    public virtual string Type { get; set; } = string.Empty;
    public virtual string Name { get; set; } = string.Empty;
    public virtual string Label { get; set; } = string.Empty;
    public virtual string Desc { get; set; } = string.Empty;
    public virtual string Placeholder { get; set; } = string.Empty;
    public virtual object? Value { get; set; }
    public virtual object? Default { get; set; }
    public virtual string ShowIf { get; set; } = string.Empty;
    public virtual bool Expand { get; set; } = true;
    public virtual bool Required { get; set; } = false;
    public virtual bool Disabled { get; set; } = false;
    public virtual string Message { get; set; } = string.Empty;
    public virtual bool Meta { get; set; } = true;
    public virtual bool Hidden { get; set; } = false;
    public virtual Func<FormField, string>? CustomRender { get; set; }
    public virtual Func<FormField, FormUI, bool>? Validate { get; set; }
    public virtual List<FormField> Childrens { get; set; } = [];

    public virtual string GetContent()
    {
        if (Hidden || Childrens.Count == 0) return "";
        return $"\n" + string.Join($"\n", Childrens.Select(field => field.Render())) + $"\n";
    }

    public virtual string GetAttributes()
    {
        return string.Join(" ", new[]{
            $@"name=""{Name}""",
            string.IsNullOrEmpty(Label) ? null : $@"label=""{Label}""",
            string.IsNullOrEmpty(Desc) ? null : $@"desc=""{Desc}""",
            string.IsNullOrEmpty(Placeholder) ? null : $@"placeholder=""{Placeholder}""",
            Default is string ? $@"fdefault=""{Default}""" : null,
            string.IsNullOrEmpty(ShowIf) ? null : $@":showif=""{ShowIf}""",
            string.IsNullOrEmpty(Message) ? null : $@"message=""{Message}""",
            Expand ? null : $@"expand=""false""",
            Required ? "required" : null,
            Disabled ? "disabled" : null,
        }.Where(attr => attr != null));
    }

    public virtual string Render()
    {
        if (Hidden) return "";
        return CustomRender?.Invoke(this) ?? $"<field-{Type} {GetAttributes()}>{GetContent()}</field-{Type}>";
    }

    public virtual object? GetValue()
    {
        if (Hidden) return null;
        return DeepCopy(Value ?? Default);
    }

    public virtual T? GetValue<T>()
    {
        if (Hidden) return default;

        var val = Value ?? Default;
        if (val == null) return default;

        if (val is T value)
            return DeepCopy(value) is T copy ? copy : value;

        if (val is Newtonsoft.Json.Linq.JValue jVal)
            return jVal.ToObject<T>();

        return (T)Convert.ChangeType(val, typeof(T));
    }

    public virtual void SetValue(object value)
    {
        if (Hidden) return;
        Value = DeepCopy(value);
    }

    public virtual bool IsValid(FormUI form)
    {
        if (Hidden) return true;
        if (Validate != null) return Validate.Invoke(this, form);
        if (Required)
        {
            if (Value == null) return false;
            return !string.IsNullOrEmpty(Value.ToString());
        }
        return true;
    }

    public virtual object? GetErrorMessage(FormUI form)
    {
        return IsValid(form) ? null : "This field is required.";
    }

    public virtual FormField Clone()
    {
        var copy = (FormField)MemberwiseClone();
        copy.Childrens = Childrens.Select(child => child.Clone()).ToList();

        // Deep copy Value & Default if needed
        copy.Value = DeepCopy(Value);
        copy.Default = DeepCopy(Default);

        return copy;
    }

    public virtual object? DeepCopy(object? source)
    {
        if (source == null) return null;
        if (source is string) return source; // strings are immutable
        if (source is ICloneable cloneable) return cloneable.Clone();
        if (source is Newtonsoft.Json.Linq.JToken token) return token.DeepClone();

        // Fallback: serialize/deserialize
        var json = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject(json, source.GetType());
    }

    public virtual FormField? Find(string name)
    {
        if (Hidden) return null;

        var segments = name.Split('.', 2);
        var currentKey = segments[0];
        var remainingKeys = segments.Length > 1 ? segments[1] : null;

        var field = Childrens.FirstOrDefault(field => field.Name == currentKey);
        if (field == null) return null;

        if (remainingKeys != null) return field.Find(remainingKeys);
        return field;
    }

    public virtual string GetKey(string prefix)
    {
        var key = Name;
        if (!string.IsNullOrEmpty(prefix))
        {
            key = $"{prefix}.{key}";
        }
        return key;
    }

    public virtual Dictionary<string, string> ToMeta(Dictionary<string, string> meta, string prefix)
    {
        if (!Hidden && Meta)
        {
            var val = GetValue<string>();
            if (val != null)
            {
                meta[GetKey(prefix)] = val;
            }
        }

        return meta;
    }

    public virtual void LoadModelData(BaseModel? model, Dictionary<string, string>? meta, string prefix = "")
    {
        if (Hidden) return;

        // Model property
        if (model != null)
        {
            // Taxonomy
            if (this is TaxonomySelect ts)
            {
                ts.Load(model.Id);
                return;
            }

            var val = PropertyGetter.GetValue(model, Name);
            if (val != null)
            {
                SetValue(val);
                return;
            }
        }

        // Meta
        var key = GetKey(prefix);
        if (Meta && meta != null && meta.ContainsKey(key))
        {
            SetValue(meta[key]);
        }
    }

    public virtual void SaveTaxonomy(int objectId)
    {
        if (Hidden) return;

        if (this is TaxonomySelect ts)
        {
            ts.Save(objectId);
        }
    }

    public virtual void HideLanguages(List<KeyValuePair<string, string>> Languages)
    {
        if (Hidden) return;

        if (this is LangGroup ts)
        {
            foreach (var lang in Languages)
            {
                if (lang.Key == "en")
                {
                    ts.Find(lang.Key)!.Label = "";
                }
                else
                {
                    ts.Find(lang.Key)!.Hidden = true;
                }
            }
        }
    }
}

public class Hidden : FormField
{
    public override string Type { get; set; } = "hidden";
}

public class Text : FormField
{
    public override string Type { get; set; } = "text";
}

public class Textarea : FormField
{
    public override string Type { get; set; } = "textarea";
    public int Rows { get; set; } = 3;

    public override string GetAttributes()
    {
        return base.GetAttributes() + $@" :rows=""{Rows}""";
    }
}

public class YesNo : FormField
{
    public override string Type { get; set; } = "yesno";
}

public class Color : FormField
{
    public override string Type { get; set; } = "color";
}

public class Date : FormField
{
    public override string Type { get; set; } = "date";
}

public class Time : FormField
{
    public override string Type { get; set; } = "time";
}

public class Editor : FormField
{
    public override string Type { get; set; } = "editor";
    public override string GetContent()
    {
        if (Hidden) return "";
        return $"{Value}";
    }
}

public class Email : FormField
{
    public override string Type { get; set; } = "email";
}

public class UploadFile : FormField
{
    public override string Type { get; set; } = "file";
    public bool ImageOnly { get; set; } = false;

    public override string GetAttributes()
    {
        var attributes = base.GetAttributes();
        if (ImageOnly) attributes += " image-only";
        return attributes;
    }
}

public class Gallery : FormField
{
    public override string Type { get; set; } = "gallery";
}

public class Mask : FormField
{
    public override string Type { get; set; } = "mask";
    public required string Format { get; set; } = "###-###-####";

    public override string GetAttributes()
    {
        Placeholder = Format;
        return base.GetAttributes() + $@" mask=""{Format}""";
    }
}

public class Number : FormField
{
    public override string Type { get; set; } = "number";
}

public class Password : FormField
{
    public override string Type { get; set; } = "password";
    public bool ShowButton { get; set; } = false;

    public override string GetAttributes()
    {
        var attrs = base.GetAttributes();
        if (ShowButton) attrs += " show-button";
        return attrs;
    }
}

public class RangeSlider : FormField
{
    public override string Type { get; set; } = "range";
    public int Min { get; set; } = 0;
    public int Max { get; set; } = 100;
    public string TipFormatter { get; set; } = "{0}";

    public override string GetAttributes()
    {
        return base.GetAttributes() + $@" :min=""{Min}"" :max=""{Max}"" tip-formatter=""{TipFormatter}""";
    }
}

public class Checkbox : FormField
{
    public override string Type { get; set; } = "checkbox";
    public override object? Default { get; set; } = new JArray();
    public virtual Dictionary<string, string> Items { get; set; } = [];
    public virtual int Cols { get; set; } = 0;

    public override string GetContent()
    {
        if (Hidden || Items.Count == 0) return "";

        if (Cols > 0)
        {
            var html = "<a-row>";

            foreach (var item in Items)
            {
                html += $@"<a-col :span=""{24 / Cols}""><a-checkbox value=""{item.Key}"">{item.Value}</a-checkbox></a-col>";
            }

            html += "</a-row>";

            return html;
        }

        return $"\n" + string.Join($"\n", Items.Select(kv => $@"<a-checkbox value=""{kv.Key}"">{kv.Value}</a-checkbox>")) + $"\n";
    }

    public override void SetValue(object value)
    {
        var val = value is string s ? JArray.Parse(string.IsNullOrEmpty(s) ? "[]" : s) : value;
        base.SetValue(val);
    }

    public override object? GetValue()
    {
        var val = base.GetValue();
        return val is string s ? JArray.Parse(string.IsNullOrEmpty(s) ? "[]" : s) : val;
    }

    public override Dictionary<string, string> ToMeta(Dictionary<string, string> meta, string prefix)
    {
        if (!Hidden && Meta)
        {
            var val = GetValue();
            if (val != null)
            {
                // val is JArray
                meta[GetKey(prefix)] = val.ToString()!;
            }
        }

        return meta;
    }
}

public class Radio : FormField
{
    public override string Type { get; set; } = "radio";
    public Dictionary<string, string> Items = [];

    public override string GetContent()
    {
        if (Hidden || Items.Count == 0) return "";
        return $"\n" + string.Join($"\n", Items.Select(kv => $@"<a-radio value=""{kv.Key}"">{kv.Value}</a-radio>")) + $"\n";
    }

    public override void SetValue(object value)
    {
        if (Hidden) return;
        Value = value.ToString();
    }
}

public class SelectSearch : FormField
{
    public override string Type { get; set; } = "select-search";
    public string ApiUrl { get; set; } = string.Empty;
    public override object? Default { get; set; } = new JArray();

    public Func<int, string>? ItemRender { get; set; }

    public override string GetContent()
    {
        //var items = GetValue<JArray>();
        //if (Hidden || ItemRender == null || items == null) return "";

        //return $"\n" + string.Join($"\n", items.Select(id => $@"<div value=""{id}"">{ItemRender.Invoke(id.Value<int>())}</div>")) + $"\n";
        return "";
    }

    public override string GetAttributes()
    {
        return $@"{base.GetAttributes()} apiuri=""{ApiUrl}""";
    }

    public override Dictionary<string, string> ToMeta(Dictionary<string, string> meta, string prefix)
    {
        if (!Hidden && Meta)
        {
            var val = GetValue();
            if (val != null)
            {
                // val is JArray
                meta[GetKey(prefix)] = val.ToString()!;
            }
        }

        return meta;
    }

    public override void LoadModelData(BaseModel? model, Dictionary<string, string>? meta, string prefix = "")
    {
        if (Hidden) return;

        // Only using MultipleSelect for meta
        var key = GetKey(prefix);
        if (Meta && meta != null && meta.ContainsKey(key) && !string.IsNullOrEmpty(meta[key]))
        {
            SetValue(JArray.Parse(meta[key]));
        }
    }

    public override object? GetValue()
    {
        var val = base.GetValue();
        return val is string s ? JArray.Parse(string.IsNullOrEmpty(s) ? "[]" : s) : val;
    }
}

public class Select : FormField
{
    public override string Type { get; set; } = "select";
    public Dictionary<string, string> Items = [];

    public override string GetContent()
    {
        if (Hidden || Items.Count == 0) return "";
        return $"\n" + string.Join($"\n", Items.Select(kv => $@"<a-select-option value=""{kv.Key}"" alt=""{kv.Value.Trim()}"">{kv.Value.Trim()}</a-select-option>")) + $"\n";
    }

    public override void SetValue(object value)
    {
        if (Hidden) return;
        Value = value.ToString();
    }
}

public class MultipleSelect : Select
{
    public override object? Default { get; set; } = new JArray();

    public override string GetAttributes()
    {
        return base.GetAttributes() + " multiple";
    }

    public override Dictionary<string, string> ToMeta(Dictionary<string, string> meta, string prefix)
    {
        if (!Hidden && Meta)
        {
            var val = GetValue();
            if (val != null)
            {
                // val is JArray
                meta[GetKey(prefix)] = val.ToString()!;
            }
        }

        return meta;
    }

    public override void LoadModelData(BaseModel? model, Dictionary<string, string>? meta, string prefix = "")
    {
        if (Hidden) return;

        // Only using MultipleSelect for meta
        var key = GetKey(prefix);
        if (Meta && meta != null && meta.ContainsKey(key) && !string.IsNullOrEmpty(meta[key]))
        {
            SetValue(JArray.Parse(meta[key]));
        }
    }

    public override object? GetValue()
    {
        var val = base.GetValue();
        return val is string s ? JArray.Parse(string.IsNullOrEmpty(s) ? "[]" : s) : val;
    }
}

public class TagsSelect : MultipleSelect
{
    public override string Type { get; set; } = "tags-select";
}

// For table: Post <-> TermRelationship <-> Term
public class TaxonomySelect : Select
{
    public override object? Default { get; set; } = "0";
    public override bool Meta { get; set; } = false;
    public string Taxonomy { get; set; } = string.Empty;
    public AppFactory? Factory { get; set; }

    public override string GetContent()
    {
        if (Hidden) return "";

        if (Factory != null)
        {
            var _taxonomyProvider = Factory.Create<ModelProvider<TermTaxonomy>>();
            var _termProvider = Factory.Create<ModelProvider<Term>>();

            var list = _termProvider.Query()
                .Join(_taxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.Id, t1.Name, t1.ParentId, t1.Order, t2.Slug })
                .Where(x => x.Slug.Equals(Taxonomy))
                .OrderBy(x => x.Order)
                .ToList();

            Items = BuildIndentedList(list).ToDictionary(x => x[0], x => x[1]);
        }

        Items["0"] = "(None)";

        return base.GetContent();
    }

    public List<List<string>> BuildIndentedList<T>(List<T> items, int parentId = 0, int level = 0) where T : class
    {
        var result = new List<List<string>>();

        var children = items.Where(x => parentId.Equals(PropertyGetter.GetValue<int>(x, "ParentId")));

        foreach (var child in children)
        {
            string indent = new string('-', level * 4);
            result.Add([PropertyGetter.GetValue<string>(child, "Id"), $"{indent} {PropertyGetter.GetValue<string>(child, "Name")}"]);

            // recursively add children
            result.AddRange(BuildIndentedList(items, PropertyGetter.GetValue<int>(child, "Id"), level + 1));
        }

        return result;
    }

    public void Save(int objectId)
    {
        if (!Hidden && Factory != null)
        {
            var _provider = Factory.Create<ModelProvider<TermRelationship>>();
            var _taxonomyProvider = Factory.Create<ModelProvider<TermTaxonomy>>();
            var _termProvider = Factory.Create<ModelProvider<Term>>();

            var matchedIds = _termProvider.Query()
                .Join(_taxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.Id, t2.Slug })
                .Where(x => x.Slug.Equals(Taxonomy))
                .Select(x => x.Id)
                .Distinct();

            _provider.Delete(x => x.ObjectId == objectId && matchedIds.Contains(x.TermId));

            var termId = GetValue<int>();
            if (termId > 0)
            {
                _provider.Add(new TermRelationship { TermId = termId, ObjectId = objectId });
            }
        }
    }

    public void Load(int objectId)
    {
        if (!Hidden && Factory != null)
        {
            var _provider = Factory.Create<ModelProvider<TermRelationship>>();
            var _taxonomyProvider = Factory.Create<ModelProvider<TermTaxonomy>>();
            var _termProvider = Factory.Create<ModelProvider<Term>>();

            var termId = _provider.Query()
                .Join(_termProvider.Query(), t1 => t1.TermId, t2 => t2.Id, (t1, t2) => new { t1.TermId, t1.ObjectId, t2.TaxonomyId })
                .Join(_taxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.TermId, t1.ObjectId, t2.Slug })
                .Where(x => x.Slug.Equals(Taxonomy) && x.ObjectId == objectId)
                .Select(x => x.TermId)
                .FirstOrDefault();

            if (termId > 0)
            {
                SetValue(termId.ToString());
            }
        }
    }
}

public class TaxonomyRelationshipSelect : Select
{
    public override object? Default { get; set; } = "0";
    public string Taxonomy { get; set; } = string.Empty;
    public int EditId { get; set; } = 0;
    public AppFactory? Factory { get; set; }
    public bool OnlyRoot { get; set; } = false;

    public override string GetContent()
    {
        if (Hidden) return "";

        if (Factory != null)
        {
            var _taxonomyProvider = Factory.Create<ModelProvider<TermTaxonomy>>();
            var _termProvider = Factory.Create<ModelProvider<Term>>();

            var list = _termProvider.Query()
                .Join(_taxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.Id, t1.Name, t1.ParentId, t1.Order, t2.Slug })
                .Where(x => x.Slug.Equals(Taxonomy) && x.Id != EditId && (OnlyRoot ? x.ParentId == 0 : true))
                .OrderBy(x => x.Order)
                .ToList();

            Items = BuildIndentedList(list).ToDictionary(x => x[0], x => x[1]);
        }

        Items["0"] = "(None)";

        return base.GetContent();
    }

    public List<List<string>> BuildIndentedList<T>(List<T> items, int parentId = 0, int level = 0) where T : class
    {
        var result = new List<List<string>>();

        var children = items.Where(x => parentId.Equals(PropertyGetter.GetValue<int>(x, "ParentId")));

        foreach (var child in children)
        {
            string indent = new string('-', level * 4);
            result.Add([PropertyGetter.GetValue<string>(child, "Id"), $"{indent} {PropertyGetter.GetValue<string>(child, "Name")}"]);

            // recursively add children
            result.AddRange(BuildIndentedList(items, PropertyGetter.GetValue<int>(child, "Id"), level + 1));
        }

        return result;
    }
}

public class TaxonomyRelationshipMultipleSelect : MultipleSelect
{
    public string Taxonomy { get; set; } = string.Empty;
    public int EditId { get; set; } = 0;
    public AppFactory? Factory { get; set; }

    public override string GetContent()
    {
        if (Hidden) return "";

        if (Factory != null)
        {
            var _taxonomyProvider = Factory.Create<ModelProvider<TermTaxonomy>>();
            var _termProvider = Factory.Create<ModelProvider<Term>>();

            var list = _termProvider.Query()
                .Join(_taxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.Id, t1.Name, t1.ParentId, t1.Order, t2.Slug })
                .Where(x => x.Slug.Equals(Taxonomy) && x.Id != EditId)
                .OrderBy(x => x.Order)
                .ToList();

            Items = BuildIndentedList(list).ToDictionary(x => x[0], x => x[1]);
        }

        return base.GetContent();
    }

    public List<List<string>> BuildIndentedList<T>(List<T> items, int parentId = 0, int level = 0) where T : class
    {
        var result = new List<List<string>>();

        var children = items.Where(x => parentId.Equals(PropertyGetter.GetValue<int>(x, "ParentId")));

        foreach (var child in children)
        {
            string indent = new string('-', level * 4);
            result.Add([PropertyGetter.GetValue<string>(child, "Id"), $"{indent} {PropertyGetter.GetValue<string>(child, "Name")}"]);

            // recursively add children
            result.AddRange(BuildIndentedList(items, PropertyGetter.GetValue<int>(child, "Id"), level + 1));
        }

        return result;
    }
}

public class TaxonomyCheckbox : Checkbox
{
    public string Taxonomy { get; set; } = string.Empty;
    public override int Cols { get; set; } = 1;
    public AppFactory? Factory { get; set; }

    public override string GetContent()
    {
        if (Hidden) return "";

        if (Factory != null)
        {
            var _taxonomyProvider = Factory.Create<ModelProvider<TermTaxonomy>>();
            var _termProvider = Factory.Create<ModelProvider<Term>>();

            var list = _termProvider.Query()
                .Join(_taxonomyProvider.Query(), t1 => t1.TaxonomyId, t2 => t2.Id, (t1, t2) => new { t1.Id, t1.Name, t1.ParentId, t1.Order, t2.Slug })
                .Where(x => x.Slug.Equals(Taxonomy))
                .OrderBy(x => x.Order)
                .ToList();

            Items = BuildIndentedList(list).ToDictionary(x => x[0], x => x[1]);
        }

        return base.GetContent();
    }

    public List<List<string>> BuildIndentedList<T>(List<T> items, int parentId = 0, int level = 0) where T : class
    {
        var result = new List<List<string>>();

        var children = items.Where(x => parentId.Equals(PropertyGetter.GetValue<int>(x, "ParentId")));

        foreach (var child in children)
        {
            var name = PropertyGetter.GetValue<string>(child, "Name");
            if (level == 0)
            {
                name = $"<b>{name}</b>";
            }
            else
            {
                string indent = new string('-', level * 4);
                name = $"{indent} {name}";
            }

            result.Add([PropertyGetter.GetValue<string>(child, "Id"), name]);

            // recursively add children
            result.AddRange(BuildIndentedList(items, PropertyGetter.GetValue<int>(child, "Id"), level + 1));
        }

        return result;
    }
}

public class PostRelationshipSelect : Select
{
    public override object? Default { get; set; } = "0";
    public string PostType { get; set; } = "post";
    public int EditId { get; set; } = 0;
    public AppFactory? Factory { get; set; }

    public override string GetContent()
    {
        if (Hidden) return "";

        if (Factory != null)
        {
            var _provider = Factory.Create<ModelProvider<Post>>();
            var list = _provider.Query()
                            .Where(x => x.Id != EditId && x.PostType.Equals(PostType))
                            .OrderBy(x => EF.Property<string>(x, "Order"))
                            .ToList();

            Items = BuildIndentedList(list).ToDictionary(x => x[0], x => x[1]);
        }

        Items["0"] = "(None)";

        return base.GetContent();
    }

    public List<List<string>> BuildIndentedList(List<Post> items, int parentId = 0, int level = 0)
    {
        var result = new List<List<string>>();

        var children = items.Where(x => x.ParentId == parentId);

        foreach (var child in children)
        {
            string indent = new string('-', level * 4);
            result.Add([child.Id.ToString(), $"{indent} {child.PostTitle}"]);

            // recursively add children
            result.AddRange(BuildIndentedList(items, child.Id, level + 1));
        }

        return result;
    }
}

public class Group : FormField
{
    public override string Type { get; set; } = "group";

    public override object? GetValue()
    {
        if (Hidden) return null;

        var data = new Dictionary<string, object>();
        foreach (var field in Childrens)
        {
            var val = field.GetValue();
            if (val != null)
            {
                data[field.Name] = val;
            }
        }
        return data;
    }

    public override void SetValue(object value)
    {
        if (Hidden) return;

        var obj = (JObject)value;
        foreach (var field in Childrens)
        {
            if (obj.ContainsKey(field.Name))
            {
                field.SetValue(obj[field.Name]!);
            }
        }
    }

    public override bool IsValid(FormUI form)
    {
        if (Hidden) return true;
        return Childrens.All(field => field.IsValid(form));
    }

    public override object? GetErrorMessage(FormUI form)
    {
        var errors = new Dictionary<string, object>();
        foreach (var field in Childrens)
        {
            var error = field.GetErrorMessage(form);
            if (error != null)
            {
                errors[field.Name] = error;
            }
        }
        return errors;
    }

    public override Dictionary<string, string> ToMeta(Dictionary<string, string> meta, string prefix)
    {
        if (!Hidden && Meta)
        {
            foreach (var field in Childrens)
            {
                meta = field.ToMeta(meta, GetKey(prefix));
            }
        }
        return meta;
    }

    public override void LoadModelData(BaseModel? model, Dictionary<string, string>? meta, string prefix = "")
    {
        if (Hidden) return;

        foreach (var field in Childrens)
        {
            field.LoadModelData(model, meta, GetKey(prefix));
        }
    }

    public override void SaveTaxonomy(int objectId)
    {
        if (Hidden) return;

        foreach (var field in Childrens)
        {
            field.SaveTaxonomy(objectId);
        }
    }
}

public class RowGroup : Group
{
    public override string Type { get; set; } = "row-group";
}

public class LangGroup : RowGroup
{
    public required FormField Field { get; set; }
    public List<KeyValuePair<string, string>> Languages { get; set; } = [];
    private List<FormField>? _childrens;
    public override List<FormField> Childrens
    {
        get
        {
            if (_childrens == null)
            {
                _childrens = Languages.Select(lang =>
                {
                    var field = Field.Clone();
                    field.Name = lang.Key;
                    field.Label = lang.Value;
                    return field;
                }).ToList();
            }
            return _childrens;
        }
    }

    public override FormField Clone()
    {
        var copy = (LangGroup)MemberwiseClone();
        copy._childrens = Languages.Select(lang =>
        {
            var field = Field.Clone();
            field.Name = lang.Key;
            field.Label = lang.Value;
            return field;
        }).ToList();

        // Deep copy Value & Default if needed
        copy.Value = DeepCopy(Value);
        copy.Default = DeepCopy(Default);

        return copy;
    }
}

public class Repeater : FormField
{
    public override string Type { get; set; } = "repeater";
    public bool TableLayout { get; set; } = false;
    public string Colsize { get; set; } = string.Empty;
    public List<Group> Rows = [];

    public override string GetAttributes()
    {
        var layout = TableLayout ? "table" : "row";
        return base.GetAttributes() + $@" layout=""{layout}"" colsize=""{Colsize}""";
    }

    public override object? GetValue()
    {
        if (Hidden) return null;
        return Rows.Select(row => row.GetValue()).Where(row => row != null).ToList();
    }

    public override void SetValue(object value)
    {
        if (Hidden) return;

        var rows = (JArray)value;

        Rows = rows.Select(row => new Group
        {
            Name = "Row",
            Childrens = Childrens.Select(field =>
            {
                var newField = field.Clone();
                if (((JObject)row).ContainsKey(newField.Name))
                {
                    newField.SetValue(row[newField.Name]!);
                }
                return newField;
            }).ToList()
        }).ToList();

    }

    public override bool IsValid(FormUI form)
    {
        if (Hidden) return true;
        return Rows.All(field => field.IsValid(form));
    }

    public override object? GetErrorMessage(FormUI form)
    {
        return Rows.Select(field => field.GetErrorMessage(form)).ToList();
    }

    public override FormField? Find(string name)
    {
        if (Hidden) return null;

        var segments = name.Split('.', 2);
        var currentKey = segments[0];
        var remainingKeys = segments.Length > 1 ? segments[1] : null;

        if (int.TryParse(currentKey, out var idx) && idx <= Rows.Count)
        {
            var field = Rows[idx];
            if (field == null) return null;

            if (remainingKeys != null) return field.Find(remainingKeys);
            return field;
        }
        return null;
    }

    public override Dictionary<string, string> ToMeta(Dictionary<string, string> meta, string prefix)
    {
        if (!Hidden && Meta)
        {
            var key = GetKey(prefix);
            for (int i = 0; i < Rows.Count; i++)
            {
                meta = Rows[i].ToMeta(meta, $"{key}.{i}");
            }
        }
        return meta;
    }

    public override void LoadModelData(BaseModel? model, Dictionary<string, string>? meta, string prefix = "")
    {
        if (Hidden) return;

        var key = GetKey(prefix);

        // create row element
        if (meta != null)
        {
            var items = meta.Where(x =>
            {
                string pattern = @"^" + Regex.Escape(key) + @"\.\d+\.Row\b";
                return Regex.IsMatch(x.Key, pattern);
            }).ToList();

            Dictionary<string, bool> idx = [];
            foreach (var item in items)
            {
                var keyId = item.Key.Substring(key.Length + 1);
                var segments = keyId.Split('.', 2);

                if (!idx.ContainsKey(segments[0])) idx[segments[0]] = true;
            }

            Rows = [];
            for (var i = 0; i < idx.Keys.Count; i++)
            {
                Rows.Add(new Group
                {
                    Name = "Row",
                    Childrens = Childrens.Select(field => field.Clone()).ToList()
                });
            }
        }

        // load data
        for (int i = 0; i < Rows.Count; i++)
        {
            Rows[i].LoadModelData(model, meta, $"{key}.{i}");
        }
    }

    public override void SaveTaxonomy(int objectId)
    {
        if (Hidden) return;

        for (int i = 0; i < Rows.Count; i++)
        {
            Rows[i].SaveTaxonomy(objectId);
        }
    }
}

public class StaticHtml : FormField
{
    public override string Type { get; set; } = "static-html";
    public virtual Func<FormField, string>? HtmlRender { get; set; }

    public override string GetContent()
    {
        if (Hidden) return "";
        return HtmlRender == null ? "" : HtmlRender.Invoke(this);
    }
}

public class FormUI
{
    public virtual string Name { get; set; } = "frm";
    public virtual List<FormField> Fields { get; set; } = [];
    public virtual List<FormAction> Actions { get; set; } = [];
    public virtual string? BackUrl { get; set; }

    public object GetData()
    {
        var data = new Dictionary<string, object>();
        foreach (var field in Fields)
        {
            if (!field.Hidden)
            {
                var val = field.GetValue();
                if (val != null)
                {
                    data[field.Name] = val;
                }
            }
        }
        return data;
    }

    public void SetData(string json)
    {
        JObject data = JObject.Parse(json);
        foreach (var field in Fields)
        {
            if (!field.Hidden)
            {
                if (data.ContainsKey(field.Name))
                {
                    field.SetValue(data[field.Name]!);
                }
            }
        }
    }

    public bool IsValid()
    {
        return Fields.All(field => field.IsValid(this));
    }

    public Dictionary<string, object> GetErrorMessage()
    {
        var errors = new Dictionary<string, object>();
        foreach (var field in Fields)
        {
            var error = field.GetErrorMessage(this);
            if (error != null)
            {
                errors[field.Name] = error;
            }
        }
        return errors;
    }

    public object? DoAction(string action)
    {
        return Actions.FirstOrDefault(btn => !btn.Hidden && btn.Name == action)?.OnAction?.Invoke(this);
    }

    public FormField? Find(string name)
    {
        var segments = name.Split('.', 2);
        var currentKey = segments[0];
        var remainingKeys = segments.Length > 1 ? segments[1] : null;

        var field = Fields.FirstOrDefault(field => !field.Hidden && field.Name == currentKey);
        if (field == null) return null;

        if (remainingKeys != null) return field.Find(remainingKeys);
        return field;
    }

    public void HideFields(List<string> fields)
    {
        foreach (var field in fields)
        {
            var _field = Find(field);
            if (_field != null)
            {
                _field.Hidden = true;
            }
        }
    }

    public Dictionary<string, string> ToMeta()
    {
        var meta = new Dictionary<string, string>();
        foreach (var field in Fields)
        {
            if (!field.Hidden)
            {
                meta = field.ToMeta(meta, "");
            }
        }

        return meta;
    }

    public void LoadModelData<T>(BaseModel? model, ModelProvider<T>? metaProvider = null) where T : BaseModel
    {
        var meta = new Dictionary<string, string>();

        if (model != null && metaProvider != null)
        {
            meta = metaProvider.GetMeta(model.Id);
        }

        foreach (var field in Fields)
        {
            if (!field.Hidden)
            {
                field.LoadModelData(model, meta, "");
            }
        }
    }

    public void SaveModelData<T>(int objectId, ModelProvider<T>? metaProvider = null) where T : BaseModel
    {
        foreach (var field in Fields)
        {
            if (!field.Hidden)
            {
                field.SaveTaxonomy(objectId);
            }
        }

        metaProvider?.UpdateMeta(objectId, ToMeta());
    }
}