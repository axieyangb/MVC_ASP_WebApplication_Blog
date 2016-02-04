
var UploadQueue = [];
Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        var file = this[i];
        if (file != null && file.name == obj.name && file.lastModified == obj.lastModified && file.size == obj.size && file.type == obj.type) {
            return i;
        }
    }
    return -1;
}

function _(el) {
    return document.getElementById(el);
}
var ajax = new XMLHttpRequest();
function uploadFile() {
    _("progressBar").style.width = "0%";
    _("upload-cancel").style.display = "initial";
    _("status").className = "alert alert-info";
    //alert(file.name+"|" + file.size +"|" + file.type);
    var formdata = new FormData();
    for (var i = 0; i < UploadQueue.length;i++)
        formdata.append("file_" + i, UploadQueue[i]);
    ajax.upload.addEventListener("progress", progressHandler, false);
    ajax.addEventListener("load", completeHandler, false);
    ajax.addEventListener("error", errorHandler, false);
    ajax.addEventListener("abort", abortHandler, false);
    ajax.open("POST", "/DashBoard/FileUpload");
    ajax.responseType = "json";
    ajax.send(formdata);
}
function progressHandler(event) {
    _("loaded_n_total").innerHTML = "Uploaded " + event.loaded + " bytes of total " + event.total + "bytes";
    var percent = (event.loaded / event.total) * 100;
    _("progressBar").style.width = Math.round(percent) + "%";
    _("status").innerHTML = Math.round(percent) + "% Uploaded... please wait";
}
function cancelFile()
{
        ajax.abort();
        _("upload-cancel").style.display = "none";
}

function completeHandler(event) {
    var res = event.currentTarget.response;
    //res=res.substring(1, res.length - 1);
    var feedback = JSON.parse(res);
    _("status").innerHTML = "Done";
    _("status").className = "alert alert-success";
    _("upload-cancel").style.display = "none";
    _("upload-submit").style.display = "none";
    $("#list").empty();
    UploadQueue = [];
    if (feedback == null) {
        _("status").innerHTML = "Session Expired";
        _("status").className = "alert alert-danger";
    }
    for (var i = 0 ; feedback != null && i < feedback.length; i++) {
        var oneRecord = document.createElement("a");
        if (feedback[i]["isAccept"] == 0) {
            oneRecord.href = "/Home/ViewImage/" + feedback[i]["UserID"];
            oneRecord.className = "list-group-item list-group-item-success";
            var innerLink = document.createElement("span");
            innerLink.className = "badge alert-success pull-right";
            innerLink.innerText = "Success";
            oneRecord.appendChild(innerLink);
            oneRecord.innerHTML += feedback[i]["fileName"];
        }
        else {
            oneRecord.className = "list-group-item list-group-item-danger";
            var innerLink = document.createElement("span");
            innerLink.className = "badge alert-danger pull-right";
            innerLink.innerText = "Failed";
            oneRecord.appendChild(innerLink);
            oneRecord.innerHTML += feedback[i]["fileName"] + "[" + feedback[i]["Error"] + "]";
        }
        _("resultList").appendChild(oneRecord);
    }
}
function errorHandler(event) {
    _("status").className = "alert alert-danger";
    _("status").innerHTML = "Upload Failed";
    if (ajax.readyState == 1) {
        ajax.abort();
    }
    _("upload-cancel").style.display = "none";
}
function abortHandler(event) {
    _("progressBar").style.width = "0%";
    _("loaded_n_total").innerHTML = "";
    _("status").className = "alert alert-warning";
    _("status").innerHTML = "Upload Aborted";
    _("upload-cancel").style.display = "none";
}
function handleFileSelect(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    var files = evt.dataTransfer.files; // FileList object.
    genList(files);
}

function cancelFile(pos) {
    UploadQueue.splice(pos, 1);
        document.getElementById('list').innerHTML = '<ul>' + htmlListGen() + '</ul>';
}

function genList(fs) {
    for (var i = 0; i < fs.length; i++) {
            UploadQueue.push(fs[i]);
    }
    document.getElementById('list').innerHTML = '<ul>' + htmlListGen() + '</ul>';
}

function htmlListGen() {
    var retHtml = "";
    var errorNum=0;
    for (var i = 0; i < UploadQueue.length; i++) {
        var str = [];
        var f = UploadQueue[i];
        if (f.type.indexOf('image/')<=-1) {
            errorNum++;
            str.push('<li>', '<strong style="color:red">[ERROR]', escape(f.name), '</strong> (', f.type || 'n/a', ') - ', f.size, ' bytes, last modified: ', f.lastModifiedDate ? f.lastModifiedDate.toLocaleDateString() : 'n/a', '<span class="glyphicon glyphicon-remove btn btn-danger btn-xs " style="position:none; margin-right:10%;margin-left:90%" onclick="cancelFile(' + i + ')">', '</span>', '</li>');
        }
        else
            str.push('<li>', '<strong>', escape(f.name), '</strong> (', f.type || 'n/a', ') - ', f.size, ' bytes, last modified: ', f.lastModifiedDate ? f.lastModifiedDate.toLocaleDateString() : 'n/a', '<span class="glyphicon glyphicon-remove btn btn-danger btn-xs " style="position:none; margin-right:10%;margin-left:90%" onclick="cancelFile(' + i + ')">', '</span>', '</li>');
        retHtml += str.join('');
    }
    if (UploadQueue.length > 0 && errorNum==0) {
        _("upload-submit").style.display = "initial";
    }
    else
        _("upload-submit").style.display = "none";
    return retHtml;
}



function handleDragOver(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    evt.dataTransfer.dropEffect = 'copy'; // Explicitly show this is a copy.
}


