
var service = service || {};
(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
    };
    adminService.prototype.dto = {
        seachDto: function () {
            this.PageIndex = 1;
            this.PageSize = 10;
            this.projectId = projectId;
            this.stuId = stuId;
            this.StartTime = "";
            this.EndTime = "";
            this.Orderby = "CreateTime";
            this.Desc = false;
        }
    };

    adminService.prototype.io = {
        // 获得学员考试集合
        getExamList: function () {
            return $.ajax({
                url: '/Student/GetStudentExamList',
                type: "get",
                data: { "stuId": stuId, "projectId": projectId },
                cache: false
            });
        },
        getPayList: function () {
            return $.ajax({
                url: '/Student/GetStudentPayList',
                type: "get",
                data: { "stuId": stuId, "projectId": projectId },
                cache: false
            });
        },
        getLearnList: function (model) {
            return $.ajax({
                url: '/Student/GetStudentLearnList',
                type: "get",
                data: { "model": model, },
                cache: false
            });
        },

    };

    service.student = service.student || new adminService("admin-service");


})();
