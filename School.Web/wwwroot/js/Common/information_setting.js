$(function(){
    var input_num = 0;
    $(document).on('click', '.input-click-animation',function (e) {
        e=e||window.e;
        //阻止事件冒泡
        e.stopPropagation();
        e.preventDefault();
        infoFillIn();
        $(this).find('.info-label').css({"top":0})
        $(this).find('.input-required').css({"border-bottom":"2px solid #00d9e2"});
        input_num=1;
    });
    $(document).click(function(e){
        if(input_num==1) {
            infoFillIn();
        }
        input_num=0;
    });

    $(document).on('click', '.radio-input input[type=radio]', function () {
        radio($(this).val());
        $(this).attr("checked", true);
        e.preventDefault();
    });
});

function infoFillIn() {
    var labels = $('.input-click-animation .info-label');
    var input_required = $('.input-click-animation .input-required');
    labels.removeAttr("style");
    input_required.removeAttr("style");
    var input_data_required = $('input[data_required=true]')
    for (var i = 0; i < input_required.length; i++) {
        if (input_required[i].value != "" & input_required[i].value != null && input_required[i].value != undefined) {
            input_required.eq(i).prev().css({ "top": 0 })
            input_required.eq(i).css("border-bottom", "2px solid #00d9e2");
        }
    }
}

//所有的单选框去选中的之外都取消选择
function radio(value) {
    var input_checkeds = $('.radio-input input[type=radio]');
    for (var i = 0; i < input_checkeds.length; i++) {
        if (input_checkeds.eq(i).val() != value) {
            input_checkeds.eq(i).removeAttr("checked");
        }
    }
}