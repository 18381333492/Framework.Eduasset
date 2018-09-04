
function slidepage() {
  
    var STATE = true;//能否加载标识
    var option = {
        url: '',//请求的路径
        page: 1,//页面
        rows: 10,//每页的数量
        params: '',//额外的参数
        sort: 'ID',
        callback: null,//回调函数
        tipId:null,
        tip:'暂无数据'
    }

    //滑动加载分页数据
    function initslide() {
        //首次加载
        slideRequest();
        //绑定滚动事件
        $(window).scroll(function () {//绑定滚动事件
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                if (STATE) {
                    slideRequest();
                }
            }
        });
    }

    //滑动请求
    function slideRequest() {
        if (option.page == 1) {
            $.showLoading("正在加载");
        }
        $.ajax({
            url: option.url,
            type: 'POST',
            dataType: 'json',
            data: option.params + '&page=' + option.page + '&rows=' + option.rows,
            success: function (r) {
                if (option.page == 1) {
                    $.hideLoading();
                }
                if (r.success) {
                    var data = JSON.parse(r.data);
                    if (data.rows.length > 0) {
                        option.callback.call(this, data);//(js的call调用一个对象的一个方法，以另一个对象替换当前对象（函数可以看成一个对象)
                        option.page = option.page + 1;
                    }
                    else {
                        if (option.page == 1) {
                            $('.no_tip', option.tipId).find('span').text(option.tip);
                            $('.no_tip', option.tipId).show();
                        }
                        STATE = false;
                    }
                }
                else {
                    alert(r.info);
                }
            }
        });
    }

    return {
        pageParameter: option,
        initslide: initslide

    }
}