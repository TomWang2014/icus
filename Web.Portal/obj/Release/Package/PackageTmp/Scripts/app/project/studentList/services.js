
var service = service || {};
(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
    };
    adminService.prototype.dto = {
        seachDto: function () {
            this.PageIndex = 1;
            this.PageSize = 10;
            this.ProjectId = projectId;
            this.StudentNo = '';
            this.CertificateType = -1;
            this.CertificateNum = '';
            this.RealName = '';
            this.IsFreeze = -1;
            this.ExamineStatus = -1;
            this.Orderby = "CreateTime";
            this.Desc = false;
        }
    };

    adminService.prototype.io = {
        // 获得学员信息集合
        getList: function (model) {
            return $.ajax({
                url: '/Project/GetStudentList',
                type: "get",
                data: { "model": model },
                cache: false
            });
        },
        setProjectStatus: function (id, type) {
            return $.ajax({
                url: '/Project/SetProjectStatus',
                data: { "id": id, "type": type },
                type: "post",
                cache: false
            });
        },
        // 批量复课
        RecoverStatusBatch: function (ids) {
            return $.ajax({
                url: '/Project/RecoverStatusBatch',
                data: { "ids": ids },
                type: "post",
                cache: false
            });
        },
        // 批量停课
        StopStatusBatch: function (ids) {
            return $.ajax({
                url: '/Project/StopStatusBatch',
                data: { "ids": ids },
                type: "post",
                cache: false
            });
        },

    };

    service.project = service.project || new adminService("admin-service");


})();
