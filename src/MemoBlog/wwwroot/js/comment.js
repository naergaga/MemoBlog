var emojiData = undefined;  //emojiData
var postId = undefined;
//获取emojiData
function afterGetEmoji(dowork) {
    $.post("/Comment/GetEmoji", null, function (data) {
        emojiData = data;
        dowork();
    }, "json");
}
//获取Emoji item
function getEmojiData(title) {
    var str = title.substring(1, title.length - 1);
    for (var i = 0; i < emojiData.length; i++) {
        if (emojiData[i].title === str) {
            return emojiData[i];
        }
    }
    return null;
}
//解析Comment
function parseComment(data) {
    $("#commentView").html(parseItem(data.data));
}
//解析Content
function parseContent(content) {
    var reg = new RegExp(/\[[^\].]+\]/, 'g');
    var str = "";
    var item1 = null;
    while ((item = reg.exec(content)) != null) {
        var index1 = item1 == null ? 0 : item1.index + item1[0].length;
        str += content.substring(index1, item.index);
        item1 = item;

        var emItem = null;
        if ((emItem = getEmojiData(item[0])) != null) {
            str += "<img src='\\images\\emoji\\" + emItem.path + "'>";
        } else {
            str += item[0];
        }
    }
    var index1 = item1 == null ? 0 : item1.index + item1[0].length;
    str += content.substring(index1);
    return str;
}
//拼接Item
function parseItem(item) {
    var str = "";
    for (var i = 0; i < item.length; i++) {
        var item2 = item[i];
        str += "<div class='commentItem'>"
            + "<div class='context'>"
            + "<div>" + parseContent(item2.content) + "</div>"
            + "<div class='right'>"
            + item2.createTime;
        if (item2.user != null) {
            str += item2.user.userName
                + "<a data-pid='" + item2.id + "' href='javascript:void(0)' class='addCommentBtn'>回复</a>";
            if (item2.user.userName == _userName) {
               str += "<a data-pid='" + item2.id + "' href='javascript:void(0)' class='delCommentBtn'>删除</a>";
            }
        }
        str += "</div></div>" + parseItem(item2.subList) + "</div>";
    }
    return str;
}
//更新评论
function updateComment(page,afterUpdateDone) {
    $.post("/Comment/GetComments", { postId: postId, page: page }, function (data) {
        parseComment(data);
        buildCommentPaging(data,page)
        afterUpdateDone();
    }, "json");

}
//-------------------------------------------------
//评论加载完成后的事件
//显示评论modal
function showCommentDiv() {
    $("#input1").val(this.dataset.pid);
    $("#myModal").modal("show");
}
//删除评论
function delComment() {
    var doit = window.confirm("删除这条评论");
    if (!doit) return;
    var pid = this.dataset.pid;
    var postItem = $(this).parent().parent().parent();
    $.post("/Comment/Delete/" + pid, null, function (data) {
        if (data.result) {
            postItem.remove();
        }
    }, 'json');
}

function afterUpdateDone() {
    $(".addCommentBtn").click(showCommentDiv);
    $(".delCommentBtn").click(delComment);
}

//--------------------------------------------------
//构建评论分页
function ReloadCommentClick() {
    var page = $(this).text();
    updateComment(page, afterUpdateDone);
}

function buildCommentPaging(data,page) {
    var str = "<ul>";
    for (var i = 0; i < data.pageCount; i++) {
        str += "<li><a href='javascript:void(0)'"+(page==(i+1)?" class='active'>":">")+(i+1)+"</a></li>";
    }
    str += "</ul>";
    $("#commentPage").html(str);
    $("#commentPage li").click(ReloadCommentClick);
}

//--------------------------------------------------
$.extend({
    //emotion 插件
    emotion: function () {
        var $txtItem = undefined;
        //icon 点击
        function iconClick() {
            var li = this;
            var img = this.childNodes[0];
            var title = '[' + img.dataset['title'] + ']';
            $txtItem.append(title);
        }
        //表情头点击
        $(".emotionTool").click(function () {
            var obj = $(this);
            var ediv = $(".emotionView");
            if (ediv.css("display") === 'none') {
                $txtItem = obj.parent().parent().find(".emAdd");
                var offset = obj.offset();
                var left = offset.left + obj.width();
                var top = offset.top + obj.height();
                ediv.css("display", "block");
                ediv.css({ 'left': left, 'top': top });
            } else {
                ediv.css("display", "none");
            }
        });

        $(".emotionView li").click(iconClick);
    }
});
//文档加载后
$(function () {
    //提交评论
    function addComment() {
        $("#commentAddForm").submit();

    }

    $("#commentAddBtn").click(addComment);
    //为textarea注册表情插入
    $.emotion(".emAdd");
    //--解析json生成html
    afterGetEmoji(function () {
        updateComment(1, afterUpdateDone);
    });

});


