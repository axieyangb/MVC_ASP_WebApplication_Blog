 function updateTitle() {
        var ajax = new XMLHttpRequest();
        ajax.addEventListener("load", completeHandler, false);
        var data = new FormData();
        data.append('Title', encodeURI($('#titleModified').val()));
        data.append('SubTitle', encodeURI($('#subTitleModified').val()));
        data.append('UserID', '@Session["LoggedUserID"]');
        data.append('ArticleID', '@Model.article.ArticleID');
        ajax.open("POST", "@Url.Action("TitleUpdate", "Article")");
        ajax.responseType = "json";
        ajax.send(data);
    }
function updateArticle() {
    var ajax = new XMLHttpRequest();
    ajax.addEventListener("load", completeHandler, false);
    var data = new FormData();
    $('#articleContent').text()
    data.append('Content', encodeURI($('#articleContent').html()).replace(/\+/g, '%2B'));
    data.append('UserID', '@Session["LoggedUserID"]');
    data.append('ArticleID', '@Model.article.ArticleID');
    ajax.open("POST", "@Url.Action("ArticleUpdate", "Article")");
    ajax.responseType = "json";
    ajax.send(data);
}
function completeHandler(event) {
    var feedback = event.currentTarget.response;
    if (feedback["isAccept"] != null && feedback["isAccept"] === "1") {
        alert("Modified Successfully");
        location.reload();
    }
    else {
        alert("Article Modified Failed <br>" + feedback["error"]);
    }
}

