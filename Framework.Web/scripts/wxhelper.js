
/*
**微信JSSDK的封装
author:[汤台]
date:2018-7-18
*/
function wxhelper() {

    //config接口注入权限验证配置
    function config(appId, timestamp, nonceStr, signature) {
        wx.config({
            debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: appId, // 必填，公众号的唯一标识
            timestamp: timestamp, // 必填，生成签名的时间戳
            nonceStr: nonceStr, // 必填，生成签名的随机串
            signature: signature,// 必填，签名
            jsApiList: ["chooseImage", "uploadImage", "scanQRCode"] // 必填，需要使用的JS接口列表
        });
    }

    //拍照或从手机相册中选图
    function chooseImage(count, success) {
        wx.chooseImage({
            count: count, // 默认9
            sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
            sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
            success: function (res) {
                success(res);
            }
            //success: function (res) {
            //    var localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
            //}
        });
    }

    //上传图片接口
    function uploadImage(localId, success) {
        wx.uploadImage({
            localId: localId, // 需要上传的图片的本地ID，由chooseImage接口获得
            isShowProgressTips: 1, // 默认为1，显示进度提示
            success: function (res) {
                success(res);
            }
        });
    }

    //扫一扫接口
    function scanQRCode(success) {
        wx.scanQRCode({
            needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
            scanType: ["qrCode","barCode"], // 可以指定扫二维码还是一维码，默认二者都有
            success: function (res) {
                //var result = res.resultStr;
                success(res);
            }
        });
    }

    return {
        config: config,
        chooseImage: chooseImage,
        uploadImage: uploadImage,
        scanQRCode: scanQRCode
    }
}