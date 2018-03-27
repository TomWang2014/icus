/*
 *------------------------------------------------------------------
 模块描述说明：
 学员管理功能模块对应的services文件
 作者：wjh
 时间：2017-02-07
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
            this.ProjectId = -1;
            this.StudentNo = '';
            this.CertificateType = -1;
            this.CertificateNum = '';
            this.RealName = '';
            this.Orderby = "CreateTime";
            this.Desc = false;
        }
    };

    adminService.prototype.io = {
        // 获得学员信息集合
        getList: function (model) {
            return $.ajax({
                url: '/Student/GetStudentList',
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

    service.student = service.student || new adminService("admin-service");


})();
