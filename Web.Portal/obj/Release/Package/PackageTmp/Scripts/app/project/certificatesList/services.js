
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
            this.StartTime = "";
            this.EndTime = "";
            this.LearnTimeTotal = 0;
            this.PayTotal = 0;
            this.AvgScore = 0;
            this.MaxScore = 0;
            this.Orderby = "CreateTime";
            this.Desc = false;
        }
    };

    adminService.prototype.io = {
        // 获得学员信息集合
        getList: function (model) {
            return $.ajax({
                url: '/Project/GetCertificateList',
                type: "get",
                data: { "model": model },
                cache: false
            });
        },
        SetCertificateStatus: function (id, type) {
            return $.ajax({
                url: '/Project/SetCertificateStatus',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },
        // 批量标记为可领证
        SetCertificateBatch: function (ids) {
            return $.ajax({
                url: '/Project/SetCertificateBatch',
                data: { "ids": ids },
                type: "post",
                cache: false
            });
        }

    };

    service.project = service.project || new adminService("admin-service");


})();
