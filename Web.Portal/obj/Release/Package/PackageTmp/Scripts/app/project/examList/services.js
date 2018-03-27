
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
            this.ExamineStatus = -1;
            this.IsFreeze = -1;
            this.Orderby = "CreateTime";
            this.Desc = false;
        }
    };

    adminService.prototype.io = {
        // 获得学员信息集合
        getList: function (model) {
            return $.ajax({
                url: '/Project/GetExamStudentList',
                type: "get",
                data: { "model": model },
                cache: false
            });
        },
        setExamStatus: function (id, type) {
            return $.ajax({
                url: '/Project/SetExamStudentStatus',
                data: { "id": id, "type": type },
                type: "post",
                cache: false
            });
        },
        // 批量审核通过
        ExamPassBatch: function (ids) {
            return $.ajax({
                url: '/Project/ExamPassBatch',
                data: { "ids": ids },
                type: "post",
                cache: false
            });
        },
        // 批量审核不通过
        ExamUnPassBatch: function (ids) {
            return $.ajax({
                url: '/Project/ExamUnPassBatch',
                data: { "ids": ids },
                type: "post",
                cache: false
            });
        },

    };

    service.project = service.project || new adminService("admin-service");


})();
