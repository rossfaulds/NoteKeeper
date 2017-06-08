    $(function() {
        var wall = new Freewall("#startgrid");
        wall.reset({
            selector: ".grid-item",
            draggable: true,
            animate: true,
            cellW: 300,
            cellH: "auto",
            fixSize: 0,
            delay: 50,
            onResize: function() {
                wall.fitWidth();
            }
        });
        wall.fitWidth();

        $("a").click(function () {
            var x = $(this).text();
            if (x === "All") {
                wall.unFilter();
                $("h2").html("Notes");
            } else {
                wall.filter("." + x);
                $("h2").html(x+" Notes");
            }
        });


        $("#startgrid").on("click", ".glyphicon-trash", function(ev) {
            ev.stopPropagation();
            var noteid = $(this).closest(".panel").data("noteid");
            $.ajax({
                url: "/api/DeleteNote/" + noteid,
                type: "DELETE",
                success: function (data) {
                    deleteItem(noteid);
                    wall.refresh();
                    wall.fitWidth();
                },
                error: function (xhr) {
                    alert("error");
                }
            });

        });
        // event delegation for dynamically generated DOM elements
        $("#startgrid").on("click", ".glyphicon-edit", function(ev)
        {
            ev.stopPropagation();

            var content = $(this).parent().siblings("#notecontent").html();
            content = content.replace(/<br>/g,"\r\n");

            var noteid =     $(this).closest(".panel").data("noteid");
            var categories = $(this).closest(".panel").data("categories");
            var colorclass = $(this).closest(".panel").data("colorclass");
            var title =      $(this).closest(".panel-body").
                                 siblings(".panel-heading").html();

            $("#UpdateNoteVisibility #Id").val(noteid);
            $("#UpdateNoteVisibility #Title").val(title);
            $("#UpdateNoteVisibility #Categories").val(categories);
            $("#UpdateNoteVisibility #ColorClass").val(colorclass);
            $("#UpdateNoteVisibility textarea").val(content);
            $("#UpdateNoteVisibility .bordercircle").removeClass("colorselected");

            var colorselector = "." + colorclass.trim();

            $("#UpdateNoteVisibility .bordercircle").find( colorselector).parent().addClass("colorselected");
            $("#updateModal").modal("show");
        });

        $(".add-note").click(function(ev) {
            var noteTitle = $("#AddNoteVisibility #Title").val();
            var editorValue = $("#AddNoteVisibility textarea").val();
            editorValue = editorValue.replace(/\r?\n/g, "<br />");

            var colorClass = $("#AddNoteVisibility #ColorClass").val();
            var categories = $("#AddNoteVisibility #Categories").val();

            $.post("/api/PostNote",
                { Title: noteTitle, Content: editorValue, Categories:categories,ColorClass : colorClass},
                function (data, status) {
                    var html = formatHtml(data,noteTitle, editorValue, categories, colorClass);
                    wall.prepend(html);
                    $("#addModal").modal("hide");
                    $("#AddNoteVisibility textarea").val("");
                    $('#AddNoteVisibility input[name="Title"').val("");
                    $("#AddNoteVisibility #Categories").val("");
                    wall.fitWidth();
                });

        });

        var formatHtml = function (id, title, notecontent, categories, colorclass) {

            var idtext = "";
            if (id) {
                idtext = 'data-noteid="' + id + '"';
            }
            var html = "<div " + idtext + ' data-categories="' + categories + '"  data-colorclass="' + colorclass.trim() + '" class="panel grid-item ' + categories + " " + colorclass + '">' +
            '<div class="panel-heading item-width ' +
                colorclass.trim() +
                '-header">' +
                title +
                "</div>" +
                '<div class="panel-body item-width ' +
                colorclass.trim() +
                '">' +
                '<div id="notecontent" class="item-width">' +
                notecontent +
                "</div>" +
                '<div class="editrelative item-width"><span class="glyphicon glyphicon-trash trashpos item-width"></span><span class="glyphicon glyphicon-edit editpos item-width"></span></div></div></div>';
            return html;
        }

        var updateItem = function(id, title, editor, categories, colorClass) {
            var html = formatHtml(id, title, editor, categories, colorClass);
            $('[data-noteid="' + id + '"]').remove();
            wall.prepend(html);
            wall.refresh();
            wall.fitWidth();
        }

        var deleteItem = function (id) {
            $('[data-noteid="' + id + '"]').remove();
            wall.refresh();
            wall.fitWidth();
        }

        $("#AddNoteVisibility .colorselector").click(function (ev) {
            ev.stopPropagation();
            var class_name = $(this).attr("class");
            $("#AddNoteVisibility .bordercircle").removeClass("colorselected");
            $(this).closest(".bordercircle").addClass("colorselected");
            var matches = class_name.match(/^(pastel-[a-zA-Z-0-9]*)\s/);
            $('#AddNoteVisibility [name="ColorClass"]').val(matches[0]);
        });

        $("#UpdateNoteVisibility .colorselector").click(function (ev) {
            ev.stopPropagation();
            var class_name = $(this).attr("class");
            $("#UpdateNoteVisibility .bordercircle").removeClass("colorselected");
            $(this).closest(".bordercircle").addClass("colorselected");
            var matches = class_name.match(/^(pastel-[a-zA-Z-0-9]*)\s/);

            $('#UpdateNoteVisibility [name="ColorClass"]').val(matches[0]);
        });
            
        $(".update-note").click(function (ev) {
            var noteId = $("#UpdateNoteVisibility #Id").val();
            var noteTitle = $("#UpdateNoteVisibility #Title").val();
            var editorValue = $("#UpdateNoteVisibility textarea").val();
            editorValue = editorValue.replace(/\r?\n/g, "<br />");

            var colorClass = $("#UpdateNoteVisibility #ColorClass").val();
            var categories = $("#UpdateNoteVisibility #Categories").val();

            $.ajax({
                url: "/api/UpdateNote/"+noteId,
                type: "PUT",
                data:  JSON.stringify({ Id: noteId, Title: noteTitle, Content: editorValue, Categories: categories, ColorClass: colorClass }),
                dataType: "json",
                contentType: "application/json",
                success : function (data) {
                    $("#updateModal").modal("hide");
                    $("#UpdateNoteVisibility textarea").val("");
                    $('#UpdateNoteVisbility input[name="Title"').val("");
                    $("#UpdateNoteVisibility Categories").val("");
                    updateItem(noteId, noteTitle, editorValue, categories, colorClass);
                    wall.fitWidth();
                },
                error: function (xhr) {
                    alert("error");
                }
            });

        });
    });
