using Newtonsoft.Json.Linq;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Common.UI;

namespace RefineCMS.Theme.Raymag.Classes.CustomUI;

[Hook("PostType_FormUI")]
public class PageHome(Output _output, AppFactory _appFactory) : IFilterHook<FormUI>
{
    public FormUI Apply(FormUI form, params object[] args)
    {
        var (id, postType, postTemplate, formData) = ((int)args[0], (string)args[1], (string)args[2], (JObject)args[3]);

        if (postType != "page" || postTemplate != "home") return form;

        var _ = _output.Trans;
        var Languages = _output.GetLanguages();
        var isEdit = id != 0;

        form.Find("Info.PostName")!.Hidden = true;
        form.Find("Info.PostName")!.Value = "";

        form.Fields.AddRange([
            new Group {
                Name = "KV",
                Label = _("KV"),
                Childrens = [
                    new LangGroup {
                        Name = "Title",
                        Label = _("Title"),
                        Languages = Languages,
                        Field = new Textarea { }
                    },
                    new LangGroup {
                        Name = "Desc",
                        Label = _("Description"),
                        Languages = Languages,
                        Field = new Textarea { }
                    },
                    new Textarea {
                        Name = "BackgroundScript",
                        Label = _("Background Script"),
                    }
                ]
            },
            new Group {
                Name = "Products",
                Label = _("Products"),
                Childrens = [
                    new Text {
                        Name = "BGTitle",
                        Label = _("BG Title")
                    },
                    new LangGroup {
                        Name = "Title",
                        Label = _("Title"),
                        Languages = Languages,
                        Field = new Text { }
                    },
                    new LangGroup {
                        Name = "Desc",
                        Label = _("Description"),
                        Languages = Languages,
                        Field = new Textarea { }
                    },
                    new RowGroup {
                        Name = "ButtonLink",
                        Label = _("Button Link"),
                        Childrens = [
                            new LangGroup {
                                Name = "Label",
                                Label = _("Label"),
                                Languages = Languages,
                                Field = new Text { }
                            },
                            new Text {
                                Name = "Link",
                                Label = _("Link")
                            }
                        ]
                    },
                ]
            },
            new Group {
                Name = "MPC",
                Label = _("MPC"),
                Childrens = [
                    new LangGroup {
                        Name = "Title",
                        Label = _("Title"),
                        Languages = Languages,
                        Field = new Text { }
                    },
                    new Repeater {
                        Name = "MPCItems",
                        Label = "Items",
                        Childrens = [
                            new UploadFile {
                                Name = "ItemImage",
                                Label = _("Image"),
                                ImageOnly = true
                            },
                            new LangGroup {
                                Name = "ItemTitle",
                                Label = _("Title"),
                                Languages = Languages,
                                Field = new Text { }
                            },
                            new LangGroup {
                                Name = "ItemDesc",
                                Label = _("Description"),
                                Languages = Languages,
                                Field = new Textarea { }
                            },
                        ]
                    }
                ]
            },
            new Group {
                Name = "Service",
                Label = _("Service"),
                Childrens = [
                    new Text {
                        Name = "BGTitle",
                        Label = _("BG Title")
                    },
                    new LangGroup {
                        Name = "Title",
                        Label = _("Title"),
                        Languages = Languages,
                        Field = new Text { }
                    },
                    new LangGroup {
                        Name = "Desc",
                        Label = _("Description"),
                        Languages = Languages,
                        Field = new Textarea { }
                    },
                    new Repeater {
                        Name = "SliderItems",
                        Label = "Slider",
                        Childrens = [
                            new LangGroup {
                                Name = "SliderTitle",
                                Label = _("Title"),
                                Languages = Languages,
                                Field = new Textarea { }
                            },
                            new LangGroup {
                                Name = "SliderSubTitle",
                                Label = _("SubTitle"),
                                Languages = Languages,
                                Field = new Text { }
                            },
                            new LangGroup {
                                Name = "SliderDesc",
                                Label = _("Description"),
                                Languages = Languages,
                                Field = new Textarea { }
                            },
                            new UploadFile {
                                Name = "SliderImage1",
                                Label = _("Image 1"),
                                ImageOnly = true
                            },
                            new UploadFile {
                                Name = "SliderImage2",
                                Label = _("Image 2"),
                                ImageOnly = true
                            },
                        ]
                    }
                ]
            },
            new Group {
                Name = "Equipment",
                Label = _("Equipment"),
                Childrens = [
                    new Text {
                        Name = "BGTitle",
                        Label = _("BG Title")
                    },
                    new LangGroup {
                        Name = "Title",
                        Label = _("Title"),
                        Languages = Languages,
                        Field = new Text { }
                    },
                    new LangGroup {
                        Name = "Desc",
                        Label = _("Description"),
                        Languages = Languages,
                        Field = new Textarea { }
                    },
                    new Repeater {
                        Name = "Items",
                        Label = "Items",
                        Childrens = [
                            new LangGroup {
                                Name = "ItemTitle",
                                Label = _("Title"),
                                Languages = Languages,
                                Field = new Text { }
                            },
                            new LangGroup {
                                Name = "ItemDesc",
                                Label = _("Description"),
                                Languages = Languages,
                                Field = new Textarea { }
                            },
                            new UploadFile {
                                Name = "ItemImage",
                                Label = _("Image"),
                                ImageOnly = true
                            },
                        ]
                    }
                ]
            },
            new Group {
                Name = "Contact",
                Label = _("Contact"),
                Childrens = [
                    new Text {
                        Name = "BGTitle",
                        Label = _("BG Title")
                    },
                    new LangGroup {
                        Name = "Title",
                        Label = _("Title"),
                        Languages = Languages,
                        Field = new Text { }
                    },
                    new LangGroup {
                        Name = "Desc",
                        Label = _("Description"),
                        Languages = Languages,
                        Field = new Textarea { }
                    },
                ]
            },
        ]);

        return form;
    }
}