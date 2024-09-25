$(document).ready(function () {
    $('#searchArticleTitle').keyup(function (event) {
        initSearchArticle();
    });
});

function initSearchArticle() {
    // autocomplete for event search
    var name = $("#searchArticleTitle").val();
    console.log(name);
    $('#searchArticleTitle').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Search/" + name +"?handler=SearchSuggestion",
                method: "Post",
                data: { },
                dataType: "json",
                success: response,
                error: function () {
                    response([]);
                },
                
            });
        },
        messages: {
            noResults: '',
            results: function () {

            }
        },
        focus: function (event, ui) {
            return false;
        },
        select: function (event, ui) {

            return false;
        },
        open: function (event, ui) {

            var linkResult = "/Search/" + event.target.value;

            $('.ui-autocomplete').append('<li><a class="btn btn-primary"  href="' + linkResult + '">Xem tất cả kết quả</a></li>');
            $(this).autocomplete('widget').css('z-index', 2048);
            $(this).autocomplete('widget').css('width', 320);
            return false;
        },
        create: function () {
            var acData = $(this).data('ui-autocomplete');
            acData._renderItem = function (ul, item) {

                var div = $("<div/>");
                div.attr("class", "ui-menu-item-wrapper");
                var a = $("<a>").text(item.name);

                if (item.isEmagazine)
                    a.attr("href", '/emagazine/' + item.id + '/' + item.slug);
                else
                    a.attr("href", '/tin-tuc/' + item.id + '/' + item.slug);

                div.append(a);
                var li = $("<li>").attr("class", "ui-menu-item").append(div);
                return li.appendTo(ul);
            };

        }
    });
};

//perform action search on button for search article
function performSearchArticle() {
    $('#searchArticleTitle').autocomplete('search');
};