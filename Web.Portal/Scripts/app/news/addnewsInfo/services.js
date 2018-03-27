/*
 *------------------------------------------------------------------
 模块描述说明：
 新增新闻功能模块对应的services文件
 作者：zw
 时间：2016-11-11 09:42:53
 ----------------------------------------------------------------- */
var service = service || {};
(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
    };

    adminService.prototype.dto = {
        addnewsInfoDto: function () {
            this.Id = 0;
            this.Title = '';
            this.Contect = '';
            this.Author = '';
            this.Editor = '';
            this.Source = '';
            this.SourceUrl = '';
            this.Abstract = '';
            this.Thumbnail = '';
            this.NewsTypeIds = [];
            this.IsTop = 0;
            this.IsPublish = 0;
        }
    };

    adminService.prototype.io = {
        //获取单个新闻详情
        GetNewsInfoDetail: function (id) {
            return $.ajax({
                url: '/News/GetNewsInfoDetail',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },
        // 修改新闻信息
        modifyNewsInfo: function (model) {
            return $.ajax({
                url: '/News/modifyNewsInfo',
                data: { "model": model },
                type: "post",
                cache: false
            });
        }
    };

    service.addnewsInfo = service.addnewsInfo || new adminService("admin-service");

})();