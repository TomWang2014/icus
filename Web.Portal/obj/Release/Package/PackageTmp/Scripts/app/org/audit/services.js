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
            this.PageSize = 10;
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
                url: '/org/GetAuditProjectList',
                data: { model: model },
                type: "get",
                cache: false
            });
        },



        // 审核通过
        adoptStatus: function (id) {
            return $.ajax({
                url: '/org/adoptStatus',
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
        // 撤回审核
        defeatedStatus: function (id) {
            return $.ajax({
                url: '/org/defeatedStatus',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },
        setFreezeStatus: function(id) {
            return $.ajax({
                url: '/org/setFreezeStatus',
                data: { "id": id },
                type: "post",
                cache: false
            });
        }



    };

    service.audit = service.audit || new adminService("admin-service");
})();