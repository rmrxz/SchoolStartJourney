$(function () {
    //动态显示或隐藏评论
    $(document).on('click','.com-special-sign .ar-reply',function (e) {
        e = e || window.e;
        //阻止事件冒泡
        e.stopPropagation();
        // 方法阻止元素发生默认的行为
        e.preventDefault();
        addCommentReply($(this))
    });

    $(document).on('click', '.com-area', function (e) {
        e.preventDefault();
        e=e||window.e;
        e.stopPropagation();
    });

    //点击评论区以外时隐藏评论输入框
    $(document).click(function(e){
        perComment();
    });

    $(document).on('click', '.per-comment', function (e) {
        // 方法阻止元素发生默认的行为
        e.preventDefault();
        e=e||window.e;
        //阻止事件冒泡
        e.stopPropagation();
        perComment();
        addComment($(this));
    })
   
});

function addComment($this) {
    $this.css("border", "none");
    $this.html('<div class="text-com com-area comment-text" contenteditable="true">' +
        '<strong class="sc-left com-area-reply" tabindex="-1" contenteditable="false">评论:</strong>&nbsp </div>' +
        ' <div class="clearfix publish-dis"><a href="#" class="publish publish-comment">发表</a></div>');
}

function addCommentReply($this) {
    console.log($this)
    var userName = $this.attr('data_userName');
    var data_acceptUserId = $this.attr('data_acceptUserId');
    perComment();
    var reply = $this.nextAll('.reply-com');
    reply.html('<div class="text-com com-area comment-text" data_acId="' + $this.attr('data_acId') + '" data_acceptUserId="' + data_acceptUserId + '" contenteditable="true">' +
        '<strong class="sc-left com-area-reply" tabindex="-1" contenteditable="false">回复' + userName + ':</strong>&nbsp </div>' +
        ' <div class="clearfix publish-dis"><a href="#" class="publish publish-comment">发表</a></div>');
}
function perComment() {
    $('.reply-com').html("");
    $('.per-comment').html(' 评论 <i class="fa fa-pencil-square-o sc-right"></i>');
    $('.per-comment').css("border", "1px solid #eee");
}