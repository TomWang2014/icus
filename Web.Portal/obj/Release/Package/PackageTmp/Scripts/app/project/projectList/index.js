/*
 *------------------------------------------------------------------
 模块描述说明：
 项目管理功能模块对应的js文件
 作者：wjh
 时间：2017-02-024
 ----------------------------------------------------------------- */
var vm = new Vue({
    el: 'body',
    data: {
        SearchDto: new service.project.dto.seachDto(),
        list: [],//项目
        tenantList: []//合作机构

    },
    methods: {
        //根据搜索条件获取项目
        getList: function () {
            service.project.io.getList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.list = [];
                $(data.List).each(function (index, item) {
                    vm.list.push(item);
                });
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },
        //获取合作机构
        getTenantList: function () {
            service.project.io.getTenantList().then(function (data) {
                vm.tenantList = data;
            });
        },
        // 设置冻结/解冻
        setFreezeStatus: function (id) {
            service.project.io.setFreezeStatus(id).then(function (data) {
                college.Msg.showSuccess();
                vm.getList();
            });
        },
        //跳转到学员管理页面
        ToStudentList: function (id, tenantName, projectName) {
            window.document.location.href = "/Project/StudentList?projectId=" + id + "&tenantName=" + tenantName + "&projectName=" + projectName;
        },
        //跳转到报名审批页面
        ToExamStudent: function (id, projectName) {
            window.document.location.href = '/Project/ExamStudentList?Id=' + id + "&projectName=" + projectName;
        },
        ToCertificate: function (id) {
            window.document.location.href = '/Project/CertificateList?Id=' + id;
        }        


    },
    watch: {

    },
    ready: function () {

        this.getTenantList();

        var pageKey = window.location.pathname;
        var pageValue = $.cookie(pageKey);
        if (pageValue) {
            this.SearchDto.PageIndex = pageValue;
        }

        this.getList();
        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            
            $.cookie(pageKey, newPage);
            vm.SearchDto.PageIndex = newPage;
            vm.getList();
        });

    }
});