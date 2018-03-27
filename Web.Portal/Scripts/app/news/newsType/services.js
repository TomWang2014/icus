/*
 *------------------------------------------------------------------
 模块描述说明：
 新闻栏目管理功能模块对应的services文件
 作者：zw
 时间：2016-11-11 09:42:53
 ----------------------------------------------------------------- */
var service = service || {};
(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
        
    };

    adminService.prototype.dto = {
        newsTypeDto: function () {
            this.Id = 0;
            this.Code = '';
            this.Name = '';
            this.OrderNum = 0;
        },
        seachDto: function () {
            this.PageIndex = 1;
            this.PageSize = 10;
            this.Code = '';
            this.Name = '';
            this.Orderby = "OrderNum";
            this.Desc = false;
        }
    };

    adminService.prototype.io = {
        // 获得新闻栏目集合
        getList: function (model) {
            return $.ajax({
                url: '/News/GetNewsTypeList',
                type: "get",
                data: { "model": model },
                cache: false
            });
        },
        // 修改新闻栏目信息
        modifyNewsType: function (model) {
            return $.ajax({
                url: '/News/modifyNewsType',
                data: { "model": model },
                type: "post",
                cache: false
            });
        },
        // 删除
        deleteNewsType: function (id) {
            return $.ajax({
                url: '/News/DeleteNewsType',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },

        // 批量删除
        deleteBatch: function (ids) {
            return $.ajax({
                url: '/News/DeleteNewsTypeBatch',
                data: { "ids": ids },
                type: "post",
                cache: false
            });
        }

    };

    service.newsType = service.newsType || new adminService("admin-service");

})();