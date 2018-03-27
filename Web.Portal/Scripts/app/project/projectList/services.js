/*
 *------------------------------------------------------------------
 模块描述说明：
 项目管理功能模块对应的services文件
 作者：wjh
 时间：2017-02-24
 ----------------------------------------------------------------- */
var service = service || {};
(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
    };
    adminService.prototype.dto = {
        seachDto: function () {
            this.PageIndex = 1;
            this.PageSize = 100;
            this.TenantId = -1;
            this.ProjectName = "";
            this.Orderby = "CreateTime";
            this.Desc = false;
            this.Status = -1;
        }
    };

    adminService.prototype.io = {
        // 获得学员信息集合
        getList: function (model) {
            return $.ajax({
                url: '/Project/GetProjectList',
                type: "get",
                data: { "model": model },
                cache: false
            });
        },
        //获取合作机构信息集合
        getTenantList: function () {
            return $.ajax({
                url: '/Student/GetTenantList',
                type: "get",
                cache: false
            });
        },        
        setFreezeStatus: function(id) {
            return $.ajax({
                url: '/Project/SetFreezeStatus',
                data: { "id": id },
                type: "post",
                cache: false
            });
        }
    };

    service.project = service.project || new adminService("admin-service");


})();
