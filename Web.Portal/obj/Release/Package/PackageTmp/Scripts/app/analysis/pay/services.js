
var service = service || {};
(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
    };
    adminService.prototype.dto = {
        seachDto: function () {
            this.PageIndex = 1;
            this.PageSize = 10;
            this.TenantId = -1;
            this.ProjectId = -1;
            this.Orderby = "CreateTime";
            this.Desc = false;
        }
    };

    adminService.prototype.io = {
        // 获得学员缴费记录集合
        getList: function (model) {
            return $.ajax({
                url: '/DataStat/GetPayRrcords',
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
        //获取项目
        getProjectList: function (model) {
            return $.ajax({
                url: '/Student/GetProjectListByTenant',
                type: "get",
                data: { "model": model },
                cache: false
            });
        }

    };

    service.analysis = service.analysis || new adminService("admin-service");


})();
