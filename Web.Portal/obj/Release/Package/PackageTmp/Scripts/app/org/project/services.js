/*
 *------------------------------------------------------------------
 模块描述说明：
 用户管理功能模块对应的services文件
 作者：李天赐
 时间：2016-11-11 09:42:53
 ----------------------------------------------------------------- */
var service = service || {};

(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
        this.pageSzie = 10;
    };

    adminService.prototype.dto = {
        sysSearchDto: function () {
            this.PageIndex = 1;
            this.PageSize = 100;
            this.Name = '';
        }
    };

    adminService.prototype.io = {

        // 获得类别
        getType: function () {
            return $.ajax({
                url: ICusCRM.Path.url("org", 'GetProjectType'),
                type: "post",
                cache: false
            });
        },

        // 获得用户集合
        getList: function (model) {
            return $.ajax({
                url: '/org/GetProjectPagedList',
                data: { model: model },
                type: "get",
                cache: false
            });
        },



        // 删除用户
        deleteItem: function (id) {
            return $.ajax({
                url: '/org/deleteItem',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },

        // 删除用户
        submitAudit: function (id) {
            return $.ajax({
                url: '/org/submitAudit',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },
        // 删除用户
        withdrawalAudit: function (id) {
            return $.ajax({
                url: '/org/withdrawalAudit',
                data: { "id": id },
                type: "post",
                cache: false
            });
        }


    };

    service.project = service.project || new adminService("admin-service");
})();