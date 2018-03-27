/*
 *------------------------------------------------------------------
 模块描述说明：
 新闻管理功能模块对应的services文件
 作者：zw
 时间：2016-11-11 09:42:53
 ----------------------------------------------------------------- */
var service = service || {};
(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
    };

    adminService.prototype.dto = {
        newsInfoDto: function () {
            this.Id = 0;
            this.Title = '';
            this.NetSysUserName = '';
            this.CreateTime = '';
            this.IsTop = 0;
            this.IsPublish = 0;
        },
        seachDto: function () {
            this.PageIndex = 1;
            this.PageSize = 10;
            this.Title = '';
            this.TypeId = -1;
            this.Orderby = "CreateTime";
            this.Desc = false;
        }
    };

    adminService.prototype.io = {
        // 获得新闻信息集合
        getList: function (model) {
            return $.ajax({
                url: '/News/GetNewsInfoList',
                type: "get",
                data: { "model": model },
                cache: false
            });
        },
        // 发布新闻信息
        pubNewsInfo:function(id)
        {
            return $.ajax({
                url: '/News/PubNewsInfo',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },
        //取消发布
        unpubNewsInfo: function (id) {
            return $.ajax({
                url: '/News/UnPubNewsInfo',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },
        // 删除新闻信息
        deleteNewsInfo: function (id) {
            return $.ajax({
                url: '/News/DeleteNewsInfo',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },
        // 批量删除新闻信息
        deleteBatch: function (ids) {
            return $.ajax({
                url: '/News/DeleteNewsInfoBatch',
                data: { "ids": ids },
                type: "post",
                cache: false
            });
        }
    };

    service.newsInfo = service.newsInfo || new adminService("admin-service");

})();