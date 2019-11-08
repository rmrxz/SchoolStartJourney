function bannerSlide(){
    var box = $('.ag7-bannerslide .slidebox'),
        buttons = $('.ag7-bannerslide .slidelist'),
        mask = $('.ag7-bannerslide .mask'),
        h1 =  $('.ag7-bannerslide h2'),
        h2 =  $('.ag7-bannerslide h3'),
        list = box.children('li'),
        len = list.length,
        curIndex = 0,
        nextIndex = 1,
        speed = 800,
        timer = null,
        delay = null,
        duration = 3000;
    var setTitle = function(index){
        var li = buttons.find('li').eq(index);
        h1.html(li.data('h1'));
        var maxwodth=80;
        h2.html(li.data('h2'));
        if($('.activity-describe').text().length>maxwodth)
        {
           var  activity=$('.activity-describe').text().substring(0,maxwodth);
            h2.html(activity+"......")
        }

    }

    var tab = function(cur,next){
        if(cur === next){
            return;
        };

        list.stop(true,true);
        list.eq(next).css({zIndex : 1, display : 'block' ,opacity : 1});
        list.eq(curIndex).animate({opacity : 0} , function(){
            $(this).css({zIndex : 0 , display : 'none'});
            list.eq(next).css('zIndex',2);
        })

        // 小图标遮罩
        mask.stop(true,true).animate({
            // left : next * (buttons.find('li').eq(next).outerWidth(true) + 6) + 32
            left : buttons.find('li').eq(next).position().left + 2
        },300,'easeOutExpo');
        setTitle(next);
        curIndex = next;
    }

    var autoPlay = function(){
        timer =  setInterval(function(){
            var next = curIndex == len - 1 ?  0 : curIndex + 1;
            tab(curIndex,next);
        },duration);
    }

    var stop = function(){
        clearInterval(timer);
    }


    buttons.on('mouseenter','li',function(){
        var next = $(this).index();
        tab(curIndex,next);

    });

    $('.ag7-bannerslide').hover(function(){
        stop();
    },function(){
        autoPlay();
    });
    list.eq(0).css('zIndex' , 2).siblings().hide();
    setTitle(0);
    autoPlay();


}