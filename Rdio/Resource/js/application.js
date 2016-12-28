
Rdio = {
    'Tools': {
        'Selectize': function (object, type) {
            if (type == 'MultiSelect') {
                $(object).selectize({
                    plugins: ['remove_button'],
                    persist: false,
                    create: true
                });
            }
            
        },
        'Pagination': {
            'Siteid': '',
            'BaseContentSearchModel': '',
            'ContentSearchModel': '',
            'Event': function (type, page) {
                $('body').on('click', '.paginatopnbtn', function () {
                    var pagenumber = $(this).data('pagenumber');
                    Rdio.Tools.Pagination.GetPagination(type,pagenumber);
                });

                if (type == 'SiteManage') {
                    Rdio.Tools.Pagination.GetPagination(type, 1);
                }

                if (type == 'RssManage') {
                    Rdio.Tools.Pagination.Siteid = $('[name="siteid"]').val();
                    Rdio.Tools.Pagination.GetPagination(type, 1);
                }

                if (type == 'BaseContentManage') {
                    var model = Rdio.BaseContent.BaseContentSearchModel;
                    model.page = page;
                    model.rssid = $('[name="rssid"]').val();
                    model.siteid = $('[name="siteid"]').val();
                    model.tags = $('[name="tags"]').val();
                    model.categories = $('[name="categories"]').val();
                    Rdio.Tools.Pagination.BaseContentSearchModel = model;
                    Rdio.Tools.Pagination.GetPagination(type, page);
                }

                if (type == 'ContentManage') {
                    var model = Rdio.Content.ContentSearchModel;
                    model.page = page;
                    model.rssid = $('[name="rssid"]').val();
                    model.siteid = $('[name="siteid"]').val();
                    model.tags = $('[name="tags"]').val();
                    model.categories = $('[name="categories"]').val();
                    Rdio.Tools.Pagination.ContentSearchModel = model;
                    Rdio.Tools.Pagination.GetPagination(type, page);
                }
            },
            'GetPagination': function (type, page) {

                if (type == 'SiteManage') {
                    loader($('#datacontainer'));
                    $.ajax({
                        method: "GET",
                        url: "/api/ContentManager/SiteManage",
                        data: { "page": page }
                    }).done(function (result) {
                        disloader($('#datacontainer'));

                        if (result.ServiceResultStatus == 0) {
                            var source = $("#datamodel").html();
                            var template = Handlebars.compile(source);
                            var html = template(result);
                            $("#datacontainer").html(html);
                        }
                        if (result.ServiceResultStatus == 1) {
                            alert(result.ServiceResultMassage);
                        }
                    });
                }

                if (type == 'RssManage') {
                    loader($('#datacontainer'));
                    $.ajax({
                        method: "GET",
                        url: "/api/ContentManager/RssManage",
                        data: { "page": page, "siteid": Rdio.Tools.Pagination.Siteid }
                    }).done(function (result) {
                        disloader($('#datacontainer'));

                        if (result.ServiceResultStatus == 0) {
                            var source = $("#datamodel").html();
                            var template = Handlebars.compile(source);
                            var html = template(result);
                            $("#datacontainer").html(html);
                        }
                        if (result.ServiceResultStatus == 1) {
                            alert(result.ServiceResultMassage);
                        }
                    });
                }

                if (type == 'BaseContentManage') {
                    loader($('#datacontainer'));
                    Rdio.Tools.Pagination.BaseContentSearchModel.page = page;
                    $.ajax({
                        method: "POST",
                        url: "/api/BaseContent/Manage",
                        data: Rdio.Tools.Pagination.BaseContentSearchModel
                    }).done(function (result) {
                        disloader($('#datacontainer'));

                        if (result.ServiceResultStatus == 0) {
                            var source = $("#datamodel").html();
                            var template = Handlebars.compile(source);
                            var html = template(result);
                            $("#datacontainer").html(html);
                        }
                        if (result.ServiceResultStatus == 1) {
                            alert(result.ServiceResultMassage);
                        }
                    });
                }

                if (type == 'ContentManage') {
                    loader($('#datacontainer'));
                    Rdio.Tools.Pagination.ContentSearchModel.page = page;
                    $.ajax({
                        method: "POST",
                        url: "/api/Content/Manage",
                        data: Rdio.Tools.Pagination.ContentSearchModel
                    }).done(function (result) {
                        disloader($('#datacontainer'));

                        if (result.ServiceResultStatus == 0) {
                            var source = $("#datamodel").html();
                            var template = Handlebars.compile(source);
                            var html = template(result);
                            $("#datacontainer").html(html);
                        }
                        if (result.ServiceResultStatus == 1) {
                            alert(result.ServiceResultMassage);
                        }
                    });
                }
            }
        }
    },
    'ContentManager': {
        'EditSiteModel': { 'title': '', 'url': '' },
        'EditRssModel': { '_id': '', 'siteid': '', 'title': '', 'url': '', 'tags': '', 'categories': '', 'lang': '' },
        'EditTemplateModel': { '_id': '', 'rssid': '', 'name': '', 'type': '', 'sampleurl': '', 'structure': '' },
        'EditTemplateStructureModel': { 'field': '', 'query': '', 'type': '' },

        'Event':{
            'SiteManage':function(){
                Rdio.Tools.Pagination.Event('SiteManage', 1);
            
            },
            'EditSite': function () {

                $('body').on('click', '#editsitesubmit', function () {
                    var editmodel = Rdio.ContentManager.EditSiteModel;
                    editmodel.title = $('#title').val();
                    editmodel.url = $('#url').val();
                    $.ajax({
                        method: "POST",
                        url: "/api/ContentManager/EditSite",
                        data: editmodel
                    }).done(function (result) {
                        if (result.ServiceResultStatus == 0) {
                            alert(result.ServiceResultMassage);
                            window.location = '/ContentManager/SiteManage';
                        }
                        if (result.ServiceResultStatus == 1) {
                            alert(result.ServiceResultMassage);
                        }
                    });
                });
            },
            'RssManage': function () {
                Rdio.Tools.Pagination.Event('RssManage', 1);

            },
            'EditRss': function () {
                Rdio.Tools.Selectize($('#tags'), 'MultiSelect');
                Rdio.Tools.Selectize($('#categories'), 'MultiSelect');

                $('body').on('click', '#editrsssubmit', function () {
                    var editmodel = Rdio.ContentManager.EditRssModel;
                    editmodel._id = $('[name="_id"]').val();
                    editmodel.siteid = $('[name="siteid"]').val();
                    editmodel.title = $('#title').val();
                    editmodel.url = $('#url').val();
                    editmodel.tags = $('#tags').val().split(',');
                    editmodel.categories = $('#categories').val().split(',');
                    editmodel.lang = $('#lang').val();
                    $.ajax({
                        method: "POST",
                        url: "/api/ContentManager/EditRss",
                        data: editmodel
                    }).done(function (result) {
                        if (result.ServiceResultStatus == 0) {
                            alert(result.ServiceResultMassage);
                            window.location = '/ContentManager/RssManage?siteid=' + editmodel.siteid;
                        }
                        if (result.ServiceResultStatus == 1) {
                            alert(result.ServiceResultMassage);
                        }
                    });
                });
            },
            'TemplateManage': function () {
                //Rdio.Tools.Pagination.Event('TemplateManage', 1);

            },
            'EditTemplate': function () {

                $('#showsampleurlmodal').on('show.bs.modal', function (event) {
                    var button = $(event.relatedTarget);
                    var modal = $(this);
                    modal.find('.modal-body #sampleurliframe').attr('src', $('#sampleurl').val());
                });

                $('body').on('click', '#showsampleurl', function () {
                    $('#showsampleurlmodal').modal('show');
                });

                $('body').on('change', '#type', function () {
                    $('.contenttemplate').hide();
                    $('#contenttemplate_' + $(this).val()).show();
                });

                $('body').on('click', '#edittemplatesubmit', function () {
                    var editmodel = Rdio.ContentManager.EditTemplateModel;
                    editmodel._id = $('[name="_id"]').val();
                    editmodel.rssid = $('[name="rssid"]').val();
                    editmodel.name = $('#name').val();
                    editmodel.sampleurl = $('#sampleurl').val();
                    editmodel.type = $('#type').val();
                    editmodel.structure = [];
                    $('#TemplateStructures .contenttemplate:visible .form-group').each(function (i, v) {
                        debugger;
                        var structurModel = {};//Rdio.ContentManager.EditTemplateStructureModel;}
                        structurModel.field = $(this).find("[data-field]").attr("data-field");
                        structurModel.query = $(this).find("[data-field]").val();
                        structurModel.type = $(this).find("#structure_type").val();
                        editmodel.structure.push(structurModel);
                    });

                    
                    $.ajax({
                        method: "POST",
                        url: "/api/ContentManager/EditTemplate",
                        data: editmodel
                    }).done(function (result) {
                        if (result.ServiceResultStatus == 0) {
                            alert(result.ServiceResultMassage);
                            //window.location = '/ContentManager/RssManage?siteid=' + editmodel.siteid;
                        }
                        if (result.ServiceResultStatus == 1) {
                            alert(result.ServiceResultMassage);
                        }
                    });
                });
            },
        }
    },
    'BaseContent': {
        'BaseContentSearchModel': { 'page': 1, 'rssid': '', 'tags': '', 'categories': '', 'siteid': '' },

        'Event': {
            'Manage': function () {

                $('body').on('click', '#basecontentsearchsubmit', function () {
                    Rdio.Tools.Pagination.Event('BaseContentManage', 1);
                });

                Rdio.Tools.Pagination.Event('BaseContentManage', 1);
            }
        }
    },
    'Content': {
        'ContentSearchModel': { 'page': 1, 'rssid': '', 'tags': '', 'categories': '', 'siteid': '' },

        'Event': {
            'Manage': function () {

                $('body').on('click', '#basecontentsearchsubmit', function () {
                    Rdio.Tools.Pagination.Event('BaseContentManage', 1);
                });

                Rdio.Tools.Pagination.Event('ContentManage', 1);
            }
        }
    },
    'Admin': {
        'Event': {
            'M': function () {

            }
        }
    }
};
$(document).ready(function () {
    handelbarfunchelperinit();

    //Rdio.ContentManager.Event.EditSite();
    //Rdio.ContentManager.Event.SiteManage();
    //Rdio.ContentManager.Event.RssManage();
    //Rdio.ContentManager.Event.EditRss();

});

